using Everplanner.WebApi.Dto;
using System.Threading.Tasks;

namespace Everplanner.WebApi;

public class Project
{
    private List<Task> _tasks;
    private List<Worker> _workers;
    private bool _isPlanned = false;

    private Project(int id, string name, IEnumerable<Task> tasks, IEnumerable<Worker> workers)
    {
        Id = id;
        Name = name;
        _tasks = tasks.ToList();
        _workers = workers.ToList();
    }

    public int Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<Task> Tasks => _tasks;
    public IReadOnlyCollection<Worker> Workers => _workers;
    public int ExpectedProjectDuration { get; init; } = int.MaxValue;
    public double EndingTime { get; private set; }
    public int UsedWorkersCount { get; private set; }

    public static Project BuildProject(ProjectRequestModel projectRequestModel)
    {
        List<Task> tasks = projectRequestModel.Tasks
            .Select(t => new Task(t.Id, t.Name, t.Complexity))
            .ToList();
        List<Worker> workers = projectRequestModel.Workers
            .Select(w => new Worker(w.Id, w.Name, w.Salary, w.DevelopmentVelocity))
            .ToList();

        if (workers.Count == 0)
        {
            throw new InvalidOperationException("Немає працівників для планування проєкту.");
        }

        // Fill up dependencies between tasks and workers.
        foreach (TaskRequestModel taskDto in projectRequestModel.Tasks)
        {
            Task task = tasks.Find(t => t.Id == taskDto.Id)!;
            foreach (int parentTaskId in taskDto.ParentTasks)
            {
                Task? parentTask = tasks.Find(t => t.Id == parentTaskId);
                if (parentTask is null)
                {
                    throw new InvalidOperationException($"Батьківська задача {parentTaskId} не існує.");
                }

                // Planning works with child tasks is easier for the algorithm than vice versa.
                parentTask.ChildTasks.Add(task);
            }

            foreach (int workerId in taskDto.AvailableWorkers)
            {
                Worker? availableWorker = workers.Find(w => w.Id == workerId);
                if (availableWorker is null)
                {
                    throw new InvalidOperationException($"Працівник {availableWorker} не існує.");
                }

                task.AvailableWorkers.Add(availableWorker);
            }
        }

        CountPriorities(tasks);
        return new Project(projectRequestModel.Id, projectRequestModel.Name, tasks, workers)
        {
            ExpectedProjectDuration = projectRequestModel.ExpectedProjectDuration
        };
    }

    public static PlannedProjectResponseModel ExportPlannedProject(Project project)
    {
        var tasksForExport = project.Tasks
            .Select(t => new TaskResponseModel(t.Id, t.Name, t.ExecutionStart, t.ExecutionDuration, t.Executor!.Id));
        var workersForExport = project.Workers.Select(w => new WorkerResponseModel(w.Id, w.Name));
        return new PlannedProjectResponseModel(project.Id, project.Name, tasksForExport, workersForExport,
            project.EndingTime, project.UsedWorkersCount);
    }

    public void PlanProjectForMinimalTime()
    {
        if (_isPlanned)
        {
            return;
        }

        List<Task> tasksByPriority = GetTasksByPriority().ToList();
        PlanProjectInner(tasksByPriority, _workers);

        _isPlanned = true;
        EndingTime = tasksByPriority.Max(task => task.ExecutionStart + task.ExecutionDuration);
        UsedWorkersCount = CountUsedWorkers(tasksByPriority);
    }

    public void PlanProjectForMinimalWorkersCount()
    {
        if (_isPlanned)
        {
            return;
        }

        List<Task> tasks = GetTasksByPriority().ToList();
        List<Worker> workers = GetWorkersByVelocity().ToList();
        // Used to properly handle situation when project ends just in time but small error make it go over expected duration.
        double errorCorrection = 0.000_001;
        for (int i = 0; i < _workers.Count; i++)
        {
            Console.WriteLine(new string('=', 20));
            Console.WriteLine($"===> Loop {i + 1}, {workers.Count} worker(s) used: [{string.Join(", ", workers.Select(w => w.Name))}]");
            PlanProjectInner(tasks, workers);

            double endingTime = tasks.Max(task => task.ExecutionStart + task.ExecutionDuration);
            Console.WriteLine($"===> Ending time: {endingTime}");
            if (endingTime - errorCorrection > ExpectedProjectDuration)
            {
                Console.WriteLine($"===> We got over project duration {ExpectedProjectDuration} - going out.");
                _isPlanned = true;
                return;
            }

            // Saving only tasks and not workers because we want to send data about ALL workers
            // and not only about chosen ones for tasks.
            _tasks = tasks;
            EndingTime = endingTime;
            UsedWorkersCount = CountUsedWorkers(tasks);
            tasks = CloneTasks(tasks);
            workers.ForEach(w => w.Availability = 0);

            Worker? workerToRemove = FindWorkerToRemove(tasks, workers);
            if (workerToRemove is null)
            {
                Console.WriteLine("===> Cannot remove more workers - going out.");
                _isPlanned = true;
                return;
            }

            Console.WriteLine($"===> Removing worker {workerToRemove.Id}: {workerToRemove.Name}\n");
            workers.Remove(workerToRemove);
            tasks.ForEach(t => t.AvailableWorkers.Remove(workerToRemove));
        }

        Console.WriteLine("===> End of planning for minimal workers count.");
    }

    private static void PlanProjectInner(List<Task> tasks, List<Worker> workers)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            Task task = tasks[i];
            Console.WriteLine($"Iteration {i + 1}, task {task.Name}: complexity {task.Complexity}");
            Console.WriteLine($"[{string.Join(", ", workers.Select(w => w.Availability))}]");
            Console.WriteLine(new string('-', 20));

            double minTaskEndingTime = double.MaxValue;
            Worker minWorker = task.AvailableWorkers.FirstOrDefault()
                ?? throw new InvalidOperationException($@"Задача ""{task.Name}"" не містить доступних співробітників.");
            foreach (Worker worker in task.AvailableWorkers)
            {
                var taskEndingTime = Math.Max(worker.Availability, task.Availability)
                    + task.Complexity * 5.0 / worker.DevelopmentVelocity;
                Console.WriteLine($"Worker {worker.Name}: {Math.Max(worker.Availability, task.Availability)}+{task.Complexity * 5.0 / worker.DevelopmentVelocity}={taskEndingTime}");
                if (taskEndingTime < minTaskEndingTime
                    || taskEndingTime == minTaskEndingTime && worker.Salary < minWorker.Salary)
                {
                    minWorker = worker;
                    minTaskEndingTime = taskEndingTime;
                }
            }

            task.Executor = minWorker;
            task.ExecutionStart = Math.Max(minWorker.Availability, task.Availability);
            task.ExecutionDuration = task.Complexity * 5.0 / minWorker.DevelopmentVelocity;
            minWorker.Availability = minTaskEndingTime;
            task.Availability = minTaskEndingTime;
            task.ChildTasks.ForEach(t => t.Availability = Math.Max(t.Availability, minTaskEndingTime));

            Console.WriteLine($"Chosen worker: {minWorker.Name}, will finish at {minTaskEndingTime}");
            Console.WriteLine();
        }
    }

    private static void CountPriorities(List<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            if (task.Priority == default)
            {
                task.Priority = GetPriorityForWork(task, 0, tasks.Count);
            }
        }
    }

    private static int GetPriorityForWork(Task task, int depth, int maxDepth)
    {
        if (depth > maxDepth)
        {
            throw new InvalidOperationException("Виявлено цикл в проєкті.");
        }
        if (task.Priority != default)
        {
            return task.Priority;
        }

        int priority;
        if (!task.ChildTasks.Any())
        {
            priority = task.Complexity;
        }
        else
        {
            priority = task.Complexity + task.ChildTasks.Max(task => GetPriorityForWork(task, depth + 1, maxDepth));
        }
        task.Priority = priority;
        return task.Priority;
    }

    private IEnumerable<Task> GetTasksByPriority()
    {
        // Для старої схеми з цілими днями.
        // return _tasks.OrderByDescending(w => w.Priority).ThenBy(w => w.Complexity);
        return _tasks.OrderByDescending(t => t.Priority).ThenByDescending(t => t.Id);
    }

    private IEnumerable<Worker> GetWorkersByVelocity()
    {
        return _workers.OrderByDescending(w => w.DevelopmentVelocity).ThenByDescending(w => w.Salary);
    }

    private static int CountUsedWorkers(List<Task> tasks)
    {
        var uniqueWorkers = new HashSet<Worker>();
        tasks.ForEach(task =>
        {
            if (task.Executor is not null)
            {
                uniqueWorkers.Add(task.Executor);
            }
        });
        return uniqueWorkers.Count;
    }

    private static List<Task> CloneTasks(List<Task> tasks)
    {
        List<Task> newTasks = tasks
            .Select(t => 
            {
                var newTask = new Task(t.Id, t.Name, t.Complexity)
                {
                    Priority = t.Priority
                };
                newTask.AvailableWorkers.AddRange(t.AvailableWorkers);
                return newTask;
            })
            .ToList();

        newTasks.ForEach(newTask =>
        {
            IEnumerable<int> childTaskIds = tasks.Find(t => t.Id == newTask.Id)!.ChildTasks.Select(t => t.Id);
            newTask.ChildTasks.AddRange(childTaskIds.Select(id => newTasks.Find(t => t.Id == id)!));
        });

        return newTasks;
    }

    private static Worker? FindWorkerToRemove(List<Task> tasks, List<Worker> sortedWorkers)
    {
        for (int i = sortedWorkers.Count - 1; i >= 0; i--)
        {
            Worker worker = sortedWorkers[i];
            bool isAnyCriticalTask = false; 
            foreach (Task task in tasks)
            {
                Worker? workerToRemove = task.AvailableWorkers.Find(w => w.Id == worker.Id);
                if (workerToRemove is not null)
                {
                    if (task.AvailableWorkers.Count == 1)
                    {
                        isAnyCriticalTask = true;
                        break;
                    }
                }
            }

            if (!isAnyCriticalTask)
            {
                return worker;
            }
        }

        return null;
    }
}

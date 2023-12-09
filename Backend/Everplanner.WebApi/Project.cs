using Everplanner.WebApi.Dto;

namespace Everplanner.WebApi;

public class Project
{
    private readonly List<Task> _tasks;
    private readonly List<Worker> _workers;

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
    public double EndingTime { get; private set; }

    public static Project? BuildProject(ProjectRequestModel projectRequestModel)
    {
        List<Task> tasks = projectRequestModel.Tasks
            .Select(t => new Task(t.Id, t.Name, t.Complexity))
            .ToList();
        List<Worker> workers = projectRequestModel.Workers
            .Select(w => new Worker(w.Id, w.Name, w.Salary, w.DevelopmentVelocity))
            .ToList();

        // Fill up dependencies between tasks and workers.
        foreach (TaskRequestModel taskDto in projectRequestModel.Tasks)
        {
            Task task = tasks.Find(t => t.Id == taskDto.Id)!;
            foreach (int parentTaskId in taskDto.ParentTasks)
            {
                Task? parentTask = tasks.Find(t => t.Id == parentTaskId);
                if (parentTask is null)
                {
                    return null;
                }

                // Planning works with child tasks is easier for the algorithm than vice versa.
                parentTask.ChildTasks.Add(task);
            }

            foreach (int workerId in taskDto.AvailableWorkers)
            {
                Worker? availableWorker = workers.Find(w => w.Id == workerId);
                if (availableWorker is null)
                {
                    return null;
                }

                task.AvailableWorkers.Add(availableWorker);
            }
        }

        CountPriorities(tasks);
        return new Project(projectRequestModel.Id, projectRequestModel.Name, tasks, workers);
    }

    private static void CountPriorities(IEnumerable<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            if (task.Priority == default)
            {
                task.Priority = GetPriorityForWork(task);
            }
        }
    }

    private static int GetPriorityForWork(Task task)
    {
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
            priority = task.Complexity + task.ChildTasks.Max(GetPriorityForWork);
        }
        task.Priority = priority;
        return task.Priority;
    }

    public IEnumerable<Task> GetTasksByPriority()
    {
        // Для старої схеми з цілими днями.
        // return _tasks.OrderByDescending(w => w.Priority).ThenBy(w => w.Complexity);
        return _tasks.OrderByDescending(w => w.Priority).ThenByDescending(w => w.Id);
    }

    public void PlanProject()
    {
        IEnumerable<Task> tasksByPriority = GetTasksByPriority();
        foreach ((Task task, int i) in tasksByPriority.Select((task, i) => (task, i)))
        {
            Console.WriteLine($"Iteration {i + 1}, task {task.Name}: complexity {task.Complexity}");
            Console.WriteLine($"[{string.Join(", ", _workers.Select(w => w.Availability))}]");
            Console.WriteLine(new string('-', 20));

            double minTaskEndingTime = double.MaxValue;
            Worker minWorker = _workers.First();
            foreach (Worker worker in _workers)
            {
                var taskEndingTime = Math.Max(worker.Availability, task.Availability) + task.Complexity * 5.0 / worker.DevelopmentVelocity;
                Console.WriteLine($"Worker {worker.Name}: {taskEndingTime}");
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

        Task lastTask = tasksByPriority.Last();
        EndingTime = lastTask.ExecutionStart + lastTask.ExecutionDuration;
    }

    public void PrintProjectWorkInfo()
    {
        Console.WriteLine(string.Join("\n", _tasks.Select(w =>
        {
            return $"Id: {w.Id}, Name: {w.Name}, Complexity: {w.Complexity}, Priority: {w.Priority}, " +
                $"Dependent works: {string.Join(", ", w.ChildTasks.Select(dw => dw.Name))}";
        })));
        Console.WriteLine();
    }

    public static PlannedProjectResponseModel ExportPlannedProject(Project project)
    {
        var tasksForExport = project.Tasks
            .Select(t => new TaskResponseModel(t.Id, t.Name, t.ExecutionStart, t.ExecutionDuration, t.Executor!.Id));
        var workersForExport = project.Workers.Select(w => new WorkerResponseModel(w.Id, w.Name));
        return new PlannedProjectResponseModel(project.Id, project.Name, tasksForExport, workersForExport);
    }
}

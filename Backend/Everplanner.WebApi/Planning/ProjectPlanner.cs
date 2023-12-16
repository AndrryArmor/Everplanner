using Everplanner.WebApi.Core;
using Everplanner.WebApi.Dto;

namespace Everplanner.WebApi.Planning;

public class ProjectPlanner
{
    private readonly Project _project;
    private readonly PlanningMode _mode;
    private readonly int _expectedProjectDuration;

    private List<Task> _tasks;
    private List<Worker> _workers;
    private PlannedProjectDto? _plannedProject;

    public ProjectPlanner(Project project, PlanningMode mode, int expectedProjectDuration = int.MaxValue)
    {
        _project = project;
        _mode = mode;
        _expectedProjectDuration = expectedProjectDuration;

        (_tasks, _workers) = CopyTasksAndWorkersFromProject(_project);
        CountPriorities(_tasks);
    }

    public PlannedProjectDto PlanProject()
    {
        if (_plannedProject is not null)
        {
            return _plannedProject;
        }

        switch (_mode)
        {
            case PlanningMode.MinimalTime:
                PlanProjectForMinimalTime();
                break;
            case PlanningMode.MinimalWorkersCount:
                PlanProjectForMinimalWorkersCount();
                break;
            default:
                throw new ArgumentOutOfRangeException("Невідомий режим планування.");
        }

        List<PlannedTaskDto> tasksForExport = _tasks
            .Select(t => new PlannedTaskDto(t.Id, t.Name, t.ExecutionStart, t.ExecutionDuration, t.Executor!.Id))
            .ToList();
        List<PlannedWorkerDto> workersForExport = _workers
            .Select(w => new PlannedWorkerDto(w.Id, w.Name))
            .ToList();

        List<PlannedWorkerDto> nonUsedWorkers = _project.Workers
            .Where(w => !workersForExport.Any(wfe => wfe.Id == w.Id))
            .Select(w => new PlannedWorkerDto(w.Id, w.Name))
            .ToList();
        workersForExport.AddRange(nonUsedWorkers);

        double endingTime = _tasks.Any() ? _tasks.Max(task => task.ExecutionStart + task.ExecutionDuration) : 0;
        int usedWorkersCount = CountUsedWorkers(_tasks);
        tasksForExport.Sort((t1, t2) => t1.Id - t2.Id);
        workersForExport.Sort((w1, w2) => w1.Id - w2.Id);
        _plannedProject = new PlannedProjectDto(_project.Id, _project.Name, tasksForExport, workersForExport,
            endingTime, usedWorkersCount);
        return _plannedProject;
    }

    private void PlanProjectForMinimalTime()
    {
        _tasks = GetTasksByPriority(_tasks);
        PlanProjectInner(_tasks, _workers);
    }

    private void PlanProjectForMinimalWorkersCount()
    {
        _tasks = GetTasksByPriority(_tasks);
        _workers = GetWorkersByVelocity(_workers);
        var lastTasks = new List<Task>();
        var lastWorkers = new List<Worker>();
        // Used to properly handle situation when project ends just in time but small error make it go over expected duration.
        double errorCorrection = 0.000_001;
        int workersCount = _workers.Count;
        for (int i = 0; i < workersCount; i++)
        {
            Console.WriteLine(new string('=', 20));
            Console.WriteLine($"===> Loop {i + 1}, {_workers.Count} worker(s) used: [{string.Join(", ", _workers.Select(w => w.Name))}]");
            PlanProjectInner(_tasks, _workers);

            double endingTime = _tasks.Max(task => task.ExecutionStart + task.ExecutionDuration);
            Console.WriteLine($"===> Ending time: {endingTime}");
            if (endingTime - errorCorrection > _expectedProjectDuration)
            {
                Console.WriteLine($"===> We got over project duration {_expectedProjectDuration} - going out.");
                break;
            }

            lastTasks.Clear();
            lastTasks.AddRange(_tasks.Select(t => new Task(t.Id, t.Name, t.Complexity)
            {
                Priority = t.Priority,
                Availability = t.Availability,
                Executor = t.Executor,
                ExecutionStart = t.ExecutionStart,
                ExecutionDuration = t.ExecutionDuration,
            }));
            lastWorkers.Clear();
            lastWorkers.AddRange(_workers.Select(w => new Worker(w.Id, w.Name, w.Salary, w.DevelopmentVelocity)
            {
                Availability = w.Availability
            }));

            _tasks.ForEach(t =>
            {
                t.Executor = null;
                t.ExecutionStart = default;
                t.ExecutionDuration = default;
                t.Availability = default;
            });
            _workers.ForEach(w => w.Availability = default);

            Worker? workerToRemove = FindWorkerToRemove(_tasks, _workers);
            if (workerToRemove is null)
            {
                Console.WriteLine("===> Cannot remove more workers - going out.");
                break;
            }

            Console.WriteLine($"===> Removing worker {workerToRemove.Id}: {workerToRemove.Name}\n");
            _workers.Remove(workerToRemove);
            _tasks.ForEach(t => t.AvailableWorkers.Remove(workerToRemove));
        }

        _tasks.Clear();
        _tasks.AddRange(lastTasks);
        _workers.Clear();
        _workers.AddRange(lastWorkers);
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

    private static List<Task> GetTasksByPriority(List<Task> tasks)
    {
        // Для старої схеми з цілими днями.
        // return tasks.OrderByDescending(t => t.Priority).ThenBy(t => t.Complexity);
        return tasks.OrderByDescending(t => t.Priority).ThenByDescending(t => t.Id).ToList();
    }

    private static List<Worker> GetWorkersByVelocity(List<Worker> workers)
    {
        return workers.OrderByDescending(w => w.DevelopmentVelocity).ThenByDescending(w => w.Salary).ToList();
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

    private static (List<Task> Tasks, List<Worker> Workers) CopyTasksAndWorkersFromProject(Project project)
    {
        var tasks = project.Tasks
            .Select(t => new Task(t.Id, t.Name, t.Complexity))
            .ToList();
        var workers = project.Workers
            .Select(w => new Worker(w.Id, w.Name, w.Salary, w.DevelopmentVelocity))
            .ToList();

        // Fill up dependencies between tasks and workers.
        foreach (Task task in tasks)
        {
            Core.Task projectTask = project.Tasks.Find(t => t.Id == task.Id)!;

            IEnumerable<Worker> availableWorkers = projectTask.AvailableWorkers
                .Select(aw => workers.Find(w => w.Id == aw.Id)!);
            task.AvailableWorkers.AddRange(availableWorkers);

            IEnumerable<Task> childTasks = projectTask.ChildTasks
                .Select(ct => tasks.Find(t => t.Id == ct.Id)!);
            task.ChildTasks.AddRange(childTasks);
        }

        return (tasks, workers);
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

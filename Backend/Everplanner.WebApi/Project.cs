using Everplanner.WebApi.Dto;

namespace Everplanner.WebApi;

public class Project
{
    private readonly List<Task> _tasks;
    private readonly List<Worker> _workers;

    private Project(string name, IEnumerable<Task> tasks, IEnumerable<Worker> workers)
    {
        Name = name;
        _tasks = tasks.ToList();
        _workers = workers.ToList();
    }

    public string Name { get; }
    public IReadOnlyCollection<Task> Tasks => _tasks;
    public IReadOnlyCollection<Worker> Workers => _workers;
    public double EndingTime { get; private set; }

    public static Project? BuildProject(ProjectDto projectDto)
    {
        List<Task> tasks = projectDto.Tasks.Select(t => new Task(t.Id, t.Name, t.Complexity)).ToList();
        List<Worker> workers = projectDto.Workers.Select(w => new Worker(w.Id, w.Name, w.Salary, w.DevelopmentVelocity)).ToList();

        // Fill up dependencies between tasks and workers.
        foreach (TaskDto taskDto in projectDto.Tasks)
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
        return new Project(projectDto.Name, tasks, workers);
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

    public void PrintGanttDiagram()
    {
        if (EndingTime == default)
        {
            Console.WriteLine("Cannot print Gantt diagram because project was not planned yet.");
            return;
        }

        Console.WriteLine($"Work | Graph");
        foreach (var (task, i) in _tasks.OrderBy(w => w.Id).Select((task, i) => (task, i)))
        {
            string graphString = string.Join(" ", Enumerable.Repeat(task.Executor!.Name, (int)Math.Round(task.ExecutionDuration)));
            Console.WriteLine($"{i + 1,4} | {graphString.PadLeft((int)Math.Round(task.ExecutionStart * 2 + graphString.Length))}");
        }

        string daysLegend = string.Empty;
        for (var i = 1; i <= EndingTime; i++)
        {
            daysLegend += $"{i,2}";
        }
        Console.WriteLine($"{new string('-', 7 + daysLegend.Length)}");
        Console.WriteLine($"{new string(' ', 4)} |{daysLegend}");
    }
}

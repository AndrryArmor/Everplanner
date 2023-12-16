using Task = Everplanner.WebApi.Core.Task;

namespace Everplanner.WebApi.Dto;

public record TaskDto(int Id, string Name, int Complexity, IEnumerable<int> ParentTasks, IEnumerable<int> AvailableWorkers)
{
    public static TaskDto FromTask(Task task, IEnumerable<Task> tasks)
    {
        IEnumerable<int> parentTasks = tasks
            .Where(t => t.ChildTasks.Contains(task))
            .Select(t => t.Id);
        return new TaskDto(task.Id, task.Name, task.Complexity, parentTasks, task.AvailableWorkers.Select(w => w.Id));
    }

    public static TaskDto FromTask(Task task)
    {
        return new TaskDto(task.Id, task.Name, task.Complexity, task.ParentTasks.Select(t => t.Id), 
            task.AvailableWorkers.Select(w => w.Id));
    }
}
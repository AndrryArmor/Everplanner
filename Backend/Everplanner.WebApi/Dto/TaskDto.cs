
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
}
using Task = Everplanner.WebApi.Core.Task;

namespace Everplanner.WebApi.Dto;

public record TaskDto(int Id, string Name, int Complexity, IEnumerable<int> ParentTasks, IEnumerable<int> AvailableWorkers)
{
    public static TaskDto FromTask(Task task)
    {
        return new TaskDto(task.Id, task.Name, task.Complexity, task.ParentTasks.Select(t => t.Id), 
            task.AvailableWorkers.Select(w => w.Id));
    }
}
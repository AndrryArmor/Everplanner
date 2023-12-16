namespace Everplanner.WebApi.Core;

public class Task
{
    public Task(string name, int complexity, int projectId)
    {
        Name = name;
        Complexity = complexity;
        ProjectId = projectId;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public int Complexity { get; private set; }

    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public List<Task> ParentTasks { get; } = new();
    public List<Task> ChildTasks { get; } = new();
    public List<Worker> AvailableWorkers { get; } = new();
}

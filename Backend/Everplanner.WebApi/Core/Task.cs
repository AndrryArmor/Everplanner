namespace Everplanner.WebApi.Core;

public class Task
{
    public Task(int id, string name, int complexity)
    {
        Id = id;
        Name = name;
        Complexity = complexity;
    }

    public int Id { get; }
    public string Name { get; } = string.Empty;
    public int Complexity { get; }
    public List<Task> ChildTasks { get; } = new();
    public List<Worker> AvailableWorkers { get; } = new();
}

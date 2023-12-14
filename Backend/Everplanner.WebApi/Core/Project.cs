namespace Everplanner.WebApi.Core;

public class Project
{
    public Project(int id, string name, IEnumerable<Task> tasks, IEnumerable<Worker> workers)
    {
        Id = id;
        Name = name;
        Tasks = tasks.ToList();
        Workers = workers.ToList();
    }

    public int Id { get; }
    public string Name { get; }
    public List<Task> Tasks { get; }
    public List<Worker> Workers { get; }
}

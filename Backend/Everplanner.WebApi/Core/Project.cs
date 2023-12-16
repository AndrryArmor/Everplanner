namespace Everplanner.WebApi.Core;

public class Project
{
    public Project(string name, int userId)
    {
        Name = name;
        UserId = userId;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;

    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    public List<Task> Tasks { get; } = new();
    public List<Worker> Workers { get; } = new();
}

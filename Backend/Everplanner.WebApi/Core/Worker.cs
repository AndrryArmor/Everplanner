namespace Everplanner.WebApi.Core;

public class Worker
{
    public Worker(string name, int salary, int developmentVelocity, int projectId)
    {
        Name = name;
        Salary = salary;
        DevelopmentVelocity = developmentVelocity;
        ProjectId = projectId;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public int Salary { get; private set; }
    public int DevelopmentVelocity { get; private set; }

    public int ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public List<Task> SubmittedTasks { get; private set; } = new();
}

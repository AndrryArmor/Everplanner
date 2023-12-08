namespace Everplanner.WebApi.Dto;

public class TaskDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Complexity { get; set; }
    public IEnumerable<int> ParentTasks { get; set; } = new List<int>();
    public IEnumerable<int> AvailableWorkers { get; set; } = new List<int>();
}
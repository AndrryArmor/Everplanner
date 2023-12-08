namespace Everplanner.WebApi.Dto;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    public IEnumerable<WorkerDto> Workers { get; set; } = new List<WorkerDto>();
}
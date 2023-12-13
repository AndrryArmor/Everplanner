namespace Everplanner.WebApi.Dto;

public record ProjectRequestModel(int Id, string Name, IEnumerable<TaskDto> Tasks, IEnumerable<WorkerDto> Workers)
{
    public int ExpectedProjectDuration { get; init; } = int.MaxValue;
};
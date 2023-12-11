namespace Everplanner.WebApi.Dto;

public record ProjectRequestModel(int Id, string Name, IEnumerable<TaskRequestModel> Tasks, IEnumerable<WorkerRequestModel> Workers)
{
    public int ExpectedProjectDuration { get; init; } = int.MaxValue;
};
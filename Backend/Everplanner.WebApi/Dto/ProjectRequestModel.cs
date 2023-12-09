namespace Everplanner.WebApi.Dto;

public record ProjectRequestModel(int Id, string Name, IEnumerable<TaskRequestModel> Tasks, IEnumerable<WorkerRequestModel> Workers);
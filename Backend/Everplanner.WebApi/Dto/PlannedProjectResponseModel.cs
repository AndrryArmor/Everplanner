namespace Everplanner.WebApi.Dto;

public record PlannedProjectResponseModel(int Id, string Name, IEnumerable<TaskResponseModel> Tasks,
                                          IEnumerable<WorkerResponseModel> Workers);

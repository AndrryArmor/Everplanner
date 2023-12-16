namespace Everplanner.WebApi.Dto;

public record PlannedProjectDto(
    int Id,
    string Name,
    IEnumerable<PlannedTaskDto> Tasks,
    IEnumerable<PlannedWorkerDto> Workers,
    double EndingTime,
    int UsedWorkersCount);

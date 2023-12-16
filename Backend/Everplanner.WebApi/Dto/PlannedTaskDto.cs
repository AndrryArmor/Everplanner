namespace Everplanner.WebApi.Dto;

public record PlannedTaskDto(int Id, string Name, double ExecutionStart, double ExecutionDuration, int Executor);
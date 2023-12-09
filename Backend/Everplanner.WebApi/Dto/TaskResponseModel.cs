namespace Everplanner.WebApi.Dto;

public record TaskResponseModel(int Id, string Name, double ExecutionStart, double ExecutionDuration, int Executor);
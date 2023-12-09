namespace Everplanner.WebApi.Dto;

public record TaskRequestModel(int Id, int Complexity, string Name, IEnumerable<int> ParentTasks, IEnumerable<int> AvailableWorkers);
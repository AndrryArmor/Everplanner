
namespace Everplanner.WebApi.Dto;

public record WorkerDto(int Id, string Name, int Salary, int DevelopmentVelocity)
{
    public static WorkerDto FromWorker(Worker worker)
    {
        return new WorkerDto(worker.Id, worker.Name, worker.Salary, worker.DevelopmentVelocity);
    }
}

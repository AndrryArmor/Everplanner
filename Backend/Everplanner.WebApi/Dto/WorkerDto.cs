namespace Everplanner.WebApi.Dto;

public class WorkerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Salary { get; set; }
    public int DevelopmentVelocity { get; set; }
}
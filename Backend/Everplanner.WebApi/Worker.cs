namespace Everplanner.WebApi;

public class Worker
{
    public Worker(int id, string name, int salary, int developmentVelocity)
    {
        Id = id;
        Name = name;
        Salary = salary;
        DevelopmentVelocity = developmentVelocity;
    }

    public int Id { get; }
    public string Name { get; }
    public int Salary { get; }
    public int DevelopmentVelocity { get; }
    public double Availability { get; set; }
}
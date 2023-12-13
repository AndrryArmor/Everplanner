namespace Everplanner.WebApi;

public class User
{
    public User(int id, string name, string email, string pasword, IEnumerable<Project> projects)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = pasword;
        Projects = projects.ToList();
    }
    
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string Password { get; }
    public List<Project> Projects { get; }
}

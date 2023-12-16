using Everplanner.WebApi.Core;
using Everplanner.WebApi.Data;
using Everplanner.WebApi.Dto;
using Everplanner.WebApi.Planning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("api/users/{userId}/projects")]
public class ProjectController : ControllerBase
{
    private readonly EverplannerDbContext _dbContext;

    public ProjectController(EverplannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> Get(int userId, int projectId)
    {
        Project? foundProject = await _dbContext.Projects
            .Include(p => p.Tasks)
                .ThenInclude(t => t.ParentTasks)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.AvailableWorkers)
            .Include(p => p.Workers)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        return Ok(ProjectDto.FromProject(foundProject));
    }

    [HttpPost("")]
    public async Task<IActionResult> Post(int userId, ProjectDto project)
    {
        bool isUserFound = await _dbContext.Users.AnyAsync(u => u.Id == userId);
        if (!isUserFound)
        {
            return NotFound("Користувача не знайдено.");
        }

        var newProject = new Project(project.Name, userId);
        _dbContext.Projects.Add(newProject);
        await _dbContext.SaveChangesAsync();

        return Ok(newProject.Id);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> Delete(int userId, int projectId)
    {
        Project? foundProject = await _dbContext.Projects
            .Include(p => p.Tasks)
                .ThenInclude(t => t.ChildTasks)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.AvailableWorkers)
            .Include(p => p.Workers)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        _dbContext.Projects.Remove(foundProject);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{projectId}/plan")]
    public async Task<IActionResult> PlanProject(int userId, int projectId, [FromQuery] string mode, [FromQuery] int expectedProjectDuration = int.MaxValue)
    {
        Project? foundProject = await _dbContext.Projects
            .Include(p => p.Tasks)
                .ThenInclude(t => t.ChildTasks)
            .Include(p => p.Tasks)
                .ThenInclude(t => t.AvailableWorkers)
            .Include(p => p.Workers)
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        try
        {
            var projectPlanner = new ProjectPlanner(foundProject, Enum.Parse<PlanningMode>(mode), expectedProjectDuration);
            PlannedProjectDto projectResult = projectPlanner.PlanProject();
            return Ok(projectResult);
        }
        catch (InvalidOperationException ex)
        {
            return Problem($"Планування проєкту не було виконано через наступну помилку: {ex.Message}");
        }
    }
}

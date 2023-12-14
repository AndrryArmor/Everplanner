using Everplanner.WebApi.Core;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Task = Everplanner.WebApi.Core.Task;
using Worker = Everplanner.WebApi.Core.Worker;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("api/users/{userId}/projects")]
public class ProjectController : ControllerBase
{
    [HttpGet("{projectId}")]
    public IActionResult Get(int userId, int projectId)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        return Ok(ProjectDto.FromProject(foundProject));
    }

    [HttpPost("")]
    public IActionResult Post(int userId, ProjectDto project)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        if (foundUser is null)
        {
            return NotFound("Користувача не знайдено.");
        }

        IEnumerable<Project> allProjects = InMemoryDatabase.Users.SelectMany(u => u.Projects);
        int newProjectId = allProjects.Any() ? allProjects.Max(u => u.Id) + 1 : 0;
        var newProject = new Project(newProjectId, project.Name, new List<Task>(), new List<Worker>());
        foundUser.Projects.Add(newProject);
        return Ok(newProjectId);
    }

    [HttpDelete("{projectId}")]
    public IActionResult Delete(int userId, int projectId)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        foundUser!.Projects.Remove(foundProject);
        return Ok();
    }

    //[HttpGet("{projectId}/plan")]
    //public IActionResult PlanProject(int userId, int projectId, [FromQuery] string mode, [FromQuery] int expectedProjectDuration = int.MaxValue)
    //{
    //    User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
    //    if (foundUser is null)
    //    {
    //        return NotFound("Користувача не знайдено.");
    //    }
    //    ProjectPlanner? foundProject = foundUser.Projects.Find(p => p.Id == projectId);
    //    if (foundProject is null)
    //    {
    //        return NotFound("Проєкт не знайдено.");
    //    }

    //    try
    //    {
    //        //Project project = Project.BuildProject(projectRequestModel);
    //        switch (mode)
    //        {
    //            case "MinimalTime":
    //                foundProject.PlanProjectForMinimalTime();
    //                break;
    //            case "MinimalWorkersCount":
    //                foundProject.PlanProjectForMinimalWorkersCount(expectedProjectDuration);
    //                break;
    //            default:
    //                return BadRequest($"{mode} є недійсним значенням для способу планування проєкту.");
    //        }
    //        PlannedProjectResponseModel projectResult = ProjectPlanner.ExportPlannedProject(foundProject);
    //        return Ok(projectResult);
    //    }
    //    catch (InvalidOperationException ex)
    //    {
    //        return Problem($"Планування проєкту не було виконано через наступну помилку: {ex.Message}");
    //    }
    //}
}

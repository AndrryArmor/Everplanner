using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class EverplannerController : ControllerBase
{
    public EverplannerController()
    {
    }

    [HttpPost("PlanProject")]
    public IActionResult PlanProject(ProjectDto projectDto)
    {
        Project? project = Project.BuildProject(projectDto);
        if (project is null)
        {
            return Problem("Project build failed.");
        }

        project.PlanProject();

        return Ok();
    }
}

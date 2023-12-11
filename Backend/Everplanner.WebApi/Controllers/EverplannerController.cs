using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Everplanner.WebApi.Controllers;
[ApiController]
[Route("Everplanner")]
public class EverplannerController : ControllerBase
{
    public EverplannerController()
    {
    }

    [HttpPost("PlanProjectForMinimalTime")]
    public IActionResult PlanProjectForMinimalTime(ProjectRequestModel projectRequestModel)
    {
        try
        {
            Project project = Project.BuildProject(projectRequestModel);
            project.PlanProjectForMinimalTime();
            PlannedProjectResponseModel projectResult = Project.ExportPlannedProject(project);
            return Ok(projectResult);
        }
        catch (InvalidOperationException ex)
        {
            return Problem($"Планування проєкту не було виконано через наступну помилку: {ex.Message}");
        }
    }

    [HttpPost("PlanProjectForMinimalWorkersCount")]
    public IActionResult PlanProjectForMinimalWorkersCount(ProjectRequestModel projectRequestModel)
    {
        try
        {
            Project project = Project.BuildProject(projectRequestModel);
            project.PlanProjectForMinimalWorkersCount();
            PlannedProjectResponseModel projectResult = Project.ExportPlannedProject(project);
            return Ok(projectResult);
        }
        catch (InvalidOperationException ex)
        {
            return Problem($"Планування проєкту не було виконано через наступну помилку: {ex.Message}");
        }
    }
}

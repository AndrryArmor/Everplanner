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
    public IActionResult PlanProject(ProjectRequestModel projectRequestModel)
    {
        Project? project = Project.BuildProject(projectRequestModel);
        if (project is null)
        {
            return Problem("Створення проєкту не відбулося через наявність неіснуючої задачі серед батьків однієї з задач" +
                "або через наявність неіснуючого доступного співробітника для однієї з задач.");
        }

        try
        {
            project.PlanProject2();
        }
        catch (InvalidOperationException)
        {
            return Problem("Планування проєкту було перервано через відсутність доступних співробітників для однієї з задач.");
        }
        PlannedProjectResponseModel projectResult = Project.ExportPlannedProject(project);
        return Ok(projectResult);
    }
}

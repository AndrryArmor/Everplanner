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
            return Problem("��������� ������ �� �������� ����� �������� �������� ������ ����� ������ ���� � �����" +
                "��� ����� �������� ���������� ���������� ����������� ��� ���� � �����.");
        }

        try
        {
            project.PlanProject2();
        }
        catch (InvalidOperationException)
        {
            return Problem("���������� ������ ���� ��������� ����� ��������� ��������� ����������� ��� ���� � �����.");
        }
        PlannedProjectResponseModel projectResult = Project.ExportPlannedProject(project);
        return Ok(projectResult);
    }
}

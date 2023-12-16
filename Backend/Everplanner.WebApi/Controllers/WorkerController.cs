using Everplanner.WebApi.Core;
using Everplanner.WebApi.Data;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Everplanner.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}/projects/{projectId}/workers")]
public class WorkerController : ControllerBase
{
    private readonly EverplannerDbContext _dbContext;

    public WorkerController(EverplannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("")]
    public async Task<IActionResult> Post(int userId, int projectId, WorkerDto worker)
    {
         bool isProjectFound = await _dbContext.Projects
            .AnyAsync(p => p.UserId == userId && p.Id == projectId);
        if (!isProjectFound)
        {
            return NotFound("Проєкт не знайдено.");
        }

        var newWorker = new Worker(worker.Name, worker.Salary, worker.DevelopmentVelocity, projectId);
        _dbContext.Workers.Add(newWorker);
        await _dbContext.SaveChangesAsync();
        return Ok(newWorker.Id);
    }

    [HttpDelete("{workerId}")]
    public async Task<IActionResult> Delete(int userId, int projectId, int workerId)
    {
        Worker? foundWorker = await _dbContext.Workers
            .FirstOrDefaultAsync(w => w.Project.UserId == userId && w.ProjectId == projectId && w.Id == workerId);
        if (foundWorker is null)
        {
            return NotFound("Співробітника не знайдено.");
        }

        _dbContext.Workers.Remove(foundWorker);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}

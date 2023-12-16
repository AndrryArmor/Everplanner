using Everplanner.WebApi.Data;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = Everplanner.WebApi.Core.Task;
using Worker = Everplanner.WebApi.Core.Worker;

namespace Everplanner.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}/projects/{projectId}/tasks")]
public class TaskController : ControllerBase
{
    private readonly EverplannerDbContext _dbContext;

    public TaskController(EverplannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("")]
    public async Task<IActionResult> Post(int userId, int projectId, TaskDto task)
    {
        bool isProjectFound = await _dbContext.Projects
            .AnyAsync(p => p.UserId == userId && p.Id == projectId);
        if (!isProjectFound)
        {
            return NotFound("Проєкт не знайдено.");
        }

        var newTask = new Task(task.Name, task.Complexity, projectId);
        List<Task> parentTasks = await _dbContext.Tasks
            .Where(t => task.ParentTasks.Contains(t.Id))
            .ToListAsync();
        List<Worker> availableWorkers = await _dbContext.Workers
            .Where(w => task.AvailableWorkers.Contains(w.Id))
            .ToListAsync();
        newTask.ParentTasks.AddRange(parentTasks);
        newTask.AvailableWorkers.AddRange(availableWorkers);
        _dbContext.Tasks.Add(newTask);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(newTask.Id);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> Delete(int userId, int projectId, int taskId)
    {
        Task? foundTask = await _dbContext.Tasks
            .Include(t => t.ChildTasks)
            .FirstOrDefaultAsync(t => t.Project.UserId == userId && t.ProjectId == projectId && t.Id == taskId);
        if (foundTask is null)
        {
            return NotFound("Задачу не знайдено.");
        }

        _dbContext.Tasks.Remove(foundTask);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> Put(int userId, int projectId, int taskId, TaskDto task)
    {
        if (taskId != task.Id)
        {
            return BadRequest("Id задачі не співпадає.");
        }

        Task? foundTask = await _dbContext.Tasks
            .Include(t => t.ParentTasks)
            .Include(t => t.AvailableWorkers)
            .FirstOrDefaultAsync(t => t.Project.UserId == userId && t.ProjectId == projectId && t.Id == taskId);
        if (foundTask is null)
        {
            return NotFound("Задачу не знайдено.");
        }

        List<Task> parentTasks = await _dbContext.Tasks
            .Where(t => task.ParentTasks.Contains(t.Id))
            .ToListAsync();
        List<Worker> availableWorkers = await _dbContext.Workers
            .Where(w => task.AvailableWorkers.Contains(w.Id))
            .ToListAsync();
        foundTask.ParentTasks.Clear();
        foundTask.ParentTasks.AddRange(parentTasks);
        foundTask.AvailableWorkers.Clear();
        foundTask.AvailableWorkers.AddRange(availableWorkers);
        _dbContext.Tasks.Update(foundTask);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}


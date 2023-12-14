using Everplanner.WebApi.Core;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Everplanner.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}/projects/{projectId}/workers")]
public class WorkerController : ControllerBase
{
    [HttpPost("")]
    public IActionResult Post(int userId, int projectId, WorkerDto worker)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        IEnumerable<Worker> allWorkers = InMemoryDatabase.Users.SelectMany(u => u.Projects.SelectMany(p => p.Workers));
        int newWorkerId = allWorkers.Any() ? allWorkers.Max(w => w.Id) + 1 : 0;
        foundProject.Workers.Add(new Worker(newWorkerId, worker.Name, worker.Salary, worker.DevelopmentVelocity));
        return Ok(newWorkerId);
    }

    [HttpDelete("{workerId}")]
    public IActionResult Delete(int userId, int projectId, int workerId)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        Worker? foundWorker = foundProject?.Workers.Find(w => w.Id == workerId);
        if (foundWorker is null)
        {
            return NotFound("Співробітника не знайдено.");
        }

        foundProject!.Workers.Remove(foundWorker);
        // Remove deleted worker from awailable workers of all tasks.
        foundProject.Tasks.ForEach(t => t.AvailableWorkers.Remove(foundWorker));

        return Ok();
    }
}

using Everplanner.WebApi.Core;
using Everplanner.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Task = Everplanner.WebApi.Core.Task;

namespace Everplanner.WebApi.Controllers;

[ApiController]
[Route("api/users/{userId}/projects/{projectId}/tasks")]
public class TaskController : ControllerBase
{
    [HttpPost("")]
    public IActionResult Post(int userId, int projectId, TaskDto task)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        if (foundProject is null)
        {
            return NotFound("Проєкт не знайдено.");
        }

        IEnumerable<Task> allTasks = InMemoryDatabase.Users.SelectMany(u => u.Projects.SelectMany(p => p.Tasks));
        int newTaskId = allTasks.Any() ? allTasks.Max(t => t.Id) + 1 : 0;
        var newTask = new Task(newTaskId, task.Name, task.Complexity);
        try
        {
            newTask.AvailableWorkers.AddRange(task.AvailableWorkers.Select(awId => foundProject.Workers.Find(w => w.Id == awId)
                ?? throw new InvalidOperationException($"Співробітника з id={awId} не існує в проєкті {foundProject.Name}.")));

            // Add new task as a child for parent tasks.
            foreach (int parentTaskId in task.ParentTasks)
            {
                Task parentTask = foundProject.Tasks.Find(t => t.Id == parentTaskId)
                    ?? throw new InvalidOperationException($"Задача з id={parentTaskId} не існує в проєкті {foundProject.Name}.");

                parentTask.ChildTasks.Add(newTask);
            }

            foundProject.Tasks.Add(newTask);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(newTaskId);
    }

    [HttpDelete("{taskId}")]
    public IActionResult Delete(int userId, int projectId, int taskId)
    {
        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        Task? foundTask = foundProject?.Tasks.Find(t => t.Id == taskId);
        if (foundTask is null)
        {
            return NotFound("Задачу не знайдено.");
        }

        foundProject!.Tasks.Remove(foundTask);
        // Removes task from children of all tasks.
        foundProject.Tasks.ForEach(t => t.ChildTasks.Remove(foundTask));

        return Ok();
    }

    [HttpPut("{taskId}")]
    public IActionResult Put(int userId, int projectId, int taskId, TaskDto task)
    {
        if (taskId != task.Id)
        {
            return BadRequest("Id задачі не співпадає.");
        }

        User? foundUser = InMemoryDatabase.Users.Find(u => u.Id == userId);
        Project? foundProject = foundUser?.Projects.Find(p => p.Id == projectId);
        Task? foundTask = foundProject?.Tasks.Find(t => t.Id == taskId);
        if (foundTask is null)
        {
            return NotFound("Задачу не знайдено.");
        }

        try
        {
            List<Worker> updatedAvailableWorkers = task.AvailableWorkers
                .Select(awId => foundProject!.Workers.Find(w => w.Id == awId)
                    ?? throw new InvalidOperationException($"Співробітника з id={awId} не існує в проєкті {foundProject.Name}."))
                .ToList();

            // Remove new task as a child for all parents outside of the list of parents.
            foundProject!.Tasks
                .Where(t => !task.ParentTasks.Contains(t.Id)) 
                .ToList()
                .ForEach(t => t.ChildTasks.Remove(foundTask));

            // Add new task as a child for newly added parent tasks.
            foreach (int parentTaskId in task.ParentTasks)
            {
                Task parentTask = foundProject.Tasks.Find(t => t.Id == parentTaskId)
                    ?? throw new InvalidOperationException($"Задача з id={parentTaskId} не існує в проєкті {foundProject.Name}.");
                    
                if (!parentTask.ChildTasks.Contains(foundTask))
                {
                    parentTask.ChildTasks.Add(foundTask);
                }
            }

            foundTask.AvailableWorkers.Clear();
            foundTask.AvailableWorkers.AddRange(updatedAvailableWorkers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}


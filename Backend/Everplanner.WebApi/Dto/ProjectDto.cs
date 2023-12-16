using Everplanner.WebApi.Core;

namespace Everplanner.WebApi.Dto;

public record ProjectDto(int Id, string Name, IEnumerable<TaskDto> Tasks, IEnumerable<WorkerDto> Workers)
{
    public static ProjectDto FromProject(Project project)
    {
        return new ProjectDto(project.Id, project.Name, project.Tasks.Select(TaskDto.FromTask),
            project.Workers.Select(WorkerDto.FromWorker));
    }
}
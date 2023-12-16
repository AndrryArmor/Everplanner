using Everplanner.WebApi.Core;

namespace Everplanner.WebApi;
public record ProjectPreviewResponseModel(int Id, string Name, int Workers, int Tasks)
{
    public static ProjectPreviewResponseModel FromProject(Project project)
    {
        return new ProjectPreviewResponseModel(project.Id, project.Name, project.Workers.Count, project.Tasks.Count);
    }
}
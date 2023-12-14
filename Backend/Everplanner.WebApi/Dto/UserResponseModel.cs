using Everplanner.WebApi.Core;

namespace Everplanner.WebApi.Dto;

public record UserResponseModel(int Id, string Name, IEnumerable<ProjectPreviewResponseModel> Projects)
{
    public static UserResponseModel FromUser(User user)
    {
        return new UserResponseModel(user.Id, user.Name, user.Projects.Select(ProjectPreviewResponseModel.FromProject));
    }
};

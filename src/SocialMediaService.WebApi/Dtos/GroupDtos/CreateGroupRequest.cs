using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.GroupDtos;

public record CreateGroupRequest(string Name,
    string Description,
    GroupVisibilities Visibility = GroupVisibilities.Public);
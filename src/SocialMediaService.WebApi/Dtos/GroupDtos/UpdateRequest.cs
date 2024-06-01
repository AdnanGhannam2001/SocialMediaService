using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.GroupDtos;

public record UpdateRequest(string? Name = null,
    string? Description = null,
    GroupVisibilities? Visibility = null);
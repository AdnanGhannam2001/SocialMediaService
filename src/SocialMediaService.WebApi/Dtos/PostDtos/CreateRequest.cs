using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.PostDtos;

public record CreateRequest(string Content, PostVisibilities Visibility);
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.WebApi.Dtos.PostDtos;

public record CreatePostRequest(string Content, PostVisibilities Visibility);
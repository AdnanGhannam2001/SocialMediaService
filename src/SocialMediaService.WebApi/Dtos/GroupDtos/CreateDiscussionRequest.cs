namespace SocialMediaService.WebApi.Dtos.GroupDtos;

public record CreateDiscussionRequest(string Title, string Content, IEnumerable<string> Tags);
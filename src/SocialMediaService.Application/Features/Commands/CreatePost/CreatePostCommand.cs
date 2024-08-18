using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.CreatePost;

public sealed record CreatePostCommand(string ProfileId,
    string Content,
    PostVisibilities Visibility,
    MediaTypes? MediaType = null) : ICommand<Post>;
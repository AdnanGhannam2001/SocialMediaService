using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Posts.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.UpdatePost;

public sealed record UpdatePostCommand(string ProfileId,
    string PostId,
    string Content,
    PostVisibilities Visibility,
    Media? Media = null)
        : ICommand<Post>;
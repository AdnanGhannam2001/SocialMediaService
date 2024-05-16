using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Posts.ValueObjects;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.CreatePost;

public sealed record CreatePostCommand(string ProfileId,
    string Content,
    PostVisibilities Visibility,
    Media? Media = null) : ICommand<Post>;
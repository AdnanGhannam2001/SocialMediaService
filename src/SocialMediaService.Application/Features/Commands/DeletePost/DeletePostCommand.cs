using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.DeletePost;

public sealed record DeletePostCommand(string ProfileId, string PostId) : ICommand<Post>;
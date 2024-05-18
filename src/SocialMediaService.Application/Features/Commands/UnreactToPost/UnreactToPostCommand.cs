using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.UnreactToPost;

public sealed record UnreactToPostCommand(string ProfileId, string PostId) : ICommand<Reaction>;
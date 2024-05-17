using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.UnhidePost;

public sealed record UnhidePostCommand(string ProfileId, string PostId) : ICommand<Post>;
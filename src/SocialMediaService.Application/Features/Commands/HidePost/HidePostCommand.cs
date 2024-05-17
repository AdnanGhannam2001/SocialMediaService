using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Application.Features.Commands.HidePost;

public sealed record HidePostCommand(string ProfileId, string PostId) : ICommand<Post>;
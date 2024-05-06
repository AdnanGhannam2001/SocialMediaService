using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.UnfollowAccount;

public sealed record UnfollowAccountCommand(string FollowerId, string FollowedId) : ICommand<Follow>;
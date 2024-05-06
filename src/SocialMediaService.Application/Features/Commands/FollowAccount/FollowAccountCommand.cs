using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.FollowAccount;

public sealed record FollowAccountCommand(string FollowerId, string ProfileId) : ICommand<Follow>;
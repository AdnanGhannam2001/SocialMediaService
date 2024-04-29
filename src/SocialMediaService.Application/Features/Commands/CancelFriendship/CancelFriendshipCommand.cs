using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.CancelFriendship;

public sealed record CancelFriendshipCommand(string Id1, string Id2) : ICommand<Friendship>;
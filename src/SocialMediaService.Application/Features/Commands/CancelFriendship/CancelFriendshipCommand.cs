using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.CancelFriendship;

public sealed record CancelFriendshipCommand(string ProfileId, string FriendId) : ICommand<Friendship>;
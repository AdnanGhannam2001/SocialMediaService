using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.CancelFriendshipRequest;

public sealed record CancelFriendshipRequestCommand(string SenderId, string ReceiverId) : ICommand<FriendshipRequest>;
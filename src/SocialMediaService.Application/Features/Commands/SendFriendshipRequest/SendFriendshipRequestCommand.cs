using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.SendFriendshipRequest;

public sealed record SendFriendshipRequestCommand(string SenderId, string ReceiverId) : ICommand<FriendshipRequest>;
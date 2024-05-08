using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public sealed record RespondToFriendshipRequestCommand(string SenderId, string ReceiverId, bool Aggreed = true)
    : ICommand<Friendship>;
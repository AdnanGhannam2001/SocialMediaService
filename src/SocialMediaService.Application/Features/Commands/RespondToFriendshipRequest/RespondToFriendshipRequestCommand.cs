using MediatR;
using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToFriendshipRequest;

public sealed record RespondToFriendshipRequestCommand(string SenderId, string ReceiverId, bool Aggreed = true)
    : ICommand<Unit>;
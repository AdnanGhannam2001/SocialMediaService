using MediatR;
using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Commands.RespondToJoinRequest;

public sealed record RespondToJoinRequestCommand(string GroupId, string JoinRequesterId, string ProfileId, bool Accept = true)
    : ICommand<Unit>;
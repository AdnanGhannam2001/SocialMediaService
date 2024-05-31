using MediatR;
using SocialMediaService.Application.Interfaces;

namespace SocialMediaService.Application.Features.Commands.LeaveGroup;

public sealed record LeaveGroupCommand(string ProfileId, string GroupId) : ICommand<Unit>;
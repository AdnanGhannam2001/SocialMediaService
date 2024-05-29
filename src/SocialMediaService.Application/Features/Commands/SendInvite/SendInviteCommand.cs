using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.SendInvite;

public sealed record SendInviteCommand(string ProfileId, string GroupId, string SenderId, string Content)
    : ICommand<Invite>;
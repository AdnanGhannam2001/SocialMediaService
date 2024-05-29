using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.RespondToInvite;

public sealed record RespondToInviteCommand(string ProfileId, string GroupId, string SenderId, bool Accept = true) : ICommand<Invite>;
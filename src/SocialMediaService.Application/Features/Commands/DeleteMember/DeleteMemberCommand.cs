using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.DeleteMember;

public sealed record DeleteMemberCommand(string ProfileId, string GroupId, string RequesterId, bool Kick = false, string Reason = "") : ICommand<Member>;
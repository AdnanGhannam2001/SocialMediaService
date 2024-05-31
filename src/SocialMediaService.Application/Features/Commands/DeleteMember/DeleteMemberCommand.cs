using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.DeleteMember;

public sealed record DeleteMemberCommand(string MemberId, string GroupId, string ProfileId, bool Kick = false, string Reason = "") : ICommand<Member>;
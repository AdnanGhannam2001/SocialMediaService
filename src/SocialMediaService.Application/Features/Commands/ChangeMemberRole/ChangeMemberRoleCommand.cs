using PR2.Shared.Enums;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Application.Features.Commands.ChangeMemberRole;

public sealed record ChangeMemberRoleCommand(string MemberId, string GroupId, string ProfileId, MemberRoleTypes Role) : ICommand<Member>;
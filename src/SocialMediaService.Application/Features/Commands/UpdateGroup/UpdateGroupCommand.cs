using PR2.Shared.Enums;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Application.Features.Commands.UpdateGroup;

public sealed record UpdateGroupCommand(string ProfileId,
    string GroupId,
    string? Name = null,
    string? Description = null,
    GroupVisibilities? Visibility = null,
    string? Image = null,
    string? CoverImage = null,   
    MemberRoleTypes? InviterRole = null,
    MemberRoleTypes? PostingRole = null,
    MemberRoleTypes? EditDetailsRole = null) : ICommand<Group>;
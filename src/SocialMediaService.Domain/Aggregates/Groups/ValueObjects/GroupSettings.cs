using PR2.Shared.Enums;

namespace SocialMediaService.Domain.Aggregates.Groups.ValueObjects;

public sealed record GroupSettings
{
    public GroupSettings(MemberRoleTypes inviterRole = MemberRoleTypes.Normal,
        MemberRoleTypes postingRole = MemberRoleTypes.Normal,
        MemberRoleTypes editDetailsRole = MemberRoleTypes.Organizer)
    {
        InviterRole = inviterRole;
        PostingRole = postingRole;
        EditDetailsRole = editDetailsRole;
    }

    public MemberRoleTypes InviterRole { get; }
    public MemberRoleTypes PostingRole { get; }
    public MemberRoleTypes EditDetailsRole { get; }
}
using PR2.Shared.Enums;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Domain.Aggregates.Groups;

public sealed class Member
{
    #pragma warning disable CS8618
    private Member() { }
    #pragma warning restore CS8618

    public Member(Group group, Profile profile, MemberRoleTypes role = MemberRoleTypes.Normal)
    {
        GroupId = group.Id;
        Group = group;

        ProfileId = profile.Id;
        Profile = profile;

        Role = role;

        JointAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public MemberRoleTypes Role { get; private set; }
    public DateTime JointAtUtc { get; private set; }

    public void ChangeRole(MemberRoleTypes role)
    {
        Role = role;
    }
}
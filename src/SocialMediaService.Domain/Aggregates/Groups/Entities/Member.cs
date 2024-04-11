using PR2.Shared.Enums;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Member
{
    public Member(Group group, Profile profile)
    {
        GroupId = group.Id;
        Group = group;

        ProfileId = profile.Id;
        Profile = profile;

        JointAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public MemberRoleTypes Role { get; private set; }
    public DateTime JointAtUtc { get; private set; }
}
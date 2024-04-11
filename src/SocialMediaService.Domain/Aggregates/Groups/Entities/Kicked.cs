using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Kicked
{
    public Kicked(Group group, Profile profile, Profile kickedBy, string reason)
    {
        GroupId = group.Id;
        Group = group;

        ProfileId = profile.Id;
        Profile = profile;

        KickedById = kickedBy.Id;
        KickedBy = kickedBy;

        Reason = reason;
        KickedAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string KickedById { get; private set; }
    public Profile KickedBy { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public string Reason { get; private set; }
    public DateTime KickedAtUtc { get; private set; }
}
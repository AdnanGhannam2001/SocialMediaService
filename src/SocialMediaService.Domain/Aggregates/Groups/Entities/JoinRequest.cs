using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class JoinRequest
{
    public JoinRequest(Group group, Profile profile, string content)
    {
        GroupId = group.Id;
        Group = group;

        ProfileId = profile.Id;
        Profile = profile;

        Content = content;
        SentAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public string? Content { get; private set; }
    public DateTime SentAtUtc { get; private set; }
}
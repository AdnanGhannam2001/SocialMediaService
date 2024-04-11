using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public class FavoriteDiscussion
{
    public FavoriteDiscussion(Profile profile, Discussion discussion)
    {
        ProfileId = profile.Id;
        Profile = profile;

        DiscussionId = discussion.Id;
        Discussion = discussion;

        AddedAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string DiscussionId { get; private set; }
    public Discussion Discussion { get; private set; }

    public DateTime AddedAtUtc { get; private set; }
}
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class FavoriteDiscussion
{
    #pragma warning disable CS8618
    private FavoriteDiscussion() { }
    #pragma warning restore CS8618

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
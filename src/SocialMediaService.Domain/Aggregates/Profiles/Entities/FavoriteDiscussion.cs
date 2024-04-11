using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public class FavoriteDiscussion
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string DiscussionId { get; private set; }
    public Discussion Discussion { get; private set; }

    public DateTime AddedAt { get; private set; }
}
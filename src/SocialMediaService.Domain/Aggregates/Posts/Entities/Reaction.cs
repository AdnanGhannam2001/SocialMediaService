using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts;

public class Reaction
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string PostId { get; private set; }
    public Post Post { get; private set; }

    public ReactionTypes Type { get; private set; }
    public DateTime ReactedAt { get; private set; }
}
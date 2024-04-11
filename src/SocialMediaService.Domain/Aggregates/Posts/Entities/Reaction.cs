using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts;

public sealed class Reaction
{
    public Reaction(Post post, Profile profile, ReactionTypes type)
    {
        PostId = post.Id;
        Post = post;

        ProfileId = profile.Id;
        Profile = profile;

        Type = type;
        ReactedAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string PostId { get; private set; }
    public Post Post { get; private set; }

    public ReactionTypes Type { get; private set; }
    public DateTime ReactedAtUtc { get; private set; }
}
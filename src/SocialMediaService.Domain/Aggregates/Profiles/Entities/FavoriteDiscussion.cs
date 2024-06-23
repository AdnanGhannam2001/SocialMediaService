using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class SavedPost
{
    #pragma warning disable CS8618
    private SavedPost() { }
    #pragma warning restore CS8618

    public SavedPost(Profile profile, Post post)
    {
        ProfileId = profile.Id;
        Profile = profile;

        PostId = post.Id;
        Post = post;

        SavedAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string PostId { get; private set; }
    public Post Post { get; private set; }

    public DateTime SavedAtUtc { get; private set; }
}
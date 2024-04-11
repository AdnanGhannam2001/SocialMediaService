namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Follow
{
    public Follow(Profile follower, Profile followed)
    {
        FollowerId = follower.Id;
        Follower = follower;

        FollowedId = followed.Id;
        Followed = followed;

        FollowedAtUtc = DateTime.UtcNow;
    }

    public string FollowerId { get; private set; }
    public Profile Follower { get; private set; }

    public string FollowedId { get; private set; }
    public Profile Followed { get; private set; }

    public DateTime FollowedAtUtc { get; private set; }
}
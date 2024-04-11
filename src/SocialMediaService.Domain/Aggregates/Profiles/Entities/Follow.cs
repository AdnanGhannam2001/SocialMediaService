namespace SocialMediaService.Domain.Aggregates.Profiles;

public class Follow
{
    public string FollowerId { get; private set; }
    public Profile Follower { get; private set; }

    public string FollowedId { get; private set; }
    public Profile Followed { get; private set; }

    public DateTime FollowedAt { get; private set; }
}
namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Friendship
{
    #pragma warning disable CS8618
    public Friendship() { }
    #pragma warning restore CS8618

    public Friendship(Profile profile, Profile friend)
    {
        ProfileId = profile.Id;
        Profile = profile;

        FriendId = friend.Id;
        Friend = friend;

        StartedAtUtc = DateTime.UtcNow;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string FriendId { get; private set; }
    public Profile Friend { get; private set; }

    public DateTime StartedAtUtc { get; private set; }
}
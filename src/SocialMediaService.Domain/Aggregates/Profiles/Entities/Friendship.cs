namespace SocialMediaService.Domain.Aggregates.Profiles;

public class Friendship
{
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
namespace SocialMediaService.Domain.Aggregates.Profiles;

public class Friendship
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string FriendId { get; private set; }
    public Profile Friend { get; private set; }

    public DateTime StartedAt { get; private set; }
}
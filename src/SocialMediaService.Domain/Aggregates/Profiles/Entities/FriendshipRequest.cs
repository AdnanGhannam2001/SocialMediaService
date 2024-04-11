namespace SocialMediaService.Domain.Aggregates.Profiles;

public class FriendshipRequest
{
    public string SenderId { get; private set; }
    public Profile Sender { get; private set; }

    public string ReceiverId { get; private set; }
    public Profile Receiver { get; private set; }

    public DateTime SentAt { get; private set; }
}
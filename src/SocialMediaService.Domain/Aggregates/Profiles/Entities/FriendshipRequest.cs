namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class FriendshipRequest
{
    public FriendshipRequest(Profile sender, Profile receiver)
    {
        SenderId = sender.Id;
        Sender = sender;

        ReceiverId = receiver.Id;
        Receiver = receiver;

        SentAtUtc = DateTime.UtcNow;
    }

    public string SenderId { get; private set; }
    public Profile Sender { get; private set; }

    public string ReceiverId { get; private set; }
    public Profile Receiver { get; private set; }

    public DateTime SentAtUtc { get; private set; }
}
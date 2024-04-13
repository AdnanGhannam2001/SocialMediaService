namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class FriendshipRequest
{
    #pragma warning disable CS8618
    private FriendshipRequest() { }
    #pragma warning restore CS8618

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
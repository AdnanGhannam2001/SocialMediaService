namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Block
{
    #pragma warning disable CS8618
    private Block() { }
    #pragma warning restore CS8618

    public Block(Profile blocker, Profile blocked, string reason)
    {
        BlockerId = blocker.Id;
        Blocker = blocker;

        BlockedId = blocked.Id;
        Blocked = blocked;

        Reason = reason;
        BlockedAtUtc = DateTime.UtcNow;
    }

    public string BlockerId { get; private set; }
    public Profile Blocker { get; private set; }

    public string BlockedId { get; private set; }
    public Profile Blocked { get; private set; }

    public string Reason { get; private set; }
    public DateTime BlockedAtUtc { get; private set; }
}
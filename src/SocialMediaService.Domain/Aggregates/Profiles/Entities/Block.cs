namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Block
{
    public string BlockerId { get; private set; }
    public Profile Blocker { get; private set; }

    public string BlockedId { get; private set; }
    public Profile Blocked { get; private set; }

    public string Reason { get; private set; }
    public DateTime BlockedAt { get; private set; }
}
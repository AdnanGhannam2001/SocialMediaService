using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Invite
{
    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string SenderId { get; private set; }
    public Profile Sender { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public string? Content { get; private set; }
    public DateTime SentAt { get; private set; }
}
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts.ValueObjects;

public sealed record Media
{
    #pragma warning disable CS8618
    private Media() { }
    #pragma warning restore CS8618

    public Media(MediaTypes type)
    {
        Type = type;
    }

    public MediaTypes Type { get; }
}
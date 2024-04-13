using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts.ValueObjects;

public sealed record Media
{
    #pragma warning disable CS8618
    private Media() { }
    #pragma warning restore CS8618

    public Media(MediaTypes type, Uri uri)
    {
        Type = type;
        Uri = uri;
    }

    public MediaTypes Type { get; }
    public Uri Uri { get; }
}
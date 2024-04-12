using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts.ValueObjects;

public sealed record Media
{
    public Media(MediaTypes type, Uri uri)
    {
        Type = type;
        Uri = uri;
    }

    public MediaTypes Type { get; }
    public Uri Uri { get; }
}
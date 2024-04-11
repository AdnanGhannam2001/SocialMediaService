using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts.ValueObjects;

public sealed record Media
{
    public Media(MediaTypes type, string url)
    {
        Type = type;
        Url = url;
    }

    public MediaTypes Type { get; }
    public string Url { get; }
}
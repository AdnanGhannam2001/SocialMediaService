using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts.ValueObjects;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts;

public sealed class Post : AggregateRoot
{
    private List<Profile> _hiddenBy = [];
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];

    public Post(Profile profile, string content, PostVisibilities visibility, Media? media = null) : base()
    {
        ProfileId = profile.Id;
        Profile = profile;

        Content = content;
        Visibility = visibility;
        Media = media;
    }

    public Post(Profile profile,
        Group group,
        string content,
        PostVisibilities visibility = PostVisibilities.Public,
        Media? media = null) : base()
    {
        ProfileId = profile.Id;
        Profile = profile;

        GroupId = group.Id;
        Group = group;

        Content = content;
        Visibility = visibility;
        Media = media;
    }

    public string Content { get; private set; }
    public PostVisibilities Visibility { get; private set; }
    public Media? Media { get; private set; }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string? GroupId { get; private set; }
    public Group? Group { get; private set; }

    public IReadOnlyCollection<Profile> HiddenBy => _hiddenBy;
    public IReadOnlyCollection<Reaction> Reactions => _reactions;
    public IReadOnlyCollection<Comment> Comments => _comments;
}
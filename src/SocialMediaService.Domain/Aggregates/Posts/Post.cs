using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts.ValueObjects;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts;

public class Post : AggregateRoot
{
    private List<Profile> _hiddenBy = [];
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];

    #pragma warning disable CS8618
    public Post() { }
    #pragma warning restore CS8618

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

    public void Update(string content, PostVisibilities visibility, Media? media = null)
    {
        Content = content;
        Visibility = visibility;
        Media = media;

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void HideTo(Profile profile)
    {
        _hiddenBy.Add(profile);
    }

    public void UnhideTo(Profile profile)
    {
        _hiddenBy.Remove(profile);
    }

    public void React(Reaction reaction)
    {
        _reactions.Add(reaction);
    }

    public void Unreact(Reaction reaction)
    {
        _reactions.Remove(reaction);
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
    }
}
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Posts;

public sealed class Comment : Entity
{
    private List<Comment> _replies = [];

    #pragma warning disable CS8618
    public Comment() { }
    #pragma warning restore CS8618

    public Comment(Post post, Profile profile, string content) : base()
    {
        PostId = post.Id;
        Post = post;

        ProfileId = profile.Id;
        Profile = profile;

        Content = content;
    }

    public Comment(Comment comment, Profile profile, string content) : base()
    {
        ParentId = comment.Id;
        Parent = comment;

        ProfileId = profile.Id;
        Profile = profile;

        Content = content;
    }

    public Comment(Discussion discussion, Profile profile, string content) : base()
    {
        DiscussionId = discussion.Id;
        Discussion = discussion;

        ProfileId = profile.Id;
        Profile = profile;

        Content = content;
    }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string? PostId { get; private set; }
    public Post? Post { get; private set; }

    public string? ParentId { get; private set; }
    public Comment? Parent { get; private set; }

    public string? DiscussionId { get; private set; }
    public Discussion? Discussion { get; private set; }

    public string Content { get; private set; }

    public IReadOnlyCollection<Comment> Replies => _replies;

    public void AddReply(Comment comment)
    {
        _replies.Add(comment);
    }

    public void RemoveReply(Comment comment)
    {
        _replies.Remove(comment);
    }
}
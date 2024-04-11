using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Posts;

public class Post : AggregateRoot
{
    private List<Profile> _hiddenBy = [];
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];

    public string Content { get; private set; }
    public PostVisibilities Visibility { get; private set; }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string? GroupId { get; private set; }
    public Group? Group { get; private set; }

    public IReadOnlyCollection<Profile> HiddenBy => _hiddenBy;
    public IReadOnlyCollection<Reaction> Reactions => _reactions;
    public IReadOnlyCollection<Comment> Comments => _comments;
}
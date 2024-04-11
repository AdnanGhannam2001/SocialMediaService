using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Discussion : Entity
{
    private List<Tag> _tags = [];
    private List<FavoriteDiscussion> _favorites = [];
    private List<Comment> _comments = [];

    public Discussion(Group group, Profile profile, string title, string content, IEnumerable<Tag> tags) : base()
    {
        GroupId = group.Id;
        Group = group;

        ProfileId = profile.Id;
        Profile = profile;

        Title = title;
        Content = content;

        _tags.AddRange(tags);
    }
    
    public string Title { get; private set; }
    public string Content { get; private set; }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public IReadOnlyCollection<Tag> Tags => _tags;
    public IReadOnlyCollection<FavoriteDiscussion> Favorites => _favorites;
    public IReadOnlyCollection<Comment> Comments => _comments;
}
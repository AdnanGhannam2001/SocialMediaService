using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;

namespace SocialMediaService.Domain.Aggregates.Groups;

public class Discussion : Entity
{
    private List<Tag> _tags = [];
    private List<FavoriteDiscussion> _favorites = [];
    private List<Comment> _comments = [];
    
    public string Title { get; private set; }
    public string Content { get; private set; }

    public string ProfileId { get; private set; }
    public Profile Profile { get; private set; }

    public string GroupId { get; private set; }
    public Group Group { get; private set; }

    public IReadOnlyCollection<Tag> Tags => _tags;
    public List<FavoriteDiscussion> Favorites => _favorites;
    public List<Comment> Comments => _comments;
}
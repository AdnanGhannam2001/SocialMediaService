using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Groups;

public sealed class Group : AggregateRoot
{
    private List<Member> _members = [];
    private List<Profile> _kicked = [];
    private List<JoinRequest> _joinRequests = [];
    private List<Invite> _invites = [];
    private List<Discussion> _discussions = [];
    private List<Post> _posts = [];

    public Group(string name,
        string description,
        Settings settings,
        GroupVisibilities visibility = GroupVisibilities.Public,
        string? image = null,
        string? coverImage = null) : base()
    {
        Name = name;
        Description = description;
        Visibility = visibility;
        Image = image;
        CoverImage = coverImage;

        SettingsId = settings.Id;
        Settings = settings;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string? Image { get; private set; }
    public string? CoverImage { get; private set; }
    public GroupVisibilities Visibility { get; private set; }

    public string SettingsId { get; private set; }
    public Settings Settings { get; private set; }

    public IReadOnlyCollection<Member> Members => _members;
    public IReadOnlyCollection<Profile> Kicked => _kicked;
    public IReadOnlyCollection<JoinRequest> JoinRequests => _joinRequests;
    public IReadOnlyCollection<Invite> Invites => _invites;
    public IReadOnlyCollection<Discussion> Discussions => _discussions;
    public IReadOnlyCollection<Post> Posts => _posts;
}
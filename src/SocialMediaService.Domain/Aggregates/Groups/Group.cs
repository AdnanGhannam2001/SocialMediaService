using PR2.Shared.Enums;
using SocialMediaService.Domain.Aggregates.Groups.ValueObjects;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Groups;

public sealed class Group : AggregateRoot
{
    private List<Member> _members = [];
    private List<Kicked> _kicked = [];
    private List<JoinRequest> _joinRequests = [];
    private List<Invite> _invites = [];
    private List<Discussion> _discussions = [];
    private List<Post> _posts = [];

    #pragma warning disable CS8618
    private Group() { }
    #pragma warning restore CS8618

    public Group(string name,
        string description,
        GroupSettings settings,
        GroupVisibilities visibility = GroupVisibilities.Public,
        Uri? image = null,
        Uri? coverImage = null) : base()
    {
        Name = name;
        Description = description;
        Visibility = visibility;
        Image = image;
        CoverImage = coverImage;
        Settings = settings;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Uri? Image { get; private set; }
    public Uri? CoverImage { get; private set; }
    public GroupVisibilities Visibility { get; private set; }

    public GroupSettings Settings { get; private set; }

    public IReadOnlyCollection<Member> Members => _members;
    public IReadOnlyCollection<Kicked> Kicked => _kicked;
    public IReadOnlyCollection<JoinRequest> JoinRequests => _joinRequests;
    public IReadOnlyCollection<Invite> Invites => _invites;
    public IReadOnlyCollection<Discussion> Discussions => _discussions;
    public IReadOnlyCollection<Post> Posts => _posts;

    public void Update(string? name = null,
        string? description = null,
        MemberRoleTypes? inviterRole = null,
        MemberRoleTypes? postingRole = null,
        MemberRoleTypes? editDetailsRole = null,
        GroupVisibilities? visibility = null,
        string? image = null,
        string? coverImage = null)
    {
        Name = name ?? Name;
        Description = description ?? Description;

        Settings = new GroupSettings(inviterRole ?? Settings.InviterRole,
            postingRole ?? Settings.PostingRole,
            editDetailsRole ?? Settings.EditDetailsRole);

        Visibility = visibility ?? Visibility;

        Image = image is null
            ? Image
            : image == string.Empty
                ? Image = null
                : new Uri(image);

        CoverImage = coverImage is null
            ? CoverImage
            : coverImage == string.Empty
                ? CoverImage = null
                : new Uri(coverImage);
    }

    public void AddJoinRequest(JoinRequest request)
    {
        _joinRequests.Add(request);
    }

    public void RemoveJoinRequest(JoinRequest request)
    {
        _joinRequests.Remove(request);
    }

    public void AddMember(Member member)
    {
        _members.Add(member);
    }

    public void RemoveMember(Member member)
    {
        _members.Remove(member);
    }

    public void AddInvite(Invite invite)
    {
        _invites.Add(invite);
    }

    public void Kick(Kicked kicked)
    {
        _kicked.Add(kicked);
    }

    public void AddDiscussion(Discussion discussion)
    {
        _discussions.Add(discussion);
    }

    public void RemoveDiscussion(Discussion discussion)
    {
        _discussions.Remove(discussion);
    }
}
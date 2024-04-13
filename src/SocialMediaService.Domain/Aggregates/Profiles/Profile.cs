using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;
using SocialMediaService.Domain.Bases;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Domain.Aggregates.Profiles;

public sealed class Profile : AggregateRoot
{
    private List<Block> _blocked = [];
    private List<Block> _blockedBy = [];
    private List<FriendshipRequest> _sentRequests = [];
    private List<FriendshipRequest> _receivedRequests = [];
    private List<Follow> _following = [];
    private List<Follow> _followedBy = [];
    private List<Friendship> _friends = [];
    private List<Friendship> _friendTo = [];
    private List<Post> _posts = [];
    private List<Post> _hidden = [];
    private List<Reaction> _reactions = [];
    private List<Comment> _comments = [];
    private List<Member> _memberOf = [];
    private List<Kicked> _kicked = [];
    private List<Kicked> _kickedFrom = [];
    private List<JoinRequest> _joinRequests = [];
    private List<Invite> _sentInvites = [];
    private List<Invite> _receivedInvites = [];
    private List<Discussion> _discussions = [];
    private List<FavoriteDiscussion> _favoriteDiscussions = [];

    #pragma warning disable CS8618
    private Profile() { }
    #pragma warning restore CS8618

    public Profile(string id,
        string firstName,
        string lastName,
        PhoneNumber phoneNumber,
        DateTime dateOfBirth,
        Genders gender,
        string bio,
        JobInformations jobInformations,
        Socials socials,
        Settings settings) : base()
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        IsActive = false;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Bio = bio;
        JobInformations = jobInformations;
        Socials = socials;
        Settings = settings;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Genders Gender { get; private set; }
    public string Bio { get; private set; }
    public JobInformations JobInformations { get; private set; }
    public Socials Socials { get; private set; }
    public Settings Settings { get; private set; }

    public IReadOnlyCollection<Block> Blocked => _blocked.AsReadOnly();
    public IReadOnlyCollection<Block> BlockedBy => _blockedBy.AsReadOnly();
    public IReadOnlyCollection<FriendshipRequest> SentRequests => _sentRequests.AsReadOnly();
    public IReadOnlyCollection<FriendshipRequest> ReceivedRequests => _receivedRequests.AsReadOnly();
    public IReadOnlyCollection<Follow> Following => _following.AsReadOnly();
    public IReadOnlyCollection<Follow> FollowedBy => _followedBy.AsReadOnly();
    public IReadOnlyCollection<Friendship> Friends => _friends.AsReadOnly();
    public IReadOnlyCollection<Friendship> FriendTo => _friendTo.AsReadOnly();
    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();
    public IReadOnlyCollection<Post> Hidden => _hidden.AsReadOnly();
    public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    public IReadOnlyCollection<Member> MemberOf => _memberOf.AsReadOnly();
    public IReadOnlyCollection<Kicked> Kicked => _kicked.AsReadOnly();
    public IReadOnlyCollection<Kicked> KickedFrom => _kickedFrom.AsReadOnly();
    public IReadOnlyCollection<JoinRequest> JoinRequests => _joinRequests.AsReadOnly();
    public IReadOnlyCollection<Invite> SentInvites => _sentInvites.AsReadOnly();
    public IReadOnlyCollection<Invite> ReceivedInvites => _receivedInvites.AsReadOnly();
    public IReadOnlyCollection<Discussion> Discussions => _discussions.AsReadOnly();
    public IReadOnlyCollection<FavoriteDiscussion> FavoriteDiscussions => _favoriteDiscussions.AsReadOnly();
}
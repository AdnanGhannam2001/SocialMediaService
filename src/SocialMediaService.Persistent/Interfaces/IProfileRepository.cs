using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Interfaces;

public interface IProfileRepository
    : IWriteRepository<Profile, string>, IReadRepository<Profile, string>
{
    Task<Page<Profile>> GetPageWithRelationsAsync(PageRequest<Profile> request,
        string? profileId = null,
        CancellationToken cancellationToken = default);

    #region Settings
    Task<Profile?> GetWithSettingsAsync(string id, CancellationToken cancellationToken = default);
    #endregion // Settings

    #region Friendship
    Task<Page<Friendship>> GetFriendshipsPageAsync(string profileId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default);

    Task<Profile?> GetWithFriendshipAsync(string profileId, string friendId, CancellationToken cancellationToken = default);

    Task<int> CountFriendshipsAsync(string profileId, Expression<Func<Friendship, bool>>? predicate = null, CancellationToken cancellationToken = default);
    #endregion // Friendship

    #region Friendship Requests
    Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string profileId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);
    Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string profileId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);

    Task<Profile?> GetWithFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default);

    Task<int> CountSentFriendshipsRequestAsync(string profileId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<int> CountReceivedFriendshipsRequestAsync(string profileId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Friendship Requests

    #region Block
    Task<Profile?> GetWithBlockedAsync(string blockerId, string blockedId, CancellationToken cancellationToken = default);
    Task<Page<Block>> GetBlockedPageAsync(string blockerId, PageRequest<Block> request, CancellationToken cancellationToken = default);
    Task<int> CountBlockedAsync(string blockerId,
        Expression<Func<Block, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Block

    #region Follow
    Task<Profile?> GetWithFollowingAsync(string followerId, string followedId, CancellationToken cancellationToken = default);
    Task<Page<Follow>> GetFollowingPageAsync(string followerId, PageRequest<Follow> request, CancellationToken cancellationToken = default);
    Task<int> CountFollowingAsync(string followerId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    Task<int> CountFollowedAsync(string followedId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Follow

    #region Invite
    Task<Profile?> GetWithInviteAsync(string profileId, string groupId, string senderId, CancellationToken cancellationToken = default);
    Task<Page<Invite>> GetInvitesPageAsync(string profileId, PageRequest<Invite> request, CancellationToken cancellationToken = default);
    Task<int> CountInvitesAsync(string profileId,
        Expression<Func<Invite, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Invite

    #region Favorite Discussion
    Task<Profile?> GetWithFavoriteDiscussionAsync(string profileId, string discussionId, CancellationToken cancellationToken = default);
    Task<Page<FavoriteDiscussion>> GetFavoriteDiscussionsPageAsync(string profileId, PageRequest<FavoriteDiscussion> request, CancellationToken cancellationToken = default);
    Task<int> CountFavoriteDiscussionsAsync(string profileId,
        Expression<Func<FavoriteDiscussion, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Favorite Discussion
}
using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Interfaces;

public interface IProfileRepository
    : IWriteRepository<Profile, string>, IReadRepository<Profile, string>
{
    #region Settings
    Task<Profile?> GetWithSettingsAsync(string id, CancellationToken cancellationToken = default);
    #endregion // Settings

    #region Friendship
    Task<Page<Friendship>> GetFriendshipsPageAsync(string userId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default);

    Task<Profile?> GetWithFriendshipAsync(string profileId, string friendId, CancellationToken cancellationToken = default);

    Task<int> CountFriendshipsAsync(string userId, Expression<Func<Friendship, bool>>? predicate = null, CancellationToken cancellationToken = default);
    #endregion // Friendship

    #region Friendship Requests
    Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string userId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);
    Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string userId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);

    Task<Profile?> GetWithFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default);

    Task<int> CountSentFriendshipsRequestAsync(string userId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<int> CountReceivedFriendshipsRequestAsync(string userId,
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
    Task<Profile?> GetWithFollowedAsync(string followerId, string followedId, CancellationToken cancellationToken = default);
    Task<Page<Follow>> GetFollowedPageAsync(string followerId, PageRequest<Follow> request, CancellationToken cancellationToken = default);
    Task<int> CountFollowedAsync(string followerId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    #endregion // Follow
}
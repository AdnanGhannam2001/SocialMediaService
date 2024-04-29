using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Interfaces;

public interface IProfileRepository
    : IWriteRepository<Profile, string>, IReadRepository<Profile, string>
{
    Task<Settings?> GetSettingsAsync(string id, CancellationToken cancellationToken = default);

    #region Friendship
    Task<Page<Friendship>> GetFriendshipsPageAsync(string userId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default);

    Task<Friendship?> GetFriendshipAsync(string id1, string id2, CancellationToken cancellationToken = default);
    Task<Friendship?> CreateFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default);
    Task<bool> DeleteFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default);

    Task<int> CountFriendshipsCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> CountFriendshipsCountAsync(string userId, Expression<Func<Friendship, bool>> predicate, CancellationToken cancellationToken = default);
    #endregion

    #region Friendship Requests
    Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string userId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);
    Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string userId, PageRequest<FriendshipRequest> request, CancellationToken cancellationToken = default);

    Task<FriendshipRequest?> GetFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default);

    Task<FriendshipRequest> CreateFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default);
    Task<bool> DeleteFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default);

    Task<int> CountFriendshipsRequestCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> CountFriendshipsRequestCountAsync(string userId,
        Expression<Func<FriendshipRequest, bool>> predicate,
        CancellationToken cancellationToken = default);
    #endregion
}
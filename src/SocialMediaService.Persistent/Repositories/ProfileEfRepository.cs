using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class ProfileEfRepository
    : EfRepository<Profile, string>, IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileEfRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    #region Settings
    public Task<Profile?> GetWithSettingsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    #endregion // Settings

    #region Friendship
    public async Task<Page<Friendship>> GetFriendshipsPageAsync(string userId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .SelectMany(x => x.Friends)
            .Where(x => x.FriendId.Equals(userId) || x.ProfileId.Equals(userId))
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Friend)
            .Include(x => x.Profile)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFriendshipsAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<Profile?> GetWithFriendshipAsync(string profileId, string friendId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Friends.Where(
                x => x.ProfileId.Equals(profileId) && x.FriendId.Equals(friendId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(profileId), cancellationToken);
    }

    public Task<int> CountFriendshipsAsync(string userId,
        Expression<Func<Friendship, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.Friends)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Friendship Requests
    public async Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string userId,
        PageRequest<FriendshipRequest> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.SentRequests)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Receiver)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountSentFriendshipsRequestAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public async Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string userId,
        PageRequest<FriendshipRequest> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.ReceivedRequests)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Sender)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountReceivedFriendshipsRequestAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<Profile?> GetWithFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.SentRequests.Where(
                x => x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(senderId), cancellationToken: cancellationToken);
    }

    public Task<int> CountSentFriendshipsRequestAsync(string userId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.SentRequests)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<int> CountReceivedFriendshipsRequestAsync(string userId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.ReceivedRequests)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Block
    public Task<Profile?> GetWithBlockedAsync(string blockerId, string blockedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Blocked.Where(
                x => x.BlockerId.Equals(blockerId) && x.BlockedId.Equals(blockedId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(blockedId), cancellationToken);
    }

    public async Task<Page<Block>> GetBlockedPageAsync(string blockerId,
        PageRequest<Block> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(blockerId))
            .SelectMany(x => x.Blocked)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Blocked)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountBlockedAsync(blockerId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountBlockedAsync(string blockerId,
        Expression<Func<Block, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(blockerId))
            .SelectMany(x => x.Blocked)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Follow
    public Task<Profile?> GetWithFollowedAsync(string followerId, string followedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Following.Where(
                x => x.FollowedId.Equals(followerId) && x.FollowedId.Equals(followedId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(followerId), cancellationToken);
    }

    public async Task<Page<Follow>> GetFollowedPageAsync(string followerId, PageRequest<Follow> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(followerId))
            .SelectMany(x => x.Following)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Followed)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFollowedAsync(followerId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountFollowedAsync(string followerId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(followerId))
            .SelectMany(x => x.Following)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion
}
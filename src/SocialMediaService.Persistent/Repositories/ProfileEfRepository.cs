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
    public Task<Settings?> GetSettingsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))
            .Select(x => x.Settings)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> UpdateSettingsAsync(Settings settings, CancellationToken cancellationToken = default)
    {
        _context.Set<Settings>().Update(settings);
        return SaveChangesAsync(cancellationToken);
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
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFriendshipsAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public async Task<Friendship?> AddFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default)
    {
        await _context.Friendships.AddAsync(friendship, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return friendship;
    }

    public Task<Friendship?> GetFriendshipAsync(string id1, string id2, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id1))
            .SelectMany(x => x.Friends)
            .Where(x => x.FriendId.Equals(id2))
            .FirstOrDefaultAsync(cancellationToken);
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

    public async Task<bool> DeleteFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default)
    {
        _context.Set<Friendship>().Remove(friendship);
        return await SaveChangesAsync(cancellationToken) > 0;
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
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountReceivedFriendshipsRequestAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }
           
    public Task<FriendshipRequest?> GetFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .SelectMany(x => x.SentRequests)
            .FirstOrDefaultAsync(x => x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId), cancellationToken: cancellationToken);
    }
           
    public async Task<FriendshipRequest> AddFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default)
    {
        await _context.Set<FriendshipRequest>().AddAsync(friendshipRequest, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return friendshipRequest;
    }

    public async Task<bool> DeleteFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default)
    {
        _context.Set<FriendshipRequest>().Remove(friendshipRequest);
        return await SaveChangesAsync(cancellationToken) > 0;
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
    public Task<Block?> GetBlockedAsync(string blockerId, string blockedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(blockerId))
            .SelectMany(x => x.Blocked)
            .FirstOrDefaultAsync(x => x.BlockedId.Equals(blockedId), cancellationToken);
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

    public async Task<Block> AddBlockAsync(Block block, CancellationToken cancellationToken = default)
    {
        await _context.Set<Block>().AddAsync(block, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return block;
    }

    public async Task<bool> DeleteBlockAsync(Block block, CancellationToken cancellationToken = default)
    {
        _context.Set<Block>().Remove(block);
        return await SaveChangesAsync(cancellationToken) > 0;
    }
    #endregion

    #region Follow
    public Task<Follow?> GetFollowedAsync(string followerId, string followedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(followerId))
            .SelectMany(x => x.Following)
            .FirstOrDefaultAsync(x => x.FollowedId.Equals(followedId), cancellationToken);
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

    public async Task<Follow> AddFollowAsync(Follow follow, CancellationToken cancellationToken = default)
    {
        await _context.Set<Follow>().AddAsync(follow, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return follow;
    }

    public async Task<bool> DeleteFollowAsync(Follow follow, CancellationToken cancellationToken = default)
    {
        _context.Set<Follow>().Remove(follow);
        return await SaveChangesAsync(cancellationToken) > 0;
    }
    #endregion
}
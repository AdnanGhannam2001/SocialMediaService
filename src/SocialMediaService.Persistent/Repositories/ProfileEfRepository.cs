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

    public Task<Settings?> GetSettingsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))
            .Select(x => x.Settings)
            .FirstOrDefaultAsync(cancellationToken);
    }

    #region Friendship
    public async Task<Page<Friendship>> GetFriendshipsPageAsync(string userId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.Friends)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFriendshipsAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), 0);
    }

    public async Task<Friendship?> CreateFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default)
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
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var total = await CountSentFriendshipsRequestAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), 0);
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
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var total = await CountReceivedFriendshipsRequestAsync(userId, request.Predicate, cancellationToken);

        return new (query.ToList(), 0);
    }
           
    public Task<FriendshipRequest?> GetFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .SelectMany(x => x.SentRequests)
            .FirstOrDefaultAsync(x => x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId), cancellationToken: cancellationToken);
    }
           
    public async Task<FriendshipRequest> CreateFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default)
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
            .FirstOrDefaultAsync(x => x.BlockerId.Equals(blockedId), cancellationToken);
    }
    #endregion
}
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
        int pageNumber,
        int pageSize,
        Expression<Func<Friendship, bool>>? predicate = null,
        Expression<Func<Friendship, dynamic>>? keySelector = null,
        bool desc = false,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.Friends);

        var orderQuery = keySelector is not null
            ? desc
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector)
            : desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var total = await CountFriendshipsCountAsync(userId, predicate ?? (_ => true), cancellationToken);

        return new (query.ToList(), 0);
    }

    public Task<Friendship?> GetFriendshipAsync(string id1, string id2, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id1))
            .SelectMany(x => x.Friends)
            .Where(x => x.FriendId.Equals(id2))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> CountFriendshipsCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.Friends)
            .CountAsync(cancellationToken: cancellationToken);
    }

    public Task<int> CountFriendshipsCountAsync(string userId, Expression<Func<Friendship, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(userId))
            .SelectMany(x => x.Friends)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> DeleteFriendshipAsync(Friendship friendship, CancellationToken cancellationToken = default)
    {
        _context.Set<Friendship>().Remove(friendship);
        return await SaveChangesAsync(cancellationToken) > 0;
    }
    #endregion

    #region Friendship Requests
    public Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
           
    public Task<FriendshipRequest?> GetFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
           
    public Task<FriendshipRequest> CreateFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFriendshipRequestAsync(FriendshipRequest friendshipRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
           
    public Task<int> CountFriendshipsRequestCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountFriendshipsRequestCountAsync(string userId,
        Expression<Func<FriendshipRequest, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}
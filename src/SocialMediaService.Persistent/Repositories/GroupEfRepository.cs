using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class GroupEfRepository : EfRepository<Group, string>, IGroupRepository
{
    public GroupEfRepository(ApplicationDbContext context) : base(context) { }

    #region Posts
    public async Task<Page<Post>> GetPostsPageAsync(string id, PageRequest<Post> request, string? profileId = null, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Posts)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderByDescending(x => x.CreatedAtUtc)
                : query.OrderBy(x => x.CreatedAtUtc);

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountPostsAsync(id, request.Predicate, cancellationToken);

        return new (query.ToList(), total);

    }

    public Task<int> CountPostsAsync(string id, Expression<Func<Post, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))    
            .SelectMany(x => x.Posts)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion // Posts


    #region Join Requests
    public async Task<Page<JoinRequest>> GetJoinRequestsPageAsync(string id, PageRequest<JoinRequest> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.JoinRequests)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderByDescending(x => x.SentAtUtc)
                : query.OrderBy(x => x.SentAtUtc);

        query = orderQuery
            .Include(x => x.Profile)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountJoinRequestsAsync(id, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountJoinRequestsAsync(string id, Expression<Func<JoinRequest, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.JoinRequests)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<Group?> GetWithJoinRequestAsync(string id, string profileId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.JoinRequests.Where(x => x.ProfileId.Equals(profileId) && x.GroupId.Equals(id)))
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    #endregion // Join Requests

    #region Membership
    public async Task<Page<Member>> GetMembersPageAsync(string id, PageRequest<Member> request, string? profileId = null, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Members)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderByDescending(x => x.JointAtUtc)
                : query.OrderBy(x => x.JointAtUtc);

        query = orderQuery
            .Include(x => x.Profile)
                .ThenInclude(x => x.Friends.Where(x => x.FriendId.Equals(profileId) || x.ProfileId.Equals(profileId)))
            .Include(x => x.Profile)
                .ThenInclude(x => x.FollowedBy.Where(x => x.FollowerId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountMembersAsync(id, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountMembersAsync(string id, Expression<Func<Member, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Members)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<Group?> GetWithMemebershipAsync(string id, string memberId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Members.Where(x => x.ProfileId.Equals(memberId) && x.GroupId.Equals(id)))
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    #endregion // Membership

    #region Kicked
    public async Task<Page<Kicked>> GetKickedPageAsync(string id, PageRequest<Kicked> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Kicked)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderByDescending(x => x.KickedAtUtc)
                : query.OrderBy(x => x.KickedAtUtc);

        query = orderQuery
            .Include(x => x.Profile)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountKickedAsync(id, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountKickedAsync(string id, Expression<Func<Kicked, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Kicked)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<Group?> GetWithKickedAsync(string id, string profileId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Kicked.Where(x => x.ProfileId.Equals(profileId) && x.GroupId.Equals(id)))
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    #endregion // Kicked
}
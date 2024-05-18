using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class PostEfRepository : EfRepository<Post, string>, IPostRepository
{
    public PostEfRepository(ApplicationDbContext context) : base(context) { }

    #region Hidden
    public async Task<Page<Post>> GetHiddenPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(request.Predicate ?? (_ => true))
            .Include(x => x.HiddenBy)
            .Where(x => x.HiddenBy.Any(x => x.Id.Equals(profileId)));

        var orderQuery = request.Desc
                ? query.OrderByDescending(request.KeySelector ?? (x => x.CreatedAtUtc))
                : query.OrderBy(request.KeySelector ?? (x => x.CreatedAtUtc));

        query = orderQuery
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountHiddenAsync(profileId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountHiddenAsync(string profileId, Expression<Func<Post, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(predicate ?? (_ => true))
            .Include(x => x.HiddenBy)
            .SelectMany(x => x.HiddenBy.Where(x => x.Id.Equals(profileId)))
            .CountAsync(cancellationToken);
    }

    public Task<Post?> GetWithHiddenAsync(string postId, string profileId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.HiddenBy.Where(x => x.Id.Equals(profileId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(postId), cancellationToken);
    }
    #endregion // Hidden

    #region Reaction
    public async Task<Page<Reaction>> GetReactionsPageAsync(string postId, PageRequest<Reaction> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(postId))
            .SelectMany(x => x.Reactions)
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

        var total = await CountReactionsAsync(postId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountReactionsAsync(string postId, Expression<Func<Reaction, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return Queryable
            .SelectMany(x => x.Reactions)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    
    public Task<Post?> GetWithReactionAsync(string postId, string profileId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId) && x.PostId.Equals(postId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(postId), cancellationToken);
    }
    #endregion // Reaction
}
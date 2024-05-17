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
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class PostEfRepository : EfRepository<Post, string>, IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostEfRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Page<Post>> GetFollowedPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default)
    {
        var query = _context.Profiles
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.Following)
                .Select(x => x.Followed)
                .SelectMany(x => x.Posts)
                    .Where(request.Predicate ?? (_ => true))
                    .Include(x => x.HiddenBy.Where(x => x.Id.Equals(profileId)))
                    .Where(x => x.HiddenBy.Count == 0)
                    .Where(x => x.Visibility.Equals(PostVisibilities.Public) && x.GroupId == null);

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        return Task.FromResult(new Page<Post>(query.ToList(), 0));
    }

    public Task<Page<Post>> GetFriendsPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default)
    {
        var query = _context.Friendships
            .AsNoTracking()
            .Where(x => x.ProfileId.Equals(profileId))
                .Select(x => x.Friend)
                .SelectMany(x => x.Posts)
                    .Where(request.Predicate ?? (_ => true))
                    .Include(x => x.HiddenBy.Where(x => x.Id.Equals(profileId)))
                    .Where(x => x.HiddenBy.Count == 0)
                    .Where(x => !x.Visibility.Equals(PostVisibilities.Private) && x.GroupId == null);

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        return Task.FromResult(new Page<Post>(query.ToList(), 0));
    }

    public Task<Page<Post>> GetSavedPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default)
    {
        var query = _context.Profiles
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.SavedPosts)
                .Select(x => x.Post)
                    .Where(request.Predicate ?? (_ => true))
                    .Where(x => !x.Visibility.Equals(PostVisibilities.Private) && x.GroupId == null);

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        return Task.FromResult(new Page<Post>(query.ToList(), 0));
    }

    public Task<Page<Post>> GetProfilePostsPageAsync(string profileId,
        PageRequest<Post> request,
        PostVisibilities includingVisibility = PostVisibilities.Public,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Profiles
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.Posts)
            .Where(request.Predicate ?? (_ => true))
            .Where(x => x.Visibility <= includingVisibility && x.GroupId == null);

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        return Task.FromResult(new Page<Post>(query.ToList(), 0));
    }

    #region Hidden
    public async Task<Page<Post>> GetHiddenPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(request.Predicate ?? (_ => true))
            .Include(x => x.HiddenBy)
            .Where(x => x.HiddenBy.Any(x => x.Id.Equals(profileId)) && x.GroupId == null);

        var orderQuery = request.Desc
                ? query.OrderByDescending(request.KeySelector ?? (x => x.CreatedAtUtc))
                : query.OrderBy(request.KeySelector ?? (x => x.CreatedAtUtc));

        query = orderQuery
            .Include(x => x.Profile)
            .Include(x => x.Media)
            .Include(x => x.Reactions.Where(x => x.ProfileId.Equals(profileId)))
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
            .Include(x => x.Profile)
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

    #region Comment
    public async Task<Page<Comment>> GetCommentsPageAsync(string postId, string? parentId, PageRequest<Comment> request, CancellationToken cancellationToken = default)
    {
        var postQuery = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(postId));

        var query = parentId is null
            ? postQuery
                .SelectMany(x => x.Comments)
                .Where(request.Predicate ?? (_ => true))
            : postQuery
                .Include(x => x.Comments)
                .SelectMany(x => x.Comments.Where(x => x.Id.Equals(parentId)))
                .SelectMany(x => x.Replies)
                .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.Desc
                ? query.OrderByDescending(request.KeySelector ?? (x => x.CreatedAtUtc))
                : query.OrderBy(request.KeySelector ?? (x => x.CreatedAtUtc));

        query = orderQuery
            .Include(x => x.Profile)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountCommentsAsync(postId, parentId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountCommentsAsync(string postId, string? parentId, Expression<Func<Comment, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var postQuery = Queryable
            .Where(x => x.Id.Equals(postId));

        var query = parentId is null
            ? postQuery.SelectMany(x => x.Comments)
            : postQuery
                .Include(x => x.Comments)
                .SelectMany(x => x.Comments.Where(x => x.Id.Equals(parentId)))
                .SelectMany(x => x.Replies);

        return query.CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<Post?> GetWithCommentAsync(string postId, string commentId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Comments.Where(x => x.Id.Equals(commentId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(postId), cancellationToken);
    }

    public Task<Post?> GetWithReplyAsync(string postId, string commentId, string replyId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Comments.Where(x => x.Id.Equals(commentId)))
                .ThenInclude(x => x.Replies.Where(x => x.Id.Equals(replyId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(postId), cancellationToken);
    }
    #endregion // Comment
}
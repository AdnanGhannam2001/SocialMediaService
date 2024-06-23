using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Persistent.Interfaces;

public interface IPostRepository
    : IWriteRepository<Post, string>, IReadRepository<Post, string>
{
    Task<Page<Post>> GetFollowedPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default);
    Task<Page<Post>> GetFriendsPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default);
    Task<Page<Post>> GetSavedPostsPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default);
    Task<Page<Post>> GetProfilePostsPageAsync(string profileId,
        PageRequest<Post> request,
        PostVisibilities includingVisibility = PostVisibilities.Public,
        CancellationToken cancellationToken = default);

    #region Hidden
    Task<Page<Post>> GetHiddenPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default);
    Task<int> CountHiddenAsync(string profileId, Expression<Func<Post, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task<Post?> GetWithHiddenAsync(string postId, string profileId, CancellationToken cancellationToken = default);
    #endregion // Hidden

    #region Reaction
    Task<Page<Reaction>> GetReactionsPageAsync(string postId, PageRequest<Reaction> request, CancellationToken cancellationToken = default);
    Task<int> CountReactionsAsync(string postId, Expression<Func<Reaction, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    Task<Post?> GetWithReactionAsync(string postId, string profileId, CancellationToken cancellationToken = default);
    #endregion // Reaction

    #region Comment
    Task<Page<Comment>> GetCommentsPageAsync(string postId, string? parentId, PageRequest<Comment> request, CancellationToken cancellationToken = default);
    Task<int> CountCommentsAsync(string postId, string? parentId, Expression<Func<Comment, bool>>? predicate = null, CancellationToken cancellationToken = default);
    
    Task<Post?> GetWithCommentAsync(string postId, string commentId, CancellationToken cancellationToken = default);
    Task<Post?> GetWithReplyAsync(string postId, string commentId, string replyId, CancellationToken cancellationToken = default);
    #endregion // Comment
}
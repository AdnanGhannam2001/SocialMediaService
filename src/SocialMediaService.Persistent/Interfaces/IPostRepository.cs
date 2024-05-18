using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Persistent.Interfaces;

public interface IPostRepository
    : IWriteRepository<Post, string>, IReadRepository<Post, string>
{
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
}
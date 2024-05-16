using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Persistent.Interfaces;

public interface IPostRepository
    : IWriteRepository<Post, string>, IReadRepository<Post, string>
{
    Task<Page<Post>> GetHiddenPageAsync(string profileId, PageRequest<Post> request, CancellationToken cancellationToken = default);
    Task<int> CountHiddenAsync(string profileId, Expression<Func<Post, bool>>? predicate = null, CancellationToken cancellationToken = default);
}
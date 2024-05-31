using System.Linq.Expressions;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Persistent.Interfaces;

public interface IGroupRepository
    : IWriteRepository<Group, string>, IReadRepository<Group, string>
{
    #region Join Requests
    Task<Page<JoinRequest>> GetJoinRequestsPageAsync(string id, PageRequest<JoinRequest> request, CancellationToken cancellationToken = default);
    Task<int> CountJoinRequestsAsync(string id, Expression<Func<JoinRequest, bool>>? predicate, CancellationToken cancellationToken = default);

    Task<Group?> GetWithJoinRequestAsync(string id, string profileId, CancellationToken cancellationToken = default);
    #endregion // Join Requests

    #region Membership
    Task<Page<Member>> GetMembersPageAsync(string id, PageRequest<Member> request, CancellationToken cancellationToken = default);
    Task<int> CountMembersAsync(string id, Expression<Func<Member, bool>>? predicate, CancellationToken cancellationToken = default);

    Task<Group?> GetWithMemebershipAsync(string id, string memberId, CancellationToken cancellationToken = default);
    #endregion // Membership

    #region Kicked
    Task<Page<Kicked>> GetKickedPageAsync(string id, PageRequest<Kicked> request, CancellationToken cancellationToken = default);
    Task<int> CountKickedAsync(string id, Expression<Func<Kicked, bool>>? predicate, CancellationToken cancellationToken = default);

    Task<Group?> GetWithKickedAsync(string id, string profileId, CancellationToken cancellationToken = default);
    #endregion // Kicked

    #region Discussion
    Task<Page<Discussion>> GetDiscussionsPageAsync(string id, PageRequest<Discussion> request, CancellationToken cancellationToken = default);
    Task<int> CountDiscussionsAsync(string id, Expression<Func<Discussion, bool>>? predicate, CancellationToken cancellationToken = default);

    Task<Group?> GetWithDiscussionAsync(string id, string discussionId, CancellationToken cancellationToken = default);
    #endregion // Discussion
}
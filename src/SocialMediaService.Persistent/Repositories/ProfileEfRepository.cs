using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PR2.Shared.Common;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class ProfileEfRepository
    : EfRepository<Profile, string>, IProfileRepository
{
    public ProfileEfRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Page<Profile>> GetPageWithRelationsAsync(PageRequest<Profile> request,
        string? profileId = null,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        // TODO exclude blocked
        query = orderQuery
            .Include(x => x.Friends.Where(x => x.FriendId.Equals(profileId) || x.ProfileId.Equals(profileId)))
            .Include(x => x.FollowedBy.Where(x => x.FollowerId.Equals(profileId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountAsync(request.Predicate ?? (_ => true), cancellationToken);

        return new (query.ToList(), total);
    }

    #region Settings
    public Task<Profile?> GetWithSettingsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }
    #endregion // Settings

    #region Friendship
    public async Task<Page<Friendship>> GetFriendshipsPageAsync(string profileId,
        PageRequest<Friendship> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .SelectMany(x => x.Friends)
            .Where(x => x.FriendId.Equals(profileId) || x.ProfileId.Equals(profileId))
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Friend)
                .ThenInclude(x => x.FollowedBy.Where(x => x.FollowerId.Equals(profileId)))
            .Include(x => x.Friend)
                .ThenInclude(x => x.ReceivedRequests.Where(x => x.SenderId.Equals(profileId)))
            .Include(x => x.Profile)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFriendshipsAsync(profileId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<Profile?> GetWithFriendshipAsync(string profileId, string friendId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Friends.Where(
                x => x.ProfileId.Equals(profileId) && x.FriendId.Equals(friendId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(profileId), cancellationToken);
    }

    public Task<int> CountFriendshipsAsync(string profileId,
        Expression<Func<Friendship, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.Friends)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Friendship Requests
    public async Task<Page<FriendshipRequest>> GetSentFriendshipRequestsPageAsync(string profileId,
        PageRequest<FriendshipRequest> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
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
            .Include(x => x.Receiver)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountSentFriendshipsRequestAsync(profileId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public async Task<Page<FriendshipRequest>> GetReceivedFriendshipRequestsPageAsync(string profileId,
        PageRequest<FriendshipRequest> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
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
            .Include(x => x.Sender)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountReceivedFriendshipsRequestAsync(profileId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<Profile?> GetWithFriendshipRequestAsync(string senderId, string receiverId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.SentRequests.Where(
                x => x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(senderId), cancellationToken: cancellationToken);
    }

    public Task<int> CountSentFriendshipsRequestAsync(string profileId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.SentRequests)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public Task<int> CountReceivedFriendshipsRequestAsync(string profileId,
        Expression<Func<FriendshipRequest, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.ReceivedRequests)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Block
    public Task<Profile?> GetWithBlockedAsync(string blockerId, string blockedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Blocked.Where(
                x => x.BlockerId.Equals(blockerId) && x.BlockedId.Equals(blockedId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(blockerId), cancellationToken);
    }

    public async Task<Page<Block>> GetBlockedPageAsync(string blockerId,
        PageRequest<Block> request,
        CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(blockerId))
            .SelectMany(x => x.Blocked)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Blocked)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountBlockedAsync(blockerId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountBlockedAsync(string blockerId,
        Expression<Func<Block, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(blockerId))
            .SelectMany(x => x.Blocked)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Follow
    public Task<Profile?> GetWithFollowingAsync(string followerId, string followedId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.Following.Where(
                x => x.FollowerId.Equals(followerId) && x.FollowedId.Equals(followedId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(followerId), cancellationToken);
    }

    public async Task<Page<Follow>> GetFollowingPageAsync(string followerId, PageRequest<Follow> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(followerId))
            .SelectMany(x => x.Following)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Followed)
                .ThenInclude(x => x.Friends.Where(x => x.FriendId.Equals(followerId) || x.ProfileId.Equals(followerId)))
            .Include(x => x.Followed)
                .ThenInclude(x => x.SentRequests.Where(x => x.SenderId.Equals(followerId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFollowingAsync(followerId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountFollowingAsync(string followerId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(followerId))
            .SelectMany(x => x.Following)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }

    public async Task<Page<Follow>> GetFollowedPageAsync(string followedId, PageRequest<Follow> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(followedId))
            .SelectMany(x => x.FollowedBy)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Follower)
                .ThenInclude(x => x.Friends.Where(x => x.FriendId.Equals(followedId) || x.ProfileId.Equals(followedId)))
            .Include(x => x.Follower)
                .ThenInclude(x => x.ReceivedRequests.Where(x => x.SenderId.Equals(followedId)))
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountFollowedAsync(followedId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountFollowedAsync(string followedId,
        Expression<Func<Follow, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(followedId))
            .SelectMany(x => x.FollowedBy)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion

    #region Invite
    public Task<Profile?> GetWithInviteAsync(string profileId, string groupId, string senderId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.ReceivedInvites.Where(x => x.GroupId.Equals(groupId) && x.ProfileId.Equals(profileId) && x.SenderId.Equals(senderId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(profileId), cancellationToken);
    }

    public async Task<Page<Invite>> GetInvitesPageAsync(string profileId, PageRequest<Invite> request, CancellationToken cancellationToken = default)
    {
        var query = Queryable
            .AsNoTracking()
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.ReceivedInvites)
            .Where(request.Predicate ?? (_ => true));

        var orderQuery = request.KeySelector is not null
            ? request.Desc
                ? query.OrderByDescending(request.KeySelector)
                : query.OrderBy(request.KeySelector)
            : request.Desc
                ? query.OrderDescending()
                : query.Order();

        query = orderQuery
            .Include(x => x.Sender)
            .Include(x => x.Group)
            .Skip(request.PageNumber * request.PageSize)
            .Take(request.PageSize);

        var total = await CountInvitesAsync(profileId, request.Predicate, cancellationToken);

        return new (query.ToList(), total);
    }

    public Task<int> CountInvitesAsync(string profileId,
        Expression<Func<Invite, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id.Equals(profileId))
            .SelectMany(x => x.ReceivedInvites)
            .CountAsync(predicate ?? (_ => true), cancellationToken);
    }
    #endregion // Invite

    #region Favorite Discussion
    public Task<Profile?> GetWithSavedPostAsync(string profileId, string discussionId, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Include(x => x.SavedPosts.Where(x => x.ProfileId.Equals(profileId) && x.PostId.Equals(discussionId)))
            .FirstOrDefaultAsync(x => x.Id.Equals(profileId), cancellationToken);
    }
    #endregion // Favorite Discussion
}
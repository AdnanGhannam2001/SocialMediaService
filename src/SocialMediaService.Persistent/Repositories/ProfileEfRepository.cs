using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Persistent.Repositories;

public sealed class ProfileEfRepository
    : EfRepository<Profile, string>, IProfileRepository
{
    public ProfileEfRepository(ApplicationDbContext context) : base(context) { }

    public Task<Settings?> GetSettingsAsync(string id, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id == id)
            .Select(x => x.Settings)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Friendship?> GetFriendshipAsync(string id1, string id2, CancellationToken cancellationToken = default)
    {
        return Queryable
            .Where(x => x.Id == id1)
            .SelectMany(x => x.Friends)
            .Where(x => x.FriendId == id2)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
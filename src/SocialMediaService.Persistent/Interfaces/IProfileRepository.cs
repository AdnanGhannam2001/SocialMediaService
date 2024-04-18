using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Interfaces;

public interface IProfileRepository
    : IWriteRepository<Profile, string>, IReadRepository<Profile, string>
{
    Task<Settings?> GetSettingsAsync(string id, CancellationToken cancellationToken = default);

    Task<Friendship?> GetFriendshipAsync(string id1, string id2, CancellationToken cancellationToken = default);
}
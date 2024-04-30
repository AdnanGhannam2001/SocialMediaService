using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Helpers;

internal static class ProfileHelper
{
    public static async Task<bool> IsBlocked(IProfileRepository repo,
        string id1,
        string id2,
        CancellationToken cancellationToken = default)
    {
        var blocked = await repo.GetBlockedAsync(id1, id2, cancellationToken);
        var blockedBy = await repo.GetBlockedAsync(id2, id1, cancellationToken);

        return blocked is not null || blockedBy is not null;
    }
}
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Domain.Enums;
using SocialMediaService.Persistent.Interfaces;

namespace SocialMediaService.Application.Helpers;

internal static class PostHelper
{
    public static async Task<bool> CanAccessPostAsync(IProfileRepository profileRepo,
        Profile profile,
        Post post,
        CancellationToken cancellationToken = default)
    {
        if (await ProfileHelper.IsBlocked(profileRepo, profile.Id, post.ProfileId, cancellationToken))
        {
            return false;
        }

        if (post.Visibility == PostVisibilities.Private && profile.Id != post.ProfileId)
        {
            return false;
        }

        if (post.Visibility == PostVisibilities.Friends)
        {
            var publisher = await profileRepo.GetWithFriendshipAsync(profile.Id, post.ProfileId, cancellationToken);

            if (publisher is null || publisher.Friends.Count == 0)
            {
                return false;
            }
        }

        // TODO: Add Group membership check
        // ...

        return true;
    }
}
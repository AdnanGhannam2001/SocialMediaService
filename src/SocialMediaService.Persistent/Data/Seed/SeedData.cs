using Microsoft.EntityFrameworkCore;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Data.Seed;

public static partial class SeedData
{
    private const int FriendshipsCount = 20;
    private const int FollowsCount = 30;
    private const int PostsCount = 50;
    private const int CommentsCount = 100;
    private const int ReactionsCount = 100;
    private static readonly Random Random = new();
    private static readonly IList<Profile> Profiles = [];

    public static async Task ApplyAsync(ApplicationDbContext context)
    {
        await ClearAsync(context);

        foreach (var profile in context.Profiles)
        {
            Profiles.Add(profile);
        }

        await SeedProfilesAsync(context);
        await SeedPostsAsync(context);
    }

    private static async Task ClearAsync(ApplicationDbContext context)
    {
        await context.Friendships.ExecuteDeleteAsync();
        await context.Follows.ExecuteDeleteAsync();
        await context.Posts.ExecuteDeleteAsync();
        await context.Reactions.ExecuteDeleteAsync();
        await context.Comments.ExecuteDeleteAsync();
    }
}
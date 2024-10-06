using MassTransit;
using Microsoft.EntityFrameworkCore;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Infrastructure.Services;

namespace SocialMediaService.Persistent.Data.Seed;

public static partial class SeedData
{
    private const int FriendshipsCount = 20;
    private const int FollowsCount = 30;
    private const int PostsCount = 50;
    private const int CommentsCount = 100;
    private const int ReactionsCount = 100;
    private const int GroupsCount = 50;
    private static IList<Profile> Profiles = [];

    public static async Task ApplyAsync(ApplicationDbContext context, IPublishEndpoint messagePublisher, bool clear = true)
    {
        if (clear) await ClearAsync(context);

        Profiles = await context.Profiles.ToListAsync();

        if (!await context.Friendships.AnyAsync() && !await context.Follows.AnyAsync())
        {
            await SeedProfilesAsync(context, messagePublisher);
        }

        if (!await context.Posts.AnyAsync())
        {
            await SeedPostsAsync(context);
        }

        if (!await context.Groups.AnyAsync())
        {
            await SeedGroupsAsync(context, messagePublisher);
        }
    }

    private static async Task ClearAsync(ApplicationDbContext context)
    {
        if (await context.Friendships.AnyAsync()) await context.Friendships.ExecuteDeleteAsync();
        if (await context.Follows.AnyAsync()) await context.Follows.ExecuteDeleteAsync();
        if (await context.Posts.AnyAsync()) await context.Posts.ExecuteDeleteAsync();
        if (await context.Reactions.AnyAsync()) await context.Reactions.ExecuteDeleteAsync();
        if (await context.Comments.AnyAsync()) await context.Comments.ExecuteDeleteAsync();
        if (await context.Groups.AnyAsync()) await context.Groups.ExecuteDeleteAsync();
    }
}
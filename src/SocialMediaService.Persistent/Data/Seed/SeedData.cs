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

    public static async Task ApplyAsync(ApplicationDbContext context, IPublishEndpoint messagePublisher, bool clear = false)
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
        await context.Friendships.ExecuteDeleteAsync();
        await context.Follows.ExecuteDeleteAsync();
        await context.Posts.ExecuteDeleteAsync();
        await context.Reactions.ExecuteDeleteAsync();
        await context.Comments.ExecuteDeleteAsync();
        await context.Groups.ExecuteDeleteAsync();
    }
}
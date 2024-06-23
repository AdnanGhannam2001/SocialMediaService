using Bogus;
using MassTransit;
using PR2.Contracts.Events;
using SocialMediaService.Domain.Aggregates.Profiles;
using SocialMediaService.Infrastructure.Services;

namespace SocialMediaService.Persistent.Data.Seed;

public static partial class SeedData
{
    private static async Task SeedProfilesAsync(ApplicationDbContext context, IPublishEndpoint messagePublisher)
    {
        for (var i = 0; i < FriendshipsCount && i < Profiles.Count; ++i)
        {
            var friendshipFaker = new Faker<Friendship>()
                .RuleFor(x => x.Profile, _ => Profiles[i])
                .RuleFor(x => x.Friend, f => f.PickRandom(Profiles))
                .RuleFor(x => x.StartedAtUtc, _ => DateTime.UtcNow);

            var friendship = friendshipFaker.Generate();

            if (friendship.Profile.Id != friendship.Friend.Id)
            {
                Profiles[i].AddFriend(friendship);
                var message = new FriendshipCreatedEvent(friendship.Profile.Id, friendship.Friend.Id);
                await messagePublisher.Publish(message);
            }
        }

        for (var i = 0; i < FollowsCount && i < Profiles.Count; ++i)
        {
            var followFaker = new Faker<Follow>()
                .RuleFor(x => x.Follower, _ => Profiles[i])
                .RuleFor(x => x.Followed, f => f.PickRandom(Profiles))
                .RuleFor(x => x.FollowedAtUtc, _ => DateTime.UtcNow);

            var follow = followFaker.Generate();

            if (follow.Follower.Id != follow.Followed.Id) Profiles[i].AddFollow(follow);
        }

        await context.SaveChangesAsync();
    }
}
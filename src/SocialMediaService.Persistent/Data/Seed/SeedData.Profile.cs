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
                friendship.Profile.AddFriend(friendship);
                friendship.Friend.AddFriend(new Friendship(friendship.Friend, friendship.Profile));
                var message = new FriendshipCreatedEvent(friendship.Profile.Id, friendship.Friend.Id);
                await messagePublisher.Publish(message);

                var notification = new NotifyEvent(friendship.Profile.Id,
                    $"{friendship.Friend.FirstName} accepted your friendship request",
                    $"profiles/{friendship.Friend.Id}");
                await messagePublisher.Publish(notification);
            }
        }

        for (var i = 0; i < FollowsCount && i < Profiles.Count; ++i)
        {
            var followFaker = new Faker<Follow>()
                .RuleFor(x => x.Follower, _ => Profiles[i])
                .RuleFor(x => x.Followed, f => f.PickRandom(Profiles))
                .RuleFor(x => x.FollowedAtUtc, _ => DateTime.UtcNow);

            var follow = followFaker.Generate();

            if (follow.Follower.Id != follow.Followed.Id)
            {
                follow.Follower.AddFollow(follow);

                var notification = new NotifyEvent(follow.Followed.Id,
                    $"You got followed by {follow.Follower.Id}",
                    $"profiles/{follow.Follower.Id}");
                await messagePublisher.Publish(notification);
            }
        }

        await context.SaveChangesAsync();
    }
}
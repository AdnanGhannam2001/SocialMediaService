using Bogus;
using MassTransit;
using PR2.Contracts.Events;
using PR2.Shared.Enums;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Groups.ValueObjects;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Persistent.Data.Seed;

public static partial class SeedData
{
    private static async Task SeedGroupsAsync(ApplicationDbContext context, IPublishEndpoint messagePublisher)
    {
        var random = new Random();

        var groupFaker = new Faker<Group>()
            .RuleFor(x => x.Name, f => f.Commerce.Department())
            .RuleFor(x => x.Description, f => f.Random.Words(30))
            .RuleFor(x => x.Visibility, f => f.PickRandom<GroupVisibilities>())
            .RuleFor(x => x.Settings, _ => new GroupSettings());

        var groups = groupFaker.Generate(GroupsCount);
        
        foreach (var group in groups)
        {
            // Create Admin for each Group
            var admin = Profiles[random.Next(0, Profiles.Count)];
            group.AddMember(new Member(group, admin, MemberRoleTypes.Admin));
            await context.AddAsync(group);
            var message = new GroupCreatedEvent(group.Id, admin.Id);
            await messagePublisher.Publish(message);
        }

        var memberFaker = new Faker<Member>()
            .RuleFor(x => x.Profile, f => f.PickRandom(Profiles))
            .RuleFor(x => x.Role, f => f.PickRandom<MemberRoleTypes>());

        foreach (var group in groups)
        {
            var members = memberFaker.Generate(random.Next(1, Profiles.Count));

            foreach (var member in members)
            {
                if (!group.Members.Any(x => x.Profile.Id.Equals(member.Profile.Id)))
                {
                    group.AddMember(member);
                    var message = new MemberJoinedEvent(group.Id, member.Profile.Id, Enum.GetName(member.Role)!);
                    await messagePublisher.Publish(message);
                }
            }
        }

        await context.SaveChangesAsync();

        var postFaker = new Faker<Post>()
            .RuleFor(x => x.Content, f => f.Hacker.Phrase())
            .RuleFor(x => x.Visibility, f => f.PickRandomWithout(PostVisibilities.Private, PostVisibilities.Friends));

        foreach (var group in groups)
        {
            var posts = postFaker.Generate(random.Next(0, 20));

            var membersCount = group.Members.Count;
            foreach (var fpost in posts)
            {
                var post = new Post(group.Members.ElementAt(random.Next(0, membersCount)).Profile, group, fpost.Content, fpost.Visibility);
                group.AddPost(post);
            }
        }

        await context.SaveChangesAsync();
    }
}
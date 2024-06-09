using Bogus;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Enums;

namespace SocialMediaService.Persistent.Data.Seed;

public static partial class SeedData
{
    private static async Task SeedPostsAsync(ApplicationDbContext context)
    {
        var postFaker = new Faker<Post>()
            .RuleFor(x => x.Profile, f => f.PickRandom(Profiles))
            .RuleFor(x => x.Content, f => f.Hacker.Phrase())
            .RuleFor(x => x.Visibility, f => f.PickRandom<PostVisibilities>());

        var posts = postFaker.Generate(PostsCount);
        context.AddRange(posts);
        await context.SaveChangesAsync();

        var commentFaker = new Faker<Comment>()
            .RuleFor(x => x.Post, f => f.PickRandom(posts))
            .RuleFor(x => x.Profile, f => f.PickRandom(Profiles))
            .RuleFor(x => x.Content, f => f.Hacker.Phrase());

        foreach (var comment in commentFaker.Generate(CommentsCount))
        {
            comment.Post?.AddComment(comment);
        }

        foreach (var post in posts)
        {
            for (var i = 0; i < ReactionsCount && i < Profiles.Count; ++i)
            {
                var reactionFaker = new Faker<Reaction>()
                    .RuleFor(x => x.Post, f => post)
                    .RuleFor(x => x.Profile, _ => Profiles[i])
                    .RuleFor(x => x.Type, f => f.PickRandom<ReactionTypes>());

                var reaction = reactionFaker.Generate();

                reaction.Post?.React(reaction);
            }
        }

        await context.SaveChangesAsync();
    }
}
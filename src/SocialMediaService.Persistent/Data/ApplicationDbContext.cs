using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SocialMediaService.Domain.Aggregates.Groups;
using SocialMediaService.Domain.Aggregates.Posts;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Settings> SettingsList { get; set; }
    public DbSet<FriendshipRequest> FriendshipRequests { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<FavoriteDiscussion> FavoriteDiscussions { get; set; }
    public DbSet<Block> Blocks { get; set; }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Group> Groups { get; set; }
    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<JoinRequest> JoinRequests { get; set; }
    public DbSet<Kicked> KickedList { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }
}
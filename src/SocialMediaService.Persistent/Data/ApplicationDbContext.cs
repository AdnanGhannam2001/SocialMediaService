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
    public DbSet<SavedPost> SavedPosts { get; set; }
    public DbSet<Block> Blocks { get; set; }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Group> Groups { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<JoinRequest> JoinRequests { get; set; }
    public DbSet<Kicked> KickedList { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Relationships Key
        {
            modelBuilder.Entity<Invite>().HasKey(x => new { x.SenderId, x.ProfileId, x.GroupId });
            modelBuilder.Entity<JoinRequest>().HasKey(x => new { x.ProfileId, x.GroupId });
            modelBuilder.Entity<Kicked>().HasKey(x => new { x.ProfileId, x.KickedById, x.GroupId });
            modelBuilder.Entity<Member>().HasKey(x => new { x.ProfileId, x.GroupId });

            modelBuilder.Entity<Reaction>().HasKey(x => new { x.PostId, x.ProfileId });

            modelBuilder.Entity<FriendshipRequest>().HasKey(x => new { x.SenderId, x.ReceiverId });
            modelBuilder.Entity<Friendship>().HasKey(x => new { x.FriendId, x.ProfileId });
            modelBuilder.Entity<Follow>().HasKey(x => new { x.FollowedId, x.FollowerId });
            modelBuilder.Entity<SavedPost>().HasKey(x => new { x.ProfileId, x.PostId });
            modelBuilder.Entity<Block>().HasKey(x => new { x.BlockedId, x.BlockerId });
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
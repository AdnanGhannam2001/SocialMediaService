using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Persistent.Data.Configurations;

internal sealed class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(30);

        builder
            .Property(x => x.LastName)
            .HasMaxLength(30);

        builder.HasQueryFilter(x => x.IsActive);

        builder
            .Property(x => x.Bio)
            .HasMaxLength(500);

        builder.OwnsOne(x => x.PhoneNumber);

        builder.OwnsOne(
            x => x.JobInformations,
            b =>
            {
                b.Property(x => x.JobTitle).HasMaxLength(50);
                b.Property(x => x.Company).HasMaxLength(50);
            });

        builder.OwnsOne(
            x => x.Socials,
            b =>
            {
                b.Property(x => x.Facebook).HasMaxLength(25);
                b.Property(x => x.Youtube).HasMaxLength(25);
                b.Property(x => x.Twitter).HasMaxLength(25);
            });

        builder.HasOne(x => x.Settings)
            .WithOne(x => x.Profile)
            .HasForeignKey<Settings>(x => x.Id)
            .IsRequired();

        builder.HasMany(x => x.Blocked)
            .WithOne(x => x.Blocker)
            .HasForeignKey(x => x.BlockerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlockedBy)
            .WithOne(x => x.Blocked)
            .HasForeignKey(x => x.BlockedId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SentRequests)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReceivedRequests)
            .WithOne(x => x.Receiver)
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Following)
            .WithOne(x => x.Follower)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.FollowedBy)
            .WithOne(x => x.Followed)
            .HasForeignKey(x => x.FollowedId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Friends)
            .WithOne(x => x.Profile)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.FriendTo)
            .WithOne(x => x.Friend)
            .HasForeignKey(x => x.FriendId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Posts)
            .WithOne(x => x.Profile)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Reactions)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MemberOf)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Kicked)
           .WithOne(x => x.KickedBy)
           .HasForeignKey(x => x.KickedById)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.KickedFrom)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.JoinRequests)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SentInvites)
           .WithOne(x => x.Sender)
           .HasForeignKey(x => x.SenderId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReceivedInvites)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SavedPosts)
           .WithOne(x => x.Profile)
           .HasForeignKey(x => x.ProfileId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Hidden)
            .WithMany(x => x.HiddenBy)
            .UsingEntity("HiddenPosts");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaService.Domain.Aggregates.Posts;

namespace SocialMediaService.Persistent.Data.Configurations;

internal sealed class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasMany(x => x.Reactions)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.Media, b => b.ToTable("PostsMedia"));
    }
}
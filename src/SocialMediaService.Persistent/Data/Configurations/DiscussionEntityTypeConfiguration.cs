using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Persistent.Data.Configurations;

internal sealed class DiscussionEntityTypeConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Favorites)
            .WithOne(x => x.Discussion)
            .HasForeignKey(x => x.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Discussion)
            .HasForeignKey(x => x.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Discussions)
            .UsingEntity("DiscussionsTags");
    }
}

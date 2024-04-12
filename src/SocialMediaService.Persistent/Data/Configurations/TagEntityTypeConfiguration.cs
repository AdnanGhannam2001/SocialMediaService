using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaService.Domain.Aggregates.Groups;

namespace SocialMediaService.Persistent.Data.Configurations;

internal sealed class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Label)
            .HasMaxLength(20);
    }
}


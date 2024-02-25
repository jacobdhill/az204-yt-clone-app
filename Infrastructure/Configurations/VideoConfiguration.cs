using Domain.Videos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder
            .ToContainer("videos")
            .HasKey(v => v.Id);

        builder
            .HasPartitionKey(v => v.Id);

        builder
            .Property(v => v.Id)
            .ToJsonProperty("id");

        builder
            .HasQueryFilter(v => v.IsDeleted == false);
    }
}

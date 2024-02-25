using Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .ToContainer("comments")
            .HasKey(c => c.Id);

        builder
            .HasPartitionKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .ToJsonProperty("id");
    }
}

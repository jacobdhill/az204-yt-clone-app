using Domain.EmailTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder
            .ToTable("EmailTemplate")
            .HasKey(m => m.EmailTemplateId);

        builder
            .Property(m => m.EmailTemplateId)
            .HasConversion(v => v.Value, v => new EmailTemplateId(v))
            .ValueGeneratedOnAdd();
    }
}

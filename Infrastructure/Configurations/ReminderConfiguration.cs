using Domain.Reminders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder
            .ToTable("Reminder")
            .HasKey(m => m.ReminderId);

        builder
            .Property(m => m.ReminderId)
            .HasConversion(v => v.Value, v => new ReminderId(v))
            .ValueGeneratedOnAdd();
    }
}

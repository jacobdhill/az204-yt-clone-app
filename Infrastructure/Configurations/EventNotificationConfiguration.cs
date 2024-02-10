using Domain.EmailTemplates;
using Domain.EventNotifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EventNotificationConfiguration : IEntityTypeConfiguration<EventNotification>
{
    public void Configure(EntityTypeBuilder<EventNotification> builder)
    {
        builder
            .ToTable("EventNotification")
            .HasKey(m => m.EventNotificationId);

        builder
            .Property(m => m.EventNotificationId)
            .HasConversion(v => v.Value, v => new EventNotificationId(v))
            .ValueGeneratedOnAdd();
    }
}

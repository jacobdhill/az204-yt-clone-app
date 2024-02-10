using System;

namespace Domain.EventNotifications;

public class EventNotification
{
    public EventNotificationId EventNotificationId { get; set; }
    public string DomainEvent { get; set; }
    public bool IsProcessed { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

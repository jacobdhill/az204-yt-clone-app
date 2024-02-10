using System;

namespace Domain.EventNotifications;

public class EventNotificationId
{
    public Guid Value { get; set; }

    public EventNotificationId(Guid value)
    {
        Value = value;
    }
}
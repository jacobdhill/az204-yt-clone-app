using System;

namespace Domain.Reminders;

public class ReminderId
{
    public Guid Value { get; set; }

    public ReminderId(Guid value)
    {
        Value = value;
    }
}

using System;

namespace Domain.Reminders;

public class ReminderNotFoundException : Exception
{
    public ReminderNotFoundException(ReminderId reminderId)
        : base($"Reminder with Id = '{reminderId.Value}' does not exist")
    {
    }
}

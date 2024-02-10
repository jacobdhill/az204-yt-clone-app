using System;

namespace Application.Reminders.List;

public class ReminderListDto
{
    public Guid ReminderId { get; set; }
    public string Description { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
}

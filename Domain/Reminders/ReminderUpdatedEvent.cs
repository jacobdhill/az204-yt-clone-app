using MediatR;

namespace Domain.Reminders;

public class ReminderUpdatedEvent : DomainEvent, IRequest<bool>
{
    public string Description { get; set; }
    public int OldNotifyDaysBefore { get; set; }
    public int OldDayOfMonth { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
}

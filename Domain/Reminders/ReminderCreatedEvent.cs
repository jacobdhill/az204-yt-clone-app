using MediatR;

namespace Domain.Reminders;

public class ReminderCreatedEvent : DomainEvent, IRequest<bool>
{
    public string Description { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
}

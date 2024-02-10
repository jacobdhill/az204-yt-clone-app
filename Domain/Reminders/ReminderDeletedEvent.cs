using MediatR;

namespace Domain.Reminders;

public class ReminderDeletedEvent : DomainEvent, IRequest<bool>
{
    public string Description { get; set; }
}

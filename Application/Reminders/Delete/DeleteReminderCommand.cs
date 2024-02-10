using Domain.Reminders;
using MediatR;

namespace Application.Reminders.Delete;

public class DeleteReminderCommand : IRequest
{
    public ReminderId ReminderId { get; set; }
}

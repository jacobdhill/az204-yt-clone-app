using Domain.Reminders;
using MediatR;

namespace Application.Reminders.Create;

public class DeleteReminderCommand : IRequest
{
    public ReminderId ReminderId { get; set; }
}

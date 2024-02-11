using Domain.Reminders;
using MediatR;

namespace Application.Reminders.List;

public class GetReminderQuery : IRequest<ReminderGetDto>
{
    public ReminderId ReminderId { get; set; }
}

using Domain.Reminders;
using MediatR;

namespace Application.Reminders.Update;

public class UpdateReminderCommand : IRequest
{
    public ReminderId ReminderId { get; set; }
    public string Description { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
}

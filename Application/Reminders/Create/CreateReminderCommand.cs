using MediatR;

namespace Application.Reminders.Create;

public class CreateReminderCommand : IRequest
{
    public string Description { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
}

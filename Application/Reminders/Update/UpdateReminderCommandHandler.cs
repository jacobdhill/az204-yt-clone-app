using Domain.Reminders;
using Infrastructure.Contexts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders.Create;

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateReminderCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _dbContext.Reminders.FindAsync(request.ReminderId);
        if (reminder == null)
        {
            throw new ReminderNotFoundException(request.ReminderId);
        }

        var reminderUpdatedEvent = new ReminderUpdatedEvent()
        {
            OldDayOfMonth = reminder.DayOfMonth,
            OldNotifyDaysBefore = reminder.NotifyDaysBefore,
            DayOfMonth = request.NotifyDaysBefore,
            NotifyDaysBefore = request.NotifyDaysBefore
        };

        reminder.DomainEvents.Add(reminderUpdatedEvent);

        reminder.DayOfMonth = request.DayOfMonth;
        reminder.NotifyDaysBefore = request.NotifyDaysBefore;
        reminder.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

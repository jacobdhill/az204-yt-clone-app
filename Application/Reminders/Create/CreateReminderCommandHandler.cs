using Domain.Reminders;
using Infrastructure.Contexts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders.Create;

public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateReminderCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = new Reminder()
        {
            ReminderId = new ReminderId(Guid.NewGuid()),
            Description = request.Description,
            NotifyDaysBefore = request.NotifyDaysBefore,
            DayOfMonth = request.DayOfMonth,
            CreatedDate = DateTime.UtcNow
        };

        var reminderCreatedEvent = new ReminderCreatedEvent()
        {
            Description = reminder.Description,
            NotifyDaysBefore = reminder.NotifyDaysBefore,
            DayOfMonth = reminder.DayOfMonth
        };
        reminder.DomainEvents.Add(reminderCreatedEvent);

        await _dbContext.Reminders.AddAsync(reminder, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

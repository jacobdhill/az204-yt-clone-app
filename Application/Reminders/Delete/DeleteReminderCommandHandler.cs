using Domain.Reminders;
using Infrastructure.Contexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders.Create;

public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteReminderCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _dbContext.Reminders.FindAsync(request.ReminderId);
        if (reminder == null)
        {
            throw new ReminderNotFoundException(request.ReminderId);
        }

        _dbContext.Remove(reminder);

        var reminderDeletedEvent = new ReminderDeletedEvent()
        {
            Description = reminder.Description
        };
        reminder.DomainEvents.Add(reminderDeletedEvent);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

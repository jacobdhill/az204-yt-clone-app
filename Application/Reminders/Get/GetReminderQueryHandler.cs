using Domain.Reminders;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders.List;

public class GetReminderQueryHandler : IRequestHandler<GetReminderQuery, ReminderGetDto>
{
    private readonly ApplicationDbContext _dbContext;

    public GetReminderQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReminderGetDto> Handle(GetReminderQuery request, CancellationToken cancellationToken)
    {
        var reminder = await _dbContext.Reminders
            .Where(reminder => reminder.ReminderId == request.ReminderId)
            .Select(reminder => new ReminderGetDto()
            {
                ReminderId = reminder.ReminderId.Value,
                DayOfMonth = reminder.DayOfMonth,
                Description = reminder.Description,
                NotifyDaysBefore = reminder.NotifyDaysBefore
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (reminder == null)
        {
            throw new ReminderNotFoundException(request.ReminderId);
        }

        return reminder;
    }
}

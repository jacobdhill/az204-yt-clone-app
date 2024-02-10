using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders.List;

public class ListReminderQueryHandler : IRequestHandler<ListReminderQuery, List<ReminderListDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public ListReminderQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ReminderListDto>> Handle(ListReminderQuery request, CancellationToken cancellationToken)
    {
        var reminders = await _dbContext.Reminders
            .Select(reminder => new ReminderListDto()
            {
                ReminderId = reminder.ReminderId.Value,
                DayOfMonth = reminder.DayOfMonth,
                Description = reminder.Description,
                NotifyDaysBefore = reminder.NotifyDaysBefore
            })
            .ToListAsync(cancellationToken);

        return reminders;
    }
}

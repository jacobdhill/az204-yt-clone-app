using MediatR;
using System.Collections.Generic;

namespace Application.Reminders.List;

public class ListReminderQuery : IRequest<List<ReminderListDto>>
{
}

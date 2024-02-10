using Domain.EmailTemplates;
using Domain.Reminders;
using Infrastructure.Contexts;
using Infrastructure.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders;

public class ReminderUpdatedEventHandler : IRequestHandler<ReminderUpdatedEvent, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailService _emailService;

    public ReminderUpdatedEventHandler(ApplicationDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ReminderUpdatedEvent request, CancellationToken cancellationToken)
    {
        var emailTemplate = await _dbContext.EmailTemplates
            .FirstOrDefaultAsync(template => template.Code == EmailTemplateConstants.ReminderUpdatedEvent);

        var emailTo = "jacob.d.hill@outlook.com";
        var subject = $"Reminder Updated - {request.Description}";
        var content = emailTemplate.Content
            .Replace("{{Description}}", request.Description)
            .Replace("{{OldDayOfMonth}}", request.OldDayOfMonth.ToString())
            .Replace("{{OldNotifyDaysBefore}}", request.OldNotifyDaysBefore.ToString())
            .Replace("{{DayOfMonth}}", request.DayOfMonth.ToString())
            .Replace("{{NotifyDaysBefore}}", request.NotifyDaysBefore.ToString());

        var isSuccessful = await _emailService.SendEmailAsync(emailTo, subject, content);

        return isSuccessful;
    }
}

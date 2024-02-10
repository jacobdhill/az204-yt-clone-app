using Domain.EmailTemplates;
using Domain.Reminders;
using Infrastructure.Contexts;
using Infrastructure.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders;

public class ReminderCreatedEventHandler : IRequestHandler<ReminderCreatedEvent, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailService _emailService;

    public ReminderCreatedEventHandler(ApplicationDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ReminderCreatedEvent request, CancellationToken cancellationToken)
    {
        var emailTemplate = await _dbContext.EmailTemplates
            .FirstOrDefaultAsync(template => template.Code == EmailTemplateConstants.ReminderCreatedEvent);

        var emailTo = "jacob.d.hill@outlook.com";
        var subject = $"New Reminder - {request.Description}";
        var content = emailTemplate.Content
            .Replace("{{Description}}", request.Description)
            .Replace("{{DayOfMonth}}", request.DayOfMonth.ToString())
            .Replace("{{NotifyDaysBefore}}", request.NotifyDaysBefore.ToString());

        var isSuccessful = await _emailService.SendEmailAsync(emailTo, subject, content);

        return isSuccessful;
    }
}

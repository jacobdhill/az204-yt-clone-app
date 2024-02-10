using Domain;
using Domain.Reminders;
using Infrastructure.Contexts;
using Infrastructure.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Reminders;

public class ReminderDeletedEventHandler : IRequestHandler<ReminderDeletedEvent, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailService _emailService;

    public ReminderDeletedEventHandler(ApplicationDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async Task<bool> Handle(ReminderDeletedEvent request, CancellationToken cancellationToken)
    {
        var emailTemplate = await _dbContext.EmailTemplates
            .FirstOrDefaultAsync(template => template.Code == EmailTemplateConstants.ReminderDeletedEvent);

        var emailTo = "jacob.d.hill@outlook.com";
        var subject = $"Reminder Deleted - {request.Description}";
        var content = emailTemplate.Content
            .Replace("{{Description}}", request.Description);

        var isSuccessful = await _emailService.SendEmailAsync(emailTo, subject, content);

        return isSuccessful;
    }
}

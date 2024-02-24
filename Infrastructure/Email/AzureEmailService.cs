using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Email;

public class AzureEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly EmailClient _emailClient;

    public AzureEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _emailClient = new EmailClient(_configuration.GetConnectionString("CommunicationServices"));
    }

    public async Task<bool> SendEmailAsync(string emailTo, string subject, string content, CancellationToken cancellationToken = default)
    {
        var emailContent = new EmailContent(subject)
        {
            Html = content
        };

        var fromAddress = _configuration.GetSection("CommunicationServices:FromAddress").Get<string>();

        var emailMessage = new EmailMessage(
            fromAddress,
            emailTo,
            emailContent);

        var emailSendOperation = await _emailClient.SendAsync(
            WaitUntil.Completed,
            emailMessage,
            cancellationToken);

        return emailSendOperation.HasCompleted && emailSendOperation.HasValue && emailSendOperation.Value.Status == EmailSendStatus.Succeeded;
    }
}

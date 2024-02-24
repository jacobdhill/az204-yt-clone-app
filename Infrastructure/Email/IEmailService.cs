using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Email;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string emailTo, string subject, string content, CancellationToken cancellationToken = default);
}

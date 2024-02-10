using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.QueueMessaging;

public interface IQueueMessagingService
{
    Task QueueDomainEvents(List<string> domainEvents);
}

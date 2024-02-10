using Domain;
using Infrastructure.QueueMessaging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors;

public class ApplicationSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IQueueMessagingService _queueMessagingService;

    public ApplicationSaveChangesInterceptor(IQueueMessagingService queueMessagingService)
    {
        _queueMessagingService = queueMessagingService;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<IDomainEntity>()
            .SelectMany(domainEntity => domainEntity.Entity.DomainEvents)
            .Select(domainEvent => JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }))
            .ToList();

        _queueMessagingService.QueueDomainEvents(domainEvents);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

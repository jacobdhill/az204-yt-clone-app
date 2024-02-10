using Domain;
using Domain.EventNotifications;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors;

public class ApplicationSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<IDomainEntity>()
            .SelectMany(domainEntity => domainEntity.Entity.DomainEvents)
            .Select(domainEvent => new EventNotification()
            {
                EventNotificationId = new EventNotificationId(Guid.NewGuid()),
                DomainEventPayload = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                IsProcessed = false,
                CreatedDate = DateTime.UtcNow
            })
            .ToList();

        eventData.Context.AddRange(domainEvents);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

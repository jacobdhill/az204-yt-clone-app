using Domain;
using Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors;

public class ApplicationSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly NotificationService _notificationService;

    public ApplicationSaveChangesInterceptor(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<IDomainEntity>()
            .SelectMany(domainEntity => domainEntity.Entity.DomainEvents)
            .Select(domainEvent => JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }))
            .ToList();

        await _notificationService.SendMessagesAsync(domainEvents);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

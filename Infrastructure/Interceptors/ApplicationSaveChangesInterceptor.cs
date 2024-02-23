using Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
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
            .Select(domainEvent => JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }))
            .ToList();

        //
        // TODO: Handle Domain Events
        //

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

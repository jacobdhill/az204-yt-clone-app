using Infrastructure.Contexts;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Function
{
    public class EventNotificationFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly ISender _sender;

        public EventNotificationFunction(
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            ApplicationDbContext context,
            ISender sender)
        {
            _logger = loggerFactory.CreateLogger<EventNotificationFunction>();
            _configuration = configuration;
            _dbContext = context;
            _sender = sender;
        }

        [Function("EventNotificationFunction")]
        public async Task Run([TimerTrigger("59 * * * * *")] TimerInfo timerInfo)
        {
            var eventNotifications = await _dbContext.EventNotifications
                .Where(notification => notification.IsProcessed == false)
                .OrderBy(notification => notification.CreatedDate)
                .ToListAsync();

            foreach (var eventNotification in eventNotifications)
            {
                var domainEvent = JsonConvert.DeserializeObject(eventNotification.DomainEvent, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                if (domainEvent is null)
                {
                    continue;
                }

                var response = await _sender.Send(domainEvent);
                if (response is bool eventWasProcessed)
                {
                    eventNotification.IsProcessed = true;
                    eventNotification.ModifiedDate = DateTime.UtcNow;

                    _dbContext.SaveChanges();
                }
            }
        }
    }
}

using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Function;

public class EventNotifierFunction
{
    private readonly ILogger<EventNotifierFunction> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISender _sender;

    public EventNotifierFunction(
        ILogger<EventNotifierFunction> logger,
        IConfiguration configuration,
        ISender sender)
    {
        _logger = logger;
        _configuration = configuration;
        _sender = sender;
    }

    [FunctionName(nameof(EventNotifierFunction))]
    public async Task Run([ServiceBusTrigger("domain-events", Connection = "ServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var domainEvent = JsonConvert.DeserializeObject(message.Body.ToString(), new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        });

        if (domainEvent is null)
        {
            await messageActions.AbandonMessageAsync(message);
            return;
        }

        var response = await _sender.Send(domainEvent);
        if (response is bool eventWasProcessed && eventWasProcessed)
        {
            await messageActions.CompleteMessageAsync(message);
            return;
        }

        await messageActions.DeadLetterMessageAsync(message);
    }
}

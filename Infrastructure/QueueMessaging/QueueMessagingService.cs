using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.QueueMessaging;

public class QueueMessagingService : IQueueMessagingService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly IConfiguration _configuration;

    public QueueMessagingService(IConfiguration configuration)
    {
        _configuration = configuration;

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        _serviceBusClient = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"), clientOptions);
    }

    public async Task QueueDomainEvents(List<string> domainEvents)
    {
        var sender = _serviceBusClient.CreateSender(_configuration.GetValue<string>("ServiceBus:QueueName"));
        using var messageBatch = await sender.CreateMessageBatchAsync();

        foreach (var domainEvent in domainEvents)
        {
            messageBatch.TryAddMessage(new ServiceBusMessage()
            {
                Body = new BinaryData(domainEvent),
                ContentType = System.Net.Mime.MediaTypeNames.Application.Json
            });
        }

        await sender.SendMessagesAsync(messageBatch);
        await sender.DisposeAsync();
    }
}

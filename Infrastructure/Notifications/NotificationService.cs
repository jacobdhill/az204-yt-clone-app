using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Notifications;

public class NotificationService
{
    private readonly QueueClient _client;

    public NotificationService(IConfiguration configuration)
    {
        _client = new QueueClient(
            configuration.GetConnectionString("BlobStorage"),
            configuration.GetSection("BlobStorage:NotificationsQueueName").Get<string>());

        _client.CreateIfNotExists();
    }

    public async Task SendMessagesAsync(List<string> notifications)
    {
        foreach (var notification in notifications)
        {
            await _client.SendMessageAsync(notification);
        }
    }

    public List<QueueMessage> ReceiveMessages()
    {
        var messages = _client.ReceiveMessages();
        return messages.Value.ToList();
    }

    public void RemoveMessages(List<QueueMessage> messages)
    {
        foreach (var message in messages)
        {
            _client.DeleteMessage(message.MessageId, message.PopReceipt);
        }
    }
}

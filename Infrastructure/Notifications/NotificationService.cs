using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
}

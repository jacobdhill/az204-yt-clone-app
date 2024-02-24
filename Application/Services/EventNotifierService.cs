using Infrastructure.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services;

public class EventNotifierService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly NotificationService _notificationService;
    private bool _lock = false;
    private Timer _timer;

    public EventNotifierService(NotificationService notificationService, IServiceScopeFactory scopeFactory)
    {
        _notificationService = notificationService;
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public void DoWorkAsync(object state)
    {
        if (_lock == true)
        {
            return;
        }

        _lock = true;

        try
        {
            var messages = _notificationService.ReceiveMessages();
            var eventMessages = messages
                .Select(message => JsonConvert.DeserializeObject(message.Body.ToString(), new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                }))
                .ToList();

            foreach (var message in eventMessages)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                    sender.Send(message).Wait();
                }
            }

            _notificationService.RemoveMessages(messages);
        }
        catch (Exception)
        {
        }
        finally
        {
            _lock = false;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

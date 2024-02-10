using Infrastructure.Contexts;
using Infrastructure.Email;
using Infrastructure.Interceptors;
using Infrastructure.QueueMessaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailService, AzureEmailService>();
        services.AddSingleton<IQueueMessagingService, QueueMessagingService>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Database"));

            var queueMessagingService = provider.GetRequiredService<IQueueMessagingService>();
            options.AddInterceptors(new ApplicationSaveChangesInterceptor(queueMessagingService));
        });

        return services;
    }
}
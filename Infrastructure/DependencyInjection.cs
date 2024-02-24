using Infrastructure.Contexts;
using Infrastructure.Interceptors;
using Infrastructure.Notifications;
using Infrastructure.Search;
using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NotificationService>();
        services.AddSingleton<StorageService>();
        services.AddSingleton<SearchService>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            var databaseName = configuration.GetSection("Database:Name").Get<string>();
            options.UseCosmos(connectionString, databaseName);

            var notificationService = provider.GetRequiredService<NotificationService>();
            options.AddInterceptors(new ApplicationSaveChangesInterceptor(notificationService));
        });


        return services;
    }
}
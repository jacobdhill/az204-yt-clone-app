using Infrastructure.Contexts;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            var databaseName = configuration.GetSection("Database:Name").Get<string>();
            
            options.UseCosmos(connectionString, databaseName);
            options.AddInterceptors(new ApplicationSaveChangesInterceptor());
        });

        return services;
    }
}
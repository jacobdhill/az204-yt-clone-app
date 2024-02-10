using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Application;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        var appConfigurationConnectionString = Environment.GetEnvironmentVariable("AppConfiguration");
        builder.AddAzureAppConfiguration(appConfigurationConnectionString);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplication(context.Configuration);
        services.AddInfrastructure(context.Configuration);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();

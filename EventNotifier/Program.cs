using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Application;
using Azure.Identity;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddAzureAppConfiguration(options =>
         {
             options.Connect(Environment.GetEnvironmentVariable("ConnectionStrings:AppConfiguration"));
             options.ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
         });
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplication(context.Configuration);
        services.AddInfrastructure(context.Configuration);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();

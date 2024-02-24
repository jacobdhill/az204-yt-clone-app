using Azure.Identity;
using Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Publisher;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Publisher;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
        {
            options.Connect(Environment.GetEnvironmentVariable("ConnectionStrings:AppConfiguration"));
            options.ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
        });
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var context = builder.GetContext();

        builder.Services.AddInfrastructure(context.Configuration);
    }
}
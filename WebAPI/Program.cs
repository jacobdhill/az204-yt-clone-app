using Application;
using Azure.Identity;
using Infrastructure;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
var assembly = typeof(Program).Assembly;
builder.Configuration
    .AddUserSecrets(assembly)
    .AddAzureAppConfiguration(options =>
    {
        options.Connect(builder.Configuration.GetConnectionString("AppConfiguration"));
        options.ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
    });
#endregion

#region Logging
var telemetryConfiguration = new TelemetryConfiguration()
{
    ConnectionString = builder.Configuration.GetConnectionString("ApplicationInsights")
};

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
    .CreateLogger();

builder.Services.AddSerilog(logger);
#endregion

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services
    .AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();

using Application;
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
    .AddAzureAppConfiguration(builder.Configuration.GetConnectionString("AppConfiguration"));
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

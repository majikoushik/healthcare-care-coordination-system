using FluentValidation;
using HealthcareCareCoordination.Notification.Worker.Features;
using HealthcareCareCoordination.Notification.Worker.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Observability;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Notification.Worker";

builder.Services.AddHealthcareApiFoundation(serviceName);

// Add Services
builder.Services.AddSingleton<INotificationRepository, MockNotificationRepository>();
builder.Services.AddSingleton<ISimulatedNotificationDispatcher, SimulatedNotificationDispatcher>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuditLogging(serviceName);

var app = builder.Build();

app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/notifications/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Notification Simulation", "Azure Cosmos DB (Mocked locally)");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapNotificationEndpoints();

app.Run();

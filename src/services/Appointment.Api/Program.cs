using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Appointment.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/appointments/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Appointment scheduling lifecycle", "SQL Server / Azure SQL");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.Run();

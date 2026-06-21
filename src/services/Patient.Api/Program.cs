using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Patient.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/patients/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Patient master profile", "SQL Server / Azure SQL");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.Run();

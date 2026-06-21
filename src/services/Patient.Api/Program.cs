using FluentValidation;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.Patient.Api.Domain;
using HealthcareCareCoordination.Patient.Api.Features;
using HealthcareCareCoordination.Patient.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Patient.Api";

builder.AddHealthcareObservability(serviceName);
builder.Services.AddHealthcareApiFoundation(serviceName);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=localhost,1433;Database=PatientDb;User Id=sa;Password=Change_this_local_demo_password_123!;TrustServerCertificate=True;";

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSqlServerReadiness<PatientDbContext>(); // Adds /health/ready dependency check
builder.Services.AddAuditLogging(serviceName);
builder.Services.AddHealthcareSecurity(builder.Configuration);

var app = builder.Build();
app.UseHealthcareApiFoundation(serviceName);
app.UseHealthcareSecurity();

app.MapGet("/api/v1/patients/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Patient master profile", "SQL Server / Azure SQL");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapPatientEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
    // Create database automatically on startup for local development demo
    db.Database.EnsureCreated();
}

app.Run();

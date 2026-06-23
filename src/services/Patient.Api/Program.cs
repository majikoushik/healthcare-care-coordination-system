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
                       ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection must be configured for Patient.Api. Use user secrets, environment variables, or docker-compose overrides; do not hardcode database credentials.");

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSqlServerReadiness<PatientDbContext>(); // Adds /health/ready dependency check
builder.Services.AddAuditLogging(serviceName, builder.Configuration);
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

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
        if (db.Database.GetMigrations().Any())
        {
            db.Database.Migrate();
        }
        else
        {
            db.Database.EnsureCreated();
        }
    }
}

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }

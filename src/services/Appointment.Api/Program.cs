using FluentValidation;
using HealthcareCareCoordination.Appointment.Api.Features;
using HealthcareCareCoordination.Appointment.Api.Infrastructure;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Appointment.Api";

builder.AddHealthcareObservability(serviceName);
builder.Services.AddHealthcareApiFoundation(serviceName);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection must be configured for Appointment.Api. Use user secrets, environment variables, or docker-compose overrides; do not hardcode database credentials.");

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuditLogging(serviceName);
builder.Services.AddSqlServerReadiness<AppointmentDbContext>(); // Adds /health/ready dependency check
builder.Services.AddHealthcareSecurity(builder.Configuration);

var app = builder.Build();
app.UseHealthcareApiFoundation(serviceName);
app.UseHealthcareSecurity();

app.MapGet("/api/v1/appointments/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Patient-provider scheduling workflow", "SQL Server / Azure SQL");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapAppointmentEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
    if (db.Database.GetMigrations().Any())
    {
        db.Database.Migrate();
    }
    else
    {
        db.Database.EnsureCreated();
    }
}

app.Run();

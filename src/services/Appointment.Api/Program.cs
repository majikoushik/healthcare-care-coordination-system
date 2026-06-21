using FluentValidation;
using HealthcareCareCoordination.Appointment.Api.Features;
using HealthcareCareCoordination.Appointment.Api.Infrastructure;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Appointment.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=localhost,1433;Database=AppointmentDb;User Id=sa;Password=Change_this_local_demo_password_123!;TrustServerCertificate=True;";

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuditLogging(serviceName);

var app = builder.Build();
app.UseHealthcareApiFoundation();

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
    // Create database automatically on startup for local development demo
    db.Database.EnsureCreated();
}

app.Run();

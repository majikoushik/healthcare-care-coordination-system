using FluentValidation;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.Provider.Api.Features;
using HealthcareCareCoordination.Provider.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Provider.Api";

builder.AddHealthcareObservability(serviceName);
builder.Services.AddHealthcareApiFoundation(serviceName);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Server=localhost,1433;Database=ProviderDb;User Id=sa;Password=Change_this_local_demo_password_123!;TrustServerCertificate=True;";

builder.Services.AddDbContext<ProviderDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSqlServerReadiness<ProviderDbContext>(); // Adds /health/ready dependency check
builder.Services.AddHealthcareSecurity(builder.Configuration);

var app = builder.Build();
app.UseHealthcareApiFoundation(serviceName);
app.UseHealthcareSecurity();

app.MapGet("/api/v1/providers/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Provider profile and availability readiness", "SQL Server / Azure SQL");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapProviderEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProviderDbContext>();
    // Create database automatically on startup for local development demo
    db.Database.EnsureCreated();
}

app.Run();

using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthcareCareCoordination.Observability;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthcareHealthChecks(this IServiceCollection services, string serviceName)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy($"{serviceName} is alive"));
            
        return services;
    }

    /// <summary>
    /// Adds a named DbContext SQL Server readiness health check.
    /// Call this after AddHealthcareApiFoundation in services that use EF Core SQL Server.
    /// </summary>
    public static IHealthChecksBuilder AddSqlServerReadiness<TContext>(this IServiceCollection services)
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        return services.AddHealthChecks()
            .AddDbContextCheck<TContext>(name: "sql-server-readiness",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "readiness", "sql" });
    }

    public static WebApplication MapHealthcareHealthChecks(this WebApplication app, string serviceName, string environmentName)
    {
        // Basic Liveness Check (is the container running?)
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self"),
            ResponseWriter = async (context, report) => await WriteHealthResponse(context, report, serviceName, environmentName)
        });

        // Deep Readiness Check (can we connect to DBs, etc.?)
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) => await WriteHealthResponse(context, report, serviceName, environmentName)
        });

        // Keep legacy /health working for backwards compatibility if needed
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) => await WriteHealthResponse(context, report, serviceName, environmentName)
        });

        return app;
    }

    private static async Task WriteHealthResponse(HttpContext context, HealthReport report, string serviceName, string environmentName)
    {
        context.Response.ContentType = "application/json";

        var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;

        var response = new
        {
            service = serviceName,
            status = report.Status.ToString(),
            environment = environmentName,
            correlationId = correlationId,
            timestamp = DateTimeOffset.UtcNow,
            totalDuration = report.TotalDuration.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.ToString()
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
    }
}

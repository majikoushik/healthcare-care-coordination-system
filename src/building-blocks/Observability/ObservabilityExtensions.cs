using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

namespace HealthcareCareCoordination.Observability;

public static class ObservabilityExtensions
{
    public static WebApplicationBuilder AddHealthcareObservability(this WebApplicationBuilder builder, string serviceName)
    {
        // 1. Serilog Integration
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Service", serviceName)
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Service}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}"));

        // 2. OpenTelemetry & Azure Monitor Readiness
        var appInsightsConnString = builder.Configuration["ApplicationInsights:ConnectionString"];
        var isAppInsightsEnabled = !string.IsNullOrWhiteSpace(appInsightsConnString);

        if (isAppInsightsEnabled)
        {
            // If connection string is provided, setup Application Insights fully.
            builder.Services.AddApplicationInsightsTelemetry(options => 
            {
                options.ConnectionString = appInsightsConnString;
            });
        }

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
            });

        return builder;
    }

    public static IServiceCollection AddHealthcareApiFoundation(this IServiceCollection services, string serviceName)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["service"] = serviceName;
                context.ProblemDetails.Extensions["correlationId"] =
                    context.HttpContext.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var correlationId)
                        ? correlationId
                        : context.HttpContext.TraceIdentifier;
            };
        });

        services.AddHealthcareHealthChecks(serviceName);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = serviceName,
                Version = "v1",
                Description = "Healthcare platform demo API with synthetic demo data only and compliance-readiness patterns."
            });
        });
        
        return services;
    }

    public static WebApplication UseHealthcareApiFoundation(this WebApplication app, string serviceName)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        
        // Serilog Request Logging can be added here if desired:
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms. CorrelationId: {CorrelationId}";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var correlationId = httpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? httpContext.TraceIdentifier;
                diagnosticContext.Set("CorrelationId", correlationId);
            };
        });

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var problem = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = "The request could not be completed. Use the correlation ID for support traceability.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://example.com/problems/internal-error"
                };
                problem.Extensions["correlationId"] = context.Items[CorrelationIdMiddleware.HeaderName] ?? context.TraceIdentifier;
                
                // Only leak exception type to response, never full stack trace.
                // Stack trace is safely logged by the default exception handler logger.
                problem.Extensions["exceptionType"] = feature?.Error.GetType().Name;
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problem);
            });
        });

        app.MapHealthcareHealthChecks(serviceName, app.Environment.EnvironmentName);

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{serviceName} v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}

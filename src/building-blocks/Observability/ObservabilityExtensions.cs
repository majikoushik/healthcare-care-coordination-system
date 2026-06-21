using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HealthcareCareCoordination.Observability;

public static class ObservabilityExtensions
{
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
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static WebApplication UseHealthcareApiFoundation(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
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
                problem.Extensions["exceptionType"] = feature?.Error.GetType().Name;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problem);
            });
        });
        app.MapHealthChecks("/health");
        app.MapGet("/api/v1/operational-readiness", (HttpContext context) => new
        {
            status = "Ready for Epic implementation",
            correlationId = context.Items[CorrelationIdMiddleware.HeaderName] ?? context.TraceIdentifier,
            timestamp = DateTimeOffset.UtcNow
        });
        return app;
    }
}

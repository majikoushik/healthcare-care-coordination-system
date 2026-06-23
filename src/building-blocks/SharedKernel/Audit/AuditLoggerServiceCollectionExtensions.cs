using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HealthcareCareCoordination.SharedKernel.Audit;

public static class AuditLoggerServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="HttpAuditLogger"/> for emitting audit events to Audit.Api.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="sourceService">Name of the calling service (e.g. "Patient.Api").</param>
    /// <param name="configuration">
    ///   Application configuration. The Audit API base URL is read from
    ///   <c>AuditApi:BaseUrl</c> (or the env var <c>AuditApi__BaseUrl</c>).
    ///   Defaults to <c>http://localhost:5085</c> for direct local runs.
    ///   In Docker Compose, set <c>AuditApi__BaseUrl=http://audit-api:8080</c>
    ///   because <c>localhost</c> inside a container resolves to that container,
    ///   not to the audit-api service.
    /// </param>
    public static IServiceCollection AddAuditLogging(
        this IServiceCollection services,
        string sourceService,
        IConfiguration? configuration = null)
    {
        // Resolve the Audit API base URL from configuration with a safe localhost default.
        var auditApiBaseUrl = configuration?["AuditApi:BaseUrl"]
                              ?? configuration?["AuditApi__BaseUrl"]
                              ?? "http://localhost:5085";

        services.AddHttpContextAccessor();
        services.AddHttpClient("AuditApi", client =>
        {
            client.BaseAddress = new Uri(auditApiBaseUrl);
        });

        services.AddTransient<IAuditLogger>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("AuditApi");
            var accessor = sp.GetRequiredService<IHttpContextAccessor>();
            var logger = sp.GetRequiredService<ILogger<HttpAuditLogger>>();

            return new HttpAuditLogger(client, accessor, logger, sourceService);
        });

        return services;
    }
}

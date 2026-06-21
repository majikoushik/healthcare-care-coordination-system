using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HealthcareCareCoordination.SharedKernel.Audit;

public static class AuditLoggerServiceCollectionExtensions
{
    public static IServiceCollection AddAuditLogging(this IServiceCollection services, string sourceService, string auditApiBaseUrl = "http://localhost:5085")
    {
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

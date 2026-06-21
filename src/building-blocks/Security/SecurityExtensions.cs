using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthcareCareCoordination.Security;

public static class SecurityExtensions
{
    public static IServiceCollection AddHealthcareSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecurityConfiguration>(configuration.GetSection("Security"));

        services.AddAuthentication(DemoAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, DemoAuthenticationHandler>(DemoAuthenticationHandler.SchemeName, null);

        services.AddAuthorization(options =>
        {
            // Create a policy for every known permission
            foreach (var permission in HealthcarePermissions.All)
            {
                options.AddPolicy(permission, policy => 
                    policy.RequireClaim("Permission", permission));
            }
            
            // By default, require authentication for anything not explicitly marked [AllowAnonymous]
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }

    public static WebApplication UseHealthcareSecurity(this WebApplication app)
    {
        // Security Headers Middleware
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            await next();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HealthcareCareCoordination.Security;

public class DemoAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "DemoAuthentication";
    private readonly SecurityConfiguration _securityConfiguration;

    public DemoAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<SecurityConfiguration> securityConfiguration)
        : base(options, logger, encoder, clock)
    {
        _securityConfiguration = securityConfiguration.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_securityConfiguration.Mode != "Demo" && _securityConfiguration.RequireAuthenticationInProduction)
        {
            return Task.FromResult(AuthenticateResult.Fail("Production mode requires real authentication."));
        }

        if (!_securityConfiguration.AllowDemoHeaders)
        {
            return Task.FromResult(AuthenticateResult.Fail("Demo headers are not allowed."));
        }

        var roleHeader = Request.Headers["X-Demo-User-Role"].FirstOrDefault();
        var idHeader = Request.Headers["X-Demo-User-Id"].FirstOrDefault();

        // If no header is provided and default role is allowed, fallback to default.
        if (string.IsNullOrEmpty(roleHeader))
        {
            if (!string.IsNullOrEmpty(_securityConfiguration.DefaultDemoRole))
            {
                roleHeader = _securityConfiguration.DefaultDemoRole;
                idHeader ??= Guid.NewGuid().ToString(); // Synthetic ID
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Demo Role Header."));
            }
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, idHeader ?? Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, roleHeader)
        };

        var permissions = HealthcareRoles.GetPermissionsForRole(roleHeader);
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

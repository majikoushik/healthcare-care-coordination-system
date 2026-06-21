# Security & RBAC Readiness

The Healthcare Care Coordination System employs a robust, policy-based Role-Based Access Control (RBAC) model. 

## Current Implementation: Demo Mode Identity
To simplify local review and portfolio demonstration, the system currently operates in a "Demo Mode".
- The React Frontend injects an `X-Demo-User-Role` HTTP Header via an Axios Interceptor.
- The `.NET` Backend uses a custom `DemoAuthenticationHandler` to map this string to a strongly-typed `ClaimsPrincipal`.

### Available Demo Roles:
- `Patient`
- `Provider`
- `CareCoordinator`
- `Admin`
- `Auditor`
- `System`

## Endpoint Protection
All APIs (Patient, Provider, Appointment, CarePlan, ClinicalInsights, Notification, Audit) are securely locked down using native Minimal API route handlers.
```csharp
group.MapPost("/", ScheduleAppointment)
     .RequireAuthorization(HealthcarePermissions.AppointmentWrite);
```

## Azure Entra ID Transition (Production Path)
To migrate this architecture to a real production environment utilizing Azure Entra ID:
1. Update `appsettings.json`:
   ```json
   "Security": {
     "Mode": "Production",
     "RequireAuthenticationInProduction": true,
     "AzureEntraId": {
       "TenantId": "YOUR_TENANT_ID",
       "ClientId": "YOUR_CLIENT_ID",
       "Audience": "api://healthcare-system",
       "Authority": "https://login.microsoftonline.com/YOUR_TENANT_ID/v2.0"
     }
   }
   ```
2. In `SecurityExtensions.cs`, replace the custom handler with `AddJwtBearer`:
   ```csharp
   services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddMicrosoftIdentityWebApi(configuration.GetSection("Security:AzureEntraId"));
   ```

## Secrets Management
- No hardcoded secrets (API keys, Connection Strings, JWT signatures) exist in the source code.
- In local development, utilize `.NET User Secrets` or Environment Variables.
- In Azure, the target deployment utilizes **Azure Key Vault** mapped to Container App environment variables.

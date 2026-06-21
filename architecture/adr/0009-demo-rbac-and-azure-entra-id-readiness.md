# ADR 0009: Demo RBAC and Azure Entra ID Readiness

## Status
Accepted

## Context
As a portfolio enterprise architecture demonstration, the Healthcare Care Coordination System must showcase deep understanding of Security and Role-Based Access Control (RBAC).
However, setting up a real Azure Entra ID (formerly Azure AD) tenant with App Registrations, Client Secrets, and JWT bearer token issuance adds massive overhead for local reviewers, CI/CD pipelines, and local development.
We need a strategy that secures the APIs with native ASP.NET Core `[Authorize]` and `ClaimsPrincipal` concepts, but allows developers to easily impersonate roles (Patient, Provider, CareCoordinator) without needing actual JWT tokens.

## Decision
We will build a custom `DemoAuthenticationHandler` in a dedicated `HealthcareCareCoordination.Security` building block.
- In **Demo Mode**, this handler intercepts incoming requests and looks for an `X-Demo-User-Role` HTTP header.
- It dynamically spins up a `ClaimsPrincipal` populated with the underlying permissions (e.g., `PatientProfile.Read`) linked to that role.
- All endpoints will enforce security using standard `.RequireAuthorization(HealthcarePermissions.PatientProfileRead)` conventions.
- The `SecurityConfiguration` object will house placeholder fields for future Azure Entra ID integrations (TenantId, ClientId, Audience).
- If the application starts with `RequireAuthenticationInProduction = true` and `Mode != "Demo"`, the Demo handler fails securely.

## Consequences
- **Positive**: We achieve 100% policy-based endpoint protection identical to production. Transitioning to real JWTs in the future simply means replacing `services.AddAuthentication("Demo").AddScheme(...)` with `.AddJwtBearer()`.
- **Positive**: Reviewers can seamlessly toggle user roles in the React UI via a global `SecurityContext` without logging in.
- **Negative**: The `X-Demo-User-Role` header mechanism is fundamentally insecure for any environment accessible by untrusted users. It must explicitly be disabled outside of localhost/demo modes.

## Alternatives Considered
- **IdentityServer4 / OpenIddict Local Container**: Considered too heavy and complex for a portfolio demonstration.
- **Anonymous Access Everywhere**: Rejected. Fails to demonstrate an understanding of clinical data isolation and RBAC.

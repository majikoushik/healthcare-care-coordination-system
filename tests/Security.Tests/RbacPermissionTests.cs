using HealthcareCareCoordination.Security;
using Xunit;

namespace Security.Tests;

/// <summary>
/// Validates the demo RBAC role permission table.
///
/// This is a demo Role-Based Access Control model for portfolio demonstration.
/// It uses a hardcoded permission mapping, NOT real Azure Entra ID or JWT claims.
/// Future production direction: Azure Entra ID with JWT authorization claims.
/// </summary>
public class RbacPermissionTests
{
    // ---------------------------------------------------------------------------
    // Admin role — should include all permissions
    // ---------------------------------------------------------------------------

    [Fact]
    public void Admin_HasAllHealthcarePermissions()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Admin);

        foreach (var permission in HealthcarePermissions.All)
        {
            Assert.Contains(permission, permissions);
        }
    }

    // ---------------------------------------------------------------------------
    // Auditor role — read-only access
    // ---------------------------------------------------------------------------

    [Fact]
    public void Auditor_CanReadAuditEvents()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Auditor);
        Assert.Contains(HealthcarePermissions.AuditRead, permissions);
    }

    [Fact]
    public void Auditor_CannotWriteAuditEvents()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Auditor);
        Assert.DoesNotContain(HealthcarePermissions.AuditWrite, permissions);
    }

    [Fact]
    public void Auditor_CannotWritePatientProfiles()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Auditor);
        Assert.DoesNotContain(HealthcarePermissions.PatientProfileWrite, permissions);
    }

    // ---------------------------------------------------------------------------
    // Patient role — restricted to own data reads
    // ---------------------------------------------------------------------------

    [Fact]
    public void Patient_CanReadOwnProfile()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Patient);
        Assert.Contains(HealthcarePermissions.PatientProfileRead, permissions);
    }

    [Fact]
    public void Patient_CannotWriteCarePlans()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Patient);
        Assert.DoesNotContain(HealthcarePermissions.CarePlanWrite, permissions);
    }

    [Fact]
    public void Patient_CannotAccessAuditLog()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Patient);
        Assert.DoesNotContain(HealthcarePermissions.AuditRead, permissions);
    }

    [Fact]
    public void Patient_CannotAnalyzeClinicalInsights()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Patient);
        Assert.DoesNotContain(HealthcarePermissions.ClinicalInsightAnalyze, permissions);
    }

    // ---------------------------------------------------------------------------
    // CareCoordinator role — broad coordination access
    // ---------------------------------------------------------------------------

    [Fact]
    public void CareCoordinator_CanReadAndWritePatientProfiles()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.CareCoordinator);
        Assert.Contains(HealthcarePermissions.PatientProfileRead, permissions);
        Assert.Contains(HealthcarePermissions.PatientProfileWrite, permissions);
    }

    [Fact]
    public void CareCoordinator_CanManageAppointments()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.CareCoordinator);
        Assert.Contains(HealthcarePermissions.AppointmentWrite, permissions);
        Assert.Contains(HealthcarePermissions.AppointmentStatusUpdate, permissions);
    }

    [Fact]
    public void CareCoordinator_CannotWriteProviderProfiles()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.CareCoordinator);
        Assert.DoesNotContain(HealthcarePermissions.ProviderProfileWrite, permissions);
    }

    [Fact]
    public void CareCoordinator_CannotAnalyzeClinicalInsights()
    {
        // CareCoordinator reviews but does not analyze (that is Provider/Admin)
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.CareCoordinator);
        Assert.DoesNotContain(HealthcarePermissions.ClinicalInsightAnalyze, permissions);
    }

    // ---------------------------------------------------------------------------
    // Unknown role guard
    // ---------------------------------------------------------------------------

    [Fact]
    public void UnknownRole_ReturnsNoPermissions()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole("UnknownRoleXyz");
        Assert.Empty(permissions);
    }
}

using HealthcareCareCoordination.Security;
using Xunit;

namespace Security.Tests;

public class HealthcareRolesTests
{
    [Fact]
    public void Patient_Role_Returns_Correct_Permissions()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.Patient);
        
        Assert.Contains(HealthcarePermissions.PatientProfileRead, permissions);
        Assert.Contains(HealthcarePermissions.CarePlanRead, permissions);
        Assert.DoesNotContain(HealthcarePermissions.CarePlanWrite, permissions);
        Assert.DoesNotContain(HealthcarePermissions.ProviderProfileWrite, permissions);
    }

    [Fact]
    public void CareCoordinator_Role_Returns_Correct_Permissions()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole(HealthcareRoles.CareCoordinator);
        
        Assert.Contains(HealthcarePermissions.PatientProfileWrite, permissions);
        Assert.Contains(HealthcarePermissions.AppointmentWrite, permissions);
        Assert.Contains(HealthcarePermissions.CarePlanWrite, permissions);
        Assert.Contains(HealthcarePermissions.ClinicalInsightReview, permissions);
    }

    [Fact]
    public void Unknown_Role_Returns_Empty_Permissions()
    {
        var permissions = HealthcareRoles.GetPermissionsForRole("InvalidRole123");
        
        Assert.Empty(permissions);
    }
}

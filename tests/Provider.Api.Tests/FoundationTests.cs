using HealthcareCareCoordination.Security;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void CareCoordinatorRoleIsPrepared()
    {
        Assert.Equal("CareCoordinator", HealthcareRoles.CareCoordinator);
    }
}

using HealthcareCareCoordination.Compliance;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void NotificationFoundationUsesComplianceReadinessWording()
    {
        Assert.Contains("compliance-readiness", SensitiveDataPolicy.ComplianceReadinessOnly);
    }
}

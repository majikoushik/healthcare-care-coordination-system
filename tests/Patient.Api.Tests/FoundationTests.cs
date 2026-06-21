using HealthcareCareCoordination.SharedKernel;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void PatientBoundaryUsesSqlStorage()
    {
        var metadata = new ServiceMetadata("Patient.Api", "Patient master profile", "SQL Server / Azure SQL");
        Assert.Contains("SQL", metadata.StorageModel);
    }
}

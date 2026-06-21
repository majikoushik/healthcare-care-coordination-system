using HealthcareCareCoordination.Cosmos;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void CarePlansPartitionByPatient()
    {
        var options = new CosmosContainerOptions("CarePlans", "/patientId", "Care plan documents");
        Assert.Equal("/patientId", options.PartitionKeyPath);
    }
}

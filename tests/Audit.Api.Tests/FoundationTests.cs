using HealthcareCareCoordination.Cosmos;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void AuditEventsUseCorrelationPartitionReadiness()
    {
        var options = new CosmosContainerOptions("AuditEvents", "/correlationId", "Audit and traceability events");
        Assert.Equal("/correlationId", options.PartitionKeyPath);
    }
}

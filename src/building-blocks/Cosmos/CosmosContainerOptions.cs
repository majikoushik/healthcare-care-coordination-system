namespace HealthcareCareCoordination.Cosmos;

public sealed record CosmosContainerOptions(
    string ContainerName,
    string PartitionKeyPath,
    string OwnedDataArea);

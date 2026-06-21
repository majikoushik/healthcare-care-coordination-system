namespace HealthcareCareCoordination.SharedKernel;

public sealed record ApiResponse<T>(
    T Data,
    string CorrelationId,
    DateTimeOffset Timestamp);

namespace HealthcareCareCoordination.Provider.Api.Domain;

public class Provider
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public Specialty Specialty { get; set; }
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}

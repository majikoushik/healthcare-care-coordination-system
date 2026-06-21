using HealthcareCareCoordination.Provider.Api.Domain;

namespace HealthcareCareCoordination.Provider.Api.DTOs;

public record RegisterProviderRequest(
    string FullName,
    Specialty Specialty,
    string Email,
    string MobileNumber,
    string Department,
    AvailabilityStatus AvailabilityStatus);

public record UpdateProviderRequest(
    string FullName,
    Specialty Specialty,
    string Email,
    string MobileNumber,
    string Department);

public record UpdateAvailabilityStatusRequest(AvailabilityStatus AvailabilityStatus);

public record ProviderResponse(
    Guid Id,
    string FullName,
    Specialty Specialty,
    string Email,
    string MobileNumber,
    string Department,
    AvailabilityStatus AvailabilityStatus,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

using HealthcareCareCoordination.Patient.Api.Domain;

namespace HealthcareCareCoordination.Patient.Api.DTOs;

public record RegisterPatientRequest(
    string FullName,
    DateTime DateOfBirth,
    Gender Gender,
    string Email,
    string MobileNumber,
    string Address,
    string EmergencyContactName,
    string EmergencyContactNumber,
    ConsentStatus ConsentStatus);

public record UpdatePatientRequest(
    string FullName,
    string Email,
    string MobileNumber,
    string Address,
    string EmergencyContactName,
    string EmergencyContactNumber);

public record UpdateConsentStatusRequest(ConsentStatus ConsentStatus);

public record PatientResponse(
    Guid Id,
    string FullName,
    DateTime DateOfBirth,
    Gender Gender,
    string Email,
    string MobileNumber,
    string Address,
    string EmergencyContactName,
    string EmergencyContactNumber,
    ConsentStatus ConsentStatus,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

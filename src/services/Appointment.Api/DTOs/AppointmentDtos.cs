using HealthcareCareCoordination.Appointment.Api.Domain;

namespace HealthcareCareCoordination.Appointment.Api.DTOs;

public record ScheduleAppointmentRequest(
    Guid PatientId,
    Guid ProviderId,
    DateTimeOffset AppointmentDateTime,
    string VisitReason,
    AppointmentType Type,
    string Notes);

public record UpdateAppointmentRequest(
    DateTimeOffset AppointmentDateTime,
    string VisitReason,
    AppointmentType Type,
    string Notes);

public record UpdateAppointmentStatusRequest(
    AppointmentStatus Status,
    string Reason,
    string UpdatedBy);

public record AppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid ProviderId,
    DateTimeOffset AppointmentDateTime,
    string VisitReason,
    AppointmentType Type,
    AppointmentStatus Status,
    string Notes,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

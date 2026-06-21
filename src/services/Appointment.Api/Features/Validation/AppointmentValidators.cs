using FluentValidation;
using HealthcareCareCoordination.Appointment.Api.DTOs;

namespace HealthcareCareCoordination.Appointment.Api.Features.Validation;

public class ScheduleAppointmentRequestValidator : AbstractValidator<ScheduleAppointmentRequest>
{
    public ScheduleAppointmentRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.AppointmentDateTime)
            .NotEmpty()
            .Must(BeInFuture).WithMessage("Appointment date and time must be in the future for new appointments.");
        RuleFor(x => x.VisitReason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Notes).MaximumLength(2000);
    }

    private bool BeInFuture(DateTimeOffset dateTime)
    {
        return dateTime > DateTimeOffset.UtcNow;
    }
}

public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentRequestValidator()
    {
        RuleFor(x => x.AppointmentDateTime).NotEmpty();
        RuleFor(x => x.VisitReason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}

public class UpdateAppointmentStatusRequestValidator : AbstractValidator<UpdateAppointmentStatusRequest>
{
    public UpdateAppointmentStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Reason).MaximumLength(500);
        RuleFor(x => x.UpdatedBy).MaximumLength(100);
    }
}

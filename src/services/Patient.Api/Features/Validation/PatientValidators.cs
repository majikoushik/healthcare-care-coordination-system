using FluentValidation;
using HealthcareCareCoordination.Patient.Api.DTOs;

namespace HealthcareCareCoordination.Patient.Api.Features.Validation;

public class RegisterPatientRequestValidator : AbstractValidator<RegisterPatientRequest>
{
    public RegisterPatientRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DateOfBirth).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Date of birth cannot be in the future.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.EmergencyContactName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EmergencyContactNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ConsentStatus).IsInEnum();
        RuleFor(x => x.Gender).IsInEnum();
    }
}

public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
{
    public UpdatePatientRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.EmergencyContactName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EmergencyContactNumber).NotEmpty().MaximumLength(50);
    }
}

public class UpdateConsentStatusRequestValidator : AbstractValidator<UpdateConsentStatusRequest>
{
    public UpdateConsentStatusRequestValidator()
    {
        RuleFor(x => x.ConsentStatus).IsInEnum();
    }
}

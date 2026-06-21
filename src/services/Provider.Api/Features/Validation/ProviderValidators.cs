using FluentValidation;
using HealthcareCareCoordination.Provider.Api.DTOs;

namespace HealthcareCareCoordination.Provider.Api.Features.Validation;

public class RegisterProviderRequestValidator : AbstractValidator<RegisterProviderRequest>
{
    public RegisterProviderRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Specialty).IsInEnum();
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Department).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AvailabilityStatus).IsInEnum();
    }
}

public class UpdateProviderRequestValidator : AbstractValidator<UpdateProviderRequest>
{
    public UpdateProviderRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Specialty).IsInEnum();
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.MobileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Department).NotEmpty().MaximumLength(200);
    }
}

public class UpdateAvailabilityStatusRequestValidator : AbstractValidator<UpdateAvailabilityStatusRequest>
{
    public UpdateAvailabilityStatusRequestValidator()
    {
        RuleFor(x => x.AvailabilityStatus).IsInEnum();
    }
}

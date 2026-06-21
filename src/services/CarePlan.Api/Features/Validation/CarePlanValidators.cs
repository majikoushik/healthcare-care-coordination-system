using FluentValidation;
using HealthcareCareCoordination.CarePlan.Api.DTOs;

namespace HealthcareCareCoordination.CarePlan.Api.Features.Validation;

public class CreateCarePlanRequestValidator : AbstractValidator<CreateCarePlanRequest>
{
    public CreateCarePlanRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ClinicalSummary).NotEmpty();
        RuleFor(x => x.Instructions).NotEmpty();
        
        RuleFor(x => x.FollowUpDate)
            .Must(BeInFuture).When(x => x.FollowUpDate.HasValue)
            .WithMessage("Follow-up date must be in the future.");

        RuleForEach(x => x.Goals).SetValidator(new CarePlanGoalRequestValidator());
        RuleForEach(x => x.Tasks).SetValidator(new CarePlanTaskRequestValidator());
    }

    private bool BeInFuture(DateTimeOffset? dateTime)
    {
        if (!dateTime.HasValue) return true;
        return dateTime.Value > DateTimeOffset.UtcNow;
    }
}

public class CarePlanGoalRequestValidator : AbstractValidator<CarePlanGoalRequest>
{
    public CarePlanGoalRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Priority).IsInEnum();
    }
}

public class CarePlanTaskRequestValidator : AbstractValidator<CarePlanTaskRequest>
{
    public CarePlanTaskRequestValidator()
    {
        RuleFor(x => x.TaskTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TaskDescription).MaximumLength(1000);
        RuleFor(x => x.Priority).IsInEnum();
    }
}

public class UpdateCarePlanStatusRequestValidator : AbstractValidator<UpdateCarePlanStatusRequest>
{
    public UpdateCarePlanStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class AddCarePlanGoalRequestValidator : AbstractValidator<AddCarePlanGoalRequest>
{
    public AddCarePlanGoalRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Priority).IsInEnum();
    }
}

public class AddCarePlanTaskRequestValidator : AbstractValidator<AddCarePlanTaskRequest>
{
    public AddCarePlanTaskRequestValidator()
    {
        RuleFor(x => x.TaskTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TaskDescription).MaximumLength(1000);
        RuleFor(x => x.Priority).IsInEnum();
    }
}

public class UpdateCarePlanTaskStatusRequestValidator : AbstractValidator<UpdateCarePlanTaskStatusRequest>
{
    public UpdateCarePlanTaskStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class UpdateCarePlanGoalStatusRequestValidator : AbstractValidator<UpdateCarePlanGoalStatusRequest>
{
    public UpdateCarePlanGoalStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

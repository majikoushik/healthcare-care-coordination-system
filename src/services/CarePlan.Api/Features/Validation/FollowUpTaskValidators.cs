using FluentValidation;
using HealthcareCareCoordination.CarePlan.Api.DTOs;

namespace HealthcareCareCoordination.CarePlan.Api.Features.Validation;

public class CreateFollowUpTaskRequestValidator : AbstractValidator<CreateFollowUpTaskRequest>
{
    public CreateFollowUpTaskRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient ID is required");
        RuleFor(x => x.CarePlanId).NotEmpty().WithMessage("Care Plan ID is required");
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200).WithMessage("Title is required and must not exceed 200 characters");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000).WithMessage("Description is required and must not exceed 2000 characters");
        RuleFor(x => x.TaskType).IsInEnum();
        RuleFor(x => x.Priority).IsInEnum();
        RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Date).WithMessage("Due date cannot be in the past for new tasks");
        RuleFor(x => x.AssignedTo).NotEmpty().WithMessage("Task must be assigned to someone");
    }
}

public class UpdateFollowUpTaskStatusRequestValidator : AbstractValidator<UpdateFollowUpTaskStatusRequest>
{
    public UpdateFollowUpTaskStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.UpdatedBy).NotEmpty().WithMessage("UpdatedBy is required");
        RuleFor(x => x.CompletionNotes).MaximumLength(2000).WithMessage("Completion notes must not exceed 2000 characters");
    }
}

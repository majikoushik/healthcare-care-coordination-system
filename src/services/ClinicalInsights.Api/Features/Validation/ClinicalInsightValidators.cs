using FluentValidation;
using HealthcareCareCoordination.ClinicalInsights.Api.DTOs;
using HealthcareCareCoordination.ClinicalInsights.Api.Domain;

namespace HealthcareCareCoordination.ClinicalInsights.Api.Features.Validation;

public class AnalyzeNoteRequestValidator : AbstractValidator<AnalyzeNoteRequest>
{
    public AnalyzeNoteRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient ID is required.");
        RuleFor(x => x.ProviderId).NotEmpty().WithMessage("Provider ID is required.");
        RuleFor(x => x.ClinicalNoteText)
            .NotEmpty().WithMessage("Clinical note text is required.")
            .MaximumLength(10000).WithMessage("Note text exceeds maximum length.");
    }
}

public class UpdateReviewStatusRequestValidator : AbstractValidator<UpdateReviewStatusRequest>
{
    public UpdateReviewStatusRequestValidator()
    {
        RuleFor(x => x.ReviewStatus).IsInEnum().WithMessage("Invalid review status.");
        RuleFor(x => x.ReviewedBy)
            .NotEmpty()
            .When(x => x.ReviewStatus == HumanReviewStatus.Reviewed || 
                       x.ReviewStatus == HumanReviewStatus.Approved || 
                       x.ReviewStatus == HumanReviewStatus.Rejected ||
                       x.ReviewStatus == HumanReviewStatus.RequiresClarification)
            .WithMessage("ReviewedBy is required when changing status.");
    }
}

using FluentValidation;

namespace HealthcareCareCoordination.Notification.Worker.Features;

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.RelatedEntityType).NotEmpty();
        RuleFor(x => x.RelatedEntityId).NotEmpty();
        RuleFor(x => x.NotificationType).NotEmpty();
        RuleFor(x => x.Channel).IsInEnum();
        RuleFor(x => x.RecipientType).IsInEnum();
        RuleFor(x => x.RecipientReference).NotEmpty();
        RuleFor(x => x.Subject).NotEmpty();
        RuleFor(x => x.MessageSummary).NotEmpty()
            .WithMessage("Message summary is required.")
            .Must(msg => !msg.Contains("clinical detail", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Message summary must not contain raw clinical details.");
    }
}

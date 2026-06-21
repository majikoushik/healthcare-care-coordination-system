using FluentValidation;

namespace HealthcareCareCoordination.Audit.Api.Features;

public class CreateAuditEventRequestValidator : AbstractValidator<CreateAuditEventRequest>
{
    public CreateAuditEventRequestValidator()
    {
        RuleFor(x => x.CorrelationId).NotEmpty();
        RuleFor(x => x.EventType).NotEmpty();
        RuleFor(x => x.EntityType).NotEmpty();
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.ActorType).NotEmpty();
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.Outcome).NotEmpty();
        RuleFor(x => x.Summary).NotEmpty();
        RuleFor(x => x.SourceService).NotEmpty();
        RuleFor(x => x.Severity).NotEmpty();
    }
}

using HealthcareCareCoordination.Audit.Api.Domain;

namespace HealthcareCareCoordination.Audit.Api.Infrastructure;

public interface IAuditEventRepository
{
    Task CreateAsync(AuditEventDocument document, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AuditEventDocument?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetByEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetByProviderIdAsync(string providerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetByEventTypeAsync(string eventType, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditEventDocument>> GetBySourceServiceAsync(string sourceService, CancellationToken cancellationToken = default);
}

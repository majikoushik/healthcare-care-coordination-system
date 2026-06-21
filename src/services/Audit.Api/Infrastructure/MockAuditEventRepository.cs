using System.Collections.Concurrent;
using HealthcareCareCoordination.Audit.Api.Domain;

namespace HealthcareCareCoordination.Audit.Api.Infrastructure;

public class MockAuditEventRepository : IAuditEventRepository
{
    private readonly ConcurrentBag<AuditEventDocument> _events = new();

    public Task CreateAsync(AuditEventDocument document, CancellationToken cancellationToken = default)
    {
        _events.Add(document);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<AuditEventDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.OrderByDescending(e => e.CreatedAt));
    }

    public Task<AuditEventDocument?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
    }

    public Task<IEnumerable<AuditEventDocument>> GetByCorrelationIdAsync(string correlationId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.CorrelationId == correlationId).OrderByDescending(e => e.CreatedAt));
    }

    public Task<IEnumerable<AuditEventDocument>> GetByEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.EntityType == entityType && e.EntityId == entityId).OrderByDescending(e => e.CreatedAt));
    }

    public Task<IEnumerable<AuditEventDocument>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.PatientId == patientId).OrderByDescending(e => e.CreatedAt));
    }

    public Task<IEnumerable<AuditEventDocument>> GetByProviderIdAsync(string providerId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.ProviderId == providerId).OrderByDescending(e => e.CreatedAt));
    }

    public Task<IEnumerable<AuditEventDocument>> GetByEventTypeAsync(string eventType, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.EventType == eventType).OrderByDescending(e => e.CreatedAt));
    }

    public Task<IEnumerable<AuditEventDocument>> GetBySourceServiceAsync(string sourceService, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<AuditEventDocument>>(_events.Where(e => e.SourceService == sourceService).OrderByDescending(e => e.CreatedAt));
    }
}

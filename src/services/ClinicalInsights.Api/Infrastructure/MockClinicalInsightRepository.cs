using System.Collections.Concurrent;
using HealthcareCareCoordination.ClinicalInsights.Api.Domain;

namespace HealthcareCareCoordination.ClinicalInsights.Api.Infrastructure;

public class MockClinicalInsightRepository : IClinicalInsightRepository
{
    private readonly ConcurrentDictionary<string, ClinicalNoteInsight> _insights = new();

    public Task<ClinicalNoteInsight> CreateAsync(ClinicalNoteInsight insight, CancellationToken cancellationToken = default)
    {
        _insights[insight.Id] = insight;
        return Task.FromResult(insight);
    }

    public Task<ClinicalNoteInsight?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        _insights.TryGetValue(id, out var insight);
        return Task.FromResult(insight);
    }

    public Task<IEnumerable<ClinicalNoteInsight>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_insights.Values.OrderByDescending(i => i.CreatedAt).AsEnumerable());
    }

    public Task<IEnumerable<ClinicalNoteInsight>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default)
    {
        var result = _insights.Values
            .Where(i => i.PatientId == patientId)
            .OrderByDescending(i => i.CreatedAt)
            .AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<IEnumerable<ClinicalNoteInsight>> GetByCarePlanIdAsync(string carePlanId, CancellationToken cancellationToken = default)
    {
        var result = _insights.Values
            .Where(i => i.RelatedCarePlanId == carePlanId)
            .OrderByDescending(i => i.CreatedAt)
            .AsEnumerable();
        return Task.FromResult(result);
    }

    public Task UpdateAsync(ClinicalNoteInsight insight, CancellationToken cancellationToken = default)
    {
        if (_insights.ContainsKey(insight.Id))
        {
            insight.UpdatedAt = DateTimeOffset.UtcNow;
            _insights[insight.Id] = insight;
        }
        return Task.CompletedTask;
    }
}

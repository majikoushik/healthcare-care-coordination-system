using HealthcareCareCoordination.ClinicalInsights.Api.Domain;

namespace HealthcareCareCoordination.ClinicalInsights.Api.Infrastructure;

public interface IClinicalInsightRepository
{
    Task<ClinicalNoteInsight> CreateAsync(ClinicalNoteInsight insight, CancellationToken cancellationToken = default);
    Task<ClinicalNoteInsight?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClinicalNoteInsight>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ClinicalNoteInsight>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClinicalNoteInsight>> GetByCarePlanIdAsync(string carePlanId, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClinicalNoteInsight insight, CancellationToken cancellationToken = default);
}

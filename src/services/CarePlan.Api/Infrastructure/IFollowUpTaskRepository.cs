using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.Infrastructure;

public interface IFollowUpTaskRepository
{
    Task<FollowUpTaskDocument> CreateAsync(FollowUpTaskDocument document);
    Task<FollowUpTaskDocument?> GetByIdAsync(Guid id);
    Task<IEnumerable<FollowUpTaskDocument>> GetAllAsync();
    Task<IEnumerable<FollowUpTaskDocument>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<FollowUpTaskDocument>> GetByCarePlanIdAsync(Guid carePlanId);
    Task<IEnumerable<FollowUpTaskDocument>> GetByClinicalInsightIdAsync(Guid clinicalInsightId);
    Task<IEnumerable<FollowUpTaskDocument>> GetDueTodayAsync();
    Task<IEnumerable<FollowUpTaskDocument>> GetOverdueAsync();
    Task<FollowUpTaskDocument> UpdateAsync(FollowUpTaskDocument document);
}

using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.Infrastructure;

public interface ICarePlanRepository
{
    Task<CarePlanDocument?> GetByIdAsync(Guid id);
    Task<IEnumerable<CarePlanDocument>> GetAllAsync();
    Task<IEnumerable<CarePlanDocument>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<CarePlanDocument>> GetByProviderIdAsync(Guid providerId);
    Task<CarePlanDocument> CreateAsync(CarePlanDocument document);
    Task<CarePlanDocument> UpdateAsync(CarePlanDocument document);
}

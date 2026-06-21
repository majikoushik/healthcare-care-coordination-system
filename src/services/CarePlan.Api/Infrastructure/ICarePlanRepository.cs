using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.Infrastructure;

public interface ICarePlanRepository
{
    Task<CarePlanDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CarePlanDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CarePlanDocument>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CarePlanDocument>> GetByProviderIdAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<CarePlanDocument> CreateAsync(CarePlanDocument document, CancellationToken cancellationToken = default);
    Task<CarePlanDocument> UpdateAsync(CarePlanDocument document, CancellationToken cancellationToken = default);
}

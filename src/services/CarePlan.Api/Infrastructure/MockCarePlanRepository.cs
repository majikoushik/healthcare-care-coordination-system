using System.Collections.Concurrent;
using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.Infrastructure;

/// <summary>
/// A thread-safe, in-memory repository to simulate Azure Cosmos DB for local development.
/// In a production environment, this would be replaced with a CosmosDbCarePlanRepository.
/// </summary>
public class MockCarePlanRepository : ICarePlanRepository
{
    // Partition key concept: in Cosmos, it would be /patientId. Here we just store everything in a dict.
    private readonly ConcurrentDictionary<Guid, CarePlanDocument> _documents = new();

    public Task<CarePlanDocument?> GetByIdAsync(Guid id)
    {
        _documents.TryGetValue(id, out var document);
        return Task.FromResult(document);
    }

    public Task<IEnumerable<CarePlanDocument>> GetAllAsync()
    {
        return Task.FromResult(_documents.Values.AsEnumerable());
    }

    public Task<IEnumerable<CarePlanDocument>> GetByPatientIdAsync(Guid patientId)
    {
        var result = _documents.Values.Where(d => d.PatientId == patientId).ToList();
        return Task.FromResult(result.AsEnumerable());
    }

    public Task<IEnumerable<CarePlanDocument>> GetByProviderIdAsync(Guid providerId)
    {
        var result = _documents.Values.Where(d => d.ProviderId == providerId).ToList();
        return Task.FromResult(result.AsEnumerable());
    }

    public Task<CarePlanDocument> CreateAsync(CarePlanDocument document)
    {
        document.CreatedAt = DateTimeOffset.UtcNow;
        _documents[document.Id] = document;
        return Task.FromResult(document);
    }

    public Task<CarePlanDocument> UpdateAsync(CarePlanDocument document)
    {
        document.UpdatedAt = DateTimeOffset.UtcNow;
        _documents[document.Id] = document;
        return Task.FromResult(document);
    }
}

using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.Infrastructure;

public class MockFollowUpTaskRepository : IFollowUpTaskRepository
{
    private readonly List<FollowUpTaskDocument> _tasks = new();

    public Task<FollowUpTaskDocument> CreateAsync(FollowUpTaskDocument document)
    {
        _tasks.Add(document);
        return Task.FromResult(document);
    }

    public Task<FollowUpTaskDocument?> GetByIdAsync(Guid id)
    {
        var document = _tasks.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(document);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(_tasks);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetByPatientIdAsync(Guid patientId)
    {
        var results = _tasks.Where(t => t.PatientId == patientId);
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(results);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetByCarePlanIdAsync(Guid carePlanId)
    {
        var results = _tasks.Where(t => t.CarePlanId == carePlanId);
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(results);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetByClinicalInsightIdAsync(Guid clinicalInsightId)
    {
        var results = _tasks.Where(t => t.ClinicalInsightId == clinicalInsightId);
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(results);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetDueTodayAsync()
    {
        var today = DateTimeOffset.UtcNow.Date;
        var results = _tasks.Where(t => t.DueDate.Date == today && t.Status != FollowUpTaskStatus.Completed && t.Status != FollowUpTaskStatus.Cancelled);
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(results);
    }

    public Task<IEnumerable<FollowUpTaskDocument>> GetOverdueAsync()
    {
        var today = DateTimeOffset.UtcNow.Date;
        var results = _tasks.Where(t => t.DueDate.Date < today && t.Status != FollowUpTaskStatus.Completed && t.Status != FollowUpTaskStatus.Cancelled);
        
        // Auto-update to overdue for demo if pending/inprogress
        foreach (var task in results)
        {
            if (task.Status == FollowUpTaskStatus.Pending || task.Status == FollowUpTaskStatus.InProgress)
            {
                task.Status = FollowUpTaskStatus.Overdue;
                task.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
        
        return Task.FromResult<IEnumerable<FollowUpTaskDocument>>(results);
    }

    public Task<FollowUpTaskDocument> UpdateAsync(FollowUpTaskDocument document)
    {
        var existing = _tasks.FirstOrDefault(t => t.Id == document.Id);
        if (existing != null)
        {
            _tasks.Remove(existing);
            document.UpdatedAt = DateTimeOffset.UtcNow;
            _tasks.Add(document);
        }
        return Task.FromResult(document);
    }
}

namespace HealthcareCareCoordination.CarePlan.Api.Domain;

public static class FollowUpTaskStatusMachine
{
    public static bool CanTransition(FollowUpTaskStatus current, FollowUpTaskStatus next)
    {
        if (current == next) return true;

        return current switch
        {
            FollowUpTaskStatus.Pending => next is FollowUpTaskStatus.InProgress or FollowUpTaskStatus.Completed or FollowUpTaskStatus.Cancelled or FollowUpTaskStatus.Overdue,
            FollowUpTaskStatus.InProgress => next is FollowUpTaskStatus.Completed or FollowUpTaskStatus.Cancelled or FollowUpTaskStatus.Overdue,
            FollowUpTaskStatus.Overdue => next is FollowUpTaskStatus.InProgress or FollowUpTaskStatus.Completed or FollowUpTaskStatus.Cancelled,
            FollowUpTaskStatus.Completed => false, // Terminal
            FollowUpTaskStatus.Cancelled => false, // Terminal
            _ => false
        };
    }
}

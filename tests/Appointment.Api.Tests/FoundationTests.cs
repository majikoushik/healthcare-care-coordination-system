using HealthcareCareCoordination.Messaging;
using Xunit;

public sealed class FoundationTests
{
    [Fact]
    public void IntegrationEventsCarryCorrelationId()
    {
        var evt = new IntegrationEvent(Guid.NewGuid(), "AppointmentScheduled", DateTimeOffset.UtcNow, "corr-1", "Appointment", "appt-1", "patient-1");
        Assert.Equal("corr-1", evt.CorrelationId);
    }
}

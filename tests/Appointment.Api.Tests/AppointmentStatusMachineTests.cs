using HealthcareCareCoordination.Appointment.Api.Domain;
using Xunit;

namespace Appointment.Api.Tests;

/// <summary>
/// Validates the appointment status machine accepts valid transitions
/// and rejects invalid or terminal-state transitions.
/// </summary>
public class AppointmentStatusMachineTests
{
    // ---------------------------------------------------------------------------
    // Valid transitions
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(AppointmentStatus.Requested, AppointmentStatus.Scheduled)]
    [InlineData(AppointmentStatus.Requested, AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.Scheduled, AppointmentStatus.CheckedIn)]
    [InlineData(AppointmentStatus.Scheduled, AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.Scheduled, AppointmentStatus.NoShow)]
    [InlineData(AppointmentStatus.CheckedIn, AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.CheckedIn, AppointmentStatus.Cancelled)]
    public void CanTransition_WhenTransitionIsValid_ReturnsTrue(
        AppointmentStatus current, AppointmentStatus next)
    {
        Assert.True(AppointmentStatusMachine.CanTransition(current, next));
    }

    // ---------------------------------------------------------------------------
    // Invalid transitions
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(AppointmentStatus.Completed, AppointmentStatus.Scheduled)]
    [InlineData(AppointmentStatus.Cancelled, AppointmentStatus.Scheduled)]
    [InlineData(AppointmentStatus.NoShow, AppointmentStatus.CheckedIn)]
    [InlineData(AppointmentStatus.Requested, AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Requested, AppointmentStatus.CheckedIn)]
    [InlineData(AppointmentStatus.Requested, AppointmentStatus.NoShow)]
    public void CanTransition_WhenTransitionIsInvalid_ReturnsFalse(
        AppointmentStatus current, AppointmentStatus next)
    {
        Assert.False(AppointmentStatusMachine.CanTransition(current, next));
    }

    // ---------------------------------------------------------------------------
    // Terminal state guard
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.NoShow)]
    public void CanTransition_FromTerminalState_AlwaysReturnsFalse(AppointmentStatus terminal)
    {
        // Terminal statuses should not allow any forward transitions
        foreach (var next in Enum.GetValues<AppointmentStatus>())
        {
            if (next == terminal) continue; // same-state is not a transition
            Assert.False(AppointmentStatusMachine.CanTransition(terminal, next),
                $"Expected no transition from terminal state {terminal} to {next}");
        }
    }
}

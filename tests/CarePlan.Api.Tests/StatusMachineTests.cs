using HealthcareCareCoordination.CarePlan.Api.Domain;
using Xunit;

namespace CarePlan.Api.Tests;

/// <summary>
/// Validates the care plan status machine accepts valid transitions
/// and rejects invalid transitions and terminal states.
/// </summary>
public class CarePlanStatusMachineTests
{
    // ---------------------------------------------------------------------------
    // Valid transitions
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(CarePlanStatus.Draft, CarePlanStatus.Active)]
    [InlineData(CarePlanStatus.Draft, CarePlanStatus.Cancelled)]
    [InlineData(CarePlanStatus.Active, CarePlanStatus.OnHold)]
    [InlineData(CarePlanStatus.Active, CarePlanStatus.Completed)]
    [InlineData(CarePlanStatus.Active, CarePlanStatus.Cancelled)]
    [InlineData(CarePlanStatus.OnHold, CarePlanStatus.Active)]
    [InlineData(CarePlanStatus.OnHold, CarePlanStatus.Cancelled)]
    public void CanTransition_WhenTransitionIsValid_ReturnsTrue(
        CarePlanStatus current, CarePlanStatus next)
    {
        Assert.True(CarePlanStatusMachine.CanTransition(current, next));
    }

    // ---------------------------------------------------------------------------
    // Invalid transitions
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(CarePlanStatus.Draft, CarePlanStatus.OnHold)]
    [InlineData(CarePlanStatus.Draft, CarePlanStatus.Completed)]
    [InlineData(CarePlanStatus.Completed, CarePlanStatus.Active)]
    [InlineData(CarePlanStatus.Cancelled, CarePlanStatus.Active)]
    [InlineData(CarePlanStatus.Active, CarePlanStatus.Draft)]
    public void CanTransition_WhenTransitionIsInvalid_ReturnsFalse(
        CarePlanStatus current, CarePlanStatus next)
    {
        Assert.False(CarePlanStatusMachine.CanTransition(current, next));
    }

    // ---------------------------------------------------------------------------
    // Terminal state guard
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(CarePlanStatus.Completed)]
    [InlineData(CarePlanStatus.Cancelled)]
    public void CanTransition_FromTerminalState_AlwaysReturnsFalse(CarePlanStatus terminal)
    {
        foreach (var next in Enum.GetValues<CarePlanStatus>())
        {
            if (next == terminal) continue;
            Assert.False(CarePlanStatusMachine.CanTransition(terminal, next),
                $"Expected no transition from terminal {terminal} to {next}");
        }
    }
}

/// <summary>
/// Validates the follow-up task status machine.
/// </summary>
public class FollowUpTaskStatusMachineTests
{
    [Theory]
    [InlineData(FollowUpTaskStatus.Pending, FollowUpTaskStatus.InProgress)]
    [InlineData(FollowUpTaskStatus.Pending, FollowUpTaskStatus.Completed)]
    [InlineData(FollowUpTaskStatus.Pending, FollowUpTaskStatus.Cancelled)]
    [InlineData(FollowUpTaskStatus.Pending, FollowUpTaskStatus.Overdue)]
    [InlineData(FollowUpTaskStatus.InProgress, FollowUpTaskStatus.Completed)]
    [InlineData(FollowUpTaskStatus.InProgress, FollowUpTaskStatus.Cancelled)]
    [InlineData(FollowUpTaskStatus.InProgress, FollowUpTaskStatus.Overdue)]
    [InlineData(FollowUpTaskStatus.Overdue, FollowUpTaskStatus.InProgress)]
    [InlineData(FollowUpTaskStatus.Overdue, FollowUpTaskStatus.Completed)]
    [InlineData(FollowUpTaskStatus.Overdue, FollowUpTaskStatus.Cancelled)]
    public void CanTransition_WhenTransitionIsValid_ReturnsTrue(
        FollowUpTaskStatus current, FollowUpTaskStatus next)
    {
        Assert.True(FollowUpTaskStatusMachine.CanTransition(current, next));
    }

    [Theory]
    [InlineData(FollowUpTaskStatus.Completed, FollowUpTaskStatus.Pending)]
    [InlineData(FollowUpTaskStatus.Completed, FollowUpTaskStatus.InProgress)]
    [InlineData(FollowUpTaskStatus.Cancelled, FollowUpTaskStatus.Pending)]
    [InlineData(FollowUpTaskStatus.Cancelled, FollowUpTaskStatus.InProgress)]
    public void CanTransition_WhenTransitionIsInvalid_ReturnsFalse(
        FollowUpTaskStatus current, FollowUpTaskStatus next)
    {
        Assert.False(FollowUpTaskStatusMachine.CanTransition(current, next));
    }

    [Fact]
    public void CanTransition_SameStatus_ReturnsTrue()
    {
        // Self-transitions are allowed (idempotent status updates)
        Assert.True(FollowUpTaskStatusMachine.CanTransition(FollowUpTaskStatus.Pending, FollowUpTaskStatus.Pending));
    }

    [Theory]
    [InlineData(FollowUpTaskStatus.Completed)]
    [InlineData(FollowUpTaskStatus.Cancelled)]
    public void CanTransition_FromTerminalState_ToOtherStatus_ReturnsFalse(FollowUpTaskStatus terminal)
    {
        foreach (var next in Enum.GetValues<FollowUpTaskStatus>())
        {
            if (next == terminal) continue; // same-state self-transition is allowed
            Assert.False(FollowUpTaskStatusMachine.CanTransition(terminal, next),
                $"Expected no transition from terminal {terminal} to {next}");
        }
    }
}

namespace HealthcareCareCoordination.CarePlan.Api.Domain;

public static class CarePlanStatusMachine
{
    public static bool CanTransition(CarePlanStatus current, CarePlanStatus next)
    {
        return (current, next) switch
        {
            (CarePlanStatus.Draft, CarePlanStatus.Active) => true,
            (CarePlanStatus.Draft, CarePlanStatus.Cancelled) => true,
            (CarePlanStatus.Active, CarePlanStatus.OnHold) => true,
            (CarePlanStatus.Active, CarePlanStatus.Completed) => true,
            (CarePlanStatus.Active, CarePlanStatus.Cancelled) => true,
            (CarePlanStatus.OnHold, CarePlanStatus.Active) => true,
            (CarePlanStatus.OnHold, CarePlanStatus.Cancelled) => true,
            _ => false
        };
    }
}

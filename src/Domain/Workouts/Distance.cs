using SharedKernel;

namespace Domain.Workouts;

public sealed class Distance
{
    private const decimal OneKilometer = 1_000.0m;

    public Distance(decimal meters)
    {
        Ensure.GreaterThanZero(meters);

        Meters = meters;
    }

    private Distance()
    {
        
    }

    public decimal Meters { get; init; }

    public decimal Kilometers => Meters / OneKilometer;
}

namespace _6;

public class RaceNoValueGenerator
{
    public int RaceDuration { get; set; }
    public int RecordDistance { get; set; }

    public int GetWiningDurationsCount()
    {
        // Distance:
        // Distance = (RaceDuration - PushDuration) * PushDuration
        // Distance = RaceDuration*PushDuration - PushDuration*PushDuration
        // Distance = RaceDuration*PushDuration - PushDuration^2
        // Distance = RaceDuration*PushDuration - PushDuration^2 === y = RaceDuration*x - x^2
        // RecordDistance < RaceDuration*PushDuration - PushDuration^2

        // Ex: RaceDuration: 7 RecordDistance: 9
        // 9 < 7*PushDuration - PushDuration^2          | RecordDistance < RaceDuration*PushDuration - PushDuration^2
        // 0 < 7*PushDuration - PushDuration^2 - 9      | 0 < RaceDuration*PushDuration - PushDuration^2 - RecordDistance
        // 0 < -PushDuration^2 + 7*PushDuration - 9     | 0 < -PushDuration^2 + RaceDuration*PushDuration - RecordDistance
        // delta = 7^2 - 4*(-1)*(-9) = 49 - 36 = 13     | delta = RaceDuration^2 - 4*(-1)*(-RecordDistance)
        // x1 = (7 + sqrt(13)) / 2 = (7 + 3.6) / 2 = 5.3
        // x2 = (7 - sqrt(13)) / 2 = (7 - 3.6) / 2 = 1.69
        // 1.4 < PushDuration < 5.3

        var delta = Math.Pow(RaceDuration, 2) - 4 * (-1) * (-RecordDistance);
        var x1 = (RaceDuration + Math.Sqrt(delta)) / 2;
        var x2 = (RaceDuration - Math.Sqrt(delta)) / 2;

        var x1UpperBound = (int)Math.Ceiling(x1);
        var x2LowerBound = (int)Math.Floor(x2) + 1;

        var length = x1UpperBound - x2LowerBound;

        return length;
    }
}
namespace _6;

public class RaceGame2
{
    public List<RaceNoValueGenerator> Races { get; init; } = new List<RaceNoValueGenerator>();
    public RaceGame2(string[] lines)
    {
        var duration = lines[0]
            .Replace("Time:", string.Empty)
            .Replace(" ", string.Empty);

        var record = lines[1]
            .Replace("Distance:", string.Empty)
            .Replace(" ", string.Empty);

        Races.Add(new RaceNoValueGenerator() { RaceDuration = Convert.ToInt64(duration), RecordDistance = Convert.ToInt64(record) });
    }

    public long GetErrorMargin()
    {
        long result = 1;
        foreach (var race in Races)
        {
            result *= race.GetWiningDurationsCount();
        }

        return result;
    }
}
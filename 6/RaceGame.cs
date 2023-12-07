namespace _6;

public class RaceGame
{
    public List<Race> Races { get; init; } = new List<Race>();
    public RaceGame(string[] lines)
    {
        var durations = lines[0]
            .Replace("Time:", string.Empty)
            .Trim()
            .Split(' ')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        var records = lines[1]
            .Replace("Distance:", string.Empty)
            .Trim()
            .Split(' ')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        for (int i = 0; i < durations.Count; i++)
        {
            Races.Add(new Race() { RaceDuration = Convert.ToInt32(durations[i]), RecordDistance = Convert.ToInt32(records[i]) });
        }
    }

    public int GetErrorMargin()
    {
        var result = 1;
        foreach (var race in Races)
        {
            result *= race.GetWiningDurations().Length;
        }

        return result;
    }
}
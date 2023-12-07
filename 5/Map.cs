namespace _5;

public class Map
{
    private readonly List<SingleRangeMap> maps;

    public string From { get; }
    public string To { get; }

    public Map(string[] line, string from, string to)
    {
        From = from;
        To = to;
        maps = line
            .Select(l => new SingleRangeMap(l))
            .OrderBy(m => m.SourceStart)
            .ToList();
    }

    public long GetTargetValue(long sourceValue)
    {
        for (int i = 0; i < maps.Count; i++)
        {
            if (maps[i].SourceStart <= sourceValue && sourceValue < maps[i].SourceEnd)
            {
                return maps[i].GetTargetValue(sourceValue);
            }
        }

        return sourceValue;
    }
}
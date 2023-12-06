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

    public int GetTargetValue(int sourceValue)
    {
        var map = maps.FirstOrDefault(m => m.SourceStart <= sourceValue && sourceValue < m.SourceStart + m.TransformationRange);
        if (map == null)
        {
            return sourceValue;
        }

        return map.GetTargetValue(sourceValue);
    }

    private class SingleRangeMap
    {
        public int TransformationRange { get; init; }

        public int SourceStart { get; init; }

        public int DestinationStart { get; init; }

        public SingleRangeMap(string line)
        {
            var values = line.Split(' ');

            this.DestinationStart = Convert.ToInt32(values[0]);
            this.SourceStart = Convert.ToInt32(values[1]);
            this.TransformationRange = Convert.ToInt32(values[2]);
        }

        public int GetTargetValue(int sourceValue)
        {
            if (sourceValue < this.SourceStart || sourceValue >= this.SourceStart + this.TransformationRange)
            {
                return sourceValue;
            }

            var sourceValueOffsetFromSourceStart = sourceValue - this.SourceStart;
            return this.DestinationStart + sourceValueOffsetFromSourceStart;
        }
    }
}
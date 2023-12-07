namespace _5;

public class SingleRangeMap
{
    public long TransformationRange { get; init; }

    public long SourceStart { get; init; }

    public long SourceEnd { get; init; }

    public long DestinationStart { get; init; }

    public SingleRangeMap(string line)
    {
        var values = line.Split(' ');

        this.DestinationStart = Convert.ToInt64(values[0]);
        this.SourceStart = Convert.ToInt64(values[1]);
        this.TransformationRange = Convert.ToInt64(values[2]);
        this.SourceEnd = this.SourceStart + this.TransformationRange;
    }

    public long GetTargetValue(long sourceValue)
    {
        var sourceValueOffsetFromSourceStart = sourceValue - this.SourceStart;
        return this.DestinationStart + sourceValueOffsetFromSourceStart;
    }
}
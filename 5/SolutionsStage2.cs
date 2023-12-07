namespace _5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Security.AccessControl;

public class SolutionsStage2(string fileName) : SolutionBase(fileName)
{
    protected override async Task<string> GetResultAsync()
    {
        var almanac = new Almanac(this.Lines.ToArray());
        var result = await almanac.GetClosestLocation();
        return result.ToString();
    }

    private class Almanac
    {
        public List<Map> Maps { get; private set; } = new List<Map>();

        private List<SeedRange> seedRanges = new List<SeedRange>();

        public Almanac(string[] lines)
        {
            ExtractSeeds(lines);
        }

        public async Task<long> GetClosestLocation()
        {
            var rangeProcessTasks = this.seedRanges
                .AsParallel()
                .Select(r => r.GetLowestLocationNumber(Maps))
                .ToList();
            
            return rangeProcessTasks
                .Min();
        }

        private void ExtractSeeds(string[] lines)
        {
            var seedRangesStrings = lines[0].Replace("seeds: ", string.Empty).Split(" ");
            ExtractMaps(lines);

            for (int seedIndex = 0; seedIndex < seedRangesStrings.Length; seedIndex += 2)
            {
                var seedRangeStart = Convert.ToInt64(seedRangesStrings[seedIndex]);
                var seedCount = Convert.ToInt64(seedRangesStrings[seedIndex + 1]);
                this.seedRanges.Add(new SeedRange(seedRangeStart, seedCount));
            }
            this.seedRanges = this.seedRanges.OrderBy(sr => sr.Start).ToList();
        }

        private record SeedRange(long Start, long Length)
        {
            public long LastItemIndex => Start + (Length - 1);

            public long GetLowestLocationNumber(List<Map> maps)
            {
                var lowestLocationNumber = long.MaxValue;
                for (long seedNumber = Start; seedNumber <= LastItemIndex; seedNumber++)
                {
                    var seed = new Seed(seedNumber, maps);
                    if (seed.LocationNumber < lowestLocationNumber)
                    {
                        lowestLocationNumber = seed.LocationNumber;
                    }

                    if (seedNumber - Start % (int)(LastItemIndex - Length / 10) == 0)
                    {
                        Console.WriteLine($"{Start}: {seedNumber}/{LastItemIndex}");
                    }
                }

                return lowestLocationNumber;
            }
        }

        private void ExtractMaps(string[] lines)
        {
            // 2 - we skip the seeds line and the empty line
            for (int lineIndex = 2; lineIndex < lines.Length; lineIndex++)
            {
                var headerLineParts = lines[lineIndex].Replace(" map:", string.Empty).Split("-");
                var from = headerLineParts[0];
                var to = headerLineParts[2]; // [1] - "to"

                // skip the header line
                lineIndex++;

                // search for the map lines
                var mapLinesStartIndex = lineIndex;
                var mapLinesEndIndex = lines.Length;
                for (; lineIndex < lines.Length; lineIndex++)
                {
                    if (string.IsNullOrEmpty(lines[lineIndex]))
                    {
                        mapLinesEndIndex = lineIndex; // not subtracting 1 here because the rage operator is exclusive here
                        break;
                    }
                }

                var mapLines = lines[mapLinesStartIndex..mapLinesEndIndex];
                var map = new Map(mapLines, from, to);
                Maps.Add(map);
            }
        }
    }

    private class Seed
    {
        public long SeedNumber { get; private set; }

        public long LocationNumber { get; private set; }

        public Seed(long seedNumber, List<Map> mapsOrdered)
        {
            SeedNumber = seedNumber;

            LocationNumber = seedNumber;
            for (int i = 0; i < mapsOrdered.Count; i++)
            {
                LocationNumber = mapsOrdered[i].GetTargetValue(LocationNumber);
            }
        }
    }
}

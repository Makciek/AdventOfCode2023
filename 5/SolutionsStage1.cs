namespace _5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

public class SolutionsStage1(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var almanac = new Almanac(this.Lines.ToArray());
        var result = almanac.GetClosestLocation();
        return result.ToString();
    }

    private class Almanac
    {
        public List<Map> Maps { get; private set; } = new List<Map>();
        
        public List<Seed> Seeds { get; private set; }

        public Almanac(string[] lines)
        {
            ExtractSeeds(lines);
        }

        public long GetClosestLocation()
        {
            return Seeds.OrderBy(s => s.LocationNumber).First().LocationNumber;
        }

        private void ExtractSeeds(string[] lines)
        {
            var seedNumbers = lines[0].Replace("seeds: ", string.Empty).Split(" ");
            ExtractMaps(lines);

            Seeds = seedNumbers.Select(s => new Seed(Convert.ToInt64(s), Maps)).ToList();
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
            foreach (var map in mapsOrdered)
            {
                LocationNumber = map.GetTargetValue(LocationNumber);
            }
        }
    }
}

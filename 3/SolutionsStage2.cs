namespace _3;

using Common;
using System.Net;

public class SolutionsStage2(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var engineSchema = new EngineSchema(this.Lines);
        var gearsRatioSum = engineSchema.GetGearRatioSum();
        return gearsRatioSum.ToString();
    }

    private class EngineSchema
    {
        private readonly AdjacentContinuousBuffer buffer;
        private readonly int originalLineLength;

        private readonly List<PartWithAdjacents> parts = new List<PartWithAdjacents>();

        public EngineSchema(IEnumerable<string> lines)
        {
            var linesArray = lines
                .Select(line => line.ToCharArray())
                .ToArray();

            this.originalLineLength = linesArray[0].Length;

            this.buffer = new AdjacentContinuousBuffer(linesArray);
            this.ParseParts();
        }

        public int GetSumfOfParts() => this.parts.Where(p => p.HasAdjacentSymbols).Sum(p => p.PartNumber);

        public int GetGearRatioSum()
        {
            var gearSum = this.parts
                .Select(p => (PartNumber: p.PartNumber,
                    GearMarks: p.AdjacentSymbols.Where(s => s.Symbol == '*').ToList()))
                .Where(p => p.GearMarks.Any())
                .SelectMany(p =>
                    p.GearMarks.Select(g => (PartNumber: p.PartNumber, LineIndex: g.LineIndex, ColIndex: g.ColIndex)))
                .GroupBy(g => (g.LineIndex, g.ColIndex))
                .Where(grouping => grouping.Count() == 2)
                .Select(grouping =>
                    grouping.First().PartNumber * grouping.Last().PartNumber) // it's always 2 elements in grouping
                .Sum();

            return gearSum;
        }

        private void ParseParts()
        {
            var flattenInput = buffer.ReadAll();

            var number = new List<char>();
            for (int i = 0; i < flattenInput.Count; i++)
            {
                var numberChar = flattenInput[i][1];
                if (char.IsDigit(numberChar))
                {
                    number.Add(numberChar);
                }
                else
                {
                    if (number.Any())
                    {
                        var numberInt = Convert.ToInt32(new string(number.ToArray()));

                        var numberStartAdjacentIndex = i - (number.Count + 1);
                        numberStartAdjacentIndex = numberStartAdjacentIndex < 0 ? 0 : numberStartAdjacentIndex;

                        var adjacentWindowLength = number.Count + 2; // +2 for adjacency
                        adjacentWindowLength = adjacentWindowLength > flattenInput.Count ? flattenInput.Count : adjacentWindowLength;

                        var symbols = new List<(char Symbol, int LineIndex, int ColIndex)>();
                        for (int columnIndex = numberStartAdjacentIndex; columnIndex < numberStartAdjacentIndex + adjacentWindowLength; columnIndex++)
                        {
                            for (int lineIndex = 0; lineIndex < 3; lineIndex++)
                            {
                                var symbolChar = flattenInput[columnIndex][lineIndex];
                                if (symbolChar != '.' && !char.IsDigit(symbolChar))
                                {
                                    var lineUnFlatten = columnIndex / this.originalLineLength + lineIndex;
                                    var colUnFlatten = columnIndex % originalLineLength;

                                    symbols.Add((symbolChar, lineUnFlatten, colUnFlatten));
                                }
                            }
                        }

                        var part = new PartWithAdjacents()
                        {
                            PartNumber = numberInt,
                            AdjacentSymbols = symbols.ToList(),
                        };
                        this.parts.Add(part);
                    }

                    number.Clear();
                }
            }
        }
    }

    private class PartWithAdjacents
    {
        public int PartNumber { get; set; }

        public List<(char Symbol, int LineIndex, int ColIndex)> AdjacentSymbols { get; set; } = new List<(char Symbol, int LineIndex, int ColIndex)>();

        public bool HasAdjacentSymbols => AdjacentSymbols.Any();
    }
}
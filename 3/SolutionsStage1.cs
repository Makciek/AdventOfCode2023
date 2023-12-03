namespace _3;

using Common;
using System.Drawing;

public class SolutionsStage1(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var engineSchema = new EngineSchema(this.Lines);
        var sum = engineSchema.GetSumfOfParts();
        return sum.ToString();
    }

    private class EngineSchema
    {
        private readonly char[][] originalInput;
        private readonly int lineCount;
        private readonly int colCount;

        private readonly List<PartWithAdjacents> parts = new List<PartWithAdjacents>();

        public EngineSchema(IEnumerable<string> lines)
        {
            this.originalInput = lines
                .Select(line => line.ToCharArray())
                .ToArray();

            this.lineCount = this.originalInput.Length;
            this.colCount = this.originalInput[0].Length;

            this.ParseCharsIntoParts();
        }

        public int GetSumfOfParts() => this.parts.Where(p => p.HasAdjacentSymbols).Sum(p => p.PartNumber);

        private void ParseCharsIntoParts()
        {
            var currentNumber = new List<char>();
            for (int line = 0; line < this.originalInput.Length; line++)
            {
                for (int col = 0; col < this.originalInput[line].Length; col++)
                {
                    var c = this.originalInput[line][col];
                    if (char.IsDigit(c))
                    {
                        currentNumber.Add(c);
                    }
                    else
                    {
                        parts.AddIfNotNull(GetNumberAdjacents(line, col, currentNumber));
                        currentNumber.Clear();
                    }
                }
            }
        }

        private PartWithAdjacents? GetNumberAdjacents(int line, int col, List<char> number)
        {
            if (!number.Any()) return null;

            var partNumber = Convert.ToInt32(new string(number.ToArray()));

            var indexOffset = 1;
            var numberStartPoint = (Line: line, Col: col - number.Count);
            var adjacentRegionWithoutValidation = (
                RegionStartLine: numberStartPoint.Line - 1,
                RegionStartCol: numberStartPoint.Col - 1, 
                RegionEndLine: numberStartPoint.Line + indexOffset + 1, 
                RegionEndCol: numberStartPoint.Col + number.Count + 1
            );

            var adjacentRegion = (
                RegionStartLine: adjacentRegionWithoutValidation.RegionStartLine > 0 ? adjacentRegionWithoutValidation.RegionStartLine : 0,
                RegionStartCol: adjacentRegionWithoutValidation.RegionStartCol > 0 ? adjacentRegionWithoutValidation.RegionStartCol : 0,
                RegionEndLine: adjacentRegionWithoutValidation.RegionEndLine <= this.lineCount ? adjacentRegionWithoutValidation.RegionEndLine : this.lineCount,
                RegionEndCol: adjacentRegionWithoutValidation.RegionEndCol <= this.colCount ? adjacentRegionWithoutValidation.RegionEndCol : this.colCount
            );

            return new PartWithAdjacents(partNumber, this.originalInput, adjacentRegion);
        }
    }

    private class PartWithAdjacents
    {
        public int PartNumber { get; set; }

        public List<char> AdjacentSymbols { get; set; } = new List<char>();

        public bool HasAdjacentSymbols => AdjacentSymbols.Any();

        public PartWithAdjacents(int parNumber, char[][] originalInput, (int RegionStartLine, int RegionStartCol, int RegionEndLine, int RegionEndCol) adjacentRegion)
        {
            PartNumber = parNumber;

            for (int line = adjacentRegion.RegionStartLine; line < adjacentRegion.RegionEndLine; line++)
            {
                for (int col = adjacentRegion.RegionStartCol; col < adjacentRegion.RegionEndCol; col++)
                {
                    var c = originalInput[line][col];
                    if (IsAdjacentSymbol(c))
                    {
                        this.AdjacentSymbols.Add(c);
                    }
                }
            }
        }

        private static bool IsAdjacentSymbol(char symbol)
        {
            return !char.IsDigit(symbol) && symbol != '.';
        }
    }
}
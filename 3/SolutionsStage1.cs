namespace _3;

using Common;

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
        private readonly AdjacentContinuousBuffer buffer;

        private readonly List<PartWithAdjacents> parts = new List<PartWithAdjacents>();

        public EngineSchema(IEnumerable<string> lines)
        {
            var linesArray = lines
                .Select(line => line.ToCharArray())
                .ToArray();
            this.buffer = new AdjacentContinuousBuffer(linesArray);
            this.ParseParts();
        }

        public int GetSumfOfParts() => this.parts.Where(p => p.HasAdjacentSymbols).Sum(p => p.PartNumber);

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
                        var numberStartAdjacentIndex = i - (number.Count + 1);
                        numberStartAdjacentIndex = numberStartAdjacentIndex < 0 ? 0 : numberStartAdjacentIndex;

                        var adjacentWindowLength = number.Count + 2; // +2 for adjacency
                        adjacentWindowLength = adjacentWindowLength > flattenInput.Count ? flattenInput.Count : adjacentWindowLength;

                        var symbols = flattenInput
                            .Slice(numberStartAdjacentIndex, adjacentWindowLength)
                            .SelectMany(col => col.Where(c => c != '.' && !char.IsDigit(c))).ToList();

                        var part = new PartWithAdjacents()
                        {
                            PartNumber = Convert.ToInt32(new string(number.ToArray())),
                            AdjacentSymbols = symbols
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
        
        public List<char> AdjacentSymbols { get; set; } = new List<char>();

        public bool HasAdjacentSymbols => AdjacentSymbols.Any();
    }
}
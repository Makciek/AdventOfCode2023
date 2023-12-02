namespace _2;

using Common;

public class Solution : SolutionBase
{
    public Solution(string fileName) : base(fileName)
    {
    }

    protected override string GetResult()
    {
        var games = ParseLinesIntoGame(this.Lines);
        return GetSumOfPowers(games).ToString();
    }

    private int GetSumOfPowers(List<Game> games) => games.Sum(g => g.GetMultipliedMinimalPossibleSet());

    private int GetSumOfIds(List<Game> games) => games.Sum(g => g.Id);

    private List<Game> GetPossibleGames(List<Game> games, int reds, int greens, int blues)
        => games
            .Where(g => g.IsPossible(reds, greens, blues))
            .ToList();

    private List<Game> ParseLinesIntoGame(List<string> lines) =>
        lines
            .Select(l =>
            {
                var colonSplit = l.Split(':');
                var semicolonSplit = colonSplit[1].Split(';');

                return new Game(
                    Id: colonSplit[0].Replace("Game ", string.Empty).ToInt() ?? throw new Exception(),
                    SubGames: semicolonSplit
                        .Select(l =>
                        {
                            var colors = l.Split(',');
                            var red = colors.SingleOrDefault(c => c.Contains("red"))?.Replace(" red", string.Empty);
                            var green = colors.SingleOrDefault(c => c.Contains("green"))?.Replace(" green", string.Empty);
                            var blue = colors.SingleOrDefault(c => c.Contains("blue"))?.Replace(" blue", string.Empty);

                            return new CubeSet(red.ToInt(), green.ToInt(), blue.ToInt());
                        })
                        .ToList()
                );
            })
            .ToList();

    private class Game(int Id, List<CubeSet> SubGames)
    {
        public int Id { get; private set; } = Id;
        public List<CubeSet> SubGames { get; private set; } = SubGames;

        public bool IsPossible(int reds, int greens, int blues) => GetMaxRed() <= reds && GetMaxGreen() <= greens && GetMaxBlue() <= blues;

        public CubeSet GetMinimalPossibleSet() => new CubeSet(this.GetMaxRed(), this.GetMaxGreen(), this.GetMaxBlue());

        public int GetMultipliedMinimalPossibleSet()
        {
            var minimalPossibleSet = this.GetMinimalPossibleSet();
            var power = (minimalPossibleSet.Red ?? 1)
                * (minimalPossibleSet.Green ?? 1)
                * (minimalPossibleSet.Blue ?? 1);
            return power;
        }

        int GetMaxRed() => SubGames.Max(sg => sg.Red ?? 0);
        int GetMaxGreen() => SubGames.Max(sg => sg.Green ?? 0);
        int GetMaxBlue() => SubGames.Max(sg => sg.Blue ?? 0);
    }

    private record CubeSet(int? Red, int? Green, int? Blue);
}
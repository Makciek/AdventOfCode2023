namespace _4;

using Common;

public class SolutionsStage1(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var cards = this.Lines.Select(l => new Card(l)).ToList();
        var totalPoints = cards.Sum(c => c.GetPoints());
        return totalPoints.ToString();
    }

    private class Card
    {
        public int CardNo { get; set; }

        public List<int> Winning { get; set; }

        public List<int> Found { get; set; }

        public List<int> Matches { get; set; }

        public Card(string line)
        {
            var colonSplit = line.Split(':');
            var verticalSplit = colonSplit[1].Split('|');

            CardNo = int.Parse(colonSplit[0].Replace("Card ", string.Empty));
            Winning = verticalSplit[0].Split(' ').Where(s => s != string.Empty).Select(int.Parse).ToList();
            Found = verticalSplit[1].Split(' ').Where(s => s != string.Empty).Select(int.Parse).ToList();

            Matches = Winning.Intersect(Found).ToList();
        }

        public int GetPoints()
        {
            if (!Matches.Any())
            {
                return 0;
            }

            return (int)Math.Pow(2, Matches.Count - 1);
        }
    }
}
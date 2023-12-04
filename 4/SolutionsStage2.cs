namespace _4;

using Common;

public class SolutionsStage2(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var cards = this.Lines.Select(l => new Card(l)).ToArray();

        ProcessCardCoping(cards);

        var totalCards = cards.Sum(c => c.Copies);
        return totalCards.ToString();
    }

    private void ProcessCardCoping(Card[] cards)
    {
        var cardCount = cards.Length;
        for (int cardIndex = 0; cardIndex < cardCount; cardIndex++)
        {
            var cardCopies = cards[cardIndex].Copies;
            for (int copyNumberIndex = 0; copyNumberIndex < cardCopies; copyNumberIndex++)
            {
                var matchesLength = cards[cardIndex].Matches.Count;
                for (int nextCardIndex = cardIndex + 1; nextCardIndex <= cardIndex + matchesLength; nextCardIndex++) // cardIndex + matchesLength is guaranteed to never exceed cardCount
                {
                    cards[nextCardIndex].Copies++;
                }
            }
        }
    }

    private class Card
    {
        public int CardNo { get; set; }

        public List<int> Winning { get; set; }

        public List<int> Found { get; set; }

        public List<int> Matches { get; set; }

        public int Copies { get; set; } = 1;

        public Card(string line)
        {
            var colonSplit = line.Split(':');
            var verticalSplit = colonSplit[1].Split('|');

            CardNo = int.Parse(colonSplit[0].Replace("Card ", string.Empty));
            Winning = verticalSplit[0].Split(' ').Where(s => s != string.Empty).Select(int.Parse).ToList();
            Found = verticalSplit[1].Split(' ').Where(s => s != string.Empty).Select(int.Parse).ToList();

            Matches = Winning.Intersect(Found).ToList();
        }
    }
}
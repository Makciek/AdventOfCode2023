namespace _7;
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
        var hands = this.Lines.Select(l => new Hand1(l)).ToList();
        hands.Sort(handComparer);

        var result = GetTotalWinnings(hands);

        return result.ToString();
    }

    private long GetTotalWinnings(List<Hand1> handsSorted)
    {
        long totalWinnings = 0;
        for (int i = 0; i < handsSorted.Count; i++)
        {
            var rank = i + 1;
            totalWinnings += handsSorted[i].Bid * rank;
        }
        return totalWinnings;
    }

    private static Comparer<Hand1> handComparer = Comparer<Hand1>.Create((x, y) =>
    {
        if (x.Type > y.Type)
            return 1;
        if (x.Type < y.Type)
            return -1;

        for (int i = 0; i < x.HandCards.Count; i++)
        {
            if (x.HandCardPowers[i] > y.HandCardPowers[i])
            {
                return 1;
            }
            else if (x.HandCardPowers[i] < y.HandCardPowers[i])
            {
                return -1;
            }
        }

        return 0;
    });
}

public class Hand1
{
    private static string labelsInOrderOfPower = "AKQJT98765432";
    private static Dictionary<char, int> cardLabelToPowerDictionary =
        labelsInOrderOfPower
            .Select((c, i) => new { c, power = labelsInOrderOfPower.Length - i })
            .ToDictionary(x => x.c, x => x.power);

    private readonly List<Card> cards = new List<Card>();

    public List<char> HandCards { get; init; } = new List<char>();
    public List<int> HandCardPowers { get; init; } = new List<int>();
    public HandType Type { get; init; }

    public int Bid { get; set; }

    public Hand1(string cardsString)
    {
        var cardsStringSplit = cardsString.Split(" ");
        var cardLabels = cardsStringSplit[0];

        Bid = int.Parse(cardsStringSplit[1]);

        foreach (var cardLabel in cardLabels)
        {
            var card = this.cards.FirstOrDefault(c => c.Label == cardLabel);
            if (card != null)
            {
                card.Count++;
            }
            else
            {
                this.cards.Add(new Card() { Label = cardLabel, Count = 1 });
            }

            HandCards.Add(cardLabel);
            HandCardPowers.Add(cardLabelToPowerDictionary[cardLabel]);
        }

        var cardsCount = this.cards.Count;
        if (cardsCount == 1) // 5
        {
            Type = HandType.FiveOfAKind;
            return;
        }

        var maxCardsCount = this.cards.MaxBy(c => c.Count)!;
        if (maxCardsCount.Count == 4) // 4
        {
            Type = HandType.FourOfAKind;
            return;
        }

        if (cardsCount == 2 && maxCardsCount.Count == 3) // Full house
        {
            Type = HandType.FullHouse;
            return;
        }

        if (maxCardsCount.Count == 3) // 3
        {
            Type = HandType.ThreeOfAKind;
            return;
        }

        var pairsCount = this.cards.Count(c => c.Count == 2);
        if (pairsCount != 0) // 1 & 2 pairs
        {
            Type = (HandType)pairsCount;
            return;
        }

        Type = HandType.HighCard;
    }

    private class Card
    {
        public char Label { get; set; }
        public int Count { get; set; }
    }
}

public enum HandType
{
    HighCard = 0,
    Pair = 1,
    TwoPairs = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6,
}

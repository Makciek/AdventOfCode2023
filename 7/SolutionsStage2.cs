namespace _7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Diagnostics;

public class SolutionsStage2(string fileName) : SolutionBase(fileName)
{
    protected override string GetResult()
    {
        var hands = this.Lines.Select(l => new Hand2(l)).ToList();
        hands.Sort(handComparer);

        var result = GetTotalWinnings(hands);

        return result.ToString();
    }

    private long GetTotalWinnings(List<Hand2> handsSorted)
    {
        long totalWinnings = 0;
        for (int i = 0; i < handsSorted.Count; i++)
        {
            var rank = i + 1;
            totalWinnings += handsSorted[i].Bid * rank;
        }
        return totalWinnings;
    }

    private static Comparer<Hand2> handComparer = Comparer<Hand2>.Create((x, y) =>
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

[DebuggerDisplay("{Type} {DebuggerCardsJoker} {DebuggerCards}")]
public class Hand2
{
    private static string labelsInOrderOfPower = "AKQT98765432J";
    private static Dictionary<char, int> cardLabelToPowerDictionary =
        labelsInOrderOfPower
            .Select((c, i) => new { c, power = labelsInOrderOfPower.Length - i })
            .ToDictionary(x => x.c, x => x.power);

    private readonly List<Card> cards = new List<Card>();
    public string DebuggerCardsJoker => string.Join(" ", cards.Select(c => $"{c.Label}({c.CountWithJokers})"));
    public string DebuggerCards => string.Join("", HandCards);

    public List<char> HandCards { get; init; } = new List<char>();
    public List<int> HandCardPowers { get; init; } = new List<int>();
    public HandType2 Type { get; init; }

    public int Bid { get; set; }

    public Hand2(string cardsString)
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

        var jokerCard = this.cards.SingleOrDefault(c => c.Label == 'J');
        var jokerCount = jokerCard?.Count ?? 0;
        if (jokerCard is not null)
        {
            jokerCard.CountWithJokers = jokerCount;
        }

        foreach (var nonJokerCard in this.cards.Where(c => c.Label != 'J'))
        {
            nonJokerCard.CountWithJokers = nonJokerCard.Count + jokerCount;
        }

        var cardsByJokerCount = this.cards.OrderByDescending(c => c.CountWithJokers).ToList();
        if (cardsByJokerCount[0].CountWithJokers == 5) // 5
        {
            Type = HandType2.FiveOfAKind;
            return;
        }
        if (cardsByJokerCount[0].CountWithJokers == 4) // 4
        {
            Type = HandType2.FourOfAKind;
            return;
        }
        if ((cardsByJokerCount[0].Count == 3 && cardsByJokerCount[1].Count == 2) ||
            (jokerCount == 1 && cardsByJokerCount[0].CountWithJokers == 3 && cardsByJokerCount[1].Count == 2)||
            (jokerCount == 1 && cardsByJokerCount[0].Count == 3 && cardsByJokerCount[1].CountWithJokers == 2)||
            (jokerCount == 2 && cardsByJokerCount[0].CountWithJokers == 3 && cardsByJokerCount[1].CountWithJokers == 2)) // Full house
        {
            Type = HandType2.FullHouse;
            return;
        }
        if (cardsByJokerCount[0].CountWithJokers == 3) // 3
        {
            Type = HandType2.ThreeOfAKind;
            return;
        }

        var pairsCount = this.cards.Count(c => c.CountWithJokers == 2);
        if (pairsCount != 0) // 1 & 2 pairs
        {
            var pairsWithoutJoker = this.cards.Count(c => c.Count == 2);
            if (pairsWithoutJoker == 0 && jokerCount == 1)
            {
                Type = HandType2.Pair;
            }
            else if (pairsWithoutJoker == 1 && jokerCount == 0)
            {
                Type = HandType2.Pair;
            }
            else
            {
                Type = HandType2.TwoPairs;
            }
            return;
        }

        Type = HandType2.HighCard;
    }

    private class Card
    {
        public char Label { get; set; }
        public int Count { get; set; }
        public int CountWithJokers { get; set; }
    }
}

public enum HandType2
{
    HighCard = 0,
    Pair = 1,
    TwoPairs = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6,
}

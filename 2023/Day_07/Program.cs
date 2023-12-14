// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var listOfHands = new List<HandState>();
foreach (var line in file)
{
    var segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var hand = new HandState()
    {
        Hand = segments[0],
        BidAmount = int.Parse(segments[1])
    };
    listOfHands.Add(hand);
}

var sortedHands = listOfHands.Order().ToArray();
var stringBuilder = new StringBuilder();
for (var i = 0; i < sortedHands.Count(); i++)
{
    sortedHands[i].Winnings = sortedHands[i].BidAmount * (i + 1);
    stringBuilder.AppendLine(
        $"hand: {sortedHands[i].Hand}, type: {sortedHands[i].GetHandType()}, rank: {i + 1}, bid: {sortedHands[i].BidAmount}, winnings: {sortedHands[i].Winnings}");
}

File.WriteAllText(@"C:\Dev\clia042\advent-of-code\2023\Day_07\output.txt", stringBuilder.ToString());

Console.WriteLine(sortedHands.Sum(x => x.Winnings));
Console.WriteLine("Hello, World!");
//250045319 too low :(
//250531671 too high
//250506580

public class HandState : IComparable
{
    public string Hand { get; init; } = null!;
    public int BidAmount { get; init; }

    public long Winnings { get; set; }

    public HandType GetHandType()
    {
        var uniques = Hand.GroupBy(c => c, c => c);
        var standardType = GetStandardHandType(uniques);
        var jokers = uniques.FirstOrDefault(c => c.Key == 'J')?.Count() ?? 0;

        return (jokers, standardType) switch
        {
            (4, HandType.FourOfAKind) => HandType.FiveOfAKind,
            (3, HandType.FullHouse) => HandType.FiveOfAKind,
            (3, HandType.ThreeOfAKind) => HandType.FourOfAKind,
            (2, HandType.FullHouse) => HandType.FiveOfAKind,
            (2, HandType.TwoPair) => HandType.FourOfAKind,
            (2, HandType.OnePair) => HandType.ThreeOfAKind,
            (1, HandType.FourOfAKind) => HandType.FiveOfAKind,
            (1, HandType.ThreeOfAKind) => HandType.FourOfAKind,
            (1, HandType.TwoPair) => HandType.FullHouse,
            (1, HandType.OnePair) => HandType.ThreeOfAKind,
            (1, HandType.HighCard) => HandType.OnePair,
            _ => standardType
        };
    }

    HandType GetStandardHandType(IEnumerable<IGrouping<char, char>>? uniques)
    {
        if (uniques.Count() == 1)
        {
            return HandType.FiveOfAKind;
        }

        if (uniques.Any(x => x.Count() == 4))
        {
            return HandType.FourOfAKind;
        }

        if (uniques.Any(x => x.Count() == 3))
        {
            if (uniques.Any(x => x.Count() == 2))
            {
                return HandType.FullHouse;
            }
            else
            {
                return HandType.ThreeOfAKind;
            }
        }

        var pairs = uniques.Count(x => x.Count() == 2);
        return pairs switch
        {
            0 => HandType.HighCard,
            1 => HandType.OnePair,
            2 => HandType.TwoPair
        };
    }

    public int CompareTo(object? obj)
    {
        var cardStrength = new Dictionary<char, int>()
        {
            { 'J', 1 },
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 }
        };

        if (obj is not HandState otherHand)
        {
            throw new NotSupportedException();
        }

        var currentHandStrength = (int)GetHandType();
        var otherHandStrength = (int)otherHand.GetHandType();
        if (currentHandStrength != otherHandStrength)
        {
            return currentHandStrength < otherHandStrength ? -1 : 1;
        }

        for (var i = 0; i < 5; i++)
        {
            var currentCardStrength = cardStrength[Hand[i]];
            var otherCardStrength = cardStrength[otherHand.Hand[i]];

            if (currentCardStrength == otherCardStrength) continue;

            return currentCardStrength < otherCardStrength ? -1 : 1;
        }

        throw new InvalidOperationException();
    }
}

public enum HandType
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7,
}
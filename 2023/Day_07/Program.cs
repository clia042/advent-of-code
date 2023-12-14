// See https://aka.ms/new-console-template for more information
using System.Text;

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

public class HandState : IComparable
{
    public string Hand { get; init; } = null!;
    public int BidAmount { get; init; }

    public long Winnings { get; set; }

    public HandType GetHandType()
    {
        var uniques = Hand.GroupBy(c => c, c => c);

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
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'J', 11 },
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
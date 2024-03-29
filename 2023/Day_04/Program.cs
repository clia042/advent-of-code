﻿// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = System.IO.File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var cardCollection = new List<Card>();
foreach (var line in file)
{
    cardCollection.Add(new Card(line));
}

Console.WriteLine("Hello, World!");

foreach (var card in cardCollection)
{
    var matches = card.MatchingNumbers;

    if (matches == 0)
    {
        continue;
    }

    for (var i = matches; i > 0; i--)
    {
        var cardNo = card.CardNumber + i;
        var nextCard = cardCollection.First(x => x.CardNumber == cardNo);
        nextCard.AddCopies(card.Count);
    }
}

Console.WriteLine(cardCollection.Sum(x => x.Count));

public class Card
{
    public Card(string line)
    {
        var segments = line.Split(':');
        CardNumber = int.Parse(segments[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
        Numbers = segments[1].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
            .ToList();
        WinningNumbers = segments[1].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
            .ToList();
        Count = 1;
    }

    public int CardNumber { get; set; }
    public List<int> Numbers { get; set; }
    public List<int> WinningNumbers { get; set; }
    
    public int Count { get; set; }

    public int Points()
    {
        var nums = Numbers.Intersect(WinningNumbers);

        if (nums.Any() == false)
        {
            return 0;
        }

        var points = 1;
        for (var i = 1; i < nums.Count(); i++)
        {
            points = points * 2;
        }

        return points;
    }

    public int MatchingNumbers => Numbers.Intersect(WinningNumbers).Count();

    public void AddCopies(int copies)
    {
        Count = Count + copies;
    }
}
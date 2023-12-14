// See https://aka.ms/new-console-template for more information

// distance = (race_time - hold_time) * (hold_time * multiplier "1")

var useSample = false;
var file = System.IO.File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var races = new List<RaceState>();
foreach (var line in file)
{
    var segment = line.Split(':');
    var raceLines = segment[1]
        .Trim()
        .Split(' ', StringSplitOptions.RemoveEmptyEntries);
    switch (segment[0])
    {
        case "Time":
            races = raceLines
                .Select(x => new RaceState()
                {
                    Time = int.Parse(x)
                })
                .ToList();
            break;
        case "Distance":
            for (var i = 0; i< raceLines.Length; i++)
            {
                races[i].Distance = int.Parse(raceLines[i]);
            }
            break;
    }
}

var winningOptions = new List<int>();
foreach (var race in races)
{
    var results = new List<int>();
    for (var h = 0; h < race.Time; h++)
    {
        results.Add(race.GetDistanceFromHoldTime(h));
    }
    winningOptions.Add(results.Count(x => x > race.Distance));
}

Console.WriteLine(winningOptions.Aggregate((a,b) => a * b));
Console.WriteLine("Hello, World!");

public class RaceState
{
    public int Time { get; set; }
    public int Distance { get; set; }

    public int GetDistanceFromHoldTime(int holdTime) => (Time - holdTime) * holdTime;
}
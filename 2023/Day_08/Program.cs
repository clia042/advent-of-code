// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input3.txt" : "input.txt");

var network = ParseNetwork(file);
var directionList = file[0];

var paths = GetStartingPaths(network);
var distances = new Dictionary<string, int>();

foreach (var start in paths)
{
    distances.Add(start, GetTraversalDistance(start));
}

Console.WriteLine($"Hello, World! part 2: {lcm(distances.Select(x => (long)x.Value).ToList())}");
return;
//8245452805243

static long gcd(long n1, long n2)
{
    if (n2 == 0) return n1;

    return gcd(n2, n1 % n2);
}

static long lcm(List<long> numbers)
{
    return numbers.Aggregate((s, val) => s * val / gcd(s, val));
}

int GetTraversalDistance(string start)
{
    var i = 0;
    var currentNode = start;
    while (currentNode.EndsWith('Z') == false)
    {
        var node = network[currentNode];
        currentNode = GetNextDirection(i, directionList) switch
        {
            'L' => node[0],
            'R' => node[1],
            _ => throw new NotSupportedException()
        };
        i++;
    }

    return i;
}

char GetNextDirection(int x, string l) => l[x % l.Length];

Dictionary<string, string[]> ParseNetwork(string[] file)
{
    var network = new Dictionary<string, string[]>();
    foreach (var line in file.Skip(2))
    {
        var segment = line.Split('=');
        network.Add(
            segment[0].Trim(),
            segment[1]
                .Split(',')
                .Select(x => new string(x.Where(char.IsLetterOrDigit).ToArray()))
                .ToArray());
    }

    return network;
}

List<string> GetStartingPaths(Dictionary<string, string[]> network)
{
    return network.Keys.Where(k => k.EndsWith('A')).ToList();
}
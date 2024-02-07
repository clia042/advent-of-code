// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var memoizeCollection = new Dictionary<string, long>();
var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var count = 0l;

foreach (var line in file)
{
    var lineSegments = Part2Modifier(line.Split(' '));

    var pattern = $".{lineSegments[0]}.";
    var sets = lineSegments[1].Split(',').Select(long.Parse).ToArray();

    var result = CalculateMatches(pattern, sets);
    count += result;
    Console.WriteLine($"{pattern} - {result}");
}

Console.Write(count);
Console.Read();
//8580 too high;
//1557394403 too low;
//1738259948652

long CalculateMatches(string pattern, long[] sets)
{
    var key = GetPatternSetKey(pattern, sets);
    if (memoizeCollection.ContainsKey(key))
    {
        return memoizeCollection[key];
    }

    var i = 0l;

    //Invalid match
    if (sets.Length == 0)
    {
        i = pattern.Contains("#") ? 0 : 1;
        return SaveAndReturn(key, i);
    }

    var firstSet = sets[0];
    var regexPattern = $"(?=([\\?\\.][\\?\\#]{{{firstSet}}}[\\?\\.]))";

    var matches = Regex.Matches(pattern, regexPattern);

    if (matches.Count == 0)
    {
        return SaveAndReturn(key, 0);
    }

    foreach (Match match in matches)
    {
        var prePattern = pattern.Substring(0, match.Groups[1].Index);
        if (prePattern.Contains("#"))
        {
            continue;
        }

        var subPattern = "." + pattern.Substring(match.Groups[1].Index + match.Groups[1].Length);
        var subSet = sets.Skip(1);

        var matchCount = CalculateMatches(subPattern, subSet.ToArray());

        if (matchCount > 0)
        {
            i += matchCount;
        }
    }

    return SaveAndReturn(key, i);
}

string GetPatternSetKey(string pattern, long[] sets) => $"{pattern}+[{string.Join(",", sets)}]";

long SaveAndReturn(string key, long result)
{
    memoizeCollection.Add(key, result);
    return result;
}

string[] Part2Modifier(string[] segments)
{
    var pattern = segments[0];
    var sets = segments[1];

    return new[]
    {
       string.Join("?", Enumerable.Range(0,5).Select(i => pattern)),
       string.Join(",", Enumerable.Range(0,5).Select(i => sets))
    };
}
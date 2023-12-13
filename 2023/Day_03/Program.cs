// See https://aka.ms/new-console-template for more information

var useSample = false;
var flagPart2 = true;
var file = System.IO.File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

Dictionary<string, int> partNumbers = new();
Dictionary<string, char> parts = new();

var yMax = file.Length;
var xMax = file[0].Length;

for (var y = 0; y < yMax; y++)
{
    var line = file[y];
    for (var x = 0; x < xMax; x++)
    {
        var value = line[x];
        if (char.IsDigit(value))
        {
            var partNumber = ReadPartNumber(x, line);
            partNumbers.Add($"{x},{y}", partNumber);
        }
        else if (value != '.')
        {
            parts.Add($"{x},{y}", value);
        }
    }
}

List<int> actualPartNumbers = new();

foreach (var part in parts)
{
    if (flagPart2 && part.Value == '*')
    {
        var adjacentPartNumbers = GetPartNumbers(part.Key);
        if (adjacentPartNumbers.Count == 2)
        {
            var ratio = adjacentPartNumbers[0] * adjacentPartNumbers[1];
            actualPartNumbers.Add(ratio);
        }
    }
    else if(!flagPart2)
    {
        var adjacentPartNumbers = GetPartNumbers(part.Key);
        actualPartNumbers.AddRange(adjacentPartNumbers);
    }
}

Console.WriteLine("Hello, World!");
Console.WriteLine(actualPartNumbers.Sum());

List<int> GetPartNumbers(string startPoint)
{
    var coordinates = startPoint.Split(',');
    var x = int.Parse(coordinates[0]);
    var y = int.Parse(coordinates[1]);

    var list = new List<int>();

    foreach (var yp in new[] { y - 1, y, y + 1 })
    {
        if (yp < 0 || yp >= yMax)
        {
            continue;
        }

        var prevNum = -1;
        foreach (var xp in new[] { x - 1, x, x + 1 })
        {
            if (xp < 0 || xp >= xMax)
            {
                continue;
            }

            if (partNumbers.TryGetValue($"{xp},{yp}", out var partNum))
            {
                if (prevNum != partNum)
                {
                    list.Add(partNum);
                    prevNum = partNum;
                }

                partNumbers.Remove($"{xp},{yp}");
            }
        }
    }

    return list;
}

int ReadPartNumber(int i, string line)
{
    var pointer = i;

    for (var p = i; p >= 0; p--)
    {
        if (char.IsDigit(line[p]) == false)
        {
            break;
        }

        pointer = p;
    }

    return int.Parse(new string(line.Substring(pointer).TakeWhile(x => char.IsDigit(x)).ToArray()));
}
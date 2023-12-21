// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");
var expanseMultiplier = 1000000;
//var matrix = ExpandUniverse(file);

var rows = ExpandRows(file);
var columns = ExpandColumns(file);

var galaxyList = ParseGalaxies(file);

var distances = CalculateDistances(galaxyList);

Dictionary<string, long> CalculateDistances(List<(int x, int y)> galaxies)
{
    var distanceList = new Dictionary<string, long>();
    var i = 1;
    foreach (var galaxy in galaxies)
    {
        var sublist = galaxies.Skip(i);
        if (sublist.Any() == false)
        {
            break;
        }

        var p = i + 1;
        foreach (var nextGalaxy in sublist)
        {
            var xSum = Math.Abs(galaxy.x - nextGalaxy.x);
            var ySum = Math.Abs(galaxy.y - nextGalaxy.y);

            var linesCrossed = ExpansesCrossed(rows, galaxy.y, nextGalaxy.y) +
                               ExpansesCrossed(columns, galaxy.x, nextGalaxy.x);

            distanceList.Add($"{i}-{p}", (xSum + ySum) + (linesCrossed * (expanseMultiplier - 1)));
            p++;
        }

        i++;
    }

    return distanceList;
}

Console.WriteLine($"Hello, World! {distances.Values.Sum()}");

List<(int x, int y)> ParseGalaxies(string[] file)
{
    var list = new List<(int x, int y)>();

    for (var y = 0; y < file.Length; y++)
    {
        var line = file[y];
        for (var x = 0; x < line.Length; x++)
        {
            if (line[x] == '#')
            {
                list.Add((x, y));
            }
        }
    }

    return list;
}

string[] ExpandUniverse(string[] file)
{
    var rows = file.ToList();

    //Expand rows
    for (var y = 0; y < rows.Count; y++)
    {
        if (rows[y].All(x => x == '.'))
        {
            rows.Insert(y, rows[y]);
            y++;
        }
    }

    //Expand columns
    for (var x = 0; x < rows[0].Length; x++)
    {
        if (rows.All(row => row[x] == '.'))
        {
            rows = rows.Select(row => row.Insert(x, ".")).ToList();
            x++;
        }
    }

    File.WriteAllLines(@"C:\Dev\clia042\advent-of-code\2023\Day_11\expanded_input.txt", rows);

    return rows.ToArray();
}

List<int> ExpandRows(string[] file)
{
    var rows = new List<int>();
    //Expand rows
    for (var y = 0; y < file.Length; y++)
    {
        if (file[y].All(x => x == '.'))
        {
            rows.Add(y);
        }
    }

    return rows;
}

List<int> ExpandColumns(string[] file)
{
    var columns = new List<int>();
    //Expand columns
    for (var x = 0; x < file[0].Length; x++)
    {
        if (file.All(row => row[x] == '.'))
        {
            columns.Add(x);
        }
    }

    return columns;
}

int ExpansesCrossed(List<int> list, int a, int b)
{
    var minPosition = Math.Min(a, b);
    var maxPosition = Math.Max(a, b);

    return list.Count(x => minPosition < x && maxPosition > x);
}
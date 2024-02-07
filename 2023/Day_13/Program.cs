// See https://aka.ms/new-console-template for more information

using System.Text;

var allowedDiff = 1;
var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var puzzles = ParseInput(file);
var sum = 0;

foreach (var puzzle in puzzles)
{
    //Console.WriteLine(puzzle);
    var result = FindMirror(puzzle);
    Console.WriteLine($"result - {result}");
    sum += result;
}

Console.WriteLine($"Hello, World! {sum}");
Console.Read();

//38770 too low - part 2
//44615

List<Puzzle> ParseInput(string[] file)
{
    var puzzles = new List<Puzzle>();

    var puzzle = new Puzzle();
    foreach (var line in file)
    {
        if (string.IsNullOrEmpty(line))
        {
            puzzles.Add(puzzle);
            puzzle = new Puzzle();
            continue;
        }

        puzzle.rows.Add(line);
    }
    puzzles.Add(puzzle);

    return puzzles;
}

int FindMirror(Puzzle puzzle)
{
    var result = ScanForMirror(puzzle.rows);

    // Horizontal line found
    if (result != null)
    {
        return result.Value * 100;

    }

    //Console.WriteLine(puzzle.ToString());
    // Transpose puzzle by 90 degrees anti-clockwise
    puzzle.Rotate();
    //Console.WriteLine(puzzle.ToString());

    result = ScanForMirror(puzzle.rows);
    // No Vertical line found
    if (result == null)
    {
        Console.WriteLine("No mirror found :(");
        return 0;
    }

    return result.Value;
}

int? ScanForMirror(List<string> rows)
{
    for (var i = 0; i < rows.Count - 1; i++)
    {
        if (RowDiff(rows[i], rows[i + 1]) > allowedDiff)
        {
            continue;
        }

        //Possible mirror location
        var idx = RippleScan(rows, i);
        if (idx != null)
        {
            return idx;
        }
    }

    // no match found
    return null;
}

int? RippleScan(List<string> rows, int i)
{
    var a = i;
    var b = i + 1;
    var misMatch = false;
    var smudgesFound = 0;
    while (a >= 0 && b < rows.Count)
    {
        var diff = RowDiff(rows[a], rows[b]);
        if (diff > allowedDiff)
        {
            misMatch = true;
            break;
        }
        //Matching so far
        if (diff == 1)
        {
            smudgesFound++;
        }
        a--;
        b++;
    }

    return (misMatch || smudgesFound != 1) ? null : i + 1;
}

int RowDiff(string rowA, string rowB)
{
    var count = 0;
    for (var i = 0; i < rowA.Length; i++)
    {
        if (rowA[i] != rowB[i])
        {
            count++;
        }
    }

    return count;
}

class Puzzle
{
    public List<string> rows { get; set; } = new();

    public void Rotate()
    {
        var columns = new List<string>();

        for (var i = rows.First().Length - 1; i >= 0; i--)
        {
            columns.Add(new string(rows.Select(x => x[i]).ToArray()));
        }
        columns.Reverse();

        rows = columns;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var row in rows)
        {
            builder.AppendLine(row.ToString());
        }

        return builder.ToString();
    }
}

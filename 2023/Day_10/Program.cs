// See https://aka.ms/new-console-template for more information

using System.Text;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input_b.txt" : "input.txt");

var start = (x: -1, y: -1);
var map = BuildMatrix(file);

var cursors = GetPaths(start, map);

var i = 0;
map[start.x, start.y] = $"*";

while (cursors.Any())
{
    i++;
    var newCursor = new List<MatrixCursor>();
    foreach (var cursor in cursors)
    {
        if (NextStep(map, cursor, i))
        {
            newCursor.Add(cursor);
        }
    }

    cursors = newCursor;
}

PrintMap(map);

Console.WriteLine($"Hello, World! {i - 1}");
return;

void PrintMap(string[,] matrix)
{
    var builder = new StringBuilder();
    for (var y = 0; y < matrix.GetLength(0); y++)
    {
        for (int x = 0; x < matrix.GetLength(1); x++)
        {
            builder.Append(matrix[x, y]);
        }

        builder.AppendLine();
    }

    File.WriteAllText(@"C:\Dev\clia042\advent-of-code\2023\Day_10\output.txt", builder.ToString());
}

List<MatrixCursor> GetPaths((int x, int y) start, string[,] matrix)
{
    var directions = new List<MatrixCursor>()
    {
        new() { X = start.x, Y = start.y - 1, Direction = Direction.Up },
        new() { X = start.x, Y = start.y + 1, Direction = Direction.Down },
        new() { X = start.x - 1, Y = start.y, Direction = Direction.Left },
        new() { X = start.x + 1, Y = start.y, Direction = Direction.Right },
    };

    return directions
        .Where(x => NextStep(map, x))
        .ToList();
}

string[,] BuildMatrix(IReadOnlyList<string> file)
{
    var xMax = file[0].Length;
    var yMax = file.Count;
    var matrix = new string[xMax, yMax];

    for (var y = 0; y < yMax; y++)
    {
        var line = file[y];
        for (var x = 0; x < xMax; x++)
        {
            matrix[x, y] = line[x].ToString();

            if (line[x] == 'S')
            {
                start = (x, y);
            }
        }
    }

    return matrix;
}

bool NextStep(string[,] matrix, MatrixCursor cursor, int? step = null)
{
    if (cursor.X >= matrix.GetLength(0) ||
        cursor.Y >= matrix.GetLength(1) ||
        cursor.X < 0 || cursor.Y < 0)
    {
        return false;
    }

    var currentLocation = matrix[cursor.X, cursor.Y];
    var nextDirection = GetNextDirection(cursor.Direction, currentLocation);
    if (nextDirection == Direction.Stop)
    {
        return false;
    }

    if (step == null)
    {
        return true;
    }

    matrix[cursor.X, cursor.Y] = "*";

    cursor.X = nextDirection switch
    {
        Direction.Left => cursor.X - 1,
        Direction.Right => cursor.X + 1,
        _ => cursor.X
    };
    cursor.Y = nextDirection switch
    {
        Direction.Up => cursor.Y - 1,
        Direction.Down => cursor.Y + 1,
        _ => cursor.Y
    };
    cursor.Direction = nextDirection;

    return true;
}

Direction GetNextDirection(Direction current, string nextValue)
{
    return (current, nextValue[0]) switch
    {
        (Direction.Up, '7') => Direction.Left,
        (Direction.Up, 'F') => Direction.Right,
        (Direction.Up, '|') => Direction.Up,
        (Direction.Down, 'J') => Direction.Left,
        (Direction.Down, 'L') => Direction.Right,
        (Direction.Down, '|') => Direction.Down,
        (Direction.Left, 'L') => Direction.Up,
        (Direction.Left, 'F') => Direction.Down,
        (Direction.Left, '-') => Direction.Left,
        (Direction.Right, 'J') => Direction.Up,
        (Direction.Right, '7') => Direction.Down,
        (Direction.Right, '-') => Direction.Right,
        _ => Direction.Stop
    };
}

public class MatrixCursor
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }
}

public enum Direction
{
    Stop,
    Up,
    Down,
    Left,
    Right,
}
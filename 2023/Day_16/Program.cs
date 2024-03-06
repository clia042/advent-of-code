// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Text;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var matrix = ParseInput(file);

var beams = new List<Beam> { new(Direction.Right, new Point(0, 0)) };

while (beams.Any())
{
    var newBeams = new List<Beam>();
    foreach (var beam in beams)
    {
        var tile = matrix[beam.Position.X, beam.Position.Y];
        newBeams.AddRange(tile.EnergizeTile(beam));
    }

    beams = newBeams
        .Select(b => b.Advance(matrix.GetLength(0), matrix.GetLength(1)))
        .Where(b => b != null)
        .ToList();
}

var counter = 0;
var builder = new StringBuilder();
for (var y = 0; y < matrix.GetLength(1); y++)
{
    for (var x = 0; x < matrix.GetLength(0); x++)
    {
        var tile = matrix[x, y];
        builder.Append(tile);
        if (tile.Energized)
        {
            counter++;
        }
    }

    builder.AppendLine();
}
Console.WriteLine(builder.ToString());

Console.WriteLine($"hello world {counter}");

Tile[,] ParseInput(string[] file)
{
    var xMax = file.First().Length;
    var yMax = file.Length;
    var matrix = new Tile[xMax, yMax];
    for (var y = 0; y < yMax; y++)
    {
        for (var x = 0; x < xMax; x++)
        {
            matrix[x, y] = new Tile() { Type = file[y][x] };
        }
    }

    return matrix;
}

class Tile
{
    public bool Energized { get; set; } = false;
    public Direction? Direction { get; set; } = null;
    public char Type { get; set; }

    public List<Beam> EnergizeTile(Beam beam)
    {
        if (Energized && Direction == beam.Direction)
        {
            return new List<Beam>();
        }

        Energized = true;
        Direction = beam.Direction;

        return Type switch
        {
            '.' => new[] { beam }.ToList(),
            '/' => new[] { ReflectBeam(beam) }.ToList(),
            '\\' => new[] { ReflectBeam(beam) }.ToList(),
            '|' => SplitBeam(beam).ToList(),
            '-' => SplitBeam(beam).ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    Beam ReflectBeam(Beam beam)
    {
        var newDirection = (beam.Direction, Type) switch
        {
            (global::Direction.Down, '/') => global::Direction.Left,
            (global::Direction.Up, '\\') => global::Direction.Left,
            (global::Direction.Left, '/') => global::Direction.Down,
            (global::Direction.Right, '\\') => global::Direction.Down,
            (global::Direction.Up, '/') => global::Direction.Right,
            (global::Direction.Down, '\\') => global::Direction.Right,
            (global::Direction.Right, '/') => global::Direction.Up,
            (global::Direction.Left, '\\') => global::Direction.Up,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Beam(newDirection, beam.Position);
    }

    List<Beam> SplitBeam(Beam beam)
    {
        if (IsPassThrough(beam.Direction))
        {
            return new List<Beam> { beam };
        }

        return Type switch
        {
            '|' => new List<Beam>
                { new(global::Direction.Up, beam.Position), new(global::Direction.Down, beam.Position) },
            '-' => new List<Beam>
                { new(global::Direction.Left, beam.Position), new(global::Direction.Right, beam.Position) },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    bool IsPassThrough(Direction d)
    {
        return (d, Type) switch
        {
            (global::Direction.Up, '|') => true,
            (global::Direction.Down, '|') => true,
            (global::Direction.Left, '-') => true,
            (global::Direction.Right, '-') => true,
            _ => false
        };
    }

    public override string ToString()
    {
        return Energized && Type == '.'
            ? Direction switch
            {
                global::Direction.Down => "v",
                global::Direction.Left => "<",
                global::Direction.Right => ">",
                global::Direction.Up => "^"
            }
            : Type.ToString();
    }
}

class Beam
{
    private int _x;
    private int _y;

    public Beam(Direction d, Point p)
    {
        Direction = d;
        _x = p.X;
        _y = p.Y;
    }

    public Direction Direction { get; set; }

    public Point Position
    {
        get { return new(_x, _y); }
        set
        {
            _x = value.X;
            _y = value.Y;
        }
    }

    public Beam? Advance(int xMax, int yMax)
    {
        switch (Direction)
        {
            case Direction.Up:
                _y--;
                break;
            case Direction.Down:
                _y++;
                break;
            case Direction.Left:
                _x--;
                break;
            case Direction.Right:
                _x++;
                break;
        }

        if (0 <= _x && _x < xMax &&
            0 <= _y && _y < yMax)
        {
            return this;
        }

        return null;
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}
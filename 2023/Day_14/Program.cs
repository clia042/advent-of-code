// See https://aka.ms/new-console-template for more information
using System.Text;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var matrix = new ReflectorMatrix(file);

Console.WriteLine(matrix.ToString());

matrix.TiltNorth();

Console.WriteLine(matrix.ToString());

var sum = matrix.CalculateLoad();

Console.WriteLine($"Hello, World! {sum}");
Console.Read();

class ReflectorMatrix
{
    const int _x = 0;
    const int _y = 1;
    char[,] Matrix { get; set; }

    public ReflectorMatrix(string[] file)
    {
        var xLength = file.First().Length;
        var yLength = file.Count();

        Matrix = new char[xLength, yLength];

        for (var y = 0; y < yLength; y++)
        {
            var row = file[y];
            for (var x = 0; x < xLength; x++)
            {
                Matrix[x, y] = row[x];
            }
        }
    }

    public void TiltNorth()
    {
        var newMatrix = new char[Matrix.GetLength(_x), Matrix.GetLength(_y)];

        for (var x = 0; x < newMatrix.GetLength(_x); x++)
        {
            var column = TiltColumn(x, Matrix);
            SaveColumn(x, column);
        }
    }

    char[] TiltColumn(int x, char[,] matrix)
    {
        var yMax = matrix.GetLength(_y);
        var array = Enumerable.Repeat('.', yMax).ToArray();
        var i = 0;

        for (var y = 0; y < yMax; y++)
        {
            var value = matrix[x, y];
            switch (value)
            {
                case '.':
                    continue;
                case '#':
                    i = y;
                    array[i] = value;
                    i++;
                    break;
                case 'O':
                    array[i] = value;
                    i++;
                    break;
            };
        }

        return array;
    }

    void SaveColumn(int x, char[] column)
    {
        for (var y = 0; y < column.Length; y++)
        {
            Matrix[x, y] = column[y];
        }
    }

    public long CalculateLoad()
    {
        var sum = 0l;
        for (var y = 0; y < Matrix.GetLength(_y); y++)
        {
            var rowCounter = 0;
            for (var x = 0; x < Matrix.GetLength(_x); x++)
            {
                if (Matrix[x, y] == 'O')
                {
                    rowCounter++;
                }
            }
            sum += rowCounter * (Matrix.GetLength(_y) - y);
        }

        return sum;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var y = 0; y < Matrix.GetLength(_y); y++)
        {
            for (var x = 0; x < Matrix.GetLength(_x); x++)
            {
                builder.Append(Matrix[x, y]);
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
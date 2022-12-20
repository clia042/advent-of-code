using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp2
{
    internal class Program
    {
        static int floorDepth;
        static void Main(string[] args)
        {
            /*
            var input = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9".Split("\r\n").ToArray();
            /*/
            var input = System.IO.File.ReadAllLines("input.txt");
            //*/

            var caveArray = new Dictionary<int, Dictionary<int, char>>();

            foreach (var line in input)
            {
                caveArray.ParseLine(line);
            }
            AddSandSource(caveArray);
            SetFloorDepth(caveArray);


            var sandCount = 0;

            while (caveArray.DropSand())
            {
                sandCount++;
                //Task.Delay(80).Wait();
            }
                caveArray.PrintArray();

            Console.WriteLine(sandCount); //475 too low
            Console.Read();
        }

        static void AddSandSource(Dictionary<int, Dictionary<int, char>> array)
        {
            if (array.ContainsKey(500) == false)
            {
                array.Add(500, new Dictionary<int, char> { { 0, '+' } });
            }
            array[500].Add(0, '+');
        }

        static void SetFloorDepth(Dictionary<int, Dictionary<int, char>> array)
        {
            var depth = array.Values.MaxBy(x => x.Keys.Max())!.Keys.Max();
            foreach (var column in array.Values)
            {
                column.Add(depth + 2, '#');
            }
        }
    }

    public static class Extensions
    {
        public static void ParseLine(this Dictionary<int, Dictionary<int, char>> array, string line)
        {
            Tuple<int, int> cp = null;
            foreach (var point in line.Split(" -> "))
            {
                var p = point.Split(',').Select(x => int.Parse(x));
                if (cp == null)
                {
                    cp = Tuple.Create(p.First(), p.Last());
                    continue;
                }
                else
                {
                    var p2 = Tuple.Create(p.First(), p.Last());
                    array.FillInRocks(cp, p2);
                    cp = p2;
                }
            }
        }

        public static void FillInRocks(this Dictionary<int, Dictionary<int, char>> array, Tuple<int, int> a, Tuple<int, int> b)
        {
            var xDiff = a.Item1 - b.Item1;
            var yDiff = a.Item2 - b.Item2;

            if (xDiff == 0)
            {
                var x = a.Item1;
                var y1 = Math.Min(a.Item2, b.Item2);
                var y2 = Math.Max(a.Item2, b.Item2);
                for (var y = y1; y <= y2; y++)
                {
                    array.FillInRock(x, y);
                }
            }
            else
            {
                var x1 = Math.Min(a.Item1, b.Item1);
                var x2 = Math.Max(a.Item1, b.Item1);
                var y = Math.Min(a.Item2, b.Item2);
                for (var x = x1; x <= x2; x++)
                {
                    array.FillInRock(x, y);
                }
            }
        }

        public static void FillInRock(this Dictionary<int, Dictionary<int, char>> array, int x, int y)
        {
            if (array.ContainsKey(x) == false)
            {
                array.Add(x, new Dictionary<int, char> { { y, '#' } });
                return;
            }

            array[x].TryAdd(y, '#');
        }

        public static bool DropSand(this Dictionary<int, Dictionary<int, char>> array)
        {
            var sand = new { x = 500, y = 0 };

            if (array.TryGetValue(sand.x, out var column) &&
                array[sand.x][sand.y] != 'o')
            {
                var resting = array.RestingDrop(column, sand.x, sand.y);
                return resting.HasValue;
            }
            return false;
        }

        public static (int, int)? RestingDrop(this Dictionary<int, Dictionary<int, char>> array, Dictionary<int, char> column, int x, int y)
        {
            var obstacle = column.Keys.Order().FirstOrDefault<int>(k => k > y);
            if (obstacle == null)
            {
                array.AbyssFloor(x);
                return (x, -1);
            }

            if (array.TryGetValue(x - 1, out var leftColumn))
            {
                if (!leftColumn.TryGetValue(obstacle, out _))
                {
                    return array.RestingDrop(leftColumn, x - 1, obstacle);
                }
            }
            else
            {
                array.AbyssFloor(x - 1);
                return (x - 1, -1);
            }

            if (array.TryGetValue(x + 1, out var rightColumn))
            {
                if (!rightColumn.TryGetValue(obstacle, out _))
                {
                    return array.RestingDrop(rightColumn, x + 1, obstacle);
                }
            }
            else
            {
                array.AbyssFloor(x + 1);
                return (x + 1, -1);
            }

            column[obstacle - 1] = 'o';
            return (x, obstacle - 1);
        }

        public static void AbyssFloor(this Dictionary<int, Dictionary<int, char>> array, int x)
        {
            var y2 = array.Values.MaxBy(x => x.Keys.Max())!.Keys.Max();
            array.Add(x, new Dictionary<int, char> { { y2 - 1, 'o' }, { y2, '#' } });
        }

        public static void PrintArray(this Dictionary<int, Dictionary<int, char>> array)
        {
            var x1 = array.Keys.Min();
            var x2 = array.Keys.Max();
            var y1 = array.Values.MinBy(x => x.Keys.Min())!.Keys.Min();
            var y2 = array.Values.MaxBy(x => x.Keys.Max())!.Keys.Max();

            var strBuilder = new StringBuilder();

            for (var y = y1; y <= y2; y++)
            {
                for (var x = x1; x <= x2; x++)
                {
                    if (array.TryGetValue(x, out var column))
                    {
                        if (column.TryGetValue(y, out var cell))
                        {
                            strBuilder.Append($"{cell}");
                            continue;
                        }
                    }
                    strBuilder.Append(".");
                }
                strBuilder.AppendLine();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(strBuilder.ToString());
        }
    }
}
using Newtonsoft.Json;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Parse Input

            /*
            var input = System.IO.File.ReadAllLines("sample-input.txt");
            var _max = 20;
            /*/
            var input = System.IO.File.ReadAllLines("input.txt");
            var _max = 4000000;
            //*/

            var _array = new HashSet<Point>();
            var _walkedPoints = new Dictionary<Tuple<int, int>, int>();

            ParseSensorAndBeacons(input, _array, _walkedPoints);

            //var sensor = new Sensor { X = 10, Y = 10 };
            //var beacon = new Beacon { X = 10, Y = 5 };
            //sensor.SetBeacon(beacon);
            //_array.Add(sensor);
            //_array.Add(beacon);
            //_walkedPoints.Add(Tuple.Create(sensor.X, sensor.Y), int.MinValue);
            //_walkedPoints.Add(Tuple.Create(beacon.X, beacon.Y), int.MinValue);

            WalkSensorEdges(_array, _max, _walkedPoints);

            var possibleResults = _walkedPoints.Where(x => x.Value >= 4).ToList();

            foreach(var sensor in _array.Where(x => x is Sensor).Select(x => x as Sensor))
            {
                foreach(var point in possibleResults.ToList())
                {
                    if (sensor.IsPointInsideScan(point.Key.Item1, point.Key.Item2))
                    {
                        possibleResults.Remove(point);
                    }
                }
            }

            var result = possibleResults.First();

            Console.WriteLine(JsonConvert.SerializeObject(new { result, amount = (result.Key.Item1 * 4000000L) + result.Key.Item2, count = possibleResults.Count }));
            //PrintArray(_walkedPoints);

            //7826724919136 - value too low
            //6327806271708 - still too low //{"result":{"Key":{"Item1":1581951,"Item2":2271708},"Value":4},"amount":6327806271708,"count":5}
            //10553942650264
            Console.Read();
        }

        private static void ParseSensorAndBeacons(string[] input, HashSet<Point> _array, Dictionary<Tuple<int, int>, int> _walkedPoints)
        {
            foreach (var line in input)
            {
                var sb = line.Split(": ");
                var s = ExtractPoint<Sensor>(sb[0]);
                var b = ExtractPoint<Beacon>(sb[1]);

                s.SetBeacon(b);

                _array.Add(s);
                _array.Add(b);

                _walkedPoints[Tuple.Create(s.X, s.Y)] = int.MinValue;
                _walkedPoints[Tuple.Create(b.X, b.Y)] = int.MinValue;
            }
        }

        static void WalkSensorEdges(HashSet<Point> _array, int _max, Dictionary<Tuple<int, int>, int> _walkedPoints)
        {
            foreach (var sensor in _array.Where(x => x is Sensor).Select(x => (Sensor)x).ToList())
            {
                var x1 = sensor.X - (sensor.Range + 1);
                var x2 = sensor.X + (sensor.Range + 1);

                x1 = x1 < 0 ? 0 : x1;   //Lower bound
                x2 = x2 < _max ? x2 : _max; //Upper bound

                for (var x = x1; x <= x2; x++)
                {
                    var currentEdges = sensor.GetEdgePoints(x)
                        .Where(y => y >= 0 && y <= _max);
                    foreach (var y in currentEdges)
                    {
                        var point = Tuple.Create(x, y);
                        var count = _walkedPoints.ContainsKey(point) ? _walkedPoints[point] : 0;
                        _walkedPoints[point] = ++count;
                    }
                }
                Console.WriteLine($"sensor : {JsonConvert.SerializeObject(sensor)}");
            }
        }

        static void PrintArray(Dictionary<Tuple<int, int>, int> array)
        {
            var y2 = array.Keys.Select(x => x.Item2).Max();
            var x2 = array.Keys.Select(x => x.Item1).Max();
            var builder = new StringBuilder();

            for (var y = 0; y <= y2; y++)
            {
                for (var x = 0; x <= x2; x++)
                {
                    var point = Tuple.Create(x, y);
                    if (array.TryGetValue(point, out var count))
                    {
                        var symbol = count switch
                        {
                            <= 0 => "T",
                            <= 9 => $"{count}",
                            _ => "X"
                        };
                        builder.Append(symbol);
                        continue;
                    }
                    builder.Append('.');
                }
                builder.AppendLine();
            }
            Console.Write(builder.ToString());
        }

        static T ExtractPoint<T>(string input) where T : Point, new()
        {
            var point = input
                .Replace("Sensor at ", "")
                .Replace("closest beacon is at ", "")
                .Split(',')
                .Select(x => x.Split("=").Last());

            return new T()
            {
                X = int.Parse(point.First()),
                Y = int.Parse(point.Last())
            };
        }
    }

    internal class Point
    {
        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    internal class Sensor : Point
    {
        Beacon _closestBeacon;

        public void SetBeacon(Beacon beacon)
        {
            _closestBeacon = beacon;
        }

        public int Range => Math.Abs(X - _closestBeacon.X) + Math.Abs(Y - _closestBeacon.Y);

        public IEnumerable<int> GetEdgePoints(int x)
        {
            var diff = (Range + 1) - Math.Abs(X - x);
            var y1 = Y - diff;
            var y2 = Y + diff;

            return new[] { y1, y2 }.Distinct();
        }

        public bool IsPointInsideScan(int x, int y)
        {
            var delta = Math.Abs(X - x) + Math.Abs(Y - y);
            return delta <= Range;
        }
    }

    internal class Beacon : Point
    {

    }
}
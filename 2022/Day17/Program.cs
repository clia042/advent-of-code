using System.Text;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //*
            var input = File.ReadAllText("sample-input.txt");
            /*/ 
            var input = File.ReadAllText("input.txt");
            //*/

            var shapesDropped = 0L;
            var currentHeight = 0L;
            var currentShape = ShapeGenerator(shapesDropped, currentHeight);
            var array = new Dictionary<Tuple<int, long>, char>();


            var i = 0;
            while (shapesDropped < 200)
            {
                do
                {
                    var nextMove = input[i % input.Length];
                    var gasStream = currentShape.TryMove(array, nextMove == '<' ? Direction.Left : Direction.Right);
                    i++;


                } while (currentShape.TryMove(array,Direction.Down));

                currentShape.DrawToArray(array);
                currentHeight = array.Keys.Max(x => x.Item2 + 1);
                if (RepeatDetected())
                {
                    //Console.Read();
                }
                //TrimArray();
                shapesDropped++;
                currentShape = ShapeGenerator(shapesDropped, currentHeight);
                Console.Write($"\rCurrent Height : {currentHeight}");
            }
            DrawState();

            Console.Read();

            void DrawState()
            {
                var stringBuilder = new StringBuilder();
                var startY = currentShape.Height;
                for(var y = startY; y >= 0; y--)
                {
                    if(array.Any(x => x.Key.Item2 == y))
                    {
                        for(var x = 0; x < 7; x++)
                        {
                            if(array.TryGetValue(Tuple.Create(x,y), out var value))
                            {
                                stringBuilder.Append(value);
                            }
                            else
                            {
                                stringBuilder.Append('.');
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Append(".......");
                    }
                    stringBuilder.AppendLine();
                }
                Console.WriteLine(stringBuilder.ToString());
            }

            void TrimArray()
            {
                var depth = array.Keys
                    .GroupBy(x => x.Item1)
                    .Select(x => new { column = x.Key, highest = x.Select(y => y.Item2).Max() })
                    .MinBy(x => x.highest).highest;

                var pointsToRemove = array.Where(x => x.Key.Item2 < depth).ToList();
                foreach(var point in pointsToRemove)
                {
                    array.Remove(point.Key);
                }
            }

            bool RepeatDetected()
            {
                var row = array!
                    .GroupBy(x => x.Key.Item2)
                    .OrderBy(x => x.Key)
                    .Select(x => RowToHexString(x.ToDictionary(k => k.Key.Item1, v => v.Value)));

                return false;
            }
        }

        private static string RowToHexString(Dictionary<int, char> row)
        {
            var stringBuilder = new StringBuilder();
            for(var i = 0; i < 7; i++)
            {
                if(row.TryGetValue(i, out var v))
                {
                    stringBuilder.Append('1');
                }
                else
                {
                    stringBuilder.Append('0');
                }
            }
            return Convert.ToInt32(stringBuilder.ToString()).ToString("X");
        }

        public static Shape ShapeGenerator(long shapesCreated, long currentHeight)
        {
            return (shapesCreated % 5) switch
            {
                0 => new HorizontalLine(currentHeight + 3),
                1 => new Cross(currentHeight + 3),
                2 => new ReverseL(currentHeight + 3),
                3 => new VerticalLine(currentHeight + 3),
                4 => new Square(currentHeight + 3)
            };
        }

        public enum Direction
        {
            Down,
            Left,
            Right
        }

        public abstract class Shape
        {
            internal Dictionary<Tuple<int, long>, char> _ShapeArray;

            public bool TryMove(Dictionary<Tuple<int, long>, char> array, Direction move)
            {
                var shapeMoved = MoveShape(move);

                if (shapeMoved == null) return false;

                if (shapeMoved.Any(p => array.ContainsKey(p.Key))) return false;

                SetNewPosition(shapeMoved);

                return true;
            }

            Dictionary<Tuple<int, long>, char>? MoveShape(Direction move)
            {
                var newPosition = new Dictionary<Tuple<int, long>, char>();
                foreach (var pair in _ShapeArray)
                {
                    var keyX = move switch
                    {
                        Direction.Left => pair.Key.Item1 - 1,
                        Direction.Right => pair.Key.Item1 + 1,
                        _ => pair.Key.Item1
                    };

                    var keyY = move == Direction.Down
                        ? pair.Key.Item2 - 1
                        : pair.Key.Item2;

                    if (keyX < 0 || keyX > 6 || keyY < 0)
                    {
                        // Invalid Move;
                        return null;
                    }

                    newPosition[Tuple.Create(keyX, keyY)] = pair.Value;
                }
                return newPosition;
            }

            void SetNewPosition(Dictionary<Tuple<int, long>, char> position) => _ShapeArray = position;

            public void DrawToArray(Dictionary<Tuple<int, long>, char> array)
            {
                foreach(var position in _ShapeArray)
                {
                    array[position.Key] = '#';
                }
            }

            public long Height => _ShapeArray.Keys.MaxBy(x => x.Item2).Item2;
        }

        public class HorizontalLine : Shape
        {
            public HorizontalLine(long spawnHeight)
            {
                _ShapeArray = new Dictionary<Tuple<int, long>, char>();
                _ShapeArray[Tuple.Create(2, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(4, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(5, spawnHeight)] = '@';
            }
        }

        public class Cross : Shape
        {
            public Cross(long spawnHeight)
            {
                _ShapeArray = new Dictionary<Tuple<int, long>, char>();
                _ShapeArray[Tuple.Create(3, spawnHeight + 2)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(4, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight)] = '@';
            }
        }

        public class ReverseL : Shape
        {
            public ReverseL(long spawnHeight)
            {
                _ShapeArray = new Dictionary<Tuple<int, long>, char>();
                _ShapeArray[Tuple.Create(4, spawnHeight + 2)] = '@';
                _ShapeArray[Tuple.Create(4, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(4, spawnHeight)] = '@';
            }
        }

        public class VerticalLine : Shape
        {
            public VerticalLine(long spawnHeight)
            {
                _ShapeArray = new Dictionary<Tuple<int, long>, char>();
                _ShapeArray[Tuple.Create(2, spawnHeight + 3)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight + 2)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight)] = '@';
            }
        }

        public class Square : Shape
        {
            public Square(long spawnHeight)
            {
                _ShapeArray = new Dictionary<Tuple<int, long>, char>();
                _ShapeArray[Tuple.Create(2, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight + 1)] = '@';
                _ShapeArray[Tuple.Create(2, spawnHeight)] = '@';
                _ShapeArray[Tuple.Create(3, spawnHeight)] = '@';
            }
        }
    }
}
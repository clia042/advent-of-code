namespace Day17.Models
{
    public abstract class Shape
    {
        internal Dictionary<Tuple<int, long>, char> _ShapeArray;

        public Dictionary<Tuple<int, long>, char>? MoveShape(Direction move)
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

        public void SetNewPosition(Dictionary<Tuple<int, long>, char> position) => _ShapeArray = position;

        public void DrawToArray(Dictionary<Tuple<int, long>, char> array)
        {
            foreach (var position in _ShapeArray)
            {
                array[position.Key] = '#';
            }
        }

        public long Height => _ShapeArray.Keys.MaxBy(x => x.Item2).Item2;

        public long LeftPosition => _ShapeArray.Keys.MinBy(x => x.Item1).Item1;
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

    public class ShapeProjection
    {
        public long Index { get; set; }
        public long Height { get; set; }
        public string HashProjection { get; set; }
        public bool IsSeen { get; set; }
    }
}
namespace Day17.Models
{
    public static class ShapeExtensions
    {
        public static bool TryMove(this Shape shape, Dictionary<Tuple<int, long>, char> array, Direction move)
        {
            var shapeMoved = shape.MoveShape(move);

            if (shapeMoved == null) return false;

            if (shapeMoved.Any(p => array.ContainsKey(p.Key))) return false;

            shape.SetNewPosition(shapeMoved);

            return true;
        }

        public static ShapeProjection ToShapeProjection(this Shape shape, long index)
        {
            return new ShapeProjection
            {
                Index = index,
                Height = shape.Height + 1,
                HashProjection = $"{shape.LeftPosition}.{shape.GetType().Name}"
            };
        }
    }
}
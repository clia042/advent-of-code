using Day18.Models;
using System.Xml.Linq;

namespace Day18
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var useSampleData = false;
            var input = File.ReadAllLines(useSampleData
                ? "Data/sample-input.txt"
                : "Data/input.txt");

            var matrix = new Dictionary<string, Coordinate>();
            var possibleVoidNode = new Dictionary<string, ShadowCoordinate>();

            foreach (var line in input)
            {
                var values = line.Split(',').Select(int.Parse).ToList();
                matrix.Add(line, new Coordinate
                {
                    X = values[0],
                    Y = values[1],
                    Z = values[2]
                });
            }

            foreach (var node in matrix.Values)
            {
                foreach (var direction in Enum.GetValues(typeof(Direction)))
                {
                    var d = (Direction)direction;
                    if (node.EvaluateDirection(matrix, d))
                    {
                        if (node.AnyInDirection(matrix, d))
                        {
                            possibleVoidNode.AddCoordinate(node, d);
                            continue;
                        }
                        node.ExposedSurfaces++;
                    }
                }
            }

            foreach (var node in possibleVoidNode.Values)
            {
                if (node.Evaluation == null)
                {
                    var isInnerVoid = node.FloodFill(matrix, possibleVoidNode.Keys.ToHashSet(), out var CoveredNodes);
                }
            }

            Console.WriteLine($"surfaces: {matrix.Sum(x => x.Value.ExposedSurfaces)}");
            Console.Read();
        }
    }

    public static class Extensions
    {
        public static bool EvaluateDirection(this Coordinate node, Dictionary<string, Coordinate> matrix, Direction direction)
        {
            var searchPosition = new Dictionary<Direction, string>
            {
                {Direction.Up,      $"{node.X},{node.Y - 1},{node.Z}"},
                {Direction.Down,    $"{node.X},{node.Y + 1},{node.Z}"},
                {Direction.Left,    $"{node.X - 1},{node.Y},{node.Z}"},
                {Direction.Right,   $"{node.X + 1},{node.Y},{node.Z}"},
                {Direction.Forward, $"{node.X},{node.Y},{node.Z - 1}"},
                {Direction.Back,    $"{node.X},{node.Y},{node.Z + 1}"}
            };

            return matrix.TryGetValue(searchPosition[direction], out _) == false;
        }

        public static bool AnyInDirection(this Coordinate node, Dictionary<string, Coordinate> matrix, Direction direction)
        {
            var searchDirection = new Dictionary<Direction, Func<Coordinate, bool>>
            {
                {Direction.Up,      n => n.X == node.X && n.Y < node.Y && n.Z == node.Z},
                {Direction.Down,    n => n.X == node.X && n.Y > node.Y && n.Z == node.Z},
                {Direction.Left,    n => n.X < node.X && n.Y == node.Y && n.Z == node.Z},
                {Direction.Right,   n => n.X > node.X && n.Y == node.Y && n.Z == node.Z},
                {Direction.Forward, n => n.X == node.X && n.Y == node.Y && n.Z < node.Z},
                {Direction.Back,    n => n.X == node.X && n.Y == node.Y && n.Z > node.Z}
            };

            return matrix.Values.Any(searchDirection[direction]);
        }

        public static void AddCoordinate(this Dictionary<string, ShadowCoordinate> matrix, Coordinate node, Direction direction)
        {
            var c = new Dictionary<Direction, (int, int, int)>
            {
                {Direction.Up,      (node.X, node.Y - 1, node.Z)},
                {Direction.Down,    (node.X, node.Y + 1, node.Z)},
                {Direction.Left,    (node.X - 1, node.Y, node.Z)},
                {Direction.Right,   (node.X + 1, node.Y, node.Z)},
                {Direction.Forward, (node.X, node.Y, node.Z - 1)},
                {Direction.Back,    (node.X, node.Y, node.Z + 1)}
            };

            var key = $"{c[direction].Item1},{c[direction].Item2},{c[direction].Item3}";
            var isNew = matrix.TryAdd(key, new ShadowCoordinate
            {
                X = c[direction].Item1,
                Y = c[direction].Item2,
                Z = c[direction].Item3,
                Neighbours = new() { $"{node.X},{node.Y},{node.Z}" }
            });

            if (isNew == false)
            {
                matrix[key].Neighbours.Add($"{node.X},{node.Y},{node.Z}");
            }
        }

        public static bool FloodFill(this ShadowCoordinate node, Dictionary<string, Coordinate> matrix, HashSet<string> shadowMatrix, out List<string> coveredNodes)
        {
            coveredNodes = new List<string>();

            if (node.Neighbours.Count == 6)
            {
                node.Evaluation = "IsInnerVoid";
                return true;
            }

            var Xboundries = (matrix.Values.MinBy(n => n.X), matrix.Values.MaxBy(n => n.X));
            var Yboundries = (matrix.Values.MinBy(n => n.Y), matrix.Values.MaxBy(n => n.Y));
            var Zboundries = (matrix.Values.MinBy(n => n.Z), matrix.Values.MaxBy(n => n.Z));

            var seen = new HashSet<string>();
        }

        public static bool Flood(FloodState state)
        {
            var possibleNeighbours = new[]
            {
                $"{node.X},{node.Y - 1},{node.Z}",
                $"{node.X},{node.Y + 1},{node.Z}",
                $"{node.X - 1},{node.Y},{node.Z}",
                $"{node.X + 1},{node.Y},{node.Z}",
                $"{node.X},{node.Y},{node.Z - 1}",
                $"{node.X},{node.Y},{node.Z + 1}"
            };

            foreach (var newNode in possibleNeighbours)
            {
                if (seen.Contains(newNode)) continue;

                if (matrix.TryGetValue(newNode, out _)) continue;

                seen.Add(newNode);

                if (shadowMatrix.Contains(newNode))
                {
                    coveredNodes.Add(newNode);
                    continue;
                }
            }

            return false;
        }
    }
}
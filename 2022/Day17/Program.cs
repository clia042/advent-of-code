using Day17.Models;
using System.Text;

internal class Program
{
    static void Main(string[] args)
    {
        var useSampleInput = false;
        var input = File.ReadAllText(useSampleInput
            ? "Data/sample-input.txt"
            : "Data/input.txt");

        var shapesDropped = 0L;
        var currentHeight = 0L;
        var targetDropped = 1000000000000;
        var array = new Dictionary<Tuple<int, long>, char>();
        var currentShape = ShapeGenerator(shapesDropped, currentHeight);

        var shapesCollection = new List<ShapeProjection>();

        var i = 0;
        while (shapesDropped < targetDropped)
        {
            do
            {
                var nextMove = input[i % input.Length];
                currentShape.TryMove(array, nextMove == '<' ? Direction.Left : Direction.Right);
                i++;
            } while (currentShape.TryMove(array, Direction.Down));
            SettleShape();
            if(ExamineCollection()) break;
        }

        //1603205128200 - wrong answer. too high
        //1584916864604 too high.

        //DrawState();
        Console.Read();

        void SettleShape()
        {
            currentShape.DrawToArray(array);
            currentHeight = array.Keys.Max(x => x.Item2 + 1);
            shapesDropped++;
            shapesCollection.Add(currentShape.ToShapeProjection(shapesDropped));

            currentShape = ShapeGenerator(shapesDropped, currentHeight);
            Console.Write($"\rCurrent Height : {currentHeight}");
        }

        bool ExamineCollection()
        {
            var lastShape = shapesCollection.Last();
            var firstInstance = shapesCollection.FirstOrDefault(x =>
                x.Index != lastShape.Index &&
                x.HashProjection == lastShape.HashProjection);
            if (firstInstance == null)
            {
                return false;
            }

            lastShape.IsSeen = true;

            var patternFound = shapesCollection
                .Where(x => x.Index > firstInstance.Index)
                .All(x => x.IsSeen);

            if (i > (5 * input.Length) && patternFound)
            {
                var patternHeight = lastShape.Height - firstInstance.Height;
                var patternShapesCount = lastShape.Index - firstInstance.Index;

                var remainingDrops = targetDropped - shapesDropped;
                var jumpToDrops = targetDropped - (remainingDrops % patternShapesCount);
                var jumpToHeight = currentHeight + (patternHeight * (remainingDrops / patternShapesCount));

                //Simulate last few drops.
                var drops = targetDropped - jumpToDrops;
                var simulatedShape = shapesCollection.First(x => x.Index == (firstInstance.Index + drops));

                Console.WriteLine($"\nSimulated Height : {jumpToHeight + (simulatedShape.Height - firstInstance.Height)}");
                return true;
            }
            return false;
        }

        void DrawState()
        {
            var stringBuilder = new StringBuilder();
            var startY = currentShape.Height;
            for (var y = startY; y >= 0; y--)
            {
                if (array.Any(x => x.Key.Item2 == y))
                {
                    for (var x = 0; x < 7; x++)
                    {
                        if (array.TryGetValue(Tuple.Create(x, y), out var value))
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
    }

    static Shape ShapeGenerator(long shapesCreated, long currentHeight)
    {
        var spawnHeight = currentHeight + 3;
        return (shapesCreated % 5) switch
        {
            0 => new HorizontalLine(spawnHeight),
            1 => new Cross(spawnHeight),
            2 => new ReverseL(spawnHeight),
            3 => new VerticalLine(spawnHeight),
            4 => new Square(spawnHeight)
        };
    }
}
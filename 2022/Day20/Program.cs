namespace Day20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var useSampleInput = true;

            var input = File.ReadAllLines(useSampleInput
                ? "Data/sample-input.txt"
                : "Data/input.txt");

            var decryptionKey = 811589153;
            var array = input.Select((x, i) => new Coordinate { Index = i, Value = (long.Parse(x) * decryptionKey) }).ToList();


            Console.WriteLine($"{string.Join(",", array.Select(x => x.Value.ToString()))}");

            for (var i = 0; i < (array.Count * 10); i++)
            {
                var value = array.First(x => x.Index == (i % array.Count));
                var currentIndex = array.IndexOf(value);

                var newIndex = GetNewIndex(currentIndex, value.Value, array.Count);
                if (newIndex == 0) continue;

                array.Remove(value);
                array.Insert(newIndex, value);
                Console.WriteLine($"{string.Join(",", array.Select(x => x.Value.ToString()))}");
            }

            var zeroIndex = array.IndexOf(array.First(x => x.Value == 0));


            var results = new[]
            {
                array[(zeroIndex + 1000) % (array.Count)].Value,
                array[(zeroIndex + 2000) % (array.Count)].Value,
                array[(zeroIndex + 3000) % (array.Count)].Value
            };

            Console.WriteLine($"1000th: {results[0]}, 2000th: {results[1]}, 3000th: {results[2]}\nSum: {results.Sum()}");

            //-5237 wrong answer
            //-5875905467720 wrong answer part 2
        }

        static int GetNewIndex(int currentIndex, long shiftBy, int arrayLength)
        {
            if (shiftBy == 0) return 0;

            if (shiftBy > 0)
            {
                return (int)((currentIndex + shiftBy) % (arrayLength - 1));

            }

            var index = (int)(currentIndex + ((shiftBy % arrayLength) - 1));

            if (index == 0) return arrayLength - 1;

            if (index < 1)
            {
                return arrayLength + index;
            }

            return index;
        }
    }

    internal class Coordinate
    {
        public int Index { get; set; }
        public long Value { get; set; }
    }
}
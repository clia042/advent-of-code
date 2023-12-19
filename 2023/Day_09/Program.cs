// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var results = new List<int>();

foreach (var line in file)
{
    var matrix = ParseLineToMatrix(line);
    //var nextSequence = GetNextSequence(matrix);
    //results.Add(nextSequence);
    //Console.WriteLine($"{line} => {nextSequence}");
    var previousSequence = GetPreviousSequence(matrix);
    results.Add(previousSequence);
    Console.WriteLine($"{previousSequence} <= {line}");
}

Console.WriteLine($"sum: {results.Sum()}");
Console.WriteLine("Hello, World!");

List<List<int>> ParseLineToMatrix(string inputLine)
{
    var c = 0; //failsafe, allows exiting if infinite looping.
    var oasisMatrix = new List<List<int>>
    {
        inputLine.Split(' ').Select(int.Parse).ToList()
    };

    while (oasisMatrix.Last().All(x => x == 0) == false)
    {
        var currentArray = oasisMatrix.Last();
        var nextArray = new List<int>();
        for (var i = 0; i < currentArray.Count; i++)
        {
            if (i + 1 >= currentArray.Count)
            {
                break;
            }

            var a = currentArray[i];
            var b = currentArray[i + 1];

            nextArray.Add(b - a);
        }

        oasisMatrix.Add(nextArray);
        c++;
        if (c > 1000000)
            break;
    }

    return oasisMatrix;
}

int GetNextSequence(List<List<int>> matrix)
{
    var depth = matrix.Count - 1;
    while (depth > 0)
    {
        var lineValue = matrix[depth].Last();
        var currentValue = matrix[depth - 1].Last();
        matrix[depth - 1].Add(currentValue + lineValue);
        depth--;
    }

    return matrix[0].Last();
}

int GetPreviousSequence(List<List<int>> matrix)
{
    var depth = matrix.Count - 1;
    while (depth > 0)
    {
        var lineValue = matrix[depth].First();
        var currentValue = matrix[depth - 1].First();
        matrix[depth - 1].Insert(0,currentValue - lineValue);
        depth--;
    }

    return matrix[0].First();
}
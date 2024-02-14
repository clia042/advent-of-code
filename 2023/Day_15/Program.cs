// See https://aka.ms/new-console-template for more information

using System.Text;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

// Console.WriteLine(ASCIIStringHelper("HASH"));

var list = new List<(string, int)>();

foreach (var sequence in file.First().Split(","))
{
    var num = HASHAlgorithm(sequence);
    list.Add(new ValueTuple<string, int>(sequence, num));
    //Console.WriteLine($"{sequence} : {num}");
}

Console.WriteLine($"Hello, World! {list.Sum(x => x.Item2)}");

int HASHAlgorithm(string input)
{
    var currentValue = 0;
    var array = Encoding.ASCII.GetBytes(input).Select(x => (int)x);
    foreach (var asciiChar in array)
    {
        currentValue += asciiChar;
        currentValue *= 17;
        currentValue %= 256;
    }

    return currentValue;
}
// See https://aka.ms/new-console-template for more information

using System.Text;

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var _separators = new[] { '=', '-' };
var boxes = new Dictionary<int, List<Lense>>();

foreach (var instruction in file.First().Split(","))
{
    var label = new string(instruction.TakeWhile(c => _separators.Contains(c) == false).ToArray());
    var boxNumber = HASHAlgorithm(label);
    var IsAddLense = instruction.Contains('=');

    if (IsAddLense)
    {
        var lense = new Lense(label, int.Parse($"{instruction.Last()}"));
        AddOrReplaceLense(boxNumber, lense);
    }
    else
    {
        RemoveLense(boxNumber, label);
    }
}

var powers = new List<int>();

foreach (var box in boxes)
{
    //Console.WriteLine($"Box {box.Key}: {string.Join(" ", box.Value.Select(x => $"[{x.Label} {x.Power}]"))}");
    var boxWeight = box.Key + 1;
    for (var i = 0; i < box.Value.Count(); i++)
    {
        var slotWeight = i + 1;
        var focalLength = box.Value[i].Power;
        powers.Add(boxWeight * slotWeight * focalLength);
    }
}

Console.WriteLine($"Hello, World! {powers.Sum()}");

void AddOrReplaceLense(int boxNumber, Lense lense)
{
    var boxList = boxes.GetValueOrDefault(boxNumber) ?? new List<Lense>();

    var existingLense = boxList.FindIndex(l => l.Label == lense.Label);
    if (existingLense != -1)
    {
        boxList.RemoveAt(existingLense);
        boxList.Insert(existingLense, lense);
    }
    else
    {
        boxList.Add(lense);
    }

    boxes[boxNumber] = boxList;
}

void RemoveLense(int boxNumber, string label)
{
    var boxList = boxes.GetValueOrDefault(boxNumber);
    if (boxList == null)
    {
        return;
    }

    boxList.RemoveAll(l => l.Label == label);
    if (boxList.Any())
    {
        boxes[boxNumber] = boxList;
    }
    else
    {
        boxes.Remove(boxNumber);
    }
}

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

class Lense
{
    public Lense(string label, int power)
    {
        Label = label;
        Power = power;
    }

    public string Label { get; set; }
    public int Power { get; set; }
}
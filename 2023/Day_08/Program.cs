// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = File.ReadAllLines(useSample ? "sample_input2.txt" : "input.txt");

var directionList = file[0];
var network = new Dictionary<string, string[]>();
foreach (var line in file.Skip(2))
{
    var segment = line.Split('=');
    network.Add(
        segment[0].Trim(),
        segment[1]
            .Split(',')
            .Select(x => new string(x.Where(char.IsLetter).ToArray()))
            .ToArray());
}

var i = 0;
var currentNode = "AAA";
while (true)
{
    if (currentNode == "ZZZ")
    {
        break;
    }
    
    var node = network[currentNode];
    string? nextNode = "";
    switch (directionList[i % directionList.Length])
    {
        case 'L' :
            nextNode = node[0];
            break;
        case 'R':
            nextNode = node[1];
            break;
    }

    currentNode = nextNode;
    i++;
}

Console.WriteLine($"current node: {currentNode}, steps: {i}");
Console.WriteLine("Hello, World!");
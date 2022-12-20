using System.Linq;
using System.Text.Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var positions = System.IO.File.ReadAllText("input.csv").Split(",").Select(x => int.Parse(x));

var median = positions.Sum() / positions.Count();

var position = median;
var currentPosition = -1;

while(position != currentPosition){
    position = currentPosition;
    currentPosition = FindLowestAlignment(position, 3);

    Console.WriteLine(currentPosition);
}

Console.WriteLine($"final position : {position}, fuel: {AlignSubs(position)}");

int FindLowestAlignment(int position, int range){
    var start = position - range;
    var list = Enumerable.Range(start < 0 ? 0 : start, range*2)
        .Select(i => new{i, fuel = AlignSubs(i)})
        .OrderBy(x => x.fuel);

    return list.First().i;
}

int AlignSubs(int position){
    var fuelUsed = positions.Select(x => {
            var moved = Math.Abs(position - x);

            return (moved*(moved + 1))/2;
        });
    // Console.WriteLine(JsonSerializer.Serialize(fuelUsed));
    // Console.WriteLine($"pos: {position}, fuel used:{fuelUsed.Sum()}");
    return fuelUsed.Sum();
}
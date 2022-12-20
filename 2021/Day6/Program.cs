using System.Linq;

var fishSchool = System.IO.File.ReadAllText(@".\input.txt").Split(",").Select(x => int.Parse(x)).ToList();

Console.Write("Initial : ");
WriteCollection(fishSchool);
var i = 1;

var batchSize = 1000;

while(i <= 256){
    fishSchool = await PassOneFishDay(fishSchool);

     Console.WriteLine($"{fishSchool.Count()}");
    // WriteCollection(fishSchool);
    i++;    
}

Console.WriteLine($"Final Count: {fishSchool.Count()}");

void WriteCollection(List<int> list){
    Console.WriteLine(string.Join(",",list));
}

async Task<List<int>> PassOneFishDay(List<int> list){
    var newList = new List<int>();
    var currentSchoolSize = list.Count();

    if(currentSchoolSize > batchSize){
        // Console.Write("|");
        newList.AddRange(await PassOneFishDay(list.Skip(batchSize).ToList()));
    }

    for(var i = 0; i < Math.Min(currentSchoolSize, batchSize); i++){
        if(list[i] > 0){
            newList.Add(list[i] - 1);
        } else {
            newList.Add(6);
            newList.Add(8);
        }
    }

    return newList;
}
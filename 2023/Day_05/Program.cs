// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = System.IO.File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var seeds = new List<SeedState>();
var maps = new Dictionary<string, Mapper>();

//1. Parse input
ParseFile();

//2. Calculate
foreach (var seed in seeds)
{
    var soil = maps["seed-to-soil"].GetDestinationNumber(seed.SeedNumber);
    var fertilizer = maps["soil-to-fertilizer"].GetDestinationNumber(soil);
    var water = maps["fertilizer-to-water"].GetDestinationNumber(fertilizer);
    var light = maps["water-to-light"].GetDestinationNumber(water);
    var temp = maps["light-to-temperature"].GetDestinationNumber(light);
    var humidity = maps["temperature-to-humidity"].GetDestinationNumber(temp);
    var location = maps["humidity-to-location"].GetDestinationNumber(humidity);

    seed.SoilNumber = soil;
    seed.FertilizerNumber = fertilizer;
    seed.WaterNumber = water;
    seed.LightNumber = light;
    seed.TempNumber = temp;
    seed.HumidityNumber = humidity;
    seed.LocationNumber = location;
}

Console.WriteLine(seeds.Min(x => x.LocationNumber));

Console.WriteLine("Hello, World!");

void ParseFile()
{
    string? currentMapper = null;
    foreach (var line in file)
    {
        if (line.StartsWith("seeds: "))
        {
            seeds = line
                .Split(':')[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new SeedState()
                {
                    SeedNumber = long.Parse(x)
                })
                .ToList();
        }
        else if (line.EndsWith(" map:"))
        {
            currentMapper = line.Split(' ')[0];
            maps.Add(currentMapper, new Mapper()
            {
                Mappings = new()
            });
        }
        else
        {
            var segments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Any() == false)
            {
                continue;
            }

            maps[currentMapper].AddMapping(segments);
        }
    }
}

public class SeedState
{
    public long SeedNumber { get; set; }
    public long SoilNumber { get; set; }
    public long FertilizerNumber { get; set; }
    public long WaterNumber { get; set; }
    public long LightNumber { get; set; }
    public long TempNumber { get; set; }
    public long HumidityNumber { get; set; }
    public long LocationNumber { get; set; }
}

public class Mapper
{
    public List<MappingSet> Mappings { get; set; }

    public void AddMapping(string[] segments)
    {
        Mappings.Add(new MappingSet()
        {
            StartDestination = long.Parse(segments[0]),
            StartOrigin = long.Parse(segments[1]),
            Range = long.Parse(segments[2])
        });
    }

    public long GetDestinationNumber(long startNumber)
    {
        var set = Mappings
            .OrderBy(x => x.StartOrigin)
            .FirstOrDefault(x => x.StartOrigin <= startNumber && x.EndOrigin > startNumber);

        if (set == null)
        {
            return startNumber;
        }

        var offset = set.StartDestination - set.StartOrigin;

        return startNumber + offset;
    }

    public class MappingSet
    {
        public long StartOrigin { get; set; }
        public long StartDestination { get; set; }
        public long Range { get; set; }
        public long EndOrigin => StartOrigin + Range;
    }
}

//Foreach seed, calculate seed states and work out location
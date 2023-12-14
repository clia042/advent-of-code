// See https://aka.ms/new-console-template for more information

var useSample = false;
var file = System.IO.File.ReadAllLines(useSample ? "sample_input.txt" : "input.txt");

var seeds = new List<SeedState>();
var maps = new Dictionary<string, Mapper>();

//1. Parse input
ParseFile();

//2. Calculate
// foreach (var seed in seeds)
// {
//     var soil = maps["seed-to-soil"].GetDestinationNumber(seed.SeedNumber);
//     var fertilizer = maps["soil-to-fertilizer"].GetDestinationNumber(soil);
//     var water = maps["fertilizer-to-water"].GetDestinationNumber(fertilizer);
//     var light = maps["water-to-light"].GetDestinationNumber(water);
//     var temp = maps["light-to-temperature"].GetDestinationNumber(light);
//     var humidity = maps["temperature-to-humidity"].GetDestinationNumber(temp);
//     var location = maps["humidity-to-location"].GetDestinationNumber(humidity);
//
//     seed.SoilNumber = soil;
//     seed.FertilizerNumber = fertilizer;
//     seed.WaterNumber = water;
//     seed.LightNumber = light;
//     seed.TempNumber = temp;
//     seed.HumidityNumber = humidity;
//     seed.LocationNumber = location;
// }

var location = 0;
while (true)
{
     var humidity = maps["humidity-to-location"].GetOriginNumber(location);
     var temp = maps["temperature-to-humidity"].GetOriginNumber(humidity);
     var light = maps["light-to-temperature"].GetOriginNumber(temp);
     var water = maps["water-to-light"].GetOriginNumber(light);
     var fertilizer = maps["fertilizer-to-water"].GetOriginNumber(water);
     var soil = maps["soil-to-fertilizer"].GetOriginNumber(fertilizer);
     var seed = maps["seed-to-soil"].GetOriginNumber(soil);

     if (seeds.Any(x => x.SeedInSet(seed)))
     {
         Console.WriteLine($"seed:{seed}, location:{location}");
         break;
     }

     location++;
}

//3. reverse lookup;
// Console.WriteLine(seeds.Min(x => x.LocationNumber));

Console.WriteLine("Hello, World!");

void ParseFile()
{
    string? currentMapper = null;
    foreach (var line in file)
    {
        if (line.StartsWith("seeds: "))
        {
            var segments = line
                .Split(':')[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();
            for (var i = 0; i < segments.Length; i += 2)
            {
                var start = segments[i];
                var range = segments[i + 1];
                
                seeds.Add(new SeedState()
                {
                    StartNumber = start,
                    Range = range
                });
            }
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
    // public long SeedNumber { get; set; }
    // public long SoilNumber { get; set; }
    // public long FertilizerNumber { get; set; }
    // public long WaterNumber { get; set; }
    // public long LightNumber { get; set; }
    // public long TempNumber { get; set; }
    // public long HumidityNumber { get; set; }
    // public long LocationNumber { get; set; }
    public long StartNumber { get; set; }
    public long Range { get; set; }

    public bool SeedInSet(long seed)
    {
        return seed >= StartNumber && seed < StartNumber + Range;
    }
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

    public long GetOriginNumber(long endNumber)
    {
        var set = Mappings
            .OrderBy(x => x.StartDestination)
            .FirstOrDefault(x => x.StartDestination <= endNumber && x.EndDestination > endNumber);

        if (set == null)
        {
            return endNumber;
        }

        var offset = set.StartDestination - set.StartOrigin;

        return endNumber - offset;
    }

    public class MappingSet
    {
        public long StartOrigin { get; set; }
        public long StartDestination { get; set; }
        public long Range { get; set; }
        public long EndOrigin => StartOrigin + Range;
        public long EndDestination => StartDestination + Range;
    }
}

//Foreach seed, calculate seed states and work out location
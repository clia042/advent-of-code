
namespace Day16.Models
{
    public static class ValveExtensions
    {
        public static Dictionary<string, Valve> ParseInput(this string[] input)
        {
            var array = new Dictionary<string, Valve>();
            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                var valveName = tokens[1];
                var flowRate = tokens[4][5..^1];
                var leadsTo = tokens[9..].Select(x => x.Replace(",", ""));

                array[valveName] = new Valve(valveName, int.Parse(flowRate), leadsTo.ToArray());
            }

            var majorValves = array.Values.Where(x => x.FlowRate > 0 || x.Name == "AA");
            foreach (var valve in majorValves)
            {
                var destinationValves = majorValves.Except(new[] { valve });
                valve.PathScores = destinationValves
                    .Select(d => new { name = d.Name, distance = TaversePath(valve.Name, d.Name, new()) })
                    .Where(x => x.distance != null)
                    .ToDictionary(x => x.name, a => (int)a.distance!);
            }

            int? TaversePath(string current, string destination, HashSet<string> visited)
            {
                if (current == destination)
                {
                    return visited.Count();
                }

                var paths = array[current].Tunnels.Where(x => !visited.Contains(x));
                var pathDistances = paths
                    .Select(path => TaversePath(path, destination, new HashSet<string>(visited) { current }))
                    .Where(x => x.HasValue).Min();

                return pathDistances;
            }

            foreach(var minorValve in array.Values.Where(x => x.PathScores is null))
            {
                array.Remove(minorValve.Name);
            }

            return array;
        }
    }
}

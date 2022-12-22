using Day16.Models;
using System.Linq;

namespace Day16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runWithSampleData = false;

            var input = File.ReadAllLines(runWithSampleData
                ? "Data/sample-input.txt"
                : "Data/input.txt");

            var map = input.ParseInput();
            var pathScores = new List<PathScore>();
            var startingState = new WorldState
            {
                Time = 26,
                Pressure = 0,
                PressureReleased = 0,
                VisitedValves = new() { "AA" },
                CurrentValve = map["AA"],
                Map = map,
                PathScores = pathScores
            };


            var result = TravelToNextValue(startingState);
            //Console.WriteLine($"final score: {result}");
            //2581 - too high
            //1066 - too low
            //2253

            var orderedScores = pathScores
                .OrderByDescending(x => x.Score);

            var combinedScores = new List<int>();
            foreach (var set in orderedScores.Take(pathScores.Count / 2))
            {
                var left = set;
                var right = orderedScores.First(x => x.Path.Overlaps(set.Path) == false);
                combinedScores.Add(left.Score + right.Score);
            }
            Console.WriteLine($"combined score : {combinedScores.Max()}");
            ;
        }

        static int TravelToNextValue(WorldState state)
        {
            state.Pressure += state.CurrentValve.FlowRate;
            var remainingValves = state.CurrentValve.PathScores
                .Where(x => !state.VisitedValves.Contains(x.Key))
                .OrderByDescending(x => x.Value)
                .ToList();

            if ((state.CurrentValve.PathScores.Count / 2) == remainingValves.Count())
            {
                return CalculateRemainingPressureReleased(state);
            }

            var score = new List<int>();
            foreach (var valve in remainingValves)
            {
                var timeTaken = valve.Value + 1;
                if (timeTaken > state.Time) continue;

                var newState = state.CopyState();
                newState.Time -= timeTaken;
                newState.PressureReleased += timeTaken * state.Pressure;
                newState.CurrentValve = state.Map[valve.Key];
                newState.VisitedValves.Add(valve.Key);

                score.Add(TravelToNextValue(newState));
            }

            if (score.Any() == false)
            {
                return CalculateRemainingPressureReleased(state);
            }

            return score.Max();
        }

        static int CalculateRemainingPressureReleased(WorldState state)
        {
            var p = state.PressureReleased + (state.Time * state.Pressure);
            Console.WriteLine($"score: {p}, state: {string.Join(",", state.VisitedValves)}");
            state.PathScores.Add(new PathScore
            {
                Path = state.VisitedValves.Except(new[] { "AA" }).ToHashSet(),
                Score = p
            });
            return p;
        }
    }
}
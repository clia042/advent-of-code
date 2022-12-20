using Day16.Models;

namespace Day16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runWithSampleData = true;
            //*
            var input = File.ReadAllLines(runWithSampleData
                ? "Data/sample-input.txt"
                : "Data/input.txt");

            var map = input.ParseInput();
            var startingState = new WorldState
            {
                Time = 26,
                Pressure = 0,
                PressureReleased = 0,
                VisitedValves = new() { "AA" },
                CurrentValve = map["AA"],
                Map = map
            };
            var result = TravelToNextValue(startingState);
            Console.WriteLine($"final score: {result}");
            //2581 - too high
            //1066 - too low
            //2253
            ;
        }

        static int TravelToNextValue(WorldState state)
        {
            state.Pressure += state.CurrentValve.FlowRate;
            var remainingValves = state.CurrentValve.PathScores
                .Where(x => !state.VisitedValves.Contains(x.Key))
                .OrderByDescending(x => x.Value)
                .ToList();

            if (remainingValves.Any() == false)
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

            if(score.Any() == false)
            {
                return CalculateRemainingPressureReleased(state);
            }

            return score.Max();
        }

        static int CalculateRemainingPressureReleased(WorldState state)
        {
            var p = state.PressureReleased + (state.Time * state.Pressure);
            Console.WriteLine($"score: {p}, state: {string.Join(",", state.VisitedValves)}");
            return p;
        }
    }
}
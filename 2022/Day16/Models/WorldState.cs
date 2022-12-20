using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16.Models
{
    public class WorldState
    {
        public int Time { get; set; }
        public int PressureReleased { get; set; }
        public int Pressure { get; set; }
        public HashSet<string> VisitedValves { get; set; }
        public Valve CurrentValve { get; set; }
        public Dictionary<string, Valve> Map { get; set; }

        public WorldState CopyState()
        {
            return new WorldState
            {
                Time = Time,
                PressureReleased = PressureReleased,
                Pressure = Pressure,
                VisitedValves = new(VisitedValves),
                Map = Map
            };
        }
    }
}

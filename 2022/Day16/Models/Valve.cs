using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16.Models
{
    public class Valve
    {
        public Valve(string name, int flowRate, string[] tunnels)
        {
            Name = name;
            FlowRate = flowRate;
            Tunnels = tunnels;
        }

        public string Name { get; }
        public int FlowRate { get; }
        public string[] Tunnels { get; }

        public Dictionary<string, int> PathScores { get; set; }
    }
}

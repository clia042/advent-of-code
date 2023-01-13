using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18.Models
{
    public class FloodState
    {
        public (int, int) Xboundries { get; set; }
        public (int, int) Yboundries { get; set; }
        public (int, int) Zboundries { get; set; }

        public HashSet<string> SeenNodes { get; set; }

        public Dictionary<string, Coordinate> Matrix { get; set; }
        public Dictionary<string, ShadowCoordinate> ShadowMatrix { get; set; }
    }
}

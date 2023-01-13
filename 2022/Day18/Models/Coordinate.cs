using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18.Models
{
    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int ExposedSurfaces { get; set; }
    }

    public class ShadowCoordinate : Coordinate
    {
        public HashSet<string> Neighbours { get; set; }

        public string? Evaluation { get; set; }
    }
}

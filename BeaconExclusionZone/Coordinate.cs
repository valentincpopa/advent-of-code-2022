using System;

namespace BeaconExclusionZone
{
    public class Coordinate
    {
        public Coordinate(int x, int y, char value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Value { get; set; }
        public int ManhattanDistance { get; set; }
    }
}

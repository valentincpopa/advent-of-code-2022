using System;
using System.Collections.Generic;
using System.Linq;

namespace PyroclasticFlow
{
    public class Rock : IEquatable<Rock>
    {
        public RockType RockType { get; set; }
        public List<Coordinate> Coordinates { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rock);
        }

        public bool Equals(Rock other)
        {
            return other is not null &&
                   RockType == other.RockType &&
                   Coordinates.Intersect(other.Coordinates).Count() == Coordinates.Count;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var coordinate in Coordinates)
                {
                    hash = hash * 31 + coordinate.GetHashCode();
                }
                return hash;
            }
        }
    }
}

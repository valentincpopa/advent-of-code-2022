using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTopTreeHouse
{
    public class Index
    {
        public Index(int heightPosition, int lengthPosition)
        {
            HeightPosition = heightPosition;
            LengthPosition = lengthPosition;
        }

        public int HeightPosition { get; set; }
        public int LengthPosition { get; set; }
    }
}

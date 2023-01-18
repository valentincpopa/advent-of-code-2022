using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyInTheMiddle
{
    public class Item
    {
        public Item(double worryLevel)
        {
            WorryLevel = worryLevel;
        }

        public double WorryLevel { get; set; }
    }
}

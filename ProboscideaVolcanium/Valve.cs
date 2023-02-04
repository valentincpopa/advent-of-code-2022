using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProboscideaVolcanium
{
    public class Valve
    {
        public int Index { get; set; }
        public string Identifier { get; set; }
        public int Cost { get; set; }
        public int FlowRate { get; set; }
        public int OpenedAt { get; set; }
        public List<Valve> Neigbours { get; set; }
        public List<string> NeigbourIdentifiers { get; set; }
    }
}

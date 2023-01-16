using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RopeBridge
{
    public class Instruction
    {
        public Instruction(string code, int count)
        {
            Code = code;
            Count = count;
        }

        public string Code { get; set; }
        public int Count { get; set; }
    }
}

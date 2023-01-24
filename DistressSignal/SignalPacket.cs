using System;

namespace DistressSignal
{
    public class SignalPacket : IComparable<SignalPacket>
    {
        public int Value { get; set; }

        public int CompareTo(SignalPacket other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}

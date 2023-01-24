using System;
using System.Collections.Generic;

namespace DistressSignal
{
    public class SignalPacketCollection : SignalPacket, IComparable<SignalPacketCollection>
    {
        public SignalPacketCollection()
        {
            SignalPackets = new List<SignalPacket>();
        }

        public List<SignalPacket> SignalPackets { get; set; }

        public int CompareTo(SignalPacketCollection other)
        {
            var length = Math.Min(SignalPackets.Count, other.SignalPackets.Count);
            var lastComparisonResult = 0;

            for (int i = 0; i < length; i++)
            {
                if (lastComparisonResult == 1 || lastComparisonResult == -1)
                {
                    return lastComparisonResult;
                }

                if (SignalPackets[i] is SignalPacketCollection && other.SignalPackets[i] is SignalPacketCollection)
                {
                    lastComparisonResult = ((SignalPacketCollection)SignalPackets[i]).CompareTo((SignalPacketCollection)other.SignalPackets[i]);
                    continue;
                }

                if (SignalPackets[i] is SignalPacketCollection)
                {
                    var signalPacketCollection = new SignalPacketCollection();
                    signalPacketCollection.SignalPackets.Add(other.SignalPackets[i]);
                    lastComparisonResult = ((SignalPacketCollection)SignalPackets[i]).CompareTo(signalPacketCollection);
                    continue;
                }

                if (other.SignalPackets[i] is SignalPacketCollection)
                {
                    var signalPacketCollection = new SignalPacketCollection();
                    signalPacketCollection.SignalPackets.Add(SignalPackets[i]);
                    lastComparisonResult = (signalPacketCollection).CompareTo((SignalPacketCollection)other.SignalPackets[i]);
                    continue;
                }

                lastComparisonResult = SignalPackets[i].CompareTo(other.SignalPackets[i]);
            }

            if (lastComparisonResult == 0)
            {
                lastComparisonResult = SignalPackets.Count.CompareTo(other.SignalPackets.Count);
            }

            return lastComparisonResult;
        }
    }
}

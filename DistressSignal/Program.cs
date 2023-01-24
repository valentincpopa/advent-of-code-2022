using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DistressSignal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ResolveFirstHalf();
            ResolveSecondHalf();
        }

        static void ResolveFirstHalf()
        {
            var input = File.ReadAllLines("./input.txt");
            var packetPairs = input
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Chunk(2)
                .ToList();

            var packetsInRightOrderIndices = new List<int>();

            foreach (var packetPair in packetPairs)
            {
                var left = ParseInput(packetPair[0]);
                var right = ParseInput(packetPair[1]);
                if (left.CompareTo(right) == -1)
                {
                    packetsInRightOrderIndices.Add(packetPairs.IndexOf(packetPair) + 1);
                }
            }

            Console.WriteLine(packetsInRightOrderIndices.Sum());
        }

        static void ResolveSecondHalf()
        {
            var input = File.ReadAllLines("./input.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var packets = new List<SignalPacketCollection>();
            input.ForEach(line => packets.Add(ParseInput(line)));

            var firstDividerPacket = BuildPredefinedSignalPacketCollection(2);
            var secondDividerPacket = BuildPredefinedSignalPacketCollection(6);

            packets.Add(firstDividerPacket);
            packets.Add(secondDividerPacket);

            var orderedPackets = packets.Order().ToList();

            var decoderKey = (orderedPackets.IndexOf(firstDividerPacket) + 1) * (orderedPackets.IndexOf(secondDividerPacket) + 1);
            Console.WriteLine(decoderKey);
        }

        static SignalPacketCollection ParseInput(string input)
        {
            string currentNumber = string.Empty;

            SignalPacketCollection currentCollection = null;
            var collectionStack = new Stack<SignalPacketCollection>();

            foreach (var character in input)
            {
                if (character == '[')
                {
                    var newCollection = new SignalPacketCollection();

                    if (currentCollection == null)
                    {
                        currentCollection = newCollection;
                    }
                    else
                    {
                        currentCollection.SignalPackets.Add(newCollection);
                    }
                    collectionStack.Push(currentCollection);

                    currentCollection = newCollection;
                }
                else if (character == ']')
                {
                    ProcessNumberInformation(currentCollection, ref currentNumber);
                    currentCollection = collectionStack.Pop();
                }
                else if (character == ',')
                {
                    ProcessNumberInformation(currentCollection, ref currentNumber);
                }
                else if (character >= '0' && character <= '9')
                {
                    currentNumber += character;
                }
            }

            return currentCollection;
        }

        static void ProcessNumberInformation(SignalPacketCollection signalPacketCollection, ref string currentNumber)
        {
            if (currentNumber != string.Empty)
            {
                signalPacketCollection.SignalPackets.Add(new SignalPacket { Value = int.Parse(currentNumber) });
                currentNumber = string.Empty;
            }
        }

        static SignalPacketCollection BuildPredefinedSignalPacketCollection(int value)
        {
            return new SignalPacketCollection
            {
                SignalPackets = new List<SignalPacket>
                {
                    new SignalPacketCollection
                    {
                        SignalPackets = new List<SignalPacket>
                        {
                            new SignalPacket{ Value = value }
                        }
                    }
                }
            };
        }
    }
}
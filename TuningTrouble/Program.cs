using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TuningTrouble
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Resolve(4);
            Resolve(14);
        }

        static void Resolve(int noOfDistinctCharacters)
        {
            var signalMessage = File.ReadAllText("./input.txt");

            var charIndex = 0;
            var marker = new Queue<char>();

            while (true)
            {
                if (marker.Count > noOfDistinctCharacters)
                {
                    marker.Dequeue();
                }

                if (marker.Count == noOfDistinctCharacters && marker.Distinct().Count() == noOfDistinctCharacters)
                {
                    Console.WriteLine(charIndex);
                    break;
                }

                var currentChar = signalMessage[charIndex];
                marker.Enqueue(currentChar);

                charIndex++;
            }
        }
    }
}
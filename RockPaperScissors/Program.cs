using System;
using System.Collections.Generic;
using System.IO;

namespace RockPaperScissors
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //first half
            var possibilities = new Dictionary<string, int> { { "A X", 4 }, { "A Y", 8 }, { "A Z", 3 },
                                                              { "B X", 1 }, { "B Y", 5 }, { "B Z", 9 },
                                                              { "C X", 7 }, { "C Y", 2 }, { "C Z", 6 } };

            using var fileStream1 = File.OpenRead("./input.txt");
            using var streamReader1 = new StreamReader(fileStream1);

            string currentLine = streamReader1.ReadLine();
            var score = 0;

            while (currentLine != null)
            {
                score += possibilities.GetValueOrDefault(currentLine);
                currentLine = streamReader1.ReadLine();
            }

            Console.WriteLine(score);

            //second half
            var outcomes = new Dictionary<string, string> { { "A X", "Z" }, { "A Y", "X" }, { "A Z", "Y" },
                                                            { "B X", "X" }, { "B Y", "Y" }, { "B Z", "Z" },
                                                            { "C X", "Y" }, { "C Y", "Z" }, { "C Z", "X" } };

            using var fileStream2 = File.OpenRead("./input.txt");
            using var streamReader2 = new StreamReader(fileStream2);

            currentLine = streamReader2.ReadLine();
            score = 0;

            while (currentLine != null)
            {
                var currentHandShape = outcomes.GetValueOrDefault(currentLine);

                score += possibilities.GetValueOrDefault(currentLine[..2] + currentHandShape);
                currentLine = streamReader2.ReadLine();
            }

            Console.WriteLine(score);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RucksackReorganization
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
            using var fileStream = File.OpenRead("./input.txt");
            using var streamReader = new StreamReader(fileStream);

            var itemsPriorities = new List<int>();
            string currentLine = streamReader.ReadLine();

            while (currentLine != null)
            {
                var firstCompartment = currentLine.Substring(0, currentLine.Length / 2);
                var secondCompartment = currentLine.Substring(currentLine.Length / 2, currentLine.Length / 2);

                var duplicatedItems = firstCompartment.Intersect(secondCompartment);
                itemsPriorities.Add(ComputePriorities(duplicatedItems));

                currentLine = streamReader.ReadLine();
            }

            Console.WriteLine(itemsPriorities.Sum());
        }

        static void ResolveSecondHalf()
        {
            var itemsPriorities = new List<int>();
            var lines = File.ReadAllLines("./input.txt");

            var rucksackGroups = lines.Chunk(3);

            foreach (var rucksackGroup in rucksackGroups)
            {
                var badge = rucksackGroup[0].Intersect(rucksackGroup[1]).Intersect(rucksackGroup[2]);
                itemsPriorities.Add(ComputePriorities(badge));
            }

            Console.WriteLine(itemsPriorities.Sum());
        }

        static int ComputePriorities(IEnumerable<char> characterList)
        {
            var sumOfPriorities = 0;

            foreach (var character in characterList)
            {
                if (char.IsUpper(character))
                {
                    sumOfPriorities += (int)character - 38;
                }
                else
                {
                    sumOfPriorities += (int)character - 96;
                }
            }

            return sumOfPriorities;
        }
    }
}
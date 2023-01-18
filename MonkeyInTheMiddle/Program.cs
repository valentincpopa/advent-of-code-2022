using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonkeyInTheMiddle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rawNotes = File.ReadAllLines("./input.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            Resolve(20, rawNotes, false);
            Resolve(10000, rawNotes, true);
        }

        static void Resolve(int rounds, List<string> rawNotes, bool reliefIsModulo)
        {
            var monkeys = new List<Monkey>();
            var monkeysRawNotes = rawNotes.Chunk(6).ToList();

            foreach (var monkeyRawNotes in monkeysRawNotes)
            {
                var monkey = new Monkey();
                monkey.ParseInput(monkeyRawNotes);
                monkeys.Add(monkey);
            }

            var leastCommonMultiple = monkeys.Select(x => x.TestCondition).Aggregate((x, y) => x * y);

            for (int i = 0; i < rounds; i++)
            {

                foreach (var monkey in monkeys)
                {
                    var processedItems = monkey.ProcessItems(reliefIsModulo, reliefIsModulo ? leastCommonMultiple : 3);

                    while (processedItems.Any())
                    {
                        (var item, var monkeyIdentifier) = processedItems.Dequeue();
                        var monkeyToReceiveItem = monkeys.First(x => x.Identifier == monkeyIdentifier);
                        monkeyToReceiveItem.Items.Enqueue(item);
                    }
                }
            }

            var mostActiveMonkeys = monkeys.OrderByDescending(x => x.InspectionsCount).Take(2).ToList();

            Console.WriteLine(mostActiveMonkeys[0].InspectionsCount * mostActiveMonkeys[1].InspectionsCount);
        }
    }
}
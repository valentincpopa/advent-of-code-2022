using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SupplyStacks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Resolve(ProcessInstruction_FirstHalf);
            Resolve(ProcessInstruction_SecondHalf);
        }

        static void Resolve(Action<List<Stack<char>>, string> instructionProcessingStrategy)
        {
            var supplyStacksInput = File.ReadAllLines("./input.txt");

            var supplyStacks = BuildSupplyStacks(supplyStacksInput);
            var instructions = supplyStacksInput
                .Where(s => s.StartsWith("move"))
                .ToList();

            instructions.ForEach(instruction => instructionProcessingStrategy(supplyStacks, instruction));

            foreach (var stack in supplyStacks)
            {
                Console.Write(stack.Pop());
            }

            Console.WriteLine();
        }        

        static List<Stack<char>> BuildSupplyStacks(string[] supplyStacksInput)
        {
            var supplyStacksRawStructure = supplyStacksInput
                .TakeWhile(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            var supplyStacksRawIds = supplyStacksRawStructure.Last();
            var supplyStacksIds = supplyStacksRawIds.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            supplyStacksRawStructure.Remove(supplyStacksRawIds);

            var supplyStacks = new List<Stack<char>>();

            foreach (var supplyStackId in supplyStacksIds)
            {
                var supplyStackItemIndex = supplyStacksRawIds.IndexOf(supplyStackId);
                var supplyItems = new List<char>();

                foreach (var rawStructure in supplyStacksRawStructure)
                {
                    var supplyItem = rawStructure[supplyStackItemIndex];
                    if (char.IsWhiteSpace(supplyItem))
                    {
                        continue;
                    }

                    supplyItems.Add(supplyItem);
                }

                var supplyStack = new Stack<char>();

                supplyItems.Reverse();
                supplyItems.ForEach(supplyStack.Push);

                supplyStacks.Add(supplyStack);
            }

            return supplyStacks;
        }

        static void ProcessInstruction_FirstHalf(List<Stack<char>> supplyStacks, string instruction)
        {
            var instructionRegex = new Regex("move ([0-9]+) from ([0-9]+) to ([0-9]+)");
            var instructionParameters = instructionRegex.Match(instruction).Groups.Values.ToList();

            for (int i = 0; i < int.Parse(instructionParameters[1].Value); i++)
            {
                var supplyItem = supplyStacks[int.Parse(instructionParameters[2].Value) - 1].Pop();
                supplyStacks[int.Parse(instructionParameters[3].Value) - 1].Push(supplyItem);
            }
        }

        static void ProcessInstruction_SecondHalf(List<Stack<char>> supplyStacks, string instruction)
        {
            var instructionRegex = new Regex("move ([0-9]+) from ([0-9]+) to ([0-9]+)");
            var instructionParameters = instructionRegex.Match(instruction).Groups.Values.ToList();

            var retrievedSupplyItems = new List<char>();
            for (int i = 0; i < int.Parse(instructionParameters[1].Value); i++)
            {
                retrievedSupplyItems.Add(supplyStacks[int.Parse(instructionParameters[2].Value) - 1].Pop());
            }
            retrievedSupplyItems.Reverse();

            foreach (var supplyItem in retrievedSupplyItems)
            {
                supplyStacks[int.Parse(instructionParameters[3].Value) - 1].Push(supplyItem);
            }
        }
    }
}
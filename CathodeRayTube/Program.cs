using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CathodeRayTube
{
    internal class Program
    {
        static List<int> cyclesToProbe = new() { 20, 60, 100, 140, 180, 220 };
        static List<int> relevantCrtCycles = new() { 40, 80, 120, 160, 200, 240 };

        static void Main(string[] args)
        {
            var rawInstructions = File.ReadAllLines("./input.txt");
            var instructions = ParseInstructions(rawInstructions);
            Resolve(instructions);
        }

        static void Resolve(Queue<Instruction> instructions)
        {
            var signalStrengths = new List<int>();
            var crtImage = new List<List<int>>();
            var instruction = instructions.Dequeue();

            var currentCycle = 1;
            var registerValue = 1;
            var currentCrtLine = new List<int>();

            var sprite = GetSprite(1);

            while (true)
            {
                if (!instructions.Any())
                {
                    break;
                }

                ProcessPixel(currentCrtLine, sprite);

                currentCycle++;
                instruction.ProcessingTime--;

                (instruction, sprite) = ProcessInstruction(ref registerValue, instruction, instructions, sprite);

                ProbeCycle(currentCycle, registerValue, signalStrengths);

                currentCrtLine = ProcessCrtLine(currentCycle, currentCrtLine, crtImage);
            }

            Console.WriteLine(signalStrengths.Sum());

            crtImage.Add(currentCrtLine);

            PrintCrtImage(crtImage);
        }

        static (Instruction, int[]) ProcessInstruction(ref int registerValue, Instruction instruction, Queue<Instruction> instructions, int[] sprite)
        {
            if (instruction.ProcessingTime == 0)
            {
                if (instruction.Value.HasValue)
                {
                    registerValue += instruction.Value.Value;
                }

                sprite = GetSprite(registerValue);
                instruction = instructions.Dequeue();
            }

            return (instruction, sprite);
        }

        static List<int> ProcessCrtLine(int currentCycle, List<int> currentCrtLine, List<List<int>> crtImage)
        {
            if (relevantCrtCycles.Any(x => x == currentCycle - 1))
            {
                crtImage.Add(currentCrtLine);
                currentCrtLine = new List<int>();
            }

            return currentCrtLine;
        }

        static void ProbeCycle(int currentCycle, int registerValue, List<int> signalStrengths)
        {
            if (cyclesToProbe.Any(x => x == currentCycle))
            {
                signalStrengths.Add(currentCycle * registerValue);
            }
        }

        static void PrintCrtImage(List<List<int>> crtImage)
        {
            foreach (var crtLine in crtImage)
            {
                foreach (var pixel in crtLine)
                {
                    Console.Write(pixel == 1 ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        static void ProcessPixel(List<int> currentCrtLine, int[] sprite)
        {
            currentCrtLine.Add(sprite[currentCrtLine.Count]);
        }

        static int[] GetSprite(int registerValue)
        {
            var sprite = new int[40];

            if (ValueIsValidSpritePixel(registerValue - 1, sprite))
            {
                sprite[registerValue - 1] = 1;
            }

            if (ValueIsValidSpritePixel(registerValue, sprite))
            {
                sprite[registerValue] = 1;
            }

            if (ValueIsValidSpritePixel(registerValue + 1, sprite))
            {
                sprite[registerValue + 1] = 1;
            }

            return sprite;
        }

        static bool ValueIsValidSpritePixel(int value, int[] sprite)
        {
            return value >= sprite.GetLowerBound(0) && value <= sprite.GetUpperBound(0);
        }

        static Queue<Instruction> ParseInstructions(string[] rawInstructions)
        {
            var instructions = new Queue<Instruction>();
            foreach (var instruction in rawInstructions)
            {
                var instructionTokens = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (instructionTokens.Length == 2)
                {
                    instructions.Enqueue(new Instruction(2, int.Parse(instructionTokens[1])));
                }
                else
                {
                    instructions.Enqueue(new Instruction(1));
                }
            }

            return instructions;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RopeBridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Resolve(2);
            Resolve(10);
        }

        static void Resolve(int noOfKnots)
        {
            var rawInstructions = File.ReadAllLines("./input.txt");

            var instructions = ParseInstructions(rawInstructions);

            var knots = new List<Coordinate>();
            for (int i = 0; i < noOfKnots; i++)
            {
                knots.Add(new Coordinate(0, 0));
            }

            var visitedTailCoordinates = new HashSet<Coordinate> { new Coordinate(0, 0) };

            foreach (var instruction in instructions)
            {
                ProcessInstruction(visitedTailCoordinates, instruction, knots);
            }

            Console.WriteLine(visitedTailCoordinates.Count);
        }

        static void ProcessInstruction(HashSet<Coordinate> visitedTailCoordinates, Instruction instruction, List<Coordinate> knots)
        {
            var computeHeadCoordinate = GetComputeHeadCoordinateStrategy(instruction, knots[0]);

            for (int i = 0; i < instruction.Count; i++)
            {
                computeHeadCoordinate(knots[0]);
                for (int j = 0; j < knots.Count - 1; j++)
                {
                    if (ShouldComputeTailCoordinates(knots[j], knots[j + 1]))
                    {
                        var computedTailCoordinate = ComputeTailCoordinate(knots[j], knots[j + 1]);
                        if (knots.Last() == computedTailCoordinate)
                        {
                            visitedTailCoordinates.Add(new Coordinate(computedTailCoordinate.X, computedTailCoordinate.Y));
                        }
                    }
                }
            }
        }

        static Action<Coordinate> GetComputeHeadCoordinateStrategy(Instruction instruction, Coordinate headCoordinate)
        {
            Action<Coordinate> computeHeadCoordinate = null;

            switch (instruction.Code)
            {
                case "R":
                    computeHeadCoordinate = (coordinate) => coordinate.X++;
                    break;
                case "L":
                    computeHeadCoordinate = (coordinate) => coordinate.X--;
                    break;
                case "U":
                    computeHeadCoordinate = (coordinate) => coordinate.Y++;
                    break;
                case "D":
                    computeHeadCoordinate = (coordinate) => coordinate.Y--;
                    break;
                default:
                    break;
            }

            return computeHeadCoordinate;
        }

        static Coordinate ComputeTailCoordinate(Coordinate headCoordinate, Coordinate tailCoordinate)
        {
            var xDiff = headCoordinate.X - tailCoordinate.X;
            var yDiff = headCoordinate.Y - tailCoordinate.Y;
            var xAbs = Math.Abs(xDiff);
            var yAbs = Math.Abs(yDiff);
            var xToAdd = xDiff + Math.Sign(xDiff) * (-1);
            var yToAdd = yDiff + Math.Sign(yDiff) * (-1);

            var newTailCoordinate = (xAbs, yAbs) switch
            {
                (2, 1) => new Coordinate(tailCoordinate.X + xDiff + Math.Sign(xDiff) * (-1), tailCoordinate.Y + yDiff),
                (1, 2) => new Coordinate(tailCoordinate.X + xDiff, tailCoordinate.Y + yDiff + Math.Sign(yDiff) * (-1)),
                (2, 0) => new Coordinate(tailCoordinate.X + xToAdd, tailCoordinate.Y),
                (0, 2) => new Coordinate(tailCoordinate.X, tailCoordinate.Y + yToAdd),
                (2, 2) => new Coordinate(tailCoordinate.X + xToAdd, tailCoordinate.Y + yToAdd),
            };

            tailCoordinate.X = newTailCoordinate.X;
            tailCoordinate.Y = newTailCoordinate.Y;
            return tailCoordinate;
        }

        static bool ShouldComputeTailCoordinates(Coordinate headCoordinate, Coordinate tailCoordinate)
        {
            var xAbs = Math.Abs(headCoordinate.X - tailCoordinate.X);
            var yAbs = Math.Abs(headCoordinate.Y - tailCoordinate.Y);

            return xAbs > 1 || yAbs > 1;
        }

        static List<Instruction> ParseInstructions(string[] rawInstructions)
        {
            var instructions = new List<Instruction>();
            foreach (var instruction in rawInstructions)
            {
                var instructionTokens = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                instructions.Add(new Instruction(instructionTokens[0], int.Parse(instructionTokens[1])));
            }

            return instructions;
        }
    }
}
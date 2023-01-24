using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegolithReservoir
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Resolve(true);
            Resolve(false);
        }

        static void Resolve(bool isFirstHalf)
        {
            var input = File.ReadAllLines("./input.txt")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var rockLinesCoordinates = GetRockLinesCoordinates(input);
            var map = BuildMap(rockLinesCoordinates, isFirstHalf);
            ProcessSandPouring(map, isFirstHalf);
        }

        static List<List<int[]>> GetRockLinesCoordinates(List<string> input)
        {
            var rockLinesCoordinates = new List<List<int[]>>();

            foreach (var line in input)
            {
                var rockLine = new List<int[]>();
                var rawRockLineCoordinates = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);

                foreach (var rawRockCoordinates in rawRockLineCoordinates)
                {
                    var rockCoordinates = rawRockCoordinates
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .ToArray();

                    rockLine.Add(rockCoordinates);
                }

                rockLinesCoordinates.Add(rockLine);
            }

            return rockLinesCoordinates;
        }

        static char[,] BuildMap(List<List<int[]>> rockLinesCoordinates, bool isFirstHalf)
        {
            var (maxDepth, maxWidth) = GetMapBoundaries(rockLinesCoordinates);

            if (!isFirstHalf)
            {
                maxDepth = maxDepth + 2;
                maxWidth = 1000;
            }

            var map = new char[maxDepth, maxWidth];
            FillCaveWithAir(map, maxDepth, maxWidth);
            FillCaveWithRocks(map, rockLinesCoordinates);

            if (!isFirstHalf)
            {
                AddRockLine(map, new int[] { 0, maxDepth - 1 }, new int[] { maxWidth - 1, maxDepth - 1 });
            }

            return map;
        }

        static (int, int) GetMapBoundaries(List<List<int[]>> rockLinesCoordinates)
        {
            var flattenedListOfCoordinates = rockLinesCoordinates.SelectMany(x => x);
            var maxWidth = flattenedListOfCoordinates.Max(x => x[0]) + 1;
            var maxDepth = flattenedListOfCoordinates.Max(x => x[1]) + 1;

            return (maxDepth, maxWidth);
        }

        static void FillCaveWithAir(char[,] map, int maxDepth, int maxWidth)
        {
            for (int i = 0; i < maxDepth; i++)
            {
                for (int j = 0; j < maxWidth; j++)
                {
                    map[i, j] = '.';
                }
            }
        }

        static void FillCaveWithRocks(char[,] map, List<List<int[]>> rockLinesCoordinates)
        {
            foreach (var rockLineCoordinates in rockLinesCoordinates)
            {
                if (rockLineCoordinates.Count == 1)
                {
                    var rockCoordinates = rockLineCoordinates.First();
                    map[rockCoordinates[0], rockCoordinates[1]] = '#';
                    continue;
                }

                for (int i = 1; i < rockLineCoordinates.Count; i++)
                {
                    var startRockCoordinates = rockLineCoordinates[i - 1];
                    var endRockCoordinates = rockLineCoordinates[i];
                    AddRockLine(map, startRockCoordinates, endRockCoordinates);
                }
            }
        }

        static void AddRockLine(char[,] map, int[] startRockCoordinates, int[] endRockCoordinates)
        {
            var horizontalDifference = startRockCoordinates[0] - endRockCoordinates[0];
            var verticalDifference = startRockCoordinates[1] - endRockCoordinates[1];

            var currentRockCoordinate = startRockCoordinates;
            map[endRockCoordinates[1], endRockCoordinates[0]] = '#';
            do
            {
                map[currentRockCoordinate[1], currentRockCoordinate[0]] = '#';
                var verticalToAdd = verticalDifference == 0 ? 0 : Math.Sign(verticalDifference) * -1;
                var horizontalToAdd = horizontalDifference == 0 ? 0 : Math.Sign(horizontalDifference) * -1;

                currentRockCoordinate = new int[] { currentRockCoordinate[0] + horizontalToAdd, currentRockCoordinate[1] + verticalToAdd };
            } while (currentRockCoordinate[0] != endRockCoordinates[0] || currentRockCoordinate[1] != endRockCoordinates[1]);
        }

        static void ProcessSandPouring(char[,] map, bool isFirstHalf)
        {
            var sandPouringSource = new int[] { 500, 0 };

            var possiblePositions = new int[3][]
            {
                new int[2] { 0, 1 },
                new int[2] { -1, 1 },
                new int[2] { 1, 1 },
            };

            int[] currentSandGrain;
            var finalGrainSettled = false;
            var noOfGrainsSettled = 0;

            do
            {
                currentSandGrain = new int[] { sandPouringSource[0], sandPouringSource[1] };
                var grainSettled = false;

                do
                {
                    var positionIterations = 0;

                    foreach (var possiblePosition in possiblePositions)
                    {
                        var temporaryPosition = new int[] { currentSandGrain[0] + possiblePosition[0], currentSandGrain[1] + possiblePosition[1] };

                        if (isFirstHalf)
                        {
                            if (temporaryPosition[1] >= map.GetLength(0))
                            {
                                finalGrainSettled = true;
                                break;
                            }
                        }

                        if (map[temporaryPosition[1], temporaryPosition[0]] == '.')
                        {
                            currentSandGrain = temporaryPosition;
                            break;
                        }

                        positionIterations++;
                    }

                    if (positionIterations == 3)
                    {
                        map[currentSandGrain[1], currentSandGrain[0]] = 'O';
                        grainSettled = true;
                        noOfGrainsSettled++;
                    }

                    if (!isFirstHalf)
                    {
                        if (currentSandGrain[0] == sandPouringSource[0] && currentSandGrain[1] == sandPouringSource[1])
                        {
                            finalGrainSettled = true;
                            break;
                        }
                    }

                    if (finalGrainSettled)
                    {
                        break;
                    }

                } while (!grainSettled);

            } while (!finalGrainSettled);

            Console.WriteLine(noOfGrainsSettled);
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProboscideaVolcanium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("./input.txt")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

            var valves = ParseInput(input);
            var distances = FindShortestPaths(valves);

            ResolveFirstHalf(valves, distances);
            ResolveSecondHalf(valves, distances);
        }

        static void ResolveFirstHalf(List<Valve> valves, int[,] distances)
        {
            var flowRates = new List<int>();
            var currentValve = valves.First(x => x.Identifier == "AA");
            var currentNoOfMinutes = 0;
            var maximumNoOfMinutes = 30;

            var currentPath = new List<Valve>();
            var paths = new List<List<Valve>>();

            Resolve(valves, distances, currentValve, currentNoOfMinutes, maximumNoOfMinutes, flowRates, ref currentPath, paths);
            Console.WriteLine(flowRates.Max());
        }

        static void ResolveSecondHalf(List<Valve> valves, int[,] distances)
        {
            var currentValve = valves.First(x => x.Identifier == "AA");
            var currentNoOfMinutes = 0;
            var maximumNoOfMinutes = 26;

            var currentPath = new List<Valve>();
            var paths = new List<List<Valve>>();

            Resolve(valves, distances, currentValve, currentNoOfMinutes, maximumNoOfMinutes, new List<int>(), ref currentPath, paths);

            var validPermutations = new ConcurrentBag<(List<Valve> humanPath, List<Valve> elephantPath)>();

            //TODO: find an optimal approach
            Parallel.ForEach(paths, path =>
            {
                if (path.Any())
                {
                    var permutations = paths.Where(y => !y.Intersect(path).Any());
                    foreach (var permutation in permutations.Where(x => x.Any()))
                    {
                        validPermutations.Add((path, permutation));
                    }
                }
            });

            var flowRates = new List<(int humanFlowRate, int elephantFlowRate)>();
            foreach (var validPermutation in validPermutations)
            {
                flowRates.Add((GetFlowRate(currentValve, validPermutation.humanPath, distances), GetFlowRate(currentValve, validPermutation.elephantPath, distances)));
            }

            var max = flowRates.Max(x => x.humanFlowRate + x.elephantFlowRate);
            Console.WriteLine(max);
        }

        static int GetFlowRate(Valve startValve, List<Valve> path, int[,] distances)
        {
            var currentValve = startValve;
            var remaining = 26;
            var flowRate = 0;

            for (int i = 0; i < path.Count; i++)
            {
                remaining = remaining - distances[currentValve.Index, path[i].Index] - 1;
                flowRate += remaining * path[i].FlowRate;
                currentValve = path[i];
            }

            return flowRate;
        }

        static void Resolve(List<Valve> valves, int[,] distances, Valve currentValve, int currentNoOfMinutes, int maximumNoOfMinutes, List<int> flowRates, ref List<Valve> currentPath, List<List<Valve>> paths)
        {
            var valvesToVisit = valves.Where(x => x.FlowRate != 0 && x.OpenedAt == -1).ToList();

            if (currentValve.Identifier != "AA")
            {
                currentPath.Add(currentValve);
            }

            var temporaryPath = new Valve[currentPath.Count];
            currentPath.CopyTo(temporaryPath);
            paths.Add(temporaryPath.ToList());

            foreach (var valveToVisit in valvesToVisit)
            {
                var distanceToValve = distances[currentValve.Index, valveToVisit.Index];
                currentNoOfMinutes = currentNoOfMinutes + distanceToValve + 1;

                if (currentNoOfMinutes < maximumNoOfMinutes)
                {
                    valveToVisit.OpenedAt = maximumNoOfMinutes - currentNoOfMinutes;
                    var totalCurrentFlowRate = valves
                        .Where(x => x.FlowRate != 0 && x.OpenedAt != -1)
                        .Aggregate(0, (total, valve) => total += valve.FlowRate * valve.OpenedAt);

                    flowRates.Add(totalCurrentFlowRate);

                    Resolve(valves, distances, valveToVisit, currentNoOfMinutes, maximumNoOfMinutes, flowRates, ref currentPath, paths);

                    temporaryPath = new Valve[currentPath.Count];
                    currentPath.CopyTo(temporaryPath);
                    currentPath = temporaryPath.ToList();
                    currentPath.Remove(currentPath.Last());

                    valveToVisit.OpenedAt = -1;
                }

                currentNoOfMinutes = currentNoOfMinutes - distanceToValve - 1;
            }
        }

        static List<Valve> ParseInput(List<string> input)
        {
            var valves = new List<Valve>();

            var identifierRegex = new Regex("Valve ([A-Z]+) has");
            var flowRateRegex = new Regex("rate=([0-9]+)");
            var neighboursRegex = new Regex("to valves? ([A-Z,\\s]+)");

            foreach (var line in input)
            {
                var identifier = identifierRegex.Match(line).Groups[1].Value;
                var flowRate = flowRateRegex.Match(line).Groups[1].Value;
                var neighbours = neighboursRegex.Match(line).Groups[1].Value;

                valves.Add(new Valve
                {
                    Cost = 1,
                    FlowRate = int.Parse(flowRate),
                    OpenedAt = -1,
                    Identifier = identifier,
                    NeigbourIdentifiers = neighbours.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList()
                });
            }

            foreach (var valve in valves)
            {
                valve.Neigbours = new List<Valve>();
                valve.Neigbours.AddRange(valves.Where(x => valve.NeigbourIdentifiers.Contains(x.Identifier)));
                valve.Index = valves.IndexOf(valve);
            }

            return valves;
        }

        static int[,] FindShortestPaths(List<Valve> valves)
        {
            var distances = new int[valves.Count, valves.Count];

            for (int i = 0; i < valves.Count; i++)
            {
                for (int j = 0; j < valves.Count; j++)
                {
                    distances[i, j] = 100000;
                }
            }

            foreach (var valve in valves)
            {
                distances[valve.Index, valve.Index] = 0;

                foreach (var neighbour in valve.Neigbours)
                {
                    distances[valve.Index, neighbour.Index] = 1;
                }
            }

            for (int k = 0; k < valves.Count; k++)
            {
                for (int i = 0; i < valves.Count; i++)
                {
                    for (int j = 0; j < valves.Count; j++)
                    {
                        if (distances[i, j] > distances[i, k] + distances[k, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                        }
                    }
                }
            }

            return distances;
        }
    }
}
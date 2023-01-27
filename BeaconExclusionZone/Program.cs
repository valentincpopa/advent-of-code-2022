using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BeaconExclusionZone
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("./input.txt")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

            var coordinates = ParseInput(input);
            ResolveFirstHalf(coordinates);
            ResolveSecondHalf(coordinates);
        }

        static List<Coordinate> ParseInput(List<string> input)
        {
            var coordinates = new List<Coordinate>();
            var regex = new Regex("x=([0-9-]+), y=([0-9-]+)");

            foreach (var line in input)
            {
                var match = regex.Matches(line);
                var rawSensorCoordinates = match.First().Groups.Values.Skip(1).ToList();
                var sensorCoordinates = new Coordinate(int.Parse(rawSensorCoordinates[0].Value), int.Parse(rawSensorCoordinates[1].Value), 'S');

                var rawBeaconCoordinates = match.Skip(1).First().Groups.Values.Skip(1).ToList();
                var beaconCoordinates = new Coordinate(int.Parse(rawBeaconCoordinates[0].Value), int.Parse(rawBeaconCoordinates[1].Value), 'B');

                coordinates.Add(sensorCoordinates);
                coordinates.Add(beaconCoordinates);

                var manhattanDistance = Math.Abs(sensorCoordinates.X - beaconCoordinates.X) + Math.Abs(sensorCoordinates.Y - beaconCoordinates.Y);
                sensorCoordinates.ManhattanDistance = manhattanDistance;
            }

            return coordinates;
        }

        static void ResolveFirstHalf(List<Coordinate> coordinates)
        {
            var boundaries = new List<(int min, int max)>();

            foreach (var sensorCoordinates in coordinates.Where(x => x.Value == 'S'))
            {
                var relevantDistance = Math.Abs(sensorCoordinates.X - sensorCoordinates.X) + Math.Abs(sensorCoordinates.Y - 2000000);

                if (sensorCoordinates.ManhattanDistance >= relevantDistance)
                {
                    var distanceDifference = (sensorCoordinates.ManhattanDistance - relevantDistance);
                    boundaries.Add((sensorCoordinates.X - distanceDifference, sensorCoordinates.X + distanceDifference));
                }
            }

            var min = boundaries.Min(x => x.min);
            var max = boundaries.Max(x => x.max);
            var total = max - min;

            Console.WriteLine(total);
        }

        static void ResolveSecondHalf(List<Coordinate> coordinates)
        {
            for (int i = 0; i < 4000000; i++)
            {
                var boundaries = new List<(int min, int max, int row)>();
                foreach (var sensorCoordinates in coordinates.Where(x => x.Value == 'S'))
                {
                    var relevantDistance = Math.Abs(sensorCoordinates.X - sensorCoordinates.X) + Math.Abs(sensorCoordinates.Y - i);

                    if (sensorCoordinates.ManhattanDistance >= relevantDistance)
                    {
                        var distanceDifference = (sensorCoordinates.ManhattanDistance - relevantDistance);

                        boundaries.Add((sensorCoordinates.X - distanceDifference, sensorCoordinates.X + distanceDifference, i));
                    }
                }

                boundaries = boundaries.OrderBy(x => x.min).ToList();

                if (boundaries.Any())
                {
                    var firstBoundary = boundaries.First();
                    var min = firstBoundary.min;
                    var max = firstBoundary.max;

                    foreach (var bound in boundaries.Skip(1))
                    {
                        if (max <= bound.max && max >= bound.min)
                        {
                            max = bound.max;
                        }
                    }

                    if (max != boundaries.Max(x => x.max))
                    {
                        var tuningFrequency = (max + 1) * 4000000L + i;
                        Console.WriteLine(tuningFrequency);
                        break;
                    }
                }
            }
        }
    }
}
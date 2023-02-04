using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PyroclasticFlow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("./input.txt")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

            var jetPattern = input.First().ToList();
            var jetPatternQueue = new Queue<char>(jetPattern);

            var rockTypeQueue = new Queue<RockType>(new List<RockType>
            {
                RockType.HorizontalLine,
                RockType.CrossShaped,
                RockType.LShaped,
                RockType.VerticalLine,
                RockType.SquareShaped
            });

            var (highestCoordinates, rocks) = Process(jetPatternQueue, rockTypeQueue);
            Console.WriteLine(highestCoordinates.Y + 1);
        }

        static (Coordinate highestRockCoordinates, List<Rock> settledRocks) Process(Queue<char> jetPatternQueue, Queue<RockType> rockTypeQueue)
        {
            var currentRockSettled = 0L;
            var settledRocks = new List<Rock>();
            var settledRocksCoordinates = new List<Coordinate> {
                new Coordinate(0, -1),
                new Coordinate(1, -1),
                new Coordinate(2, -1),
                new Coordinate(3, -1),
                new Coordinate(4, -1),
                new Coordinate(5, -1),
                new Coordinate(6, -1),
            };

            var highestRockCoordinates = new Coordinate(0, -1);
            while (currentRockSettled < 2022)
            {
                var rockIsSettled = false;
                var currentRockType = rockTypeQueue.Dequeue();
                var currentRock = CreateRock(currentRockType, highestRockCoordinates);

                while (!rockIsSettled)
                {
                    var currentJet = jetPatternQueue.Dequeue();

                    ProcessGas(currentRock, settledRocksCoordinates, currentJet);
                    ProcessFall(currentRock, settledRocksCoordinates, ref rockIsSettled);

                    jetPatternQueue.Enqueue(currentJet);
                }

                settledRocksCoordinates.AddRange(currentRock.Coordinates);
                highestRockCoordinates = settledRocksCoordinates.OrderByDescending(x => x.Y).First();

                rockTypeQueue.Enqueue(currentRockType);
                currentRockSettled++;
                settledRocks.Add(currentRock);
            }

            return (highestRockCoordinates, settledRocks);
        }

        private static void ProcessFall(Rock rock, List<Coordinate> rocksCoordinates, ref bool rockIsSettled)
        {
            if (ProcessDownMove(rock, rocksCoordinates))
            {
                rockIsSettled = false;
            }
            else
            {
                rockIsSettled = true;
            }
        }

        static bool ProcessRightMove(Rock rock, List<Coordinate> rocksCoordinates)
        {
            MoveRight(rock);
            if (rock.Coordinates.Intersect(rocksCoordinates).Any() || rock.Coordinates.Any(x => x.X > 6))
            {
                MoveLeft(rock);
                return false;
            }
            return true;
        }

        static bool ProcessLeftMove(Rock rock, List<Coordinate> rocksCoordinates)
        {
            MoveLeft(rock);
            if (rock.Coordinates.Intersect(rocksCoordinates).Any() || rock.Coordinates.Any(x => x.X < 0))
            {
                MoveRight(rock);
                return false;
            }
            return true;
        }

        static void MoveRight(Rock currentRock)
        {
            currentRock.Coordinates.ForEach(x => x.X++);
        }

        static void MoveLeft(Rock currentRock)
        {
            currentRock.Coordinates.ForEach(x => x.X--);
        }

        static bool ProcessDownMove(Rock rock, List<Coordinate> rocksCoordinates)
        {
            MoveDown(rock);
            if (rock.Coordinates.Intersect(rocksCoordinates).Any())
            {
                MoveUp(rock);
                return false;
            }
            return true;
        }

        static void MoveUp(Rock rock)
        {
            rock.Coordinates.ForEach(x => x.Y++);
        }

        static void MoveDown(Rock rock)
        {
            rock.Coordinates.ForEach(x => x.Y--);
        }

        private static void ProcessGas(Rock rock, List<Coordinate> rocksCoordinates, char hotGasJet)
        {
            if (hotGasJet == '>')
            {
                ProcessRightMove(rock, rocksCoordinates);
            }
            else if (hotGasJet == '<')
            {
                ProcessLeftMove(rock, rocksCoordinates);
            }
        }

        static Rock CreateRock(RockType rockType, Coordinate coordinate)
        {
            switch (rockType)
            {
                case RockType.HorizontalLine:
                    return BuildHorizontalLineRock(coordinate);
                case RockType.VerticalLine:
                    return BuildVerticalLineRock(coordinate);
                case RockType.LShaped:
                    return BuildLShapedRock(coordinate);
                case RockType.CrossShaped:
                    return BuildCrossShapedRock(coordinate);
                case RockType.SquareShaped:
                    return BuildSquareShapedRock(coordinate);
                default:
                    throw new InvalidOperationException();
            }
        }

        static Rock BuildHorizontalLineRock(Coordinate coordinate)
        {
            var rock = new Rock
            {
                RockType = RockType.HorizontalLine,
                Coordinates = new List<Coordinate>
                {
                    new Coordinate(2, coordinate.Y + 1 + 3),
                    new Coordinate(3, coordinate.Y + 1 + 3),
                    new Coordinate(4, coordinate.Y + 1 + 3),
                    new Coordinate(5, coordinate.Y + 1 + 3),
                }
            };

            return rock;
        }

        static Rock BuildVerticalLineRock(Coordinate coordinate)
        {
            var rock = new Rock
            {
                RockType = RockType.VerticalLine,
                Coordinates = new List<Coordinate>
                {
                    new Coordinate(2, coordinate.Y + 1 + 6),
                    new Coordinate(2, coordinate.Y + 1 + 5),
                    new Coordinate(2, coordinate.Y + 1 + 4),
                    new Coordinate(2, coordinate.Y + 1 + 3),
                }
            };

            return rock;
        }

        static Rock BuildCrossShapedRock(Coordinate coordinate)
        {
            var rock = new Rock
            {
                RockType = RockType.CrossShaped,
                Coordinates = new List<Coordinate>
                {
                    new Coordinate(3, coordinate.Y + 1 + 5),
                    new Coordinate(2, coordinate.Y + 1 + 4),
                    new Coordinate(3, coordinate.Y + 1 + 4),
                    new Coordinate(4, coordinate.Y + 1 + 4),
                    new Coordinate(3, coordinate.Y + 1 + 3),
                }
            };

            return rock;
        }

        static Rock BuildLShapedRock(Coordinate coordinate)
        {
            var rock = new Rock
            {
                RockType = RockType.LShaped,
                Coordinates = new List<Coordinate>
                {
                    new Coordinate(4, coordinate.Y + 1 + 5),
                    new Coordinate(4, coordinate.Y + 1 + 4),
                    new Coordinate(4, coordinate.Y + 1 + 3),
                    new Coordinate(3, coordinate.Y + 1 + 3),
                    new Coordinate(2, coordinate.Y + 1 + 3),
                }
            };

            return rock;
        }

        static Rock BuildSquareShapedRock(Coordinate coordinate)
        {
            var rock = new Rock
            {
                RockType = RockType.SquareShaped,
                Coordinates = new List<Coordinate>
                {
                    new Coordinate(2, coordinate.Y + 1 + 4),
                    new Coordinate(3, coordinate.Y + 1 + 4),
                    new Coordinate(2, coordinate.Y + 1 + 3),
                    new Coordinate(3, coordinate.Y + 1 + 3),
                }
            };

            return rock;
        }
    }
}
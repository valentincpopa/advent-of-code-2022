using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HillClimbingAlgorithm
{
    internal class Program
    {
        static (int i, int j, char value) start;
        static (int i, int j, char value) end;

        static void Main(string[] args)
        {
            Resolve(true);
            Resolve(false);
        }

        static void Resolve(bool isPartOne)
        {
            var map = File.ReadAllLines("./input.txt");
            var height = map.Length;
            var width = map.First().Length;

            var noOfVertices = height * width;
            var verticesVisited = new List<(int i, int j, char value)>();

            InitializeMap(map);

            var directions = new List<(int, int)>
            {
                ( 1,  0),
                (-1,  0),
                ( 0, -1),
                ( 0,  1),
            };

            var unorderedPath = new List<(int i, int j, char value)>();
            for (int i = 0; i < noOfVertices; i++)
            {
                unorderedPath.Add((0, 0, '-'));
            }

            var queue = new Queue<(int i, int j)>();

            if (isPartOne)
            {
                queue.Enqueue((start.i, start.j));
                verticesVisited.Add(start);
            }
            else
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (map[i][j] == 'a')
                        {
                            queue.Enqueue((i, j));
                            verticesVisited.Add((i, j, map[i][j]));
                        }
                    }
                }
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var direction in directions)
                {
                    var (di, dj) = direction;
                    var ni = current.i + di;
                    var nj = current.j + dj;

                    var validNeighbour = 0 <= ni && ni < height && 0 <= nj && nj < width;
                    var isPath = validNeighbour && ((map[ni][nj] - map[current.i][current.j] <= 1));

                    if (isPath)
                    {
                        if (!verticesVisited.Contains((ni, nj, map[ni][nj])))
                        {
                            queue.Enqueue((ni, nj));
                            verticesVisited.Add((ni, nj, map[ni][nj]));
                            unorderedPath[ni * width + nj] = (current.i, current.j, map[ni][nj]);
                        }
                    }
                }
            }

            var currentPosition = end;
            var path = new List<(int i, int j, char value)> { currentPosition };

            while (currentPosition != start && currentPosition != (0, 0, '-'))
            {
                currentPosition = unorderedPath[currentPosition.i * width + currentPosition.j];
                path.Add(currentPosition);
            }
        }

        static void InitializeMap(string[] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Contains('S'))
                {
                    start = (i, map[i].IndexOf("S"), 'a');
                    map[i] = map[i].Replace("S", "a");
                }

                if (map[i].Contains('E'))
                {
                    end = (i, map[i].IndexOf("E"), 'z');
                    map[i] = map[i].Replace("E", "z");
                }
            }
        }
    }
}
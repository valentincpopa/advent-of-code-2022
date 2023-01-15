namespace TreeTopTreeHouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // in an attempt to create an optimal way of solving this I actually managed to make it worse.
            var rawMap = File.ReadAllLines("./input.txt");
            var map = BuildMap(rawMap);

            var indexTable = BuildIndexTable(map);

            ResolveFirstHalf(map, indexTable);
            ResolveSecondHalf(map, indexTable);
        }

        static void ResolveFirstHalf(int[,] map, Dictionary<int, List<Index>> indexTable)
        {
            var maxHeight = map.GetUpperBound(0) + 1;
            var maxLength = map.GetUpperBound(1) + 1;
            var noOfVisibleTrees = 0;

            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxLength; j++)
                {
                    var treeHeight = map[i, j];
                    if (IsVisible(treeHeight, new Index(i, j), indexTable))
                    {
                        noOfVisibleTrees++;
                    }
                }
            }

            Console.WriteLine(noOfVisibleTrees);
        }

        static void ResolveSecondHalf(int[,] map, Dictionary<int, List<Index>> indexTable)
        {
            var maxHeight = map.GetUpperBound(0) + 1;
            var maxLength = map.GetUpperBound(1) + 1;
            var maxScenicScore = 0;

            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxLength; j++)
                {
                    var treeHeight = map[i, j];
                    var currentScenicScore = GetScenicScore(treeHeight, new Index(i, j), indexTable, maxHeight - 1, maxLength - 1);
                    maxScenicScore = currentScenicScore > maxScenicScore ? currentScenicScore : maxScenicScore;
                }
            }

            Console.WriteLine(maxScenicScore);
        }

        static Dictionary<int, List<Index>> BuildIndexTable(int[,] map)
        {
            var indexTable = InitializeIndexTable();

            var maxHeight = map.GetUpperBound(0) + 1;
            var maxLength = map.GetUpperBound(1) + 1;

            for (int i = 0; i < maxHeight; i++)
            {
                for (int j = 0; j < maxLength; j++)
                {
                    var treeHeight = map[i, j];
                    indexTable[treeHeight].Add(new Index(i, j));
                }
            }

            return indexTable;
        }

        static bool IsVisible(int treeHeight, Index index, Dictionary<int, List<Index>> indexTable)
        {
            var filteredIndexTable = indexTable.Where(x => x.Key >= treeHeight).SelectMany(x => x.Value).ToList();

            return !(filteredIndexTable.Any(x => x.HeightPosition == index.HeightPosition && x.LengthPosition < index.LengthPosition)
                && filteredIndexTable.Any(x => x.HeightPosition > index.HeightPosition && x.LengthPosition == index.LengthPosition)
                && filteredIndexTable.Any(x => x.HeightPosition == index.HeightPosition && x.LengthPosition > index.LengthPosition)
                && filteredIndexTable.Any(x => x.HeightPosition < index.HeightPosition && x.LengthPosition == index.LengthPosition));

        }

        static int GetScenicScore(int treeHeight, Index index, Dictionary<int, List<Index>> indexTable, int maxHeight, int maxLength)
        {
            var filteredIndexTable = indexTable.OrderByDescending(x => x.Key).Where(x => x.Key >= treeHeight).SelectMany(x => x.Value).ToList();

            var leftTree = filteredIndexTable.Where(x => x.HeightPosition == index.HeightPosition && x.LengthPosition < index.LengthPosition).OrderByDescending(x => x.LengthPosition).FirstOrDefault();
            var rightTree = filteredIndexTable.Where(x => x.HeightPosition == index.HeightPosition && x.LengthPosition > index.LengthPosition).OrderBy(x => x.LengthPosition).FirstOrDefault();
            var topTree = filteredIndexTable.Where(x => x.HeightPosition < index.HeightPosition && x.LengthPosition == index.LengthPosition).OrderByDescending(x => x.HeightPosition).FirstOrDefault();
            var bottomTree = filteredIndexTable.Where(x => x.HeightPosition > index.HeightPosition && x.LengthPosition == index.LengthPosition).OrderBy(x => x.HeightPosition).FirstOrDefault();

            var leftTreeScenicScore = leftTree == null ? index.LengthPosition : index.LengthPosition - leftTree.LengthPosition;
            var rightTreeScenicScore = rightTree == null ? maxLength - index.LengthPosition : rightTree.LengthPosition - index.LengthPosition;
            var topTreeScenicScore = topTree == null ? index.HeightPosition : index.HeightPosition - topTree.HeightPosition;
            var bottomTreeScenicScore = bottomTree == null ? maxHeight - index.HeightPosition : bottomTree.HeightPosition - index.HeightPosition;

            var scenicScore = leftTreeScenicScore * rightTreeScenicScore * topTreeScenicScore * bottomTreeScenicScore;
            return scenicScore;
        }

        static Dictionary<int, List<Index>> InitializeIndexTable()
        {
            var indexTable = new Dictionary<int, List<Index>>();
            for (int i = 0; i < 10; i++)
            {
                indexTable.Add(i, new List<Index>());
            }

            return indexTable;
        }

        static int[,] BuildMap(string[] rawMap)
        {
            var map = new int[rawMap.Length, rawMap.First().Length];

            for (int i = 0; i < rawMap.Length; i++)
            {
                for (int j = 0; j < rawMap.First().Length; j++)
                {
                    map[i, j] = int.Parse(rawMap[i][j].ToString());
                }
            }

            return map;
        }
    }
}
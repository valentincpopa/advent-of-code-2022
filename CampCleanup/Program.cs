using System;
using System.IO;
using System.Linq;

namespace CampCleanup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ResolveFirstHalf();
            ResolveSecondHalf();
        }

        static void ResolveFirstHalf()
        {
            using var fileStream = File.OpenRead("./input.txt");
            using var streamReader = new StreamReader(fileStream);

            var noOfRedundantAssignments = 0;
            string currentLine = streamReader.ReadLine();

            while (currentLine != null)
            {
                var ranges = currentLine.Split(',');
                var firstPairAssignedSections = ranges[0].Split('-');
                var secondPairAssignedSections = ranges[1].Split('-');

                var firstAssignmentIsContainedInTheSecond =
                       int.Parse(firstPairAssignedSections[0]) >= int.Parse(secondPairAssignedSections[0])
                    && int.Parse(firstPairAssignedSections[1]) <= int.Parse(secondPairAssignedSections[1]);

                var secondAssignmentIsContainedInTheFirst =
                       int.Parse(firstPairAssignedSections[0]) <= int.Parse(secondPairAssignedSections[0])
                    && int.Parse(firstPairAssignedSections[1]) >= int.Parse(secondPairAssignedSections[1]);

                if (firstAssignmentIsContainedInTheSecond || secondAssignmentIsContainedInTheFirst)
                {
                    noOfRedundantAssignments++;
                }

                currentLine = streamReader.ReadLine();
            }

            Console.WriteLine(noOfRedundantAssignments);
        }

        private static void ResolveSecondHalf()
        {
            using var fileStream = File.OpenRead("./input.txt");
            using var streamReader = new StreamReader(fileStream);

            var noOfOverlappingAssignments = 0;
            string currentLine = streamReader.ReadLine();

            while (currentLine != null)
            {
                var ranges = currentLine.Split(',');
                var firstPairAssignedSections = ranges[0].Split('-');
                var secondPairAssignedSections = ranges[1].Split('-');

                if (OverlappingRange(0, firstPairAssignedSections, secondPairAssignedSections) 
                    || OverlappingRange(1, firstPairAssignedSections, secondPairAssignedSections) 
                    || OverlappingRange(0, secondPairAssignedSections, firstPairAssignedSections)
                    || OverlappingRange(1, secondPairAssignedSections, firstPairAssignedSections))
                {
                    noOfOverlappingAssignments++;
                }

                currentLine = streamReader.ReadLine();
            }

            Console.WriteLine(noOfOverlappingAssignments);
        }

        static bool OverlappingRange(int rangeIndex, string[] assignmentPair1, string[] assignmentPair2)
        {
            return int.Parse(assignmentPair1[rangeIndex]) >= int.Parse(assignmentPair2[0])
                && int.Parse(assignmentPair1[rangeIndex]) <= int.Parse(assignmentPair2[1]);
        }
    }
}
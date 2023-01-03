using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalorieCounting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var fileStream = File.OpenRead("./input.txt");
            using var streamReader = new StreamReader(fileStream);

            string currentLine = null;
            var caloriesPerElf = new List<double>();
            var currentElfCalories = 0d;

            do
            {
                currentLine = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(currentLine))
                {
                    caloriesPerElf.Add(currentElfCalories);
                    currentElfCalories = 0;
                    continue;
                }
                
                var itemCalories = double.Parse(currentLine);
                currentElfCalories += itemCalories;

            } 
            while (currentLine != null);

            Console.WriteLine(caloriesPerElf.Max());

            var topThreeElvesCalories = caloriesPerElf.OrderDescending().Take(3).Sum();

            Console.WriteLine(topThreeElvesCalories);
        }
    }
}
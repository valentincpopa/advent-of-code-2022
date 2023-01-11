using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace NoSpaceLeftOnDevice
{
    internal class Program
    {
        static DeviceDirectory root = new("/", null);
        static DeviceDirectory currentDirectory;

        static void Main(string[] args)
        {
            BuildFileSystem();
            root.ComputeSize();
            var flattenedListOfDirectories = GetDirectories(root);

            ResolveFirstHalf(flattenedListOfDirectories);
            ResolveSecondHalf(flattenedListOfDirectories);
        }

        static void ResolveFirstHalf(List<DeviceDirectory> deviceDirectories)
        {
            var filteredDirectories = deviceDirectories.Where(x => x.Size < 100000);
            var totalSize = filteredDirectories.Sum(x => x.Size);

            Console.WriteLine(totalSize);
        }

        static void ResolveSecondHalf(List<DeviceDirectory> deviceDirectories)
        {
            var totalFileSystemSpaceSize = 70000000d;
            var requiredUnusedSpaceSize = 30000000d;

            var usedSpaceSize = root.Size;
            var remainingSpaceSize = totalFileSystemSpaceSize - usedSpaceSize;
            var minimumDirectorySizeToRemove = requiredUnusedSpaceSize - remainingSpaceSize;

            var directoryToRemove = deviceDirectories
                .OrderBy(x => x.Size)
                .First(x => x.Size >= minimumDirectorySizeToRemove);

            Console.WriteLine(directoryToRemove.Size);
        }

        static void BuildFileSystem()
        {
            using var fileStream = File.OpenRead("./input.txt");
            using var streamReader = new StreamReader(fileStream);

            string currentLine = streamReader.ReadLine();

            while (currentLine != null)
            {
                ProcessLine(currentLine);
                currentLine = streamReader.ReadLine();
            }

        }

        static void ProcessLine(string line)
        {
            if (line.StartsWith("$"))
            {
                ProcessCommand(line);
            }
            else
            {
                ProcessOutput(line);
            }
        }

        static void ProcessCommand(string command)
        {
            var commandTokens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (commandTokens[1] == "cd")
            {
                ProcessChangeDirectory(commandTokens[2]);
            }

            return;

        }

        static void ProcessChangeDirectory(string option)
        {
            if (option == "/")
            {
                currentDirectory = root;
            }
            else if (option == "..")
            {
                currentDirectory = currentDirectory.Parent;
            }
            else
            {
                currentDirectory = currentDirectory.GetChildDirectory(option);
            }
        }

        static void ProcessOutput(string output)
        {
            var outputTokens = output.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (outputTokens[0] == "dir")
            {
                ProcessDirectory(outputTokens[1]);
            }
            else
            {
                ProcessFile(outputTokens[1], outputTokens[0]);
            }
        }

        static void ProcessFile(string name, string size)
        {
            currentDirectory.ChildItems.Add(new DeviceFile(name, double.Parse(size), currentDirectory));
        }

        static void ProcessDirectory(string name)
        {
            currentDirectory.ChildItems.Add(new DeviceDirectory(name, currentDirectory));
        }

        static List<DeviceDirectory> GetDirectories(DeviceDirectory directory)
        {
            var directories = new List<DeviceDirectory>
            {
                directory
            };

            var childDirectories = directory.ChildItems
                .OfType<DeviceDirectory>();

            foreach (var childDirectory in childDirectories)
            {
                directories.AddRange(GetDirectories(childDirectory));
            }

            return directories;
        }
    }
}
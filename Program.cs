using aoc2023.Solutions;
using aoc2023.Utils;

if (args.Length < 1 || args.Length > 2)
{
    throw new Exception("Invalid args");
}

var testSuffix = args.Length == 2 && args[1].ToLower() == "test" ? "test" : "";

var fileData = await FileReaderUtils.ReadFile($"{args[0]}{testSuffix}");

switch (args[0])
{
    case "day1":
        Console.WriteLine($"Part 1: {Day1.CalibrationTotalP1(fileData)}");
        Console.WriteLine($"Part 2: {Day1.CalibrationTotalP2(fileData)}");
        break;
    case "day2":
        Console.WriteLine($"Part 1: {Day2.PossibleGames(fileData)}");
        Console.WriteLine($"Part 2: {Day2.CubeMinimums(fileData)}");
        break;
    case "day3":
        Console.WriteLine($"Part 1: {Day3.NumbersBySymbols(fileData)}");
        Console.WriteLine($"Part 2: {Day3.GearRatios(fileData)}");
        break;
    case "day4":
        Console.WriteLine($"Part 1: {Day4.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day4.Part2(fileData)}");
        break;
    case "day5":
        Console.WriteLine($"Part 1: {Day5.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day5.Part2(fileData)}");
        break;
    case "day6":
        Console.WriteLine($"Part 1: {Day6.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day6.Part2(fileData)}");
        break;
    case "day7":
        Console.WriteLine($"Part 1: {Day7.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day7.Part2(fileData)}");
        break;
    case "day8":
        Console.WriteLine($"Part 1: {Day8.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day8.Part2(fileData)}");
        break;
    case "day9":
        Console.WriteLine($"Part 1: {Day9.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day9.Part2(fileData)}");
        break;
    case "day10":
        Console.WriteLine($"Part 1: {Day10.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day10.Part2(fileData)}");
        break;
    case "day11":
        Console.WriteLine($"Part 1: {Day11.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day11.Part2(fileData)}");
        break;
    case "day12":
        Console.WriteLine($"Part 1: {Day12.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day12.Part2(fileData)}");
        break;
    case "day13":
        Console.WriteLine($"Part 1: {Day13.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day13.Part2(fileData)}");
        break;
    case "day14":
        Console.WriteLine($"Part 1: {Day14.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day14.Part2(fileData)}");
        break;
    case "day15":
        Console.WriteLine($"Part 1: {Day15.Part1(fileData[0])}");
        Console.WriteLine($"Part 2: {Day15.Part2(fileData[0])}");
        break;
    case "day16":
        Console.WriteLine($"Part 1: {Day16.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day16.Part2(fileData)}");
        break;
    case "day17":
        Console.WriteLine($"Part 1: {Day17.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day17.Part2(fileData)}");
        break;
    case "day18":
        Console.WriteLine($"Part 1: {Day18.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day18.Part2(fileData)}");
        break;
    case "day19":
        Console.WriteLine($"Part 1: {Day19.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day19.Part2(fileData)}");
        break;
    case "day20":
        // Console.WriteLine($"Part 1: {Day20.Part1(fileData)}");
        Console.WriteLine($"Part 2: {Day20.Part2(fileData)}");
        break;
    default:
        Console.WriteLine("Invalid");
        break;
}

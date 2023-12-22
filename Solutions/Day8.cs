using System.Text.Json;

namespace aoc2023.Solutions
{
    public class Day8
    {
        public static int Part1(string[] input)
        {
            string instructions = input[0];

            Dictionary<string, string[]> AdjacencyList = new Dictionary<string, string[]>();

            for (int i = 2; i < input.Length; i++)
            {
                string[] splitString = input[i].Split(" = ");
                string neighborsString = splitString[1].Substring(1, splitString[1].Length - 2);
                string[] neighbors = neighborsString.Split(", ");
                AdjacencyList.Add(splitString[0], neighbors);
            }

            int result = 0;
            string current = "AAA";

            while (current != "ZZZ" && result <= Int32.MaxValue)
            {
                char currentInstruction = instructions[result % instructions.Length];
                int index = currentInstruction == 'L' ? 0 : 1;
                current = AdjacencyList[current][index];
                result++;
            }

            return result;
        }

        private static long GCD(long a, long b)
        {
            while (b > 0)
            {
                long temp = a;
                a = b;
                b = temp % b;
            }
            return a;
        }

        private static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }

        public static long Part2(string[] input)
        {
            string instructions = input[0];

            Dictionary<string, string[]> AdjacencyList = new Dictionary<string, string[]>();

            for (int i = 2; i < input.Length; i++)
            {
                string[] splitString = input[i].Split(" = ");
                string neighborsString = splitString[1].Substring(1, splitString[1].Length - 2);
                string[] neighbors = neighborsString.Split(", ");
                AdjacencyList.Add(splitString[0], neighbors);
            }
            List<string> starts = new List<string>();

            foreach (string key in AdjacencyList.Keys)
            {
                if (key[^1] == 'A')
                {
                    starts.Add(key);
                }
            }

            List<long> finishes = new List<long>();

            foreach (string start in starts)
            {
                string current = start;
                int finish = 0;
                while (current[^1] != 'Z' && finish <= Int32.MaxValue)
                {
                    char currentInstruction = instructions[finish % instructions.Length];
                    int index = currentInstruction == 'L' ? 0 : 1;
                    current = AdjacencyList[current][index];
                    finish++;
                }
                finishes.Add(finish);
            }

            long result = 1;

            Console.WriteLine(JsonSerializer.Serialize(finishes));
            foreach (long count in finishes)
            {
                result = LCM(result, count);
            }

            return result;
        }
    }
}

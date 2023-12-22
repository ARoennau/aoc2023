using System.Text.Json;
using System.Text.Json.Serialization;

namespace aoc2023.Solutions
{
    public class Day5
    {
        private struct Mapping
        {
            public long DestinationMinimum { get; set; }
            public long DestinationMaximum { get; set; }
            public long SourceMinimum { get; set; }
            public long SourceMaximum { get; set; }
        }

        private static Dictionary<string, List<Mapping>> GenerateMappings(string[] input)
        {
            Dictionary<string, List<Mapping>> maps = new Dictionary<string, List<Mapping>>();
            string? current = null;
            for (int i = 2; i < input.Length; i++)
            {
                string line = input[i];
                if (line == "")
                {
                    current = null;
                    continue;
                }

                if (line[^1] == ':')
                {
                    current = line.Split(" ")[0];
                    maps.Add(current, new List<Mapping>());
                    continue;
                }

                if (current == null || !maps.ContainsKey(current))
                    throw new Exception("An error occurred");

                long[] mapValues = Array.ConvertAll(input[i].Split(" "), Int64.Parse);
                maps[current].Add(
                    new Mapping
                    {
                        DestinationMinimum = mapValues[0],
                        DestinationMaximum = mapValues[0] + mapValues[2] - 1,
                        SourceMinimum = mapValues[1],
                        SourceMaximum = mapValues[1] + mapValues[2] - 1,
                    }
                );
            }
            return maps;
        }

        public static long Part1(string[] input)
        {
            long[] seeds = Array.ConvertAll(input[0].Split(": ")[1].Split(" "), Int64.Parse);
            Dictionary<string, List<Mapping>> maps = GenerateMappings(input);

            string[] stepsInOrder =
            {
                "seed-to-soil",
                "soil-to-fertilizer",
                "fertilizer-to-water",
                "water-to-light",
                "light-to-temperature",
                "temperature-to-humidity",
                "humidity-to-location"
            };

            long minimum = Int64.MaxValue;

            foreach (long seed in seeds)
            {
                long currentValue = seed;
                foreach (string step in stepsInOrder)
                {
                    foreach (Mapping mapping in maps[step])
                    {
                        if (
                            mapping.SourceMinimum <= currentValue
                            && mapping.SourceMaximum >= currentValue
                        )
                        {
                            long offset = currentValue - mapping.SourceMinimum;
                            currentValue = mapping.DestinationMinimum + offset;
                            break;
                        }
                    }
                }
                minimum = Math.Min(minimum, currentValue);
            }

            return minimum;
        }

        public static long Part2(string[] input)
        {
            long[] seedRow = Array.ConvertAll(input[0].Split(": ")[1].Split(" "), Int64.Parse);
            List<long[]> seedRanges = new List<long[]>();
            Dictionary<string, List<Mapping>> maps = GenerateMappings(input);
            for (int i = 0; i < seedRow.Length - 1; i++)
            {
                seedRanges.Add(new long[] { seedRow[i], seedRow[i] + seedRow[i + 1] - 1 });
                i += 1;
            }

            string[] stepsInOrder =
            {
                "humidity-to-location",
                "temperature-to-humidity",
                "light-to-temperature",
                "water-to-light",
                "fertilizer-to-water",
                "soil-to-fertilizer",
                "seed-to-soil",
            };

            for (long i = 0; i < Int64.MaxValue; i++)
            {
                long currentValue = i;
                foreach (string step in stepsInOrder)
                {
                    foreach (Mapping mapping in maps[step])
                    {
                        if (
                            mapping.DestinationMinimum <= currentValue
                            && mapping.DestinationMaximum >= currentValue
                        )
                        {
                            long offset = currentValue - mapping.DestinationMinimum;
                            currentValue = mapping.SourceMinimum + offset;
                            break;
                        }
                    }
                }
                foreach (long[] seedRange in seedRanges)
                {
                    if (seedRange[0] <= currentValue && seedRange[1] >= currentValue)
                        return i;
                }
            }
            return -1;
        }
    }
}

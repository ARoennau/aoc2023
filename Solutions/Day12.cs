using System.Text.Json;

namespace aoc2023.Solutions
{
    public class Day12
    {
        private static Dictionary<string, long> Cache = new Dictionary<string, long>();

        private static long Count(string spring, int substringStartIndex, int[] remainingGroups)
        {
            if (remainingGroups.Length == 0)
            {
                for (int i = substringStartIndex; i < spring.Length; i++)
                {
                    if (spring[i] == '#')
                    {
                        return 0;
                    }
                }
                return 1;
            }

            if (substringStartIndex >= spring.Length)
                return 0;

            if (spring[substringStartIndex] == '.' && substringStartIndex != spring.Length - 1)
            {
                return Count(spring, substringStartIndex + 1, remainingGroups);
            }

            string key = $"{JsonSerializer.Serialize(remainingGroups)}-{substringStartIndex}";
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            if (substringStartIndex == spring.Length - 1 && spring[substringStartIndex] == '.')
            {
                return remainingGroups.Length == 0 ? 1 : 0;
            }

            if (substringStartIndex + remainingGroups[0] > spring.Length)
            {
                return 0;
            }

            if (spring[substringStartIndex] == '#')
            {
                bool chunkWorks = true;
                for (int i = 1; i < remainingGroups[0]; i++)
                {
                    if (spring[substringStartIndex + i] == '.')
                    {
                        chunkWorks = false;
                        break;
                    }
                }

                if (substringStartIndex + remainingGroups[0] == spring.Length && chunkWorks)
                {
                    long value = remainingGroups.Length == 1 ? 1 : 0;
                    Cache.Add(key, value);
                    return value;
                }

                if (substringStartIndex + remainingGroups[0] >= spring.Length)
                {
                    if (!Cache.ContainsKey(key))
                    {
                        Cache.Add(key, 0);
                    }
                    return 0;
                }

                if (!chunkWorks || spring[substringStartIndex + remainingGroups[0]] == '#')
                {
                    if (!Cache.ContainsKey(key))
                    {
                        Cache.Add(key, 0);
                    }
                    return 0;
                }

                long count = Count(
                    spring,
                    substringStartIndex + remainingGroups[0] + 1,
                    remainingGroups.Skip(1).ToArray()
                );
                Cache.Add(key, count);
                return count;
            }

            if (spring[substringStartIndex] == '?')
            {
                long countIfPeriod = Count(spring, substringStartIndex + 1, remainingGroups);
                bool chunkWorks = true;
                for (int i = 1; i < remainingGroups[0]; i++)
                {
                    if (spring[substringStartIndex + i] == '.')
                    {
                        chunkWorks = false;
                        break;
                    }
                }

                if (substringStartIndex + remainingGroups[0] == spring.Length && chunkWorks)
                {
                    long value = remainingGroups.Length == 1 ? 1 : 0;
                    Cache.Add(key, value);
                    return value;
                }

                if (substringStartIndex + remainingGroups[0] >= spring.Length)
                {
                    if (!Cache.ContainsKey(key))
                    {
                        Cache.Add(key, 0);
                    }
                    return 0;
                }

                if (!chunkWorks || spring[substringStartIndex + remainingGroups[0]] == '#')
                {
                    if (!Cache.ContainsKey(key))
                    {
                        Cache.Add(key, countIfPeriod);
                    }
                    return countIfPeriod;
                }

                long count = Count(
                    spring,
                    substringStartIndex + remainingGroups[0] + 1,
                    remainingGroups.Skip(1).ToArray()
                );

                Cache.Add(key, count + countIfPeriod);
                return count + countIfPeriod;
            }

            return 0;
        }

        public static long Part1(string[] input)
        {
            long total = 0;
            foreach (string row in input)
            {
                string[] split = row.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int[] groups = Array.ConvertAll(split[1].Split(","), Int32.Parse);
                string springs = split[0];
                int[] otherGroups = groups.Skip(1).ToArray();
                Cache = new Dictionary<string, long>();
                long rowCount = Count(springs, 0, groups);
                total += rowCount;
            }
            return total;
        }

        public static long Part2(string[] input)
        {
            long total = 0;
            foreach (string row in input)
            {
                string[] split = row.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int[] groups = Array.ConvertAll(split[1].Split(","), Int32.Parse);
                string springs = split[0];
                string springsCopy = springs;

                List<int> groupsList = new List<int>(groups);
                for (int i = 1; i < 5; i++)
                {
                    springsCopy += $"?{springs}";
                    foreach (int num in groups)
                    {
                        groupsList.Add(num);
                    }
                }
                int[] otherGroups = groups.Skip(1).ToArray();
                Cache = new Dictionary<string, long>();
                long rowCount = Count(springsCopy, 0, groupsList.ToArray());
                total += rowCount;
            }
            return total;
        }
    }
}

namespace aoc2023.Solutions
{
    public class Day9
    {
        public static long Part1(string[] input)
        {
            long total = 0;
            foreach (string row in input)
            {
                List<long> numbers = Array
                    .ConvertAll(row.Split(" ", StringSplitOptions.RemoveEmptyEntries), Int64.Parse)
                    .ToList();
                long currentTotal = numbers[^1];

                bool zeroDifferences = false;
                while (!zeroDifferences)
                {
                    zeroDifferences = true;
                    List<long> newRow = new List<long>();
                    for (int i = 1; i < numbers.Count; i++)
                    {
                        long currentDifference = numbers[i] - numbers[i - 1];
                        if (currentDifference != 0)
                        {
                            zeroDifferences = false;
                        }
                        newRow.Add(currentDifference);
                    }
                    currentTotal += newRow[^1];
                    numbers = newRow;
                }

                total += currentTotal;
            }

            return total;
        }

        public static long Part2(string[] input)
        {
            long total = 0;
            foreach (string row in input)
            {
                List<long> numbers = Array
                    .ConvertAll(row.Split(" ", StringSplitOptions.RemoveEmptyEntries), Int64.Parse)
                    .Reverse()
                    .ToList();
                long currentTotal = numbers[^1];

                bool zeroDifferences = false;
                while (!zeroDifferences)
                {
                    zeroDifferences = true;
                    List<long> newRow = new List<long>();
                    for (int i = 1; i < numbers.Count; i++)
                    {
                        long currentDifference = numbers[i] - numbers[i - 1];
                        if (currentDifference != 0)
                        {
                            zeroDifferences = false;
                        }
                        newRow.Add(currentDifference);
                    }
                    currentTotal += newRow[^1];
                    numbers = newRow;
                }

                total += currentTotal;
            }

            return total;
        }
    }
}

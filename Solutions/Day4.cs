namespace aoc2023.Solutions
{
    public static class Day4
    {
        public static int Part1(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                string numbers = line.Split(": ")[1];
                string[] numberSplit = numbers.Split(" | ");
                string[] winningSplit = numberSplit[0].Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries
                );
                HashSet<string> winningNumbers = new HashSet<string>(winningSplit);

                int count = 0;
                string[] yourNumbers = numberSplit[1].Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries
                );
                foreach (string number in yourNumbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        count++;
                    }
                }

                if (count > 0)
                    total += 1 << (count - 1);
            }

            return total;
        }

        public static int Part2(string[] input)
        {
            Dictionary<int, int> cardCounts = new Dictionary<int, int>();
            int N = input.Length;

            foreach (string row in input)
            {
                string leftSplit = row.Split(":")[0];
                string number = leftSplit.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                cardCounts.Add(Int32.Parse(number) - 1, 1);
            }

            for (int i = 0; i < N; i++)
            {
                string line = input[i];
                string numbers = line.Split(": ")[1];
                string[] numberSplit = numbers.Split(" | ");
                string[] winningSplit = numberSplit[0].Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries
                );
                HashSet<string> winningNumbers = new HashSet<string>(winningSplit);

                int count = 0;
                string[] yourNumbers = numberSplit[1].Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries
                );
                foreach (string number in yourNumbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        count++;
                    }
                }

                for (int j = 1; j <= count && i + j < N; j++)
                {
                    cardCounts[i + j] += cardCounts[i];
                }
            }

            return cardCounts.Values.Sum();
        }
    }
}

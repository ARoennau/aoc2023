namespace aoc2023.Solutions
{
    public static class Day1
    {
        private static Dictionary<string, int> numbers = new Dictionary<string, int>()
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        public static int CalibrationTotalP1(string[] input)
        {
            var total = 0;
            foreach (string line in input)
            {
                int leftIndex = 0;
                int rightIndex = line.Length - 1;

                while (
                    leftIndex < line.Length
                    && rightIndex >= 0
                    && (!Char.IsNumber(line[leftIndex]) || !Char.IsNumber(line[rightIndex]))
                )
                {
                    if (!Char.IsNumber(line[leftIndex]))
                        leftIndex++;
                    if (!Char.IsNumber(line[rightIndex]))
                        rightIndex--;
                }
                int current = (line[leftIndex] - '0') * 10 + (line[rightIndex] - '0');
                total += current;
            }

            return total;
        }

        public static int CalibrationTotalP2(string[] input)
        {
            var total = 0;

            foreach (string line in input)
            {
                int leftIndex = 0;
                int rightIndex = line.Length - 1;
                int leftValue = -1;
                int rightValue = -1;

                while (
                    leftIndex < line.Length && rightIndex >= 0 && (leftValue < 0 || rightValue < 0)
                )
                {
                    if (leftValue < 0)
                    {
                        if (Char.IsNumber(line[leftIndex]))
                        {
                            leftValue = line[leftIndex] - '0';
                        }
                        else
                        {
                            var startsWithChar = numbers.Keys.Where(
                                number => number[0] == line[leftIndex]
                            );
                            foreach (string number in startsWithChar)
                            {
                                if (leftIndex + number.Length < line.Length)
                                {
                                    if (line.Substring(leftIndex, number.Length) == number)
                                    {
                                        leftValue = numbers[number];
                                    }
                                }
                            }
                            if (leftValue < 0)
                            {
                                leftIndex++;
                            }
                        }
                    }
                    if (rightValue < 0)
                    {
                        if (Char.IsNumber(line[rightIndex]))
                        {
                            rightValue = line[rightIndex] - '0';
                        }
                        else
                        {
                            var endsWithChar = numbers.Keys.Where(
                                number => number[number.Length - 1] == line[rightIndex]
                            );
                            foreach (string number in endsWithChar)
                            {
                                if (rightIndex - number.Length >= 0)
                                {
                                    if (
                                        line.Substring(
                                            rightIndex - number.Length + 1,
                                            number.Length
                                        ) == number
                                    )
                                    {
                                        rightValue = numbers[number];
                                    }
                                }
                            }
                            if (rightValue < 0)
                            {
                                rightIndex--;
                            }
                        }
                    }
                }
                int current = leftValue * 10 + rightValue;
                total += current;
            }

            return total;
        }
    }
}

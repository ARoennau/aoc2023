using System.Text.Json;

namespace aoc2023.Solutions
{
    public static class Day18
    {
        struct DigInstruction
        {
            public char Direction { get; set; }
            public int Amount { get; set; }
            public string Color { get; set; }

            public DigInstruction(char direction, int amount, string color)
            {
                Direction = direction;
                Amount = amount;
                Color = color;
            }
        }

        struct DigInstruction2
        {
            public char Direction { get; set; }
            public long Amount { get; set; }

            public DigInstruction2(char direction, long amount)
            {
                Direction = direction;
                Amount = amount;
            }
        }

        private static List<DigInstruction> ParseInput(string[] input)
        {
            List<DigInstruction> result = new List<DigInstruction>();
            foreach (string row in input)
            {
                string[] split = row.Split(" ");
                char direction = split[0][0];
                int amount = Int32.Parse(split[1]);
                string color = split[2].Substring(1, 6);
                result.Add(new DigInstruction(direction, amount, color));
            }
            return result;
        }

        private static List<DigInstruction2> ParseInputPart2(string[] input)
        {
            Dictionary<char, char> map = new Dictionary<char, char>()
            {
                { '0', 'R' },
                { '1', 'D' },
                { '2', 'L' },
                { '3', 'U' }
            };

            Dictionary<char, int> digitMap = new Dictionary<char, int>()
            {
                { '0', 0 },
                { '1', 1 },
                { '2', 2 },
                { '3', 3 },
                { '4', 4 },
                { '5', 5 },
                { '6', 6 },
                { '7', 7 },
                { '8', 8 },
                { '9', 9 },
                { 'a', 10 },
                { 'b', 11 },
                { 'c', 12 },
                { 'd', 13 },
                { 'e', 14 },
                { 'f', 15 },
            };
            List<DigInstruction2> result = new List<DigInstruction2>();
            foreach (string row in input)
            {
                string[] split = row.Split(" ");
                string hex = split[2].Substring(2, 6);
                char direction = map[hex[^1]];
                long amount = 0;
                for (int i = 0; i < 5; i++)
                {
                    long current = digitMap[hex[i]] * (long)Math.Pow(16, 5 - i - 1);
                    amount += current;
                }
                result.Add(new DigInstruction2(direction, amount));
            }
            return result;
        }

        // Array is [startRow, startCol, numRows, numCols]
        private static int[] CalculateStartAndDimensions(List<DigInstruction> instructions)
        {
            int row = 0;
            int col = 0;
            int minRow = 0;
            int minCol = 0;
            int maxRow = 0;
            int maxCol = 0;

            foreach (DigInstruction instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case 'L':
                        col -= instruction.Amount;
                        minCol = Math.Min(col, minCol);
                        break;
                    case 'R':
                        col += instruction.Amount;
                        maxCol = Math.Max(col, maxCol);
                        break;
                    case 'U':
                        row -= instruction.Amount;
                        minRow = Math.Min(row, minRow);
                        break;
                    case 'D':
                        row += instruction.Amount;
                        maxRow = Math.Max(row, maxRow);
                        break;
                }
            }

            return new int[] { minRow * -1, minCol * -1, maxRow - minRow + 1, maxCol - minCol + 1 };
        }

        private static long MathStuff2(List<DigInstruction2> instructions)
        {
            List<long[]> vertices = new List<long[]>();
            vertices.Add(new long[] { 0, 0 });
            long currentCol = 0;
            long currentRow = 0;
            long lengthOfPath = 0;
            foreach (var instruction in instructions)
            {
                currentRow +=
                    (
                        instruction.Direction == 'U'
                            ? -1
                            : instruction.Direction == 'D'
                                ? 1
                                : 0
                    ) * instruction.Amount;
                currentCol +=
                    (
                        instruction.Direction == 'L'
                            ? -1
                            : instruction.Direction == 'R'
                                ? 1
                                : 0
                    ) * instruction.Amount;

                vertices.Add(new long[] { currentRow, currentCol });
                lengthOfPath += instruction.Amount;
            }
            long total = 0;
            for (long i = 0; i < vertices.Count; i++)
            {
                long next = (i + 1) % vertices.Count;
                long current =
                    (vertices[(int)i][0] * vertices[(int)next][1])
                    - (vertices[(int)i][1] * vertices[(int)next][0]);
                total += current;
            }
            return (Math.Abs(total) + lengthOfPath) / 2 + 1;
        }

        private static long MathStuff(List<DigInstruction> instructions)
        {
            List<int[]> vertices = new List<int[]>();
            vertices.Add(new int[] { 0, 0 });
            int currentCol = 0;
            int currentRow = 0;
            long lengthOfPath = 0;
            foreach (var instruction in instructions)
            {
                currentRow +=
                    (
                        instruction.Direction == 'U'
                            ? -1
                            : instruction.Direction == 'D'
                                ? 1
                                : 0
                    ) * instruction.Amount;
                currentCol +=
                    (
                        instruction.Direction == 'L'
                            ? -1
                            : instruction.Direction == 'R'
                                ? 1
                                : 0
                    ) * instruction.Amount;

                vertices.Add(new int[] { currentRow, currentCol });
                lengthOfPath += instruction.Amount;
            }
            long total = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                int next = (i + 1) % vertices.Count;
                int current =
                    (vertices[i][0] * vertices[next][1]) - (vertices[i][1] * vertices[next][0]);
                total += current;
            }
            return (Math.Abs(total) + lengthOfPath) / 2 + 1;
        }

        public static long Part1(string[] input)
        {
            List<DigInstruction> instructions = ParseInput(input);

            long mathResult = MathStuff(instructions);

            int[] start = CalculateStartAndDimensions(instructions);
            bool[][] dig = new bool[start[2]][];
            for (int i = 0; i < start[2]; i++)
            {
                dig[i] = new bool[start[3]];
            }

            int currentRow = start[0];
            int currentCol = start[1];

            dig[currentRow][currentCol] = true;

            foreach (var instruction in instructions)
            {
                int rowOffset =
                    instruction.Direction == 'U'
                        ? -1
                        : instruction.Direction == 'D'
                            ? 1
                            : 0;
                int colOffset =
                    instruction.Direction == 'L'
                        ? -1
                        : instruction.Direction == 'R'
                            ? 1
                            : 0;

                for (int i = 0; i < instruction.Amount; i++)
                {
                    currentRow += rowOffset;
                    currentCol += colOffset;
                    dig[currentRow][currentCol] = true;
                }
            }

            int count = 0;
            foreach (var value in dig[0])
            {
                if (value)
                    count++;
            }

            foreach (var value in dig[^1])
            {
                if (value)
                    count++;
            }

            for (int row = 1; row < dig.Length - 1; row++)
            {
                bool isInside = false;
                for (int col = 0; col < dig[0].Length; col++)
                {
                    if (!isInside && dig[row][col])
                    {
                        count++;
                        if (col + 1 < dig[0].Length && !dig[row][col + 1])
                        {
                            isInside = true;
                        }
                        continue;
                    }

                    if (isInside)
                    {
                        if (dig[row][col])
                        {
                            count++;
                            isInside = false;
                            continue;
                        }
                        count++;
                    }
                }
            }

            return count;
        }

        public static long Part2(string[] input)
        {
            var instructions = ParseInputPart2(input);
            return MathStuff2(instructions);
        }
    }
}

namespace aoc2023.Solutions
{
    public static class Day3
    {
        private static HashSet<char> nonSymbols = new HashSet<char>()
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '.'
        };

        private static bool isAdjacentToSymbol(string[] input, int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (
                        row + i < 0
                        || row + i >= input.Length
                        || col + j < 0
                        || col + j >= input[0].Length
                        || (i == 0 && j == 0)
                    )
                    {
                        continue;
                    }

                    if (!nonSymbols.Contains(input[row + i][col + j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int NumbersBySymbols(string[] input)
        {
            int total = 0;
            int rowIndex = 0;
            foreach (string row in input)
            {
                int numberStart = -1;
                bool hasSeenSymbol = false;
                for (int col = 0; col < row.Length; col++)
                {
                    if (numberStart >= 0)
                    {
                        if (Char.IsDigit(row[col]))
                        {
                            hasSeenSymbol =
                                hasSeenSymbol || isAdjacentToSymbol(input, rowIndex, col);

                            if (col == row.Length - 1 && hasSeenSymbol)
                            {
                                int length = col - numberStart + 1;
                                total += Int32.Parse(row.Substring(numberStart, length));
                            }
                        }
                        else
                        {
                            if (hasSeenSymbol)
                            {
                                int length = col - numberStart;
                                total += Int32.Parse(row.Substring(numberStart, length));
                            }
                            numberStart = -1;
                            hasSeenSymbol = false;
                        }
                    }
                    else
                    {
                        if (Char.IsDigit(row[col]))
                        {
                            numberStart = col;
                            hasSeenSymbol =
                                hasSeenSymbol || isAdjacentToSymbol(input, rowIndex, col);
                            if (col == row.Length - 1 && hasSeenSymbol)
                            {
                                total += row[col] - '0';
                            }
                        }
                    }
                }
                rowIndex += 1;
            }
            return total;
        }

        private static List<int[]>? AdjacentNumbers(string[] input, int row, int col)
        {
            List<int[]> numberCoords = new List<int[]>();

            if (col > 0 && col < input[0].Length - 1)
            {
                if (row > 0)
                {
                    if (
                        Char.IsDigit(input[row - 1][col - 1])
                        && !Char.IsDigit(input[row - 1][col])
                        && Char.IsDigit(input[row - 1][col + 1])
                    )
                    {
                        numberCoords.Add(new int[] { row - 1, col - 1 });
                        numberCoords.Add(new int[] { row - 1, col + 1 });
                    }
                    else
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            if (Char.IsDigit(input[row - 1][col + i]))
                            {
                                numberCoords.Add(new int[] { row - 1, col + i });
                                break;
                            }
                        }
                    }
                }

                if (row < input.Length - 1)
                {
                    if (
                        Char.IsDigit(input[row + 1][col - 1])
                        && !Char.IsDigit(input[row + 1][col])
                        && Char.IsDigit(input[row + 1][col + 1])
                    )
                    {
                        numberCoords.Add(new int[] { row + 1, col - 1 });
                        numberCoords.Add(new int[] { row + 1, col + 1 });
                    }
                    else
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            if (Char.IsDigit(input[row + 1][col + i]))
                            {
                                numberCoords.Add(new int[] { row + 1, col + i });
                                break;
                            }
                        }
                    }
                }
                if (col > 0 && Char.IsDigit(input[row][col - 1]))
                    numberCoords.Add(new int[] { row, col - 1 });
                if (col < input[0].Length - 1 && Char.IsDigit(input[row][col + 1]))
                    numberCoords.Add(new int[] { row, col + 1 });
            }

            if (numberCoords.Count != 2)
                return null;
            return numberCoords;
        }

        public static int GearRatios(string[] input)
        {
            int total = 0;
            int rowIndex = 0;
            foreach (string row in input)
            {
                int colIndex = 0;
                foreach (char c in row)
                {
                    if (c == '*')
                    {
                        var coords = AdjacentNumbers(input, rowIndex, colIndex);
                        if (coords != null)
                        {
                            int product = 1;
                            foreach (int[] point in coords)
                            {
                                int leftIndex = point[1];
                                int rightIndex = point[1];
                                bool leftIsAtEnd = false;
                                bool rightIsAtEnd = false;
                                while (!leftIsAtEnd || !rightIsAtEnd)
                                {
                                    leftIsAtEnd =
                                        leftIndex == 0
                                        || !char.IsDigit(input[point[0]][leftIndex - 1]);
                                    rightIsAtEnd =
                                        rightIndex == input[0].Length - 1
                                        || !char.IsDigit(input[point[0]][rightIndex + 1]);
                                    if (!leftIsAtEnd)
                                        leftIndex--;
                                    if (!rightIsAtEnd)
                                        rightIndex++;
                                }
                                int current = Int32.Parse(
                                    input[point[0]].Substring(leftIndex, rightIndex - leftIndex + 1)
                                );
                                product *= current;
                            }
                            total += product;
                        }
                    }
                    colIndex += 1;
                }
                rowIndex += 1;
            }
            return total;
        }
    }
}

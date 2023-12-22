using System.Text.Json;

namespace aoc2023.Solutions
{
    public static class Day14
    {
        private static void MoveUpwards(char[][] matrix, int row, int col)
        {
            int i = row - 1;
            while (i >= 0 && matrix[i][col] == '.')
            {
                matrix[i][col] = matrix[i + 1][col];
                matrix[i + 1][col] = '.';
                i--;
            }
        }

        private static void MoveDownwards(char[][] matrix, int row, int col)
        {
            int i = row + 1;
            while (i < matrix.Length && matrix[i][col] == '.')
            {
                matrix[i][col] = matrix[i - 1][col];
                matrix[i - 1][col] = '.';
                i++;
            }
        }

        private static void MoveLeftwards(char[][] matrix, int row, int col)
        {
            int i = col - 1;
            while (i >= 0 && matrix[row][i] == '.')
            {
                matrix[row][i] = matrix[row][i + 1];
                matrix[row][i + 1] = '.';
                i--;
            }
        }

        private static void MoveRightwards(char[][] matrix, int row, int col)
        {
            int i = col + 1;
            while (i < matrix[row].Length && matrix[row][i] == '.')
            {
                matrix[row][i] = matrix[row][i - 1];
                matrix[row][i - 1] = '.';
                i++;
            }
        }

        public static long Part1(string[] input)
        {
            char[][] matrix = Array.ConvertAll(input, (row) => row.ToCharArray());
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] == 'O')
                    {
                        MoveUpwards(matrix, row, col);
                    }
                }
            }

            long total = 0;
            for (int i = 1; i <= matrix.Length; i++)
            {
                foreach (char c in matrix[matrix.Length - i])
                {
                    if (c == 'O')
                        total += i;
                }
            }
            return total;
        }

        private static void TiltUpwards(char[][] matrix)
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] == 'O')
                    {
                        MoveUpwards(matrix, row, col);
                    }
                }
            }
        }

        private static void TiltLeftwards(char[][] matrix)
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] == 'O')
                    {
                        MoveLeftwards(matrix, row, col);
                    }
                }
            }
        }

        private static void TiltDownwards(char[][] matrix)
        {
            for (int row = matrix.Length - 1; row >= 0; row--)
            {
                for (int col = matrix[row].Length - 1; col >= 0; col--)
                {
                    if (matrix[row][col] == 'O')
                    {
                        MoveDownwards(matrix, row, col);
                    }
                }
            }
        }

        private static void TiltRightwards(char[][] matrix)
        {
            for (int row = matrix.Length - 1; row >= 0; row--)
            {
                for (int col = matrix[row].Length - 1; col >= 0; col--)
                {
                    if (matrix[row][col] == 'O')
                    {
                        MoveRightwards(matrix, row, col);
                    }
                }
            }
        }

        private static long CalculateLoad(char[][] matrix)
        {
            long total = 0;
            for (int i = 1; i <= matrix.Length; i++)
            {
                foreach (char c in matrix[matrix.Length - i])
                {
                    if (c == 'O')
                        total += i;
                }
            }
            return total;
        }

        public static long Part2(string[] input)
        {
            Dictionary<string, long> cache = new Dictionary<string, long>();
            char[][] matrix = Array.ConvertAll(input, (row) => row.ToCharArray());
            for (long i = 1; i <= 1000; i++)
            {
                TiltUpwards(matrix);
                TiltLeftwards(matrix);
                TiltDownwards(matrix);
                TiltRightwards(matrix);
                string stringified = JsonSerializer.Serialize(matrix);
                if (cache.ContainsKey(stringified))
                {
                    long start = cache[stringified];
                    long end = i - 1;
                    long indexToGo = (1_000_000_000 - start) % (end - start + 1);

                    for (int j = 0; j < indexToGo; j++)
                    {
                        TiltUpwards(matrix);
                        TiltLeftwards(matrix);
                        TiltDownwards(matrix);
                        TiltRightwards(matrix);
                    }
                    return CalculateLoad(matrix);
                }
                cache.Add(stringified, i);
            }

            return -1;
        }
    }
}

using System.Data;
using System.Text.Json;

namespace aoc2023.Solutions
{
    public class Day11
    {
        public static int Part1(string[] input)
        {
            List<List<char>> matrix = Array
                .ConvertAll(input, (row) => row.ToCharArray().ToList())
                .ToList();

            for (int row = 0; row < matrix.Count; row++)
            {
                if (matrix[row].Where(c => c != '.').ToList().Count == 0)
                {
                    matrix.Insert(row, new List<char>(matrix[row]));
                    row++;
                }
            }

            for (int col = 0; col < matrix[0].Count; col++)
            {
                bool isEmpty = true;
                for (int row = 0; row < matrix.Count(); row++)
                {
                    if (matrix[row][col] != '.')
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    for (int i = 0; i < matrix.Count(); i++)
                    {
                        matrix[i].Insert(col, '.');
                    }
                    col++;
                }
            }

            Queue<int[]> coords = new Queue<int[]>();

            for (int row = 0; row < matrix.Count; row++)
            {
                for (int col = 0; col < matrix[row].Count; col++)
                {
                    if (matrix[row][col] == '#')
                    {
                        coords.Enqueue(new int[] { row, col });
                    }
                }
            }

            int total = 0;
            while (coords.Count > 1)
            {
                int[] currentCoords = coords.Dequeue();

                foreach (int[] coord in coords)
                {
                    int distance =
                        Math.Abs(currentCoords[0] - coord[0])
                        + Math.Abs(currentCoords[1] - coord[1]);
                    total += distance;
                }
            }

            return total;
        }

        public static long Part2(string[] input)
        {
            HashSet<int> emptyRows = new HashSet<int>();
            HashSet<int> emptyCols = new HashSet<int>();

            for (int row = 0; row < input.Length; row++)
            {
                if (input[row].ToCharArray().Where(c => c == '#').Count() == 0)
                {
                    emptyRows.Add(row);
                }
            }

            for (int col = 0; col < input[0].Length; col++)
            {
                bool isEmpty = true;
                for (int row = 0; row < input.Length; row++)
                {
                    if (input[row][col] == '#')
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    emptyCols.Add(col);
                }
            }

            Queue<int[]> coords = new Queue<int[]>();

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == '#')
                    {
                        coords.Enqueue(new int[] { row, col });
                    }
                }
            }

            long total = 0;

            while (coords.Count > 1)
            {
                int[] currentCoords = coords.Dequeue();
                foreach (int[] coord in coords)
                {
                    int minRow = Math.Min(coord[0], currentCoords[0]);
                    int maxRow = Math.Max(coord[0], currentCoords[0]);
                    for (int i = minRow; i < maxRow; i++)
                    {
                        total += emptyRows.Contains(i) ? 1_000_000 : 1;
                    }

                    int minCol = Math.Min(coord[1], currentCoords[1]);
                    int maxCol = Math.Max(coord[1], currentCoords[1]);
                    for (int i = minCol + 1; i <= maxCol; i++)
                    {
                        total += emptyCols.Contains(i) ? 1_000_000 : 1;
                    }
                }
            }

            return total;
        }
    }
}

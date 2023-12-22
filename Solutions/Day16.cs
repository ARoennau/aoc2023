using System.Text.Json;

namespace aoc2023.Solutions
{
    public static class Day16
    {
        struct NodeValues
        {
            public long row { get; set; }
            public long col { get; set; }
            public string direction { get; set; }
        }

        private static int MarkPath(string[][] matrix, int row, int col, string direction)
        {
            Dictionary<string, HashSet<string>> visited = new Dictionary<string, HashSet<string>>();

            Queue<NodeValues> queue = new Queue<NodeValues>();
            queue.Enqueue(
                new NodeValues
                {
                    row = row,
                    col = col,
                    direction = direction
                }
            );
            while (queue.Count > 0)
            {
                NodeValues current = queue.Dequeue();

                if (
                    current.row < 0
                    || current.row >= matrix.Length
                    || current.col < 0
                    || current.col >= matrix[0].Length
                )
                    continue;

                string key = JsonSerializer.Serialize(new long[] { current.row, current.col });
                if (visited.ContainsKey(key) && visited[key].Contains(current.direction))
                    continue;

                if (!visited.ContainsKey(key))
                {
                    visited.Add(key, new HashSet<string>());
                }

                visited[key].Add(current.direction);

                switch (matrix[current.row][current.col])
                {
                    case "-":
                        if (current.direction == "right" || current.direction == "left")
                        {
                            int nextCol = current.direction == "right" ? 1 : -1;
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col + nextCol,
                                    row = current.row,
                                    direction = current.direction
                                }
                            );
                        }
                        else
                        {
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col + 1,
                                    row = current.row,
                                    direction = "right"
                                }
                            );
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col - 1,
                                    row = current.row,
                                    direction = "left"
                                }
                            );
                            continue;
                        }
                        break;
                    case "|":
                        if (current.direction == "up" || current.direction == "down")
                        {
                            int nextRow = current.direction == "down" ? 1 : -1;
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col,
                                    row = current.row + nextRow,
                                    direction = current.direction
                                }
                            );
                        }
                        else
                        {
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col,
                                    row = current.row + 1,
                                    direction = "down"
                                }
                            );
                            queue.Enqueue(
                                new NodeValues
                                {
                                    col = current.col,
                                    row = current.row - 1,
                                    direction = "up"
                                }
                            );
                            continue;
                        }
                        break;
                    case "/":
                        NodeValues reflected = new NodeValues
                        {
                            col = current.col,
                            row = current.row,
                            direction = current.direction
                        };
                        switch (current.direction)
                        {
                            case "up":
                                reflected.col++;
                                reflected.direction = "right";
                                break;
                            case "down":
                                reflected.col--;
                                reflected.direction = "left";
                                break;
                            case "left":
                                reflected.row++;
                                reflected.direction = "down";
                                break;
                            case "right":
                                reflected.row--;
                                reflected.direction = "up";
                                break;
                        }
                        queue.Enqueue(reflected);
                        break;
                    case "\\":
                        NodeValues newValues = new NodeValues
                        {
                            col = current.col,
                            row = current.row,
                            direction = current.direction
                        };
                        switch (current.direction)
                        {
                            case "down":
                                newValues.col++;
                                newValues.direction = "right";
                                break;
                            case "up":
                                newValues.col--;
                                newValues.direction = "left";
                                break;
                            case "right":
                                newValues.row++;
                                newValues.direction = "down";
                                break;
                            case "left":
                                newValues.row--;
                                newValues.direction = "up";
                                break;
                        }
                        queue.Enqueue(newValues);
                        break;
                    default:
                        NodeValues values = new NodeValues
                        {
                            col = current.col,
                            row = current.row,
                            direction = current.direction
                        };
                        switch (current.direction)
                        {
                            case "right":
                                values.col++;
                                break;
                            case "left":
                                values.col--;
                                break;
                            case "up":
                                values.row--;
                                break;
                            case "down":
                                values.row++;
                                break;
                        }
                        queue.Enqueue(values);
                        break;
                }
            }
            return visited.Count;
        }

        public static int Part1(string[] input)
        {
            string[][] matrix = new string[input.Length][];

            for (int i = 0; i < input.Length; i++)
            {
                matrix[i] = Array.ConvertAll(input[i].ToCharArray(), (c) => c.ToString());
            }

            return MarkPath(matrix, 0, 0, "right");
        }

        public static int Part2(string[] input)
        {
            string[][] matrix = new string[input.Length][];

            for (int i = 0; i < input.Length; i++)
            {
                matrix[i] = Array.ConvertAll(input[i].ToCharArray(), (c) => c.ToString());
            }

            int max = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                int fromLeft = MarkPath(matrix, i, 0, "right");
                max = Math.Max(max, fromLeft);
                int fromRight = MarkPath(matrix, i, matrix[i].Length - 1, "left");
                max = Math.Max(max, fromRight);
            }

            for (int i = 0; i < matrix[0].Length; i++)
            {
                int fromTop = MarkPath(matrix, 0, i, "down");
                max = Math.Max(max, fromTop);
                int fromBottom = MarkPath(matrix, matrix.Length - 1, i, "up");
                max = Math.Max(max, fromBottom);
            }

            return max;
        }
    }
}

using System.Text.Json;

namespace aoc2023.Solutions
{
    struct NodeData
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Direction { get; set; }
        public int NumberInDirection { get; set; }
        public int Total { get; set; }

        public NodeData(int row, int col, string direction, int numberInDirection, int total)
        {
            Row = row;
            Col = col;
            Direction = direction;
            NumberInDirection = numberInDirection;
            Total = total;
        }
    }

    public static class Day17
    {
        public static long Part1(string[] input)
        {
            int[][] matrix = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                matrix[i] = Array.ConvertAll(input[i].ToCharArray(), (c) => c - '0');
            }
            PriorityQueue<NodeData, int> priorityQueue = new PriorityQueue<NodeData, int>();
            priorityQueue.Enqueue(new NodeData(0, 1, "right", 1, matrix[0][1]), matrix[0][1]);
            priorityQueue.Enqueue(new NodeData(1, 0, "down", 1, matrix[1][0]), matrix[1][0]);
            HashSet<string> visited = new HashSet<string>();
            while (priorityQueue.Count > 0)
            {
                NodeData current = priorityQueue.Dequeue();
                if (current.Row == matrix.Length - 1 && current.Col == matrix[0].Length - 1)
                {
                    return current.Total;
                }

                string key = JsonSerializer.Serialize(
                    new
                    {
                        Row = current.Row,
                        Col = current.Col,
                        Direction = current.Direction,
                        NumInDir = current.NumberInDirection,
                    }
                );
                if (visited.Contains(key))
                    continue;

                visited.Add(key);

                if (current.Direction == "right" || current.Direction == "left")
                {
                    if (current.Row - 1 >= 0)
                    {
                        int total = current.Total + matrix[current.Row - 1][current.Col];
                        priorityQueue.Enqueue(
                            new NodeData(current.Row - 1, current.Col, "up", 1, total),
                            total
                        );
                    }

                    if (current.Row + 1 < matrix.Length)
                    {
                        int total = current.Total + matrix[current.Row + 1][current.Col];
                        priorityQueue.Enqueue(
                            new NodeData(current.Row + 1, current.Col, "down", 1, total),
                            total
                        );
                    }
                }

                if (current.Direction == "up" || current.Direction == "down")
                {
                    if (current.Col - 1 >= 0)
                    {
                        int total = current.Total + matrix[current.Row][current.Col - 1];
                        priorityQueue.Enqueue(
                            new NodeData(current.Row, current.Col - 1, "left", 1, total),
                            total
                        );
                    }

                    if (current.Col + 1 < matrix[0].Length)
                    {
                        int total = current.Total + matrix[current.Row][current.Col + 1];
                        priorityQueue.Enqueue(
                            new NodeData(current.Row, current.Col + 1, "right", 1, total),
                            total
                        );
                    }
                }

                if (current.NumberInDirection < 3)
                {
                    int newY =
                        current.Row
                        + (
                            current.Direction == "down"
                                ? 1
                                : current.Direction == "up"
                                    ? -1
                                    : 0
                        );
                    int newX =
                        current.Col
                        + (
                            current.Direction == "right"
                                ? 1
                                : current.Direction == "left"
                                    ? -1
                                    : 0
                        );

                    if (newX < 0 || newX >= matrix[0].Length || newY < 0 || newY >= matrix.Length)
                        continue;

                    int total = current.Total + matrix[newY][newX];
                    priorityQueue.Enqueue(
                        new NodeData(
                            newY,
                            newX,
                            current.Direction,
                            current.NumberInDirection + 1,
                            total
                        ),
                        total
                    );
                }
            }
            return 0;
        }

        public static long Part2(string[] input)
        {
            // Off by 1 on real input
            int[][] matrix = new int[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                matrix[i] = Array.ConvertAll(input[i].ToCharArray(), (c) => c - '0');
            }
            PriorityQueue<NodeData, int> priorityQueue = new PriorityQueue<NodeData, int>();
            priorityQueue.Enqueue(new NodeData(0, 1, "right", 1, matrix[0][1]), matrix[0][1]);
            priorityQueue.Enqueue(new NodeData(1, 0, "down", 1, matrix[1][0]), matrix[1][0]);

            HashSet<string> visited = new HashSet<string>();
            while (priorityQueue.Count > 0)
            {
                NodeData current = priorityQueue.Dequeue();
                if (
                    current.Row == matrix.Length - 1
                    && current.Col == matrix[0].Length - 1
                    && current.NumberInDirection >= 4
                )
                {
                    return current.Total;
                }

                string key = JsonSerializer.Serialize(
                    new
                    {
                        Row = current.Row,
                        Col = current.Col,
                        Direction = current.Direction,
                        NumInDir = current.NumberInDirection,
                    }
                );
                if (visited.Contains(key))
                    continue;

                visited.Add(key);

                if (current.NumberInDirection <= 10)
                {
                    int newY =
                        current.Row
                        + (
                            current.Direction == "down"
                                ? 1
                                : current.Direction == "up"
                                    ? -1
                                    : 0
                        );
                    int newX =
                        current.Col
                        + (
                            current.Direction == "right"
                                ? 1
                                : current.Direction == "left"
                                    ? -1
                                    : 0
                        );

                    if (newX >= 0 && newX < matrix[0].Length && newY >= 0 && newY < matrix.Length)
                    {
                        int total = current.Total + matrix[newY][newX];
                        priorityQueue.Enqueue(
                            new NodeData(
                                newY,
                                newX,
                                current.Direction,
                                current.NumberInDirection + 1,
                                total
                            ),
                            total
                        );
                    }
                }

                if (current.NumberInDirection >= 4 && current.NumberInDirection <= 10)
                {
                    if (current.Direction == "right" || current.Direction == "left")
                    {
                        if (current.Row - 1 >= 0)
                        {
                            int total = current.Total + matrix[current.Row - 1][current.Col];
                            priorityQueue.Enqueue(
                                new NodeData(current.Row - 1, current.Col, "up", 1, total),
                                total
                            );
                        }

                        if (current.Row + 1 < matrix.Length)
                        {
                            int total = current.Total + matrix[current.Row + 1][current.Col];
                            priorityQueue.Enqueue(
                                new NodeData(current.Row + 1, current.Col, "down", 1, total),
                                total
                            );
                        }
                    }

                    if (current.Direction == "up" || current.Direction == "down")
                    {
                        if (current.Col - 1 >= 0)
                        {
                            int total = current.Total + matrix[current.Row][current.Col - 1];
                            priorityQueue.Enqueue(
                                new NodeData(current.Row, current.Col - 1, "left", 1, total),
                                total
                            );
                        }

                        if (current.Col + 1 < matrix[0].Length)
                        {
                            int total = current.Total + matrix[current.Row][current.Col + 1];
                            priorityQueue.Enqueue(
                                new NodeData(current.Row, current.Col + 1, "right", 1, total),
                                total
                            );
                        }
                    }
                }
            }
            return 0;
        }
    }
}

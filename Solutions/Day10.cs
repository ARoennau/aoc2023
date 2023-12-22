using System.Text.Json;

namespace aoc2023.Solutions
{
    public class Day10
    {
        private static HashSet<char> validUps = new HashSet<char> { '|', 'F', '7' };
        private static HashSet<char> validDowns = new HashSet<char> { '|', 'J', 'L' };
        private static HashSet<char> validLefts = new HashSet<char> { '-', 'L', 'F' };
        private static HashSet<char> validRights = new HashSet<char> { '-', 'J', '7' };
        private static HashSet<char> verticalPipes = new HashSet<char> { '|', 'J', 'L', 'F', '7' };

        private static int[] GetLocationOfS(char[][] matrix)
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[0].Length; col++)
                {
                    if (matrix[row][col] == 'S')
                        return new int[] { row, col };
                }
            }
            throw new Exception("No S found");
        }

        private static int FollowPipe(
            int[] coords,
            char[][] matrix,
            string direction,
            HashSet<string>? pipeCoords
        )
        {
            int[] currentCoords = coords;
            string currentDirection = direction;
            int count = 0;

            while (count <= Int32.MaxValue)
            {
                char currentValue = matrix[currentCoords[0]][currentCoords[1]];
                if (pipeCoords != null)
                {
                    pipeCoords.Add(JsonSerializer.Serialize(currentCoords));
                }
                switch (currentValue)
                {
                    case 'S':
                        return count + 1;
                    case '-':
                        if (currentDirection == "left")
                        {
                            if (currentCoords[1] == 0)
                                return -1;

                            currentCoords = new int[] { currentCoords[0], currentCoords[1] - 1 };
                            count++;
                            currentDirection = "left";
                            break;
                        }
                        if (currentDirection == "right")
                        {
                            if (currentCoords[1] == matrix[0].Length - 1)
                                return -1;

                            currentCoords = new int[] { currentCoords[0], currentCoords[1] + 1 };
                            count++;
                            currentDirection = "right";
                            break;
                        }
                        return -1;
                    case '|':
                        if (currentDirection == "up")
                        {
                            if (currentCoords[0] == 0)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] - 1, currentCoords[1] };
                            count++;
                            currentDirection = "up";
                            break;
                        }
                        if (currentDirection == "down")
                        {
                            if (currentCoords[0] == matrix.Length - 1)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] + 1, currentCoords[1] };
                            count++;
                            currentDirection = "down";
                            break;
                        }
                        return -1;
                    case 'L':
                        if (currentDirection == "down")
                        {
                            if (currentCoords[1] == matrix[0].Length - 1)
                                return -1;
                            currentCoords = new int[] { currentCoords[0], currentCoords[1] + 1 };
                            count++;
                            currentDirection = "right";
                            break;
                        }
                        if (currentDirection == "left")
                        {
                            if (currentCoords[0] == 0)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] - 1, currentCoords[1] };
                            count++;
                            currentDirection = "up";
                            break;
                        }
                        return -1;
                    case 'J':
                        if (currentDirection == "down")
                        {
                            if (currentCoords[1] == 0)
                                return -1;
                            currentCoords = new int[] { currentCoords[0], currentCoords[1] - 1 };
                            count++;
                            currentDirection = "left";
                            break;
                        }
                        if (currentDirection == "right")
                        {
                            if (currentCoords[0] == 0)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] - 1, currentCoords[1] };
                            count++;
                            currentDirection = "up";
                            break;
                        }
                        return -1;
                    case '7':
                        if (currentDirection == "up")
                        {
                            if (currentCoords[1] == 0)
                                return -1;
                            currentCoords = new int[] { currentCoords[0], currentCoords[1] - 1 };
                            count++;
                            currentDirection = "left";
                            break;
                        }
                        if (currentDirection == "right")
                        {
                            if (currentCoords[0] == matrix.Length - 1)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] + 1, currentCoords[1] };
                            count++;
                            currentDirection = "down";
                            break;
                        }
                        return -1;
                    case 'F':
                        if (currentDirection == "up")
                        {
                            if (currentCoords[1] == matrix[0].Length - 1)
                                return -1;
                            currentCoords = new int[] { currentCoords[0], currentCoords[1] + 1 };
                            count++;
                            currentDirection = "right";
                            break;
                        }
                        if (currentDirection == "left")
                        {
                            if (currentCoords[0] == matrix.Length - 1)
                                return -1;
                            currentCoords = new int[] { currentCoords[0] + 1, currentCoords[1] };
                            count++;
                            currentDirection = "down";
                            break;
                        }
                        return -1;
                    default:
                        return -1;
                }
            }
            return count;
        }

        public static int Part1(string[] input)
        {
            char[][] matrix = Array.ConvertAll(input, (row) => row.ToCharArray());

            int[] sCoords = GetLocationOfS(matrix);

            if (sCoords[0] > 0)
            {
                int count = FollowPipe(
                    new int[] { sCoords[0] - 1, sCoords[1] },
                    matrix,
                    "up",
                    null
                );
                if (count >= 0)
                    return count / 2;
            }

            if (sCoords[1] > 0)
            {
                int count = FollowPipe(
                    new int[] { sCoords[0], sCoords[1] - 1 },
                    matrix,
                    "left",
                    null
                );
                if (count >= 0)
                    return count / 2;
            }

            if (sCoords[0] < matrix.Length - 1)
            {
                int count = FollowPipe(
                    new int[] { sCoords[0] + 1, sCoords[1] },
                    matrix,
                    "down",
                    null
                );
                if (count >= 0)
                    return count / 2;
            }

            if (sCoords[1] < matrix[0].Length - 1)
            {
                int count = FollowPipe(
                    new int[] { sCoords[0], sCoords[1] + 1 },
                    matrix,
                    "down",
                    null
                );
                if (count >= 0)
                    return count / 2;
            }

            return -100;
        }

        private static char GetSPipe(char[][] matrix, int row, int col)
        {
            bool isValidUp = row > 0 && validUps.Contains(matrix[row - 1][col]);
            bool isValidDown = row < matrix.Length - 1 && validDowns.Contains(matrix[row + 1][col]);
            bool isValidLeft = col > 0 && validLefts.Contains(matrix[row][col - 1]);
            bool isValidRight =
                col < matrix[row].Length - 1 && validRights.Contains(matrix[row][col + 1]);

            if (isValidUp && isValidDown)
                return '|';
            if (isValidUp && isValidLeft)
                return 'J';
            if (isValidUp && isValidRight)
                return 'L';
            if (isValidLeft && isValidRight)
                return '-';
            if (isValidDown && isValidLeft)
                return '7';
            if (isValidDown && isValidRight)
                return 'F';
            throw new Exception("Invalid character");
        }

        public static bool IsCrossing(char current, char previous)
        {
            return current == '|'
                || (previous == 'F' && current == 'J')
                || (previous == 'L' && current == '7');
        }

        public static int Part2(string[] input)
        {
            char[][] matrix = Array.ConvertAll(input, (row) => row.ToCharArray());

            int[] sCoords = GetLocationOfS(matrix);
            HashSet<string> pipeCoords = new HashSet<string>();

            if (sCoords[0] > 0 && validUps.Contains(matrix[sCoords[0] - 1][sCoords[1]]))
            {
                FollowPipe(new int[] { sCoords[0] - 1, sCoords[1] }, matrix, "up", pipeCoords);
            }

            if (sCoords[1] > 0 && validLefts.Contains(matrix[sCoords[0]][sCoords[1] - 1]))
            {
                FollowPipe(new int[] { sCoords[0], sCoords[1] - 1 }, matrix, "left", pipeCoords);
            }

            if (
                sCoords[0] < matrix.Length - 1
                && validDowns.Contains(matrix[sCoords[0] + 1][sCoords[1]])
            )
            {
                FollowPipe(new int[] { sCoords[0] + 1, sCoords[1] }, matrix, "down", pipeCoords);
            }
            char sChar = GetSPipe(matrix, sCoords[0], sCoords[1]);
            matrix[sCoords[0]][sCoords[1]] = sChar;

            int count = 0;
            for (int row = 0; row < matrix.Length; row++)
            {
                bool isInside = false;
                char previous = '|';
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    char current = matrix[row][col];
                    if (
                        verticalPipes.Contains(current)
                        && pipeCoords.Contains(JsonSerializer.Serialize(new int[] { row, col }))
                    )
                    {
                        if (IsCrossing(current, previous))
                        {
                            isInside = !isInside;
                        }
                        previous = current;
                        continue;
                    }

                    if (
                        isInside
                        && !pipeCoords.Contains(JsonSerializer.Serialize(new int[] { row, col }))
                    )
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}

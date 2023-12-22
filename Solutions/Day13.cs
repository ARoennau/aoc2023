namespace aoc2023.Solutions
{
    public class Day13
    {
        private static string ColToString(List<string> chunk, int col)
        {
            string result = "";
            foreach (string row in chunk)
            {
                result += row[col];
            }
            return result;
        }

        private static int CompareRowStrings(string a, string b)
        {
            int differences = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    differences++;
            }
            return differences;
        }

        private static int CompareCols(List<string> chunk, int colA, int colB)
        {
            int differences = 0;
            foreach (string row in chunk)
            {
                if (row[colA] != row[colB])
                    differences++;
            }
            return differences;
        }

        public static long Part2(string[] input)
        {
            long total = 0;
            List<string> current = new List<string>();
            for (int rowIndex = 0; rowIndex < input.Length; rowIndex++)
            {
                string row = input[rowIndex];
                if (row != "")
                {
                    current.Add(row);
                    if (rowIndex != input.Length - 1)
                        continue;
                }

                bool isReflection = true;

                for (int i = 0; i < current[0].Length - 1; i++)
                {
                    isReflection = true;
                    int differences = CompareCols(current, i, i + 1);
                    if (differences <= 1)
                    {
                        int j = 1;
                        while (i - j >= 0 && i + j + 1 < current[0].Length)
                        {
                            differences += CompareCols(current, i - j, i + j + 1);
                            if (differences > 1)
                            {
                                isReflection = false;
                                break;
                            }
                            j++;
                        }

                        if (isReflection && differences == 1)
                        {
                            total += i + 1;
                            break;
                        }
                    }
                    isReflection = false;
                }

                if (!isReflection)
                {
                    for (int i = 0; i < current.Count - 1; i++)
                    {
                        int differences = CompareRowStrings(current[i], current[i + 1]);
                        if (differences <= 1)
                        {
                            isReflection = true;
                            int j = 1;
                            while (i - j >= 0 && i + j + 1 < current.Count)
                            {
                                differences += CompareRowStrings(
                                    current[i - j],
                                    current[i + j + 1]
                                );
                                if (differences > 1)
                                {
                                    isReflection = false;
                                    break;
                                }
                                j++;
                            }

                            if (isReflection && differences == 1)
                            {
                                total += (i + 1) * 100;
                                break;
                            }
                        }
                    }
                }
                current = new List<string>();
            }
            return total;
        }

        public static long Part1(string[] input)
        {
            long total = 0;
            List<string> current = new List<string>();
            for (int rowIndex = 0; rowIndex < input.Length; rowIndex++)
            {
                string row = input[rowIndex];
                if (row != "")
                {
                    current.Add(row);
                    if (rowIndex != input.Length - 1)
                        continue;
                }

                bool isReflection = true;

                for (int i = 0; i < current[0].Length - 1; i++)
                {
                    isReflection = true;
                    if (ColToString(current, i) == ColToString(current, i + 1))
                    {
                        int j = 1;
                        while (i - j >= 0 && i + j + 1 < current[0].Length)
                        {
                            string left = ColToString(current, i - j);
                            string right = ColToString(current, i + j + 1);
                            if (ColToString(current, i - j) != ColToString(current, i + j + 1))
                            {
                                isReflection = false;
                                break;
                            }
                            j++;
                        }

                        if (isReflection)
                        {
                            total += i + 1;
                            break;
                        }
                    }
                    isReflection = false;
                }

                if (!isReflection)
                {
                    for (int i = 0; i < current.Count - 1; i++)
                    {
                        if (current[i] == current[i + 1])
                        {
                            isReflection = true;
                            int j = 1;
                            while (i - j >= 0 && i + j + 1 < current.Count)
                            {
                                if (current[i - j] != current[i + j + 1])
                                {
                                    isReflection = false;
                                    break;
                                }
                                j++;
                            }

                            if (isReflection)
                            {
                                total += (i + 1) * 100;
                                break;
                            }
                        }
                    }
                }
                current = new List<string>();
            }
            return total;
        }
    }
}

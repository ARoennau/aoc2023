namespace aoc2023.Solutions
{
    public static class Day2
    {
        public static int PossibleGames(string[] input)
        {
            Dictionary<string, int> maximums = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };
            int total = 0;

            foreach (string line in input)
            {
                string[] initialSplit = line.Split(": ");
                string[] leftSplit = initialSplit[0].Split(" ");
                int id = Int32.Parse(leftSplit[1]);

                string[] cubeSets = initialSplit[1].Split("; ");
                bool isValid = true;
                int i = 0;
                while (i < cubeSets.Length && isValid)
                {
                    string cubeSet = cubeSets[i];
                    string[] colorCounts = cubeSet.Split(", ");
                    foreach (var colorCount in colorCounts)
                    {
                        string[] colorValue = colorCount.Split(" ");
                        if (
                            !maximums.ContainsKey(colorValue[1])
                            || Int32.Parse(colorValue[0]) > maximums[colorValue[1]]
                        )
                        {
                            isValid = false;
                            break;
                        }
                    }
                    i++;
                }

                if (isValid)
                    total += id;
            }

            return total;
        }

        public static int CubeMinimums(string[] input)
        {
            int total = 0;

            foreach (string line in input)
            {
                string[] initialSplit = line.Split(": ");

                string[] cubeSets = initialSplit[1].Split("; ");

                Dictionary<string, int> maximums = new Dictionary<string, int>()
                {
                    { "blue", 0 },
                    { "green", 0 },
                    { "red", 0 }
                };

                foreach (string cubeSet in cubeSets)
                {
                    string[] colorCounts = cubeSet.Split(", ");
                    foreach (var colorCount in colorCounts)
                    {
                        string[] colorValue = colorCount.Split(" ");
                        maximums[colorValue[1]] = Math.Max(
                            maximums[colorValue[1]],
                            Int32.Parse(colorValue[0])
                        );
                    }
                }

                int power = 1;
                foreach (int value in maximums.Values)
                {
                    power *= value;
                }
                total += power;
            }

            return total;
        }
    }
}

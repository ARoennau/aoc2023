namespace aoc2023.Solutions
{
    public class Day6
    {
        public static int Part1(string[] input)
        {
            int[] times = Array.ConvertAll(
                input[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries),
                Int32.Parse
            );
            int[] distances = Array.ConvertAll(
                input[1].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries),
                Int32.Parse
            );

            int product = 1;

            for (int i = 0; i < times.Length; i++)
            {
                int time = times[i];
                int goal = distances[i];
                int midPoint = time / 2;
                if ((time - midPoint) * midPoint <= goal)
                {
                    product = 0;
                    continue;
                }

                int count = 0;
                while (count <= midPoint)
                {
                    int offset = midPoint - count;
                    if ((time - offset) * offset <= goal)
                        break;
                    count++;
                }
                int result = count * 2;
                if (Int32.IsEvenInteger(time))
                {
                    result -= 1;
                }

                product *= result;
            }

            return product;
        }

        public static long Part2(string[] input)
        {
            string[] times = input[0].Split(":")[1].Split(
                " ",
                StringSplitOptions.RemoveEmptyEntries
            );
            string[] distances = input[1].Split(":")[1].Split(
                " ",
                StringSplitOptions.RemoveEmptyEntries
            );

            long time = Int64.Parse(string.Join("", times));
            long goal = Int64.Parse(string.Join("", distances));

            long midPoint = time / 2;
            if ((time - midPoint) * midPoint <= goal)
            {
                return 0;
            }

            long count = 0;
            while (count <= midPoint)
            {
                long offset = midPoint - count;
                if ((time - offset) * offset <= goal)
                    break;
                count++;
            }
            long result = count * 2;
            if (Int64.IsEvenInteger(time))
            {
                result -= 1;
            }

            return result;
        }
    }
}

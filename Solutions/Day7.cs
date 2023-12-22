namespace aoc2023.Solutions
{
    public static class Day7
    {
        public static Dictionary<char, int> CardToValueMap = new Dictionary<char, int>()
        {
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'J', 11 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 },
        };

        public static Dictionary<char, int> CardToValueMap2 = new Dictionary<char, int>()
        {
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'J', 1 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 },
        };

        public static int CalculateHandValue1(string hand)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            foreach (char card in hand)
            {
                if (!counts.ContainsKey(card))
                {
                    counts[card] = 1;
                    continue;
                }
                counts[card]++;
            }

            List<int> valueList = counts.Values.ToList();
            valueList.Sort();

            if (valueList.Count == 1)
                return 6;
            if (valueList.Count == 2)
            {
                return valueList[0] == 1 ? 5 : 4;
            }
            if (valueList.Count == 3)
            {
                return valueList[^1] == 3 ? 3 : 2;
            }
            if (valueList.Count == 4)
                return 1;
            return 0;
        }

        public static int CalculateHandValue2(string hand)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            foreach (char card in hand)
            {
                if (!counts.ContainsKey(card))
                {
                    counts[card] = 1;
                    continue;
                }
                counts[card]++;
            }

            List<int> valueList = counts.Values.ToList();
            valueList.Sort();

            if (counts.ContainsKey('J'))
            {
                int jokerCount = counts['J'];
                if (jokerCount == 0)
                    throw new Exception("Error");

                if (jokerCount >= 4)
                    return 6;
                if (jokerCount == 3)
                    return valueList.Count == 2 ? 6 : 5;
                if (jokerCount == 2)
                {
                    if (valueList[^1] == 3)
                        return 6;
                    if (valueList[^1] == 2 && valueList[^2] == 2)
                        return 5;
                    return 3;
                }
                if (valueList[^1] == 4)
                    return 6;
                if (valueList[^1] == 3)
                    return 5;
                if (valueList[^1] == 2)
                    return valueList[^2] == 2 ? 4 : 3;
                return 1;
            }

            if (valueList.Count == 1)
                return 6;
            if (valueList.Count == 2)
            {
                return valueList[0] == 1 ? 5 : 4;
            }
            if (valueList.Count == 3)
            {
                return valueList[^1] == 3 ? 3 : 2;
            }
            if (valueList.Count == 4)
                return 1;
            return 0;
        }

        public static int ComparePart1(string[] x, string[] y)
        {
            string hand1 = x[0];
            string hand2 = y[0];
            int hand1Value = CalculateHandValue1(hand1);
            int hand2Value = CalculateHandValue1(hand2);

            if (hand1Value == hand2Value)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (hand1[i] == hand2[i])
                        continue;
                    if (CardToValueMap[hand1[i]] > CardToValueMap[hand2[i]])
                        return 1;
                    return -1;
                }
                return 0;
            }
            else if (hand1Value > hand2Value)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public static int Part1(string[] input)
        {
            string[][] hands = Array.ConvertAll(
                input,
                (row) => row.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            );
            Array.Sort(hands, ComparePart1);

            int total = 0;

            int rank = 1;
            foreach (string[] hand in hands)
            {
                total += Int32.Parse(hand[1]) * rank;
                rank++;
            }

            return total;
        }

        public static int ComparePart2(string[] x, string[] y)
        {
            string hand1 = x[0];
            string hand2 = y[0];
            int hand1Value = CalculateHandValue2(hand1);
            int hand2Value = CalculateHandValue2(hand2);

            if (hand1Value == hand2Value)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (hand1[i] == hand2[i])
                        continue;
                    int returnValue =
                        CardToValueMap2[hand1[i]] > CardToValueMap2[hand2[i]] ? 1 : -1;
                    return returnValue;
                }
                return 0;
            }
            else if (hand1Value > hand2Value)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public static int Part2(string[] input)
        {
            string[][] hands = Array.ConvertAll(
                input,
                (row) => row.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            );
            Array.Sort(hands, ComparePart2);

            int total = 0;

            int rank = 1;
            foreach (string[] hand in hands)
            {
                total += Int32.Parse(hand[1]) * rank;
                rank++;
            }

            return total;
        }
    }
}

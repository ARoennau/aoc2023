namespace aoc2023.Solutions
{
    public static class Day15
    {
        public static long Part1(string input)
        {
            long total = 0;
            string[] sections = input.Split(",");

            foreach (string section in sections)
            {
                long value = 0;
                foreach (char c in section)
                {
                    value += (int)c;
                    value *= 17;
                    value %= 256;
                }
                total += value;
            }

            return total;
        }

        private static long Hash(string section)
        {
            long value = 0;
            foreach (char c in section)
            {
                value += (int)c;
                value *= 17;
                value %= 256;
            }
            return value;
        }

        struct Slot
        {
            public string Label { get; set; }
            public long FocalLength { get; set; }
        }

        private static void Remove(List<Slot> slots, string label)
        {
            foreach (Slot slot in slots)
            {
                if (slot.Label == label)
                {
                    slots.Remove(slot);
                    return;
                }
            }
        }

        private static bool ContainsLabel(List<Slot> slots, string label)
        {
            foreach (Slot slot in slots)
            {
                if (slot.Label == label)
                {
                    return true;
                }
            }
            return false;
        }

        private static void UpdateLength(List<Slot> slots, string label, long focalLength)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.Label == label)
                {
                    slots.Remove(slot);
                    slots.Insert(i, new Slot { Label = label, FocalLength = focalLength });
                }
            }
        }

        public static long Part2(string input)
        {
            Dictionary<long, List<Slot>> boxes = new Dictionary<long, List<Slot>>();

            string[] sections = input.Split(",");
            foreach (string section in sections)
            {
                if (section[^1] == '-')
                {
                    string[] split = section.Split('-');
                    long hash = Hash(split[0]);
                    if (!boxes.ContainsKey(hash))
                        continue;
                    Remove(boxes[hash], split[0]);
                    if (boxes[hash].Count == 0)
                    {
                        boxes.Remove(hash);
                    }
                }
                else
                {
                    string[] split = section.Split('=');
                    long hash = Hash(split[0]);
                    if (!boxes.ContainsKey(hash))
                    {
                        boxes.Add(hash, new List<Slot>());
                    }

                    if (!ContainsLabel(boxes[hash], split[0]))
                    {
                        boxes[hash].Add(
                            new Slot { Label = split[0], FocalLength = Int64.Parse(split[1]) }
                        );
                    }
                    else
                    {
                        UpdateLength(boxes[hash], split[0], Int64.Parse(split[1]));
                    }
                }
            }
            long total = 0;
            foreach (long box in boxes.Keys)
            {
                for (int i = 0; i < boxes[box].Count; i++)
                {
                    long current = (box + 1) * (i + 1) * boxes[box][i].FocalLength;
                    total += current;
                }
            }

            return total;
        }
    }
}

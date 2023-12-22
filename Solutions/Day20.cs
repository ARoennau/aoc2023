namespace aoc2023.Solutions
{
    enum PulseType
    {
        high,
        low,
    };

    struct Pulse
    {
        public string Start { get; set; }
        public string End { get; set; }
        public PulseType Type { get; set; }
    }

    interface Module
    {
        public string[] Outputs { get; set; }
        public string Name { get; set; }
        public List<Pulse> Process(Pulse pulse);
    }

    struct Conjunction : Module
    {
        public string[] Outputs { get; set; }
        public Dictionary<string, PulseType> Inputs { get; set; }
        public string Name { get; set; }

        public List<Pulse> Process(Pulse pulse)
        {
            List<Pulse> emits = new List<Pulse>();
            Inputs[pulse.Start] = pulse.Type;

            PulseType emitType =
                Inputs.Values.Where(t => t == PulseType.low).ToList().Count == 0
                    ? PulseType.low
                    : PulseType.high;

            foreach (string output in Outputs)
            {
                emits.Add(
                    new Pulse
                    {
                        Start = Name,
                        End = output,
                        Type = emitType
                    }
                );
            }
            return emits;
        }
    }

    struct FlipFlip : Module
    {
        public string[] Outputs { get; set; }
        public bool IsOn { get; set; }
        public string Name { get; set; }

        public List<Pulse> Process(Pulse pulse)
        {
            List<Pulse> emits = new List<Pulse>();
            if (pulse.Type == PulseType.high)
                return emits;

            IsOn = !IsOn;
            PulseType emitType = IsOn ? PulseType.high : PulseType.low;

            foreach (string output in Outputs)
            {
                emits.Add(
                    new Pulse
                    {
                        Start = Name,
                        End = output,
                        Type = emitType
                    }
                );
            }
            return emits;
        }
    }

    public static class Day20
    {
        private static Dictionary<string, Module> Parse(string[] input)
        {
            Dictionary<string, Module> modules = new Dictionary<string, Module>();

            foreach (string mod in input)
            {
                bool isFlipFlop = mod[0] == '%';
                string[] modSplit = mod.Split(" -> ");
                if (modSplit[0] == "broadcaster")
                    continue;
                string name = modSplit[0].Substring(1);
                string[] outputs = modSplit[1].Split(", ");
                if (isFlipFlop)
                {
                    modules.Add(
                        name,
                        new FlipFlip
                        {
                            Outputs = outputs,
                            IsOn = false,
                            Name = name
                        }
                    );
                }
                else
                {
                    modules.Add(
                        name,
                        new Conjunction
                        {
                            Outputs = outputs,
                            Inputs = new Dictionary<string, PulseType>(),
                            Name = name
                        }
                    );
                }
            }

            foreach (Module module in modules.Values)
            {
                foreach (string output in module.Outputs)
                {
                    if (output == "output")
                        continue;
                    if (modules.ContainsKey(output) && modules[output] is Conjunction)
                    {
                        Conjunction conjunction = (Conjunction)modules[output];
                        if (!conjunction.Inputs.ContainsKey(module.Name))
                        {
                            conjunction.Inputs.Add(module.Name, PulseType.low);
                        }
                    }
                }
            }
            return modules;
        }

        public static long Part1(string[] input)
        {
            Dictionary<string, Module> modules = Parse(input);
            string broadcaster = "";
            foreach (string row in input)
            {
                if (row.Split(" -> ")[0] == "broadcaster")
                {
                    broadcaster = row;
                    break;
                }
            }
            string[] starts = broadcaster.Split(" -> ")[1].Split(", ");
            long lowCount = 0;
            long highCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                lowCount++;
                Queue<Pulse> pulseQueue = new Queue<Pulse>();
                foreach (string start in starts)
                {
                    pulseQueue.Enqueue(
                        new Pulse
                        {
                            Start = "-",
                            End = start,
                            Type = PulseType.low
                        }
                    );
                }

                while (pulseQueue.Count > 0)
                {
                    Pulse current = pulseQueue.Dequeue();
                    if (current.Type == PulseType.high)
                    {
                        highCount++;
                    }
                    else
                    {
                        lowCount++;
                    }
                    if (current.End == "output")
                        continue;

                    if (!modules.ContainsKey(current.End))
                        continue;
                    List<Pulse> next = modules[current.End].Process(current);
                    foreach (Pulse pulse in next)
                    {
                        pulseQueue.Enqueue(pulse);
                    }
                }
            }
            return lowCount * highCount;
        }

        public static long Part2(string[] input)
        {
            Dictionary<string, Module> modules = Parse(input);
            string broadcaster = "";
            foreach (string row in input)
            {
                if (row.Split(" -> ")[0] == "broadcaster")
                {
                    broadcaster = row;
                    break;
                }
            }
            string[] starts = broadcaster.Split(" -> ")[1].Split(", ");
            long buttonPresses = 0;
            List<long> nearEnd = new List<long>();
            while (nearEnd.Count < 4)
            {
                buttonPresses++;
                Queue<Pulse> pulseQueue = new Queue<Pulse>();
                foreach (string start in starts)
                {
                    pulseQueue.Enqueue(
                        new Pulse
                        {
                            Start = "-",
                            End = start,
                            Type = PulseType.low
                        }
                    );
                }

                while (pulseQueue.Count > 0)
                {
                    Pulse current = pulseQueue.Dequeue();
                    if (current.End == "output")
                        continue;

                    if (
                        (
                            current.Start == "tx"
                            || current.Start == "dd"
                            || current.Start == "nz"
                            || current.Start == "ph"
                        )
                        && current.Type == PulseType.high
                    )
                    {
                        nearEnd.Add(buttonPresses);
                        Console.WriteLine($"found for {current.Start}");
                    }

                    if (!modules.ContainsKey(current.End))
                        continue;
                    List<Pulse> next = modules[current.End].Process(current);
                    foreach (Pulse pulse in next)
                    {
                        pulseQueue.Enqueue(pulse);
                    }
                }
            }
            long result = 1;
            foreach (long num in nearEnd)
            {
                result *= num;
            }
            return result;
        }
    }
}

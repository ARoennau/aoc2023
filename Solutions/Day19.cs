using System.Text.Json;

namespace aoc2023.Solutions
{
    struct Part
    {
        public long X { get; set; }
        public long M { get; set; }
        public long A { get; set; }
        public long S { get; set; }

        public Part(long x, long m, long a, long s)
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }
    }

    struct Ranges
    {
        public long[] XRange { get; set; }
        public long[] MRange { get; set; }
        public long[] ARange { get; set; }
        public long[] SRange { get; set; }

        public Ranges(long[] xRange, long[] mRange, long[] aRange, long[] sRange)
        {
            XRange = xRange;
            MRange = mRange;
            ARange = aRange;
            SRange = sRange;
        }
    }

    struct Rule
    {
        public string Letter { get; set; }
        public string ResultIfTrue { get; set; }
        public bool IsGreaterThan { get; set; }
        public long Value { get; set; }

        public Rule(string letter, string resultIfTrue, bool isGreaterThan, long value)
        {
            Letter = letter;
            ResultIfTrue = resultIfTrue;
            IsGreaterThan = isGreaterThan;
            Value = value;
        }
    }

    public static class Day19
    {
        private static Func<Part, string?> GenerateDestinationFunction(string rule)
        {
            string[] splitRule = rule.Split(":");
            if (splitRule.Length == 1)
            {
                return (p) => rule;
            }

            if (splitRule[0].Contains(">"))
            {
                string[] splitCondition = splitRule[0].Split(">");
                long number = Int64.Parse(splitCondition[1]);
                string property = splitCondition[0].ToUpper();
                return (p) =>
                {
                    long? value = (long?)p.GetType().GetProperty(property)?.GetValue(p);
                    if (value > number)
                        return splitRule[1];
                    return null;
                };
            }
            else
            {
                string[] splitCondition = splitRule[0].Split("<");
                long number = Int64.Parse(splitCondition[1]);
                string property = splitCondition[0].ToUpper();
                return (p) =>
                {
                    long? value = (long?)p.GetType().GetProperty(property)?.GetValue(p);
                    if (value < number)
                        return splitRule[1];
                    return null;
                };
            }
            throw new Exception("An error occurred");
        }

        private static Dictionary<string, List<Func<Part, string?>>> ParseWorkflows(
            List<string> workflowStrings
        )
        {
            Dictionary<string, List<Func<Part, string?>>> workflows =
                new Dictionary<string, List<Func<Part, string?>>>();

            foreach (string workflow in workflowStrings)
            {
                string[] initialSplit = workflow.Split("{");
                string name = initialSplit[0];
                string rulesString = initialSplit[1].Substring(0, initialSplit[1].Length - 1);
                string[] rulesSplit = rulesString.Split(",");
                List<Func<Part, string?>> rules = new List<Func<Part, string?>>();
                foreach (string rule in rulesSplit)
                {
                    rules.Add(GenerateDestinationFunction(rule));
                }
                workflows.Add(name, rules);
            }
            return workflows;
        }

        private static Dictionary<string, List<Rule>> ParseWorkflows2(List<string> workflowStrings)
        {
            Dictionary<string, List<Rule>> workflows = new Dictionary<string, List<Rule>>();

            foreach (string workflow in workflowStrings)
            {
                string[] initialSplit = workflow.Split("{");
                string name = initialSplit[0];
                string rulesString = initialSplit[1].Substring(0, initialSplit[1].Length - 1);
                string[] rulesSplit = rulesString.Split(",");
                List<Rule> rules = new List<Rule>();
                foreach (string rule in rulesSplit)
                {
                    string[] splitRule = rule.Split(":");

                    if (splitRule.Length == 1)
                    {
                        rules.Add(new Rule("-", splitRule[0], false, -1));
                        continue;
                    }

                    if (splitRule[0].Contains(">"))
                    {
                        string[] splitCondition = splitRule[0].Split(">");
                        long number = Int64.Parse(splitCondition[1]);
                        rules.Add(new Rule(splitCondition[0], splitRule[1], true, number));
                    }
                    else
                    {
                        string[] splitCondition = splitRule[0].Split("<");
                        long number = Int64.Parse(splitCondition[1]);
                        rules.Add(new Rule(splitCondition[0], splitRule[1], false, number));
                    }
                }
                workflows.Add(name, rules);
            }
            return workflows;
        }

        public static long Part1(string[] input)
        {
            List<string> workflowStrings = new List<string>();
            int i = 0;
            while (input[i] != "")
            {
                workflowStrings.Add(input[i]);
                i++;
            }

            Dictionary<string, List<Func<Part, string?>>> workflows = ParseWorkflows(
                workflowStrings
            );

            long total = 0;

            i++;
            while (i < input.Length)
            {
                string rawString = input[i].Substring(1, input[i].Length - 2);
                Part part = new Part();
                string[] partData = rawString.Split(",");
                foreach (string valueString in partData)
                {
                    string[] eqSplit = valueString.Split("=");
                    var info = part.GetType().GetProperty(eqSplit[0].ToUpper());
                    if (info == null)
                        throw new Exception("no info");
                    object p = part;
                    info.SetValue(p, Int64.Parse(eqSplit[1]));
                    part = (Part)p;
                }

                string currentWorkflow = "in";
                while (true)
                {
                    var checks = workflows[currentWorkflow];
                    foreach (var check in checks)
                    {
                        var result = check(part);
                        if (result != null)
                        {
                            currentWorkflow = result;
                            break;
                        }
                    }
                    if (currentWorkflow == "A")
                    {
                        total += part.X + part.M + part.A + part.S;
                        break;
                    }

                    if (currentWorkflow == "R")
                    {
                        break;
                    }
                }
                i++;
            }

            return total;
        }

        private static Ranges SetRangeValue(Ranges range, string propertyName, long[] value)
        {
            var info = range.GetType().GetProperty(propertyName);
            if (info == null)
                throw new Exception("no info");
            object r = range;
            info.SetValue(r, value);
            return (Ranges)r;
        }

        private static void GetRanges(
            Dictionary<string, List<Rule>> workflows,
            string currentWorkflowName,
            Ranges currentRanges,
            List<Ranges> rangesList
        )
        {
            List<Rule> currentWorkflow = workflows[currentWorkflowName];
            foreach (Rule rule in currentWorkflow)
            {
                if (rule.Letter == "-")
                {
                    if (rule.ResultIfTrue == "A")
                    {
                        rangesList.Add(
                            JsonSerializer.Deserialize<Ranges>(
                                JsonSerializer.Serialize(currentRanges)
                            )
                        );
                    }
                    else if (rule.ResultIfTrue != "R")
                    {
                        GetRanges(workflows, rule.ResultIfTrue, currentRanges, rangesList);
                    }
                }
                else if (rule.IsGreaterThan)
                {
                    string propertyName = $"{rule.Letter.ToUpper()}Range";
                    long[]? rangeForProperty = (long[]?)
                        currentRanges.GetType().GetProperty(propertyName)?.GetValue(currentRanges);
                    if (rangeForProperty == null)
                        throw new Exception("Error");

                    if (rule.Value >= rangeForProperty[1])
                        continue;

                    Ranges rangeClone = JsonSerializer.Deserialize<Ranges>(
                        JsonSerializer.Serialize(currentRanges)
                    );
                    if (rule.Value >= rangeForProperty[0])
                    {
                        rangeClone = SetRangeValue(
                            rangeClone,
                            propertyName,
                            new long[] { rule.Value + 1, rangeForProperty[1] }
                        );
                    }
                    if (rule.ResultIfTrue == "A")
                    {
                        rangesList.Add(rangeClone);
                    }
                    else if (rule.ResultIfTrue != "R")
                    {
                        GetRanges(workflows, rule.ResultIfTrue, rangeClone, rangesList);
                    }

                    if (rule.Value >= rangeForProperty[0] && rule.Value < rangeForProperty[1])
                    {
                        rangeClone = SetRangeValue(
                            rangeClone,
                            propertyName,
                            new long[] { rangeForProperty[0], rule.Value }
                        );
                    }
                    currentRanges = rangeClone;
                }
                else
                {
                    string propertyName = $"{rule.Letter.ToUpper()}Range";
                    long[]? rangeForProperty = (long[]?)
                        currentRanges.GetType().GetProperty(propertyName)?.GetValue(currentRanges);
                    if (rangeForProperty == null)
                        throw new Exception("Error");

                    if (rule.Value <= rangeForProperty[0])
                        continue;

                    Ranges rangeClone = JsonSerializer.Deserialize<Ranges>(
                        JsonSerializer.Serialize(currentRanges)
                    );
                    if (rule.Value <= rangeForProperty[1])
                    {
                        rangeClone = SetRangeValue(
                            rangeClone,
                            propertyName,
                            new long[] { rangeForProperty[0], rule.Value - 1 }
                        );
                    }
                    if (rule.ResultIfTrue == "A")
                    {
                        rangesList.Add(rangeClone);
                    }
                    else if (rule.ResultIfTrue != "R")
                    {
                        GetRanges(workflows, rule.ResultIfTrue, rangeClone, rangesList);
                    }

                    if (rule.Value <= rangeForProperty[1] && rule.Value > rangeForProperty[0])
                    {
                        rangeClone = SetRangeValue(
                            rangeClone,
                            propertyName,
                            new long[] { rule.Value, rangeForProperty[1] }
                        );
                    }
                    currentRanges = rangeClone;
                }
            }
        }

        public static long Part2(string[] input)
        {
            List<string> workflowStrings = new List<string>();
            int i = 0;
            while (input[i] != "")
            {
                workflowStrings.Add(input[i]);
                i++;
            }
            Dictionary<string, List<Rule>> workflows = ParseWorkflows2(workflowStrings);
            List<Ranges> rangesList = new List<Ranges>();

            GetRanges(
                workflows,
                "in",
                new Ranges
                {
                    XRange = new long[] { 1, 4000 },
                    MRange = new long[] { 1, 4000 },
                    ARange = new long[] { 1, 4000 },
                    SRange = new long[] { 1, 4000 },
                },
                rangesList
            );

            long total = 0;
            foreach (var range in rangesList)
            {
                long lengthX = range.XRange[1] - range.XRange[0] + 1;
                long lengthM = range.MRange[1] - range.MRange[0] + 1;
                long lengthA = range.ARange[1] - range.ARange[0] + 1;
                long lengthS = range.SRange[1] - range.SRange[0] + 1;
                long product = lengthX * lengthM * lengthA * lengthS;
                total += product;
            }
            return total;
        }
    }
}

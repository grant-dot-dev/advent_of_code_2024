namespace Day5_Csharp;

public static class Helpers
{
    public static (bool IsValid, int[] OrderedUpdate) CheckOrder(int[] input, Dictionary<int, List<int>> rules)
    {
        // Create a dictionary to store the position of each number in the input list for easy comparison
        var positions = input.Select((num, index) => new { num, index })
            .ToDictionary(x => x.num, x => x.index);


        foreach (var pageRule in rules)
        {
            var key = pageRule.Key;
            var subordinates = pageRule.Value;

            if (positions.TryGetValue(key, out var keyPosition))
            {
                foreach (var sub in subordinates)
                {
                    if (positions.TryGetValue(sub, out var subPosition) && subPosition < keyPosition)
                    {
                        // Invalid order found
                        return (false, FixOrder(input, rules));
                    }
                }
            }
        }

        return (true, input);
    }

    private static int[] FixOrder(int[] update, Dictionary<int, List<int>> rules)
    {
        bool changesMade;
        do
        {
            changesMade = false;

            for (var i = 0; i < update.Length; i++)
            {
                var page = update[i];

                if (!rules.TryGetValue(page, out var ruleSet)) continue;

                foreach (var subordinate in ruleSet)
                {
                    // if the subordinate is before or equal to the current index, move on as already processed or not needed.
                    var subIndex = Array.IndexOf(update, subordinate);
                    if (subIndex <= i) continue;

                    // Swap positions to ensure the page comes before its subordinate
                    Swap(ref update[i], ref update[subIndex]);
                    changesMade = true;
                }
            }
        } while (changesMade);

        return update;
    }

    private static void Swap(ref int a, ref int b)
    {
        (a, b) = (b, a);
    }


    public static (Dictionary<int, List<int>> Rules, List<int[]> Updates) ParseInput(string[] lines)
    {
        Dictionary<int, List<int>> rules = new();
        List<int[]> updates = [];

        foreach (var line in lines)
        {
            if (line.Contains('|'))
            {
                // Parse ordering rules
                var numbers = line.Split('|');
                var number1 = int.Parse(numbers[0]);
                var number2 = int.Parse(numbers[1]);
                HandleOrderingStorage(rules, number1, number2);
            }
            else if (string.IsNullOrEmpty(line))
            {
                // Ignore empty lines
                continue;
            }
            else
            {
                // Parse updates
                var numbers = line.Split(',');
                updates.Add(numbers.Select(int.Parse).ToArray());
            }
        }


        return (rules, updates);
    }


    private static void HandleOrderingStorage(Dictionary<int, List<int>> ordering, int number1, int number2)
    {
        if (!ordering.TryGetValue(number1, out var value))
        {
            ordering[number1] = [number2];
        }

        value?.Add(number2);
    }
}
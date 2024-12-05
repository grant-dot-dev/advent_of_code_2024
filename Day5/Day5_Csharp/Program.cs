using Day5_Csharp;

var input = File.ReadAllLines("input.txt");

var (orderingRules, updates) = Helpers.ParseInput(input);
var validUpdates = new List<int[]>();
var invalidUpdates = new List<int[]>();


foreach (var update in updates)
{
    var (isValid, orderedUpdate) = Helpers.CheckOrder(update, orderingRules);

    if (isValid)
    {
        validUpdates.Add(orderedUpdate);
    }
    else
    {
        invalidUpdates.Add(orderedUpdate);
    }
}

// Part 1:
var part1Total = validUpdates.Sum(update => update[update.Length / 2]);
Console.WriteLine("Total Part 1: " + part1Total);

// Part 2:
var part2Total = invalidUpdates.Sum(update => update[update.Length / 2]);
Console.WriteLine("Total Part 2: " + part2Total);
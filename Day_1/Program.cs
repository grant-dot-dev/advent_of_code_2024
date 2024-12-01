var input = File.ReadAllLines("./input_1.txt")
    .Select(s => s.Split([' '], StringSplitOptions.RemoveEmptyEntries)).ToList();

var lists = ProcessLines(input);

Part1(lists.list1, lists.list2);

Part2(lists.list1, lists.list2);
return;


void Part1(List<int> list1, List<int> list2)
{
    var ordered1 = list1.OrderBy(x => x).ToList();
    var ordered2 = list2.OrderBy(x => x).ToList();
    var totals = ordered1
        .Select((t, i) => Math.Abs(t - ordered2[i]))
        .Select(difference => (long)difference)
        .ToList();


    Console.WriteLine("Sum of differences: " + totals.Sum());
}


void Part2(List<int> list1, List<int> list2)
{
    var counts = list1.ToDictionary(
        number => number,
        number => list2.Count(x => x == number)
    );

    var sum = counts.Sum(x => x.Value * x.Key);
    Console.WriteLine("Sum of counts: " + sum);
}

(List<int> list1, List<int> list2) ProcessLines(List<string[]> rows)
{
    var list1 = new List<int>();
    var list2 = new List<int>();

    foreach (var row in rows)
    {
        list1.Add(int.Parse(row[0]));
        list2.Add(int.Parse(row[1]));
    }

    return (list1, list2);
}
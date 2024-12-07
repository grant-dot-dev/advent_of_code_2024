
string[] lines = File.ReadAllLines("../input1.txt");
long total = 0;

foreach (string line in lines)
{
    string[] parts = line.Split(':');
    long target = long.Parse(parts[0].Trim());
    long[] numbers = Array.ConvertAll(parts[1].Trim().Split(), long.Parse);

    // Use backtracking to check if the target can be matched
    if (EqualsTarget(target, numbers))
    {
        total += target;
    }
}

Console.WriteLine($"Total: {total}");
return;

bool EqualsTarget(long target, long[] numbers)
{
    return Backtrack(1, numbers[0], target, numbers);
}

bool Backtrack(long index, long currentValue, long target, long[] numbers)
{
    // Base case: if all numbers have been processed, check if the current value matches the target
    if (index == numbers.Length)
    {
        return currentValue == target;
    }

    // Try addition
    if (Backtrack(index + 1, currentValue + numbers[index], target, numbers))
    {
        return true;
    }

    // Try multiplication
    if (Backtrack(index + 1, currentValue * numbers[index], target, numbers))
    {
        return true;
    }

    // Try concatenation
    long concatValue = long.Parse(currentValue.ToString() + numbers[index].ToString());
    if (Backtrack(index + 1, concatValue, target, numbers))
    {
        return true;
    }

    // No valid combination found
    return false;
}

namespace Day11_Chsharp;

public class Part2
{
    public static Dictionary<long, long> ProcessInput(string input)
    {
        var stones = new Dictionary<long, long>();
        var split = input.Split(' ');
        foreach (var value in split)
        {
            var stone = long.Parse(value);
            if (!stones.ContainsKey(stone))
                stones[stone] = 0;
            stones[stone]++;
        }

        return stones;
    }

    public static void BlinkXTimes(Dictionary<long, long> stones, int blinks)
    {
        for (var i = 0; i < blinks; i++)
        {
            Blink(stones);
        }
    }

    private static void Blink(Dictionary<long, long> masterStones)
    {
        var tempStones = new Dictionary<long, long>(masterStones);

        foreach (var (stone, currentCount) in tempStones.Where(stone => stone.Value != 0))
        {
            // We remove the current amount of this type of stone, from master to simulate being replaced
            masterStones[stone] -= currentCount;

            if (stone == 0)
            {
                AddStone(masterStones, 1, currentCount);
            }
            // if even number of digits
            else if (NumberOfDigits(stone) % 2 == 0)
            {
                var (left, right) = SplitNumberInHalf(stone);
                AddStone(masterStones, left, currentCount);
                AddStone(masterStones, right, currentCount);
            }
            else
            {
                AddStone(masterStones, stone * 2024, currentCount);
            }
        }
    }

    private static void AddStone(Dictionary<long, long> stones, long stone, long count)
    {
        if (!stones.ContainsKey(stone))
            stones[stone] = 0;
        stones[stone] += count;
    }

    private static int NumberOfDigits(long number)
    {
        if (number == 0) return 1;
        return (int)Math.Floor(Math.Log10(Math.Abs(number)) + 1);
    }

    private static (long, long) SplitNumberInHalf(long number)
    {
        var digits = NumberOfDigits(number);
        var divisor = (long)Math.Pow(10, digits / 2);

        var left = number / divisor;
        var right = number % divisor;

        return (left, right);
    }

    private static long SumValues(Dictionary<long, long> stones)
    {
        return stones.Values.Sum();
    }
}
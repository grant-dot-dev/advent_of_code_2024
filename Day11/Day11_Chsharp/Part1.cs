namespace Day11_Chsharp;

public class Part1
{
    public void Solve()
    {
        const string input = "8069 87014 98 809367 525 0 9494914 5";

        var numbers = input.Split(" ").Select(long.Parse).ToList();

        const int blinkCount = 25;

        for (var blink = 0; blink < blinkCount; blink++)
        {
            List<string> newStones = [];

            foreach (var stone in numbers)
            {
                switch (stone)
                {
                    case 0:
                        newStones.Add("1");
                        break;
                    default:
                    {
                        if (NumberOfDigits(stone) % 2 == 0)
                        {
                            var (first, second) = SplitNumberInHalf(stone);
                            newStones.AddRange([first.ToString(), second.ToString()]);
                        }
                        else
                        {
                            newStones.Add((stone * 2024).ToString());
                        }

                        break;
                    }
                }
            }

            numbers = newStones.Select(long.Parse).ToList();
        }

        Console.WriteLine("Total for part 1: " + numbers.Count);
    }

    long NumberOfDigits(long number)
    {
        if (number == 0) return 1;
        return (long)Math.Floor(Math.Log10(Math.Abs(number)) + 1);
    }

    (long first, long second) SplitNumberInHalf(long number)
    {
        var numberOfDigits = NumberOfDigits(number);
        var divisor = (long)Math.Pow(10, numberOfDigits / 2);

        var firstHalf = number / divisor; // First half
        var secondHalf = number % divisor; // Second half


        return (firstHalf, secondHalf);
    }
}
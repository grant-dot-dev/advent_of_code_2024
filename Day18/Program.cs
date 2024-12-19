using SolutionTools;

namespace Day18;

class Program
{
    static void Main()
    {
        var solver = new Solver();

        var result = solver.SolvePart1();
        Console.WriteLine($"Part 1 result: {result}");

        var result2 = solver.SolvePart2();
        Console.WriteLine($"Part 2 result: {result2}");
    }
}
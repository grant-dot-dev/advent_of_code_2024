using SolutionTools;

namespace Day16_Csharp;

class Program
{
    static void Main()
    {
        // Load input grid
        var inputGrid = Tools.ParseGridFromFile("input.txt");
        var solver = new PartSolver(inputGrid);

        var cost = solver.SolvePart1();
        Console.WriteLine($"Part 1 Result: {cost}");

        solver.SolvePart2();
    }
}
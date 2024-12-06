using Day6_CSharp;

var inputData = File.ReadAllLines("input1.txt");


Console.WriteLine($"Part 1 = {new Part1().Solve(inputData)}");
Console.WriteLine($"Part 2 = {new Part2().Solve(inputData)}");
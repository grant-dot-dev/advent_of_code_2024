using System.Diagnostics;
using Day6_CSharp;

var inputData = File.ReadAllLines("input1.txt");

Stopwatch sw = new Stopwatch();
sw.Start();
Console.WriteLine("Running...");
Console.WriteLine($"Part 1 = {new Part1().Solve(inputData)}");
Console.WriteLine($"Part 1 => {sw.Elapsed}ms");

Console.WriteLine($"Part 2 = {new Part2().Solve(inputData)}");
Console.WriteLine($"Part 2 => {sw.Elapsed}ms");

sw.Stop();
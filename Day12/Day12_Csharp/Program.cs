using System.Diagnostics;
using SolutionTools;

var sw = new Stopwatch();
sw.Start();
var input = File.ReadAllText("example.txt");
var map = Map.ConvertToCharMap(input);

var part1 = GridSolver.Part1("example.txt");
var part2 = GridSolver.Part2("example.txt");
sw.Stop();

Console.WriteLine($"Part 1 => {sw.Elapsed}ms");
Console.WriteLine(part1);
Console.WriteLine(part2);


return;
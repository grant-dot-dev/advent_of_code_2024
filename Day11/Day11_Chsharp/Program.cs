using Day11_Chsharp;

// Todo: update this to read from Console input
const string input = "<enter input here>";
const int blinkCount = 75;

var part1 = new Part1();
part1.Solve();

var stones = Part2.ProcessInput(input);
Part2.BlinkXTimes(stones, blinkCount);
Console.WriteLine($"Total for Part 2: {stones.Values.Sum()}");
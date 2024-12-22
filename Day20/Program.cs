using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionTools;

class Program
{
    record Step(Point Pos, int Steps);

    static void Main()
    {
        var solution = Solve();

        Console.WriteLine($"Part 1: {solution.Part1}");
        Console.WriteLine($"Part 2: {solution.Part2}");
    }

    static (int Part1, int Part2) Solve()
    {
        var grid = Tools.ParseGridFromFile("input.txt");
        var points = FindStartEnd(grid);
        var start = new Point(points[0].Item1, points[0].Item2);
        var end = new Point(points[1].Item1, points[1].Item2);

        var path = FindPath(grid, start, end);

        return (
            Part1: SolvePart(path, 2),
            Part2: SolvePart(path, 20)
        );
    }


    static Dictionary<string, Step> FindPath(char[,] grid, Point start, Point end)
    {
        var EndId = PosId(end);
        var path = new Dictionary<string, Step>();
        var queue = new Queue<Point>();

        var steps = 0;
        path[PosId(start)] = new Step(start, steps);
        queue.Enqueue(start);

        while (queue.Any())
        {
            var current = queue.Dequeue();

            if (PosId(current) == EndId)
                return path;

            foreach (var neighbor in GetNeighbors(grid, current, pos =>
                         grid[pos.Y, pos.X] != '#' && !path.ContainsKey(PosId(pos))))
            {
                steps++;
                path[PosId(neighbor)] = new Step(neighbor, steps);
                queue.Enqueue(neighbor);
            }
        }

        return path;
    }

    static (int, int)[] FindStartEnd(char[,] grid)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        var start = (-1, -1);
        var end = (-1, -1);

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                switch (grid[r, c])
                {
                    case 'S':
                        start = (r, c);
                        break;
                    case 'E':
                        end = (r, c);
                        break;
                }
            }
        }

        return [start, end];
    }

    static IEnumerable<Point> GetNeighbors(char[,] grid, Point pos, Func<Point, bool> isNeighborAllowed)
    {
        var deltas = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        foreach (var (dx, dy) in deltas)
        {
            var neighbor = new Point(pos.X + dx, pos.Y + dy);

            if (neighbor.X < 0 || neighbor.X >= grid.GetLength(0) || neighbor.Y < 0 ||
                neighbor.Y >= grid.GetLength(1) ||
                grid[neighbor.X, neighbor.Y] == '#' && !isNeighborAllowed(neighbor))
            {
                continue;
            }


            yield return neighbor;
        }
    }

    static int SolvePart(Dictionary<string, Step> path, int maxCheatTime)
    {
        var cheats = FindCheats(path, maxCheatTime);

        return cheats
            .GroupBy(savedSteps => savedSteps)
            .Where(group => group.Key >= 100)
            .Sum(group => group.Count());
    }

    static List<int> FindCheats(Dictionary<string, Step> pathMap, int maxCheatTime)
    {
        var pathArr = pathMap.Values.ToList();
        var cheats = new List<int>();

        for (var i = 0; i < pathArr.Count - 1; i++)
        {
            for (var j = i + 1; j < pathArr.Count; j++)
            {
                var posA = pathArr[i];
                var posB = pathArr[j];
                var stepsSaved = posB.Steps - posA.Steps;
                var distance = Math.Abs(posA.Pos.X - posB.Pos.X) + Math.Abs(posA.Pos.Y - posB.Pos.Y);

                if (distance > maxCheatTime) continue;

                var saved = stepsSaved - distance;
                if (saved > 0)
                    cheats.Add(saved);
            }
        }

        return cheats;
    }

    static string PosId(Point pos) => $"{pos.X},{pos.Y}";
}
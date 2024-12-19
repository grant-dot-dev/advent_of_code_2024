using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolutionTools;

public static class GridSolver
{
    private static bool IsValid(char[,] grid, int row, int col)
    {
        return row >= 0 && row < grid.GetLength(0) && col >= 0 && col < grid.GetLength(1);
    }

    private static HashSet<Point> FindRegion(char[,] grid, char area, int row, int col, HashSet<Point> seen)
    {
        if (!IsValid(grid, row, col) || grid[row, col] != area || seen.Contains(new Point(row, col)))
            return new HashSet<Point>();

        var result = new HashSet<Point> { new Point(row, col) };
        seen.Add(new Point(row, col));

        foreach (var direction in Directions.WithoutDiagonals)
        {
            var newRow = row + direction.X;
            var newCol = col + direction.Y;

            if (IsValid(grid, newRow, newCol) && !seen.Contains(new Point(newRow, newCol)))
            {
                result.UnionWith(FindRegion(grid, area, newRow, newCol, seen));
            }
        }

        return result;
    }

    private static List<HashSet<Point>> FindAllRegions(char[,] grid)
    {
        var seen = new HashSet<Point>();
        var regions = new List<HashSet<Point>>();

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                var point = new Point(row, col);
                if (!seen.Contains(point))
                {
                    var region = FindRegion(grid, grid[row, col], row, col, new HashSet<Point>());
                    seen.UnionWith(region);
                    regions.Add(region);
                }
            }
        }

        return regions;
    }

    public static int Part1(string filePath)
    {
        var grid = ReadGrid(filePath);
        var regions = FindAllRegions(grid);

        int result = 0;

        foreach (var region in regions)
        {
            int area = region.Count;
            int perimeter = 0;

            foreach (var point in region)
            {
                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var neighbor = new Point(point.X + direction.X, point.Y + direction.Y);

                    if (!region.Contains(neighbor))
                    {
                        perimeter++;
                    }
                }
            }

            result += area * perimeter;
        }

        return result;
    }

    public static int Part2(string filePath)
    {
        var grid = ReadGrid(filePath);
        var regions = FindAllRegions(grid);

        int result = 0;

        foreach (var region in regions)
        {
            int area = region.Count;
            var seen = new HashSet<(double, double)>();
            int corners = 0;

            foreach (var point in region)
            {
                foreach (var offset in new (double, double)[]
                         {
                             (-0.5, -0.5), (0.5, -0.5), (0.5, 0.5), (-0.5, 0.5)
                         })
                {
                    var corner = (point.X + offset.Item1, point.Y + offset.Item2);

                    if (seen.Contains(corner))
                        continue;

                    seen.Add(corner);

                    int adjacent = new (double, double)[]
                    {
                        (-0.5, -0.5), (0.5, -0.5), (0.5, 0.5), (-0.5, 0.5)
                    }.Count(offset2 =>
                    {
                        var neighbor = new Point((int)(corner.Item1 + offset2.Item1),
                            (int)(corner.Item2 + offset2.Item2));
                        return region.Contains(neighbor);
                    });

                    if (adjacent == 1 || adjacent == 3)
                    {
                        corners++;
                    }
                    else if (adjacent == 2)
                    {
                        bool[] pattern = new[]
                        {
                            region.Contains(new Point((int)(corner.Item1 - 0.5), (int)(corner.Item2 - 0.5))),
                            region.Contains(new Point((int)(corner.Item1 + 0.5), (int)(corner.Item2 + 0.5)))
                        };

                        if (pattern[0] == pattern[1])
                        {
                            corners += 2;
                        }
                    }
                }
            }

            result += area * corners;
        }

        return result;
    }

    private static char[,] ReadGrid(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var grid = new char[lines.Length, lines[0].Length];

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                grid[row, col] = lines[row][col];
            }
        }

        return grid;
    }
}
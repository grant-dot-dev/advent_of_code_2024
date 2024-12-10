using SolutionTools;

namespace Day10_CSharp;

public class BothParts
{
    private static Dictionary<Point, List<(int x, int y)>> ComputeAllValidNeighbours(int[,] map)
    {
        var neighbors = new Dictionary<Point, List<(int x, int y)>>();
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                var currentPoint = new Point(x, y);
                neighbors[currentPoint] = (from direction in Directions.WithoutDiagonals
                    let nextX = x + direction.X
                    let nextY = y + direction.Y
                    where nextX >= 0 && nextX < map.GetLength(0)
                    where nextY >= 0 && nextY < map.GetLength(1)
                    where map[nextX, nextY] - map[x, y] == 1
                    select (nextX, nextY)).ToList();
            }
        }

        return neighbors;
    }


    public static (long uniquePathsToNine, long totalPathsToNine) SolveGridPaths(int[,] map)
    {
        long uniquePathsToNine = 0;
        long totalPathsToNine = 0;

        var neighbors = ComputeAllValidNeighbours(map);
        foreach (var startPoint in GetStartingPoints(map))
        {
            var visitedEndPoints = new HashSet<Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(startPoint);

            while (queue.Count > 0)
            {
                var currentPoint = queue.Dequeue();
                if (map[currentPoint.X, currentPoint.Y] == 9)
                {
                    visitedEndPoints.Add(currentPoint);
                    totalPathsToNine++;
                    continue;
                }

                foreach (var neighbor in neighbors[currentPoint])
                {
                    queue.Enqueue(new Point(neighbor.x, neighbor.y));
                }
            }

            uniquePathsToNine += visitedEndPoints.Count;
        }

        return (uniquePathsToNine, totalPathsToNine);
    }

    private static List<(int x, int y)> GetStartingPoints(int[,] grid)
    {
        var startingPoints = new List<(int x, int y)>();

        var rowCount = grid.GetLength(0);
        var colCount = grid.GetLength(1);

        for (var row = 0; row < rowCount; row++)
        {
            for (var col = 0; col < colCount; col++)
            {
                if (grid[row, col] == 0)
                {
                    startingPoints.Add((row, col));
                }
            }
        }

        return startingPoints;
    }
}
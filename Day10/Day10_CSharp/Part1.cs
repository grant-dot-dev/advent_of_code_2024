using SolutionTools;

namespace Day10_CSharp;

public class BothParts
{
    // Find valid neighboring points for a given point in the map
    private static List<(int x, int y)> GetValidNeighbours(int[,] map, Point currentPoint)
    {
        return (from direction in Directions.WithoutDiagonals
            let nextX = currentPoint.X + direction.X
            let nextY = currentPoint.Y + direction.Y
            where nextX >= 0 && nextX < map.GetLength(0) && nextY >= 0 && nextY < map.GetLength(1)
            where map[nextX, nextY] - map[currentPoint.X, currentPoint.Y] == 1
            select (nextX, nextY)).ToList();
    }

    // Solve the problem for both parts
    public static (long uniquePathsToNine, long totalPathsToNine) SolveGridPaths(int[,] map)
    {
        long uniquePathsToNine = 0;
        long totalPathsToNine = 0;

        // Iterate through all starting points
        foreach (var startPoint in GetStartingPoints(map))
        {
            var visitedEndPoints = new HashSet<Point>(); // Tracks unique endpoints for part 1
            var pointsToExplore = new Queue<Point>(); // Queue to process points 
            pointsToExplore.Enqueue(startPoint);

            while (pointsToExplore.Count > 0)
            {
                var currentPoint = pointsToExplore.Dequeue();
                var currentValue = map[currentPoint.X, currentPoint.Y];

                if (currentValue == 9)
                {
                    visitedEndPoints.Add(currentPoint);
                    totalPathsToNine++;
                }
                else
                {
                    // Explore valid neighbors and add to the queue
                    var neighbors = GetValidNeighbours(map, currentPoint);
                    foreach (var neighbor in neighbors.Where(neighbor => !visitedEndPoints.Contains(neighbor)))
                    {
                        pointsToExplore.Enqueue(neighbor);
                    }
                }
            }

            // Add the count of unique endpoints to the total for part 1
            uniquePathsToNine += visitedEndPoints.Count;
        }

        return (uniquePathsToNine, totalPathsToNine);
    }

    // Find all starting points in the grid where the value is 0
    private static List<(int x, int y)> GetStartingPoints(int[,] grid)
    {
        var startingPoints = new List<(int x, int y)>();

        int rowCount = grid.GetLength(0);
        int colCount = grid.GetLength(1);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                if (grid[row, col] == 0)
                {
                    startingPoints.Add((row, col)); // Add each valid starting point
                }
            }
        }

        return startingPoints;
    }
}
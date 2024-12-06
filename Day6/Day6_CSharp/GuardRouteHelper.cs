public static class GuardRouteHelper
{
    public static HashSet<(int, int)> TraverseRoute((int, int) startPos, int nextRow, int nextCol, char[][] grid)
    {
        var visited = new HashSet<(int, int)>();
        var rowCount = grid.Length;
        var colCount = grid[0].Length;
        var (currRow, currCol) = startPos;

        while (true)
        {
            // Add current position to visited
            visited.Add((currRow, currCol));

            // Check bounds (stop if the guard is about to leave the grid)
            if (currRow + nextRow < 0 || currRow + nextRow >= rowCount || currCol + nextCol < 0 ||
                currCol + nextCol >= colCount)
            {
                break;
            }

            // Check for obstacle; if hit, change direction
            if (grid[currRow + nextRow][currCol + nextCol] == '#')
            {
                (nextRow, nextCol) = (nextCol, -nextRow); // Rotate 90 degrees
            }
            else
            {
                // Move to the next position
                currRow += nextRow;
                currCol += nextCol;
            }
        }

        return visited;
    }

    public static (int, int)? GetStartPosition(char[][] grid)
    {
        for (var rowIdx = 0; rowIdx < grid.Length; rowIdx++)
        {
            var colIdx = Array.IndexOf(grid[rowIdx], '^');
            if (colIdx != -1)
            {
                return (rowIdx, colIdx);
            }
        }

        return null;
    }
}
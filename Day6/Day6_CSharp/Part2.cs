namespace Day6_CSharp;

public class Part2
{
    public int Solve(string[] dataInput)
    {
        var total = 0;
        var grid = Array.ConvertAll(dataInput, row => row.ToCharArray());

        var startPosition = GuardRouteHelper.GetStartPosition(grid);
        if (startPosition == null)
        {
            throw new ArgumentNullException("startPosition");
        }

        // Always start up
        int nextRow = -1, nextCol = 0;

        // Use centralized helper for initial route
        var visitedPositions = GuardRouteHelper.TraverseRoute(startPosition.Value, nextRow, nextCol, grid);

        // Loop through the visited positions only to add obstacles
        foreach (var (row, col) in visitedPositions)
        {
            if (grid[row][col] != '.')
            {
                continue;
            }

            grid[row][col] = '#';
            if (PatrolLoop(startPosition.Value, nextRow, nextCol, grid))
            {
                total++;
            }

            grid[row][col] = '.';
        }

        return total;
    }

    private bool PatrolLoop((int, int) startPos, int nextRow, int nextCol, char[][] grid)
    {
        var rowCount = grid.Length;
        var colCount = grid[0].Length;
        var (currRow, currCol) = startPos;
        var visited = new HashSet<(int, int, int, int)>();

        while (true)
        {
            // Add current state to visited
            visited.Add((currRow, currCol, nextRow, nextCol));

            // Check bounds (stop if the guard is about to leave the grid)
            if (currRow + nextRow < 0 || currRow + nextRow >= rowCount || currCol + nextCol < 0 ||
                currCol + nextCol >= colCount)
            {
                break;
            }

            // Check for obstacle; if hit, change direction
            if (grid[currRow + nextRow][currCol + nextCol] == '#')
            {
                (nextRow, nextCol) = (nextCol, -nextRow);
            }
            else
            {
                // Move to the next position
                currRow += nextRow;
                currCol += nextCol;
            }

            // Check if looped
            if (visited.Contains((currRow, currCol, nextRow, nextCol)))
            {
                return true;
            }
        }

        return false;
    }
}
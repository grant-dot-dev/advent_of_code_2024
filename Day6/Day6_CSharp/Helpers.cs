namespace Day6_CSharp;

public class Helpers
{
    public static void FindRoute(HashSet<(int, int)> visited, (int, int) startPos, int nextRow, int nextCol,
        char[][] grid)
    {
        var rowCount = grid.Length;
        var colCount = grid[0].Length;
        var (currRow, currCol) = startPos;

        while (true)
        {
            // Add coords to visited
            visited.Add((currRow, currCol));

            // Bounds check (is guard gonna leave)
            if (currRow + nextRow < 0 || currRow + nextRow >= rowCount || currCol + nextCol < 0 ||
                currCol + nextCol >= colCount)
            {
                break;
            }

            // Check for obstacle else move
            if (grid[currRow + nextRow][currCol + nextCol] == '#')
            {
                (nextCol, nextRow) = (nextCol, nextCol);
            }
            else
            {
                currRow += nextRow;
                currCol += nextCol;
            }
        }
    }
}
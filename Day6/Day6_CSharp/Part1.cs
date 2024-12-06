public class Part1
{
    public int Solve(string[] dataInput)
    {
        var total = 0;
        var visited = new HashSet<(int, int)>();
        var grid = Array.ConvertAll(dataInput, row => row.ToCharArray());

        // Get start position of guard
        (int, int)? startPos = null;
        for (var rowIdx = 0; rowIdx < grid.Length; rowIdx++)
        {
            var row = grid[rowIdx];
            if (Array.IndexOf(row, '^') != -1)
            {
                var colIdx = Array.IndexOf(row, '^');
                startPos = (rowIdx, colIdx);
                break;
            }
        }

        int nextRow = -1, nextCol = 0;
        Patrol(visited, startPos.Value, nextRow, nextCol, grid);

        return visited.Count;
    }

    void Patrol(HashSet<(int, int)> visited, (int, int) startPos, int nextRow, int nextCol, char[][] grid)
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
                (nextRow, nextCol) = (nextCol, -nextRow);
            }
            else
            {
                currRow += nextRow;
                currCol += nextCol;
            }
        }
    }
}
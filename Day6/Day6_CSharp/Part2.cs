public class Part2
{
    bool PatrolLoop((int, int) startPos, int nextRow, int nextCol, char[][] grid)
    {
        var rowCount = grid.Length;
        var colCount = grid[0].Length;
        var (currRow, currCol) = startPos;
        var visited = new HashSet<(int, int, int, int)>();

        while (true)
        {
            // Add coords to visited
            visited.Add((currRow, currCol, nextRow, nextCol));
            // Bounds check (is guard gonna leave)
            if (currRow + nextRow < 0 || currRow + nextRow >= rowCount || currCol + nextCol < 0 ||
                currCol + nextCol >= colCount)
            {
                break;
            }

            // Check for obstacle else move
            if (grid[currRow + nextRow][currCol + nextCol] == '#')
            {
                (nextCol, nextRow) = (-nextRow, nextCol);
            }
            else
            {
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


    public int Solve(string[] dataInput)
    {
        var total = 0;
        var grid = Array.ConvertAll(dataInput, row => row.ToCharArray());

        // Get start position of guard
        (int, int)? startPos = null;
        for (var rowIdx = 0; rowIdx < grid.Length; rowIdx++)
        {
            var row = grid[rowIdx];
            if (Array.IndexOf(row, '^') == -1) continue;

            var colIdx = Array.IndexOf(row, '^');
            startPos = (rowIdx, colIdx);
            break;
        }

        int nextRow = -1, nextCol = 0;

        // Loop the rows and columns, adding an obstacle and check for looped route
        foreach (var row in grid)
        {
            for (var col = 0; col < grid[0].Length; col++)
            {
                if (row[col] != '.')
                {
                    continue;
                }

                row[col] = '#';
                if (PatrolLoop(startPos.Value, nextRow, nextCol, grid))
                {
                    total++;
                }

                row[col] = '.';
            }
        }

        return total;
    }
}
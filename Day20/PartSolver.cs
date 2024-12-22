public class PartSolver
{
    private readonly char[,] _grid;
    private readonly int _rows;
    private readonly int _cols;

    public PartSolver(char[,] grid)
    {
        _grid = grid;
        _rows = grid.GetLength(0);
        _cols = grid.GetLength(1);
    }

    public int SolvePart1()
    {
        return CountCheatsSavingAtLeast100(_grid);
    }

    public int CountCheatsSavingAtLeast100(char[,] grid)
    {
        var startEndPoints = FindStartEnd();
        var (sr, sc) = startEndPoints[0];
        var (er, ec) = startEndPoints[1];

        int count = 0;

        for (var r = 0; r < grid.GetLength(0); r++)
        {
            for (var c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == '#' || grid[r, c] == 'E') continue;

                int originalCost = FindShortestPath(grid, r, c, er, ec, allowCheat: false);

                // If there's no path, skip this configuration
                if (originalCost == -1) continue;

                // Compute cost with cheat
                int cheatedCost = FindShortestPath(grid, r, c, er, ec, allowCheat: true);

                // If savings are 100 or more, increment count
                if (originalCost - cheatedCost >= 100)
                {
                    count++;
                }
            }
        }

        return count;
    }


    private int FindShortestPath(char[,] grid, int sr, int sc, int er, int ec, bool allowCheat)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        var pq = new PriorityQueue<(int cost, int r, int c, bool cheatUsed, int cheatSteps), int>();
        pq.Enqueue((0, sr, sc, false, 0), 0);

        var seen = new HashSet<(int row, int column, bool cheatUsed, int cheatSteps)>();
        seen.Add((sr, sc, false, 0));

        var directions = new (int dr, int dc)[]
        {
            (0, 1), // Right
            (1, 0), // Down
            (0, -1), // Left
            (-1, 0) // Up
        };

        while (pq.Count > 0)
        {
            var (cost, row, col, cheatUsed, cheatSteps) = pq.Dequeue();

            // If we've reached the target position
            if (row == er && col == ec)
            {
                return cost;
            }

            // Explore all 4 possible directions
            foreach (var (dr, dc) in directions)
            {
                int newRow = row + dr;
                int newCol = col + dc;
                int newCost = cost + 1;

                // Skip out-of-bounds cells
                if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                {
                    continue;
                }

                bool isWall = grid[newRow, newCol] == '#';

                // Case 1: Regular track movement
                if (!isWall)
                {
                    if (seen.Add((newRow, newCol, cheatUsed, 0)))
                    {
                        pq.Enqueue((newCost, newRow, newCol, cheatUsed, 0), newCost);
                    }
                }
                // Case 2: Wall movement (only if cheat is allowed)
                else if (allowCheat && (!cheatUsed || (cheatUsed && cheatSteps < 2)))
                {
                    // Use the cheat or continue cheat mode
                    int nextCheatSteps = cheatUsed ? cheatSteps + 1 : 1;
                    if (seen.Add((newRow, newCol, true, nextCheatSteps)))
                    {
                        pq.Enqueue((newCost, newRow, newCol, true, nextCheatSteps), newCost);
                    }
                }
            }
        }

        return -1; // No path found
    }

    private (int, int)[] FindStartEnd()
    {
        var start = (-1, -1);
        var end = (-1, -1);

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _cols; c++)
            {
                switch (_grid[r, c])
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
}
namespace SolutionTools;

public static class Map
{
    public static char[,] ConvertToCharMap(string input)
    {
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


        var rows = lines.Length;
        var cols = lines[0].Length;

        var map = new char[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                map[row, col] = lines[row][col];
            }
        }

        return map;
    }


    public static void PrintGrid(char[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(array[i, j]);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Finds shortest path using Dijkstra algorithm Up, Down, Left, Right, where cost for all movements is 1
    /// </summary>
    /// <param name="grid">Map to be navigated</param>
    /// <param name="sr">Starting row index</param>
    /// <param name="sc">Starting column index</param>
    /// <param name="er">Ending row index</param>
    /// <param name="ec"> Ending column index</param>
    /// <returns>Number of steps</returns>
    public static int FindShortestPath(char[,] grid, int sr, int sc, int er, int ec)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        var pq = new PriorityQueue<(int cost, int r, int c), int>();
        pq.Enqueue((0, sr, sc), 0);

        var seen = new HashSet<(int row, int column)> { (sr, sc) };

        var directions = new (int dr, int dc)[]
        {
            (0, 1), // Right
            (1, 0), // Down
            (0, -1), // Left
            (-1, 0) // Up
        };

        while (pq.Count > 0)
        {
            var (cost, row, col) = pq.Dequeue();

            // If we've reached the target position
            if (row == er && col == ec)
            {
                Map.PrintGrid(grid);
                return cost;
            }

            // Explore all 4 possible directions
            foreach (var (dr, dc) in directions)
            {
                int newRow = row + dr;
                int newCol = col + dc;
                int newCost = cost + 1;

                // Skip out-of-bounds or blocked cells
                if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || grid[newRow, newCol] == '#')
                {
                    continue;
                }

                // Skip already-seen cells
                if (!seen.Add((newRow, newCol)))
                {
                    continue;
                }

                // Enqueue new position with updated cost
                pq.Enqueue((newCost, newRow, newCol), newCost);
            }
        }

        return -1; // No path found
    }
}
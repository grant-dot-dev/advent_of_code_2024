using SolutionTools;

namespace Day18
{
    public class Solver
    {
        private const bool IsDevelopment = false;
        private readonly char[,] _grid;
        private readonly int _rows;
        private readonly int _cols;
        private int _corruptions = IsDevelopment ? 12 : 1024;

        public Solver()
        {
            var gridSize = IsDevelopment ? 7 : 71;

            _grid = new char[gridSize, gridSize];
            _rows = _grid.GetLength(0);
            _cols = _grid.GetLength(1);


            if (!IsDevelopment) return;

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    _grid[i, j] = '.';
                }
            }

            Map.PrintGrid(_grid);
        }

        // Dijkstras Algorithms
        public int SolvePart1()
        {
            var lines = File.ReadAllLines(IsDevelopment ? "example.txt" : "input.txt");


            for (var index = 0; index < _corruptions; index++)
            {
                var line = lines[index];
                var coordinates = line.Split(',').Select(x => int.Parse(x.Trim())).ToArray();
                _grid[coordinates[0], coordinates[1]] = '#';
            }


            int sr = 0, sc = 0;

            var pq = new PriorityQueue<(int cost, int r, int c), int>();
            pq.Enqueue((0, sr, sc), 0);

            var seen = new HashSet<(int row, int column)> { (sr, sc) };

            // Direction arrays for moving up, down, left, right
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
                if (row == (_rows - 1) && col == (_cols - 1))
                {
                    Map.PrintGrid(_grid);
                    return cost;
                }

                // Explore all 4 possible moves
                foreach (var (dr, dc) in directions)
                {
                    int newRow = row + dr;
                    int newCol = col + dc;
                    int newCost = cost + 1;

                    // Skip out-of-bounds or blocked cells
                    if (newRow < 0 || newRow >= _rows || newCol < 0 || newCol >= _cols || _grid[newRow, newCol] == '#')
                    {
                        continue;
                    }

                    // Skip already-seen cells
                    if (!seen.Add((newRow, newCol)))
                    {
                        continue;
                    }

                    // Enqueue new position with updated cost
                    _grid[newRow, newCol] = '0';
                    pq.Enqueue((newCost, newRow, newCol), newCost);
                }
            }

            return -1; // No path found
        }

        public (int, int) SolvePart2()
        {
            int sr = 0, sc = 0; // Start position (0, 0)
            int er = _grid.GetLength(0) - 1, ec = _grid.GetLength(1) - 1;

            var lines = File.ReadAllLines(IsDevelopment ? "example.txt" : "input.txt")
                .Select(line => line.Split(','))
                .Select(x => (int.Parse(x[0]), int.Parse(x[1])))
                .ToArray();
            ;


            foreach (var (br, bc) in lines)
            {
                // Mark the position as blocked
                _grid[br, bc] = '#';

                // Check if the path is still reachable
                if (!IsPathReachable(sr, sc, er, ec))
                {
                    return (br, bc);
                }
            }

            return (-1, -1); // No blockage found
        }

        private bool IsPathReachable(int sr, int sc, int targetRow, int targetCol)
        {
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


                if (row == targetRow && col == targetCol)
                {
                    return true;
                }


                foreach (var (dr, dc) in directions)
                {
                    int newRow = row + dr;
                    int newCol = col + dc;
                    int newCost = cost + 1;

                    // Skip out-of-bounds or blocked cells
                    if (newRow < 0 || newRow >= _rows || newCol < 0 || newCol >= _cols || _grid[newRow, newCol] == '#')
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

            return false; // No path found
        }
    }
}
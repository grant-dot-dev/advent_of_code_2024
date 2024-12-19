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

            // Initialize the grid to default state
            InitializeGrid();

            // Print initial grid for debugging
            Map.PrintGrid(_grid);
        }

        private void InitializeGrid()
        {
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _cols; j++)
                {
                    _grid[i, j] = '.';
                }
            }
        }

        // Part 1: Solve the pathfinding with corruptions
        public int SolvePart1()
        {
            var lines = File.ReadAllLines(IsDevelopment ? "example.txt" : "input.txt");
            ApplyCorruptions(lines);

            int sr = 0, sc = 0;
            return FindShortestPath(sr, sc, _rows - 1, _cols - 1);
        }

        // Part 2: Find the first byte that blocks the path
        public (int, int) SolvePart2()
        {
            int sr = 0, sc = 0; // Start position (0, 0)
            int er = _rows - 1, ec = _cols - 1; // End position

            var lines = File.ReadAllLines(IsDevelopment ? "example.txt" : "input.txt")
                .Select(line => line.Split(','))
                .Select(x => (int.Parse(x[0]), int.Parse(x[1])))
                .ToArray();

            // Apply corruptions one by one and check if path is blocked
            foreach (var (br, bc) in lines)
            {
                _grid[br, bc] = '#';
                if (!IsPathReachable(sr, sc, er, ec))
                {
                    return (br, bc);
                }
            }

            return (-1, -1); // No blockage found
        }

        // Apply corruption based on input lines (obstacle placement)
        private void ApplyCorruptions(string[] lines)
        {
            for (var index = 0; index < _corruptions; index++)
            {
                var line = lines[index];
                var coordinates = line.Split(',').Select(x => int.Parse(x.Trim())).ToArray();
                _grid[coordinates[0], coordinates[1]] = '#';
            }
        }

        private int FindShortestPath(int sr, int sc, int er, int ec)
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

                // If we've reached the target position
                if (row == er && col == ec)
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
                    pq.Enqueue((newCost, newRow, newCol), newCost);
                }
            }

            return -1; // No path found
        }

        // Pathfinding check if a path exists
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

                    pq.Enqueue((newCost, newRow, newCol), newCost);
                }
            }

            return false; // No path found
        }
    }
}
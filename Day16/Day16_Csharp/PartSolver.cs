namespace Day16_Csharp;

public class PartSolver
{
    private readonly char[,] _grid;
    private readonly int _rows;
    private readonly int _cols;

    public PartSolver(char[,] inputGrid)
    {
        _grid = inputGrid;
        _rows = _grid.GetLength(0);
        _cols = _grid.GetLength(1);
    }

    public int SolvePart1()
    {
        int sr = -1, sc = -1;
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _cols; c++)
            {
                if (_grid[r, c] != 'S') continue;

                sr = r;
                sc = c;
                break;
            }

            if (sr != -1) break;
        }

        var pq = new PriorityQueue<(int cost, int r, int c, int dr, int dc), int>();
        pq.Enqueue((0, sr, sc, 0, 1), 0);

        var seen = new HashSet<(int row, int column, int dirRow, int dirColumn)> { (sr, sc, 0, 1) };

        while (pq.Count > 0)
        {
            var (cost, row, col, dirRow, dirCol) = pq.Dequeue();

            if (_grid[row, col] == 'E')
            {
                return cost;
            }

            // Possible moves
            var moves = new (int newCost, int newRow, int newCol, int newDirRow, int newDirCol)[]
            {
                (cost + 1, row + dirRow, col + dirCol, newDirRow: dirRow, newDirCol: dirCol),
                (cost + 1000, newRow: row, newCol: col, newDirRow: dirCol, -dirRow),
                (cost + 1000, newRow: row, newCol: col, -dirCol, newDirCol: dirRow)
            };

            foreach (var (newCost, newRow, newCol, newDirRow, newDirCol) in moves)
            {
                // Skip out-of-bounds or blocked cells
                if (newRow < 0 || newRow >= _rows || newCol < 0 || newCol >= _cols ||
                    _grid[newRow, newCol] == '#') continue;

                // Skip already-seen states
                if (!seen.Add((newRow, newCol, newDirRow, newDirCol))) continue;

                pq.Enqueue((newCost, newRow, newCol, newDirRow, newDirCol), newCost);
            }
        }

        return -1; // No path found
    }

    public void SolvePart2()
    {
        (int sr, int sc) = FindStart();

        var pq = new PriorityQueue<(int cost, int r, int c, int dr, int dc), int>();
        pq.Enqueue((0, sr, sc, 0, 1), 0);

        // Data structures
        var lowestCost = new Dictionary<(int, int, int, int), int> { { (sr, sc, 0, 1), 0 } };
        var backtrack = new Dictionary<(int, int, int, int), HashSet<(int, int, int, int)>>();
        var endStates = new HashSet<(int, int, int, int)>();
        int bestCost = int.MaxValue;

        // Dijkstra loop
        while (pq.TryDequeue(out var state, out var cost))
        {
            var (currentCost, r, c, dr, dc) = state;

            // Skip if not optimal
            if (currentCost > lowestCost.GetValueOrDefault((r, c, dr, dc), int.MaxValue))
                continue;

            // Found the end position
            if (_grid[r, c] == 'E')
            {
                if (currentCost > bestCost) break;
                bestCost = currentCost;
                endStates.Add((r, c, dr, dc));
            }

            // Explore possible movements
            var directions = new (int, int, int, int, int)[]
            {
                (currentCost + 1, r + dr, c + dc, dr, dc), // Move forward
                (currentCost + 1000, r, c, dc, -dr), // Turn right
                (currentCost + 1000, r, c, -dc, dr) // Turn left
            };

            foreach (var (newCost, nr, nc, ndr, ndc) in directions)
            {
                if (!IsValid(nr, nc) || _grid[nr, nc] == '#') continue;

                var newKey = (nr, nc, ndr, ndc);
                var lowest = lowestCost.GetValueOrDefault(newKey, int.MaxValue);

                if (newCost > lowest) continue;

                if (newCost < lowest)
                {
                    backtrack[newKey] = new HashSet<(int, int, int, int)>();
                    lowestCost[newKey] = newCost;
                }

                backtrack[newKey].Add((r, c, dr, dc));
                pq.Enqueue((newCost, nr, nc, ndr, ndc), newCost);
            }
        }

        // BFS for Backtracking 
        var states = new Queue<(int, int, int, int)>(endStates);
        var seen = new HashSet<(int, int, int, int)>(endStates);

        while (states.Count > 0)
        {
            Console.WriteLine("States:" + states.Count);
            var key = states.Dequeue();

            if (!backtrack.TryGetValue(key, out var previousStates)) continue;

            foreach (var last in previousStates.Where(last => !seen.Contains(last)))
            {
                seen.Add(last);
                states.Enqueue(last);
            }
        }

        // Output result: unique (r, c) positions visited
        var uniquePositions = new HashSet<(int, int)>();
        foreach (var (r, c, _, _) in seen)
        {
            uniquePositions.Add((r, c));
        }

        Console.WriteLine($"Part 2 Result: {uniquePositions.Count}");
    }

    private (int, int) FindStart()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (_grid[r, c] == 'S')
                    return (r, c);
            }
        }

        throw new Exception("Start position not found.");
    }

    private bool IsValid(int r, int c)
    {
        return r >= 0 && r < _rows && c >= 0 && c < _cols;
    }
}
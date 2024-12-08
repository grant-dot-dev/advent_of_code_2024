using SolutionTools;

var lines = File.ReadAllLines("test.txt")
                .Select(line => line.ToCharArray())
                .ToArray();


var grid = ParseGrid(lines);
var antennaLocations = BuildAntennaLocations(grid);

Console.WriteLine($"Part 1: {Part1()}");
Console.WriteLine($"Part 2: {Part2()}");
return;

int Part1()
{
    var antinodes = new HashSet<Point>();

    foreach (var coords in antennaLocations.Values)
    {
        foreach (var ((ay, ax), (by, bx)) in Combinations(coords, 2))
        {
            int dy = ay - by;
            int dx = ax - bx;

            var aa = new Point(ax + dx, ay + dy);
            var bb = (bx - dx, by - dy);

            if (grid.Contains(aa)) antinodes.Add(aa);
            if (grid.Contains(bb)) antinodes.Add(bb);
        }
    }

    return antinodes.Count;
}

int Part2()
{
    var antinodes = new HashSet<Point>();

    foreach (var coords in antennaLocations.Values)
    {
        foreach (var ((ay, ax), (by, bx)) in Combinations<Point>(coords, 2))
        {
            int dy = ay - by;
            int dx = ax - bx;
            int gcd = Gcd(dx, dy);
            dy /= gcd;
            dx /= gcd;

            int i = 0;
            while (true)
            {
                var aa = (ay + dy * i, ax + dx * i);
                if (grid.Contains(aa))
                {
                    antinodes.Add(aa);
                    i++;
                }
                else
                {
                    break;
                }
            }

            i = 0;
            while (true)
            {
                var bb = (by - dy * i, bx - dx * i);
                if (grid.Contains(bb))
                {
                    antinodes.Add(bb);
                    i++;
                }
                else
                {
                    break;
                }
            }
        }
    }

    return antinodes.Count;
}

HashSet<Point> ParseGrid(char[][] input)
{
    var gridPoints = new HashSet<Point>();

    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            gridPoints.Add(new Point(x, y));
        }
    }
    return gridPoints;
}

Dictionary<char, List<Point>> BuildAntennaLocations(HashSet<Point> grid)
{
    var antennas = new Dictionary<char, List<Point>>();
    foreach (var (point, val) in from point in grid
                                 let val = lines[point.Y][point.X]
                                 where val != '.'
                                 select (point, val))
    {
        if (!antennas.ContainsKey(val))
        {
            antennas[val] = new List<Point>();
        }

        antennas[val].Add(new Point(point.X, point.Y));
    }

    return antennas;
}

static IEnumerable<(T, T)> Combinations<T>(List<T> items, int k)
{
    if (k == 2)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                yield return (items[i], items[j]);
            }
        }
    }
}

static int Gcd(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return Math.Abs(a);
}
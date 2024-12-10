using Day10_CSharp;

var lines = File.ReadAllLines("./input.txt");
var gridSize = lines.Length;


var grid = new int[gridSize, gridSize];

for (var row = 0; row < gridSize; row++)
{
    for (var col = 0; col < gridSize; col++)
    {
        grid[row, col] = lines[row][col] - '0';
    }
}


var (total1, total2) = BothParts.SolveGridPaths(grid);

Console.WriteLine("Total 1 Count:" + total1);
Console.WriteLine("Total 2 Count:" + total2);
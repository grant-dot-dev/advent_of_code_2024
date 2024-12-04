namespace SolutionTools;

public class Tools
{
    public static char[,] ParseGridFromFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var gridSize = lines.Length;


        var grid = new char[gridSize, gridSize];

        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                grid[row, col] = lines[row][col];
            }
        }

        return grid;
    }
}
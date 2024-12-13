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
}
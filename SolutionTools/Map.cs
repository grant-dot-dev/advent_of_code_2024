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
}
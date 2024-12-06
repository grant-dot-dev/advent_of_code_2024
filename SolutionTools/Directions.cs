namespace SolutionTools;

public static class Directions
{
    public static Point[] WithoutDiagonals { get; } =
    [
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0),
    ];

    public static Point[] DiagonalsOnly { get; } =
    [
        (1, 1),
        (-1, 1),
        (1, -1),
        (-1, -1)
    ];

    public static Point[] WithDiagonals { get; } =
    [
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0),
        (1, 1),
        (-1, 1),
        (1, -1),
        (-1, -1)
    ];

    public static bool IsOutOfBounds(Point position, int width, int height)
    {
        return position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height;
    }
}
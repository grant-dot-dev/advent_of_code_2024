namespace SolutionTools;

public static class Directions
{
    public static (int, int) UP = (-1, 0);
    public static (int, int) DOWN = (1, 0);
    public static (int, int) LEFT = (0, -1);
    public static (int, int) RIGHT = (0, 1);

    public static Point[] WithoutDiagonals { get; } =
    [
        RIGHT,
        DOWN,
        LEFT,
        UP
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
        RIGHT,
        DOWN,
        LEFT,
        UP,
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
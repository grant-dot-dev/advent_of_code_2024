namespace SolutionTools;

public record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

    public static Point operator *(Point point, int multiple) => new Point(point.X * multiple, point.Y * multiple);

    public static implicit operator Point((int X, int Y) tuple) => new Point(tuple.X, tuple.Y);

    public int ManhattanDistance(Point b) => Math.Abs(X - b.X) + Math.Abs(Y - b.Y);
}
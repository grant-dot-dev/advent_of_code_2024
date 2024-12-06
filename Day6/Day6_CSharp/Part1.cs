namespace Day6_CSharp;

public class Part1
{
    public int Solve(string[] dataInput)
    {
        var grid = Array.ConvertAll(dataInput, row => row.ToCharArray());

        var startPosition = GuardRouteHelper.GetStartPosition(grid);

        // Always starts up
        int nextRow = -1, nextCol = 0;

        if (startPosition == null)
        {
            throw new ArgumentNullException("startPosition");
        }

        var visited = GuardRouteHelper.TraverseRoute(startPosition.Value, nextRow, nextCol, grid);

        return visited.Count;
    }
}
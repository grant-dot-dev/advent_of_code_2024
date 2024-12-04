using SolutionTools;

namespace Day4_Csharp
{
    public class Part2
    {
        private int _mapWidth;
        private int _mapHeight;
        private readonly char[,] _map = Tools.ParseGridFromFile("input1.txt");

        // Main method to solve the puzzle
        public int Solve()
        {
            _mapHeight = _map.GetLength(0);
            _mapWidth = _map.GetLength(1);

            var foundCount = 0;


            for (var x = 0; x < _mapWidth; x++)
            {
                for (var y = 0; y < _mapHeight; y++)
                {
                    var diagonalMatchCount = (from diagonalDirection in Directions.DiagonalsOnly
                        let oppositeDirection = diagonalDirection * -1
                        let startingPoint = (x, y) + oppositeDirection
                        where IsWordFoundAtStartingPoint(startingPoint, diagonalDirection, "MAS")
                        select diagonalDirection).Count();

                    if (diagonalMatchCount == 2)
                    {
                        foundCount++;
                    }
                }
            }

            return foundCount;
        }

        private bool IsWordFoundAtStartingPoint(Point startingPoint, Point direction, string word)
        {
            for (var characterIndex = 0; characterIndex < word.Length; characterIndex++)
            {
                var currentPosition = startingPoint + direction * characterIndex;

                if (IsOutOfBounds(currentPosition) ||
                    _map[currentPosition.X, currentPosition.Y] != word[characterIndex])
                {
                    return false;
                }
            }

            return true;
        }


        private bool IsOutOfBounds(Point position)
        {
            return position.X < 0 || position.X >= _mapWidth || position.Y < 0 || position.Y >= _mapHeight;
        }
    }
}
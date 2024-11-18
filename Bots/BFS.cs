using BlocksConsole.GameLogic.models;
using BlocksConsole.GUI;

namespace BlocksConsole.Bots
{
    internal class BFS
    {
        public Game? Solve(Game state)
        {
            var visited = new Dictionary<string, bool>();

            Queue<Game> queue = new Queue<Game>();

            queue.Enqueue(state);

            while (queue.Count > 0)
            {
                Game currentState = queue.Dequeue();

                if (currentState.CheckForWinning())
                    return currentState;

                visited.Add(GenerateStateKey(currentState), true);

                List<Game> possible_moves = currentState.getPossibleMoves();
                foreach (var move in possible_moves)
                {
                    if (visited.Count > 0 && visited.ContainsKey(GenerateStateKey(move)))
                    {
                        continue;
                    }

                    if (move.CheckForWinning())
                        return move;

                    queue.Enqueue(move);
                    Display display = new Display();
                    display.Board(move, false);
                    Console.WriteLine(
                        $"Exploring state at depth {queue.Count}, visited count: {visited.Count}, placed pieces: {currentState.PlacedPieces.Count}"
                    );
                }
            }
            return null;
        }
        private string GenerateStateKey(Game game)
        {
            return string.Join(
                "|",
                game.PlacedPieces.Select(p =>
                    $"{p.AbsolutePosition.X},{p.AbsolutePosition.Y},{p.Blocks.GetHashCode()}"
                )
            );
        }
    }
}

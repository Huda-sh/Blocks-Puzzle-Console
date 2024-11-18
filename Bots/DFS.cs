using BlocksConsole.GameLogic.models;
using BlocksConsole.GUI;

namespace BlocksConsole.Bots
{
    internal class DFS
    {
        public Game? Solve(Game initialState)
        {
            return SolveWithStack(initialState);
        }

        public Game? SolveWithStack(Game state)
        {
            var visited = new Dictionary<string, bool>();

            Stack<Game> stack = new Stack<Game>();

            stack.Push(state);

            while (stack.Count > 0)
            {
                Game currentState = stack.Pop();

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

                    stack.Push(move);
                    Display display = new Display();
                    display.Board(move, false);
                    Console.WriteLine(
                        $"Exploring state at depth {stack.Count}, visited count: {visited.Count}, placed pieces: {currentState.PlacedPieces.Count}"
                    );
                }
            }
            return null;
        }
        public Game? SolveRecursively(Game state)
        {
            var visited = new Dictionary<string, bool>();
            return SolveRecursivelyHelper(state, visited);
        }

        private Game? SolveRecursivelyHelper(Game currentState, Dictionary<string, bool> visited)
        {
            string stateKey = GenerateStateKey(currentState);

            if (currentState.CheckForWinning())
                return currentState;

            if (visited.ContainsKey(stateKey))
                return null;

            visited.Add(stateKey, true);

            List<Game> possibleMoves = currentState.getPossibleMoves();

            foreach (var move in possibleMoves)
            {
                if (visited.ContainsKey(GenerateStateKey(move)))
                    continue;

                Game? result = SolveRecursivelyHelper(move, visited);
                if (result != null)
                {
                    return result;
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

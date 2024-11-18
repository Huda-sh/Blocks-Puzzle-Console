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

        bool placed_piece = false;

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

                List<Game> possible_moves = getPossibleMoves(currentState);
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

        private List<Game> getPossibleMoves(Game game)
        {
            List<Game> possible_moves = new List<Game>();
            Piece piece = game.AvailablePieces[0];
            for (int x = 0; x < game.Board.Width; x++)
            {
                for (int y = 0; y < game.Board.Height; y++)
                {
                    if (game.Board.CanPlacePiece(game.AvailablePieces[0], new Position(x, y)))
                    {
                        Game state = (Game)game.Clone();
                        state.ParentState = game;
                        state.Board.PlacePiece(state.AvailablePieces[0], new Position(x, y));
                        state.AvailablePieces[0].SetAbsolutePosition(new Position(x, y));
                        state.PlacedPieces.Add(state.AvailablePieces[0]);
                        state.AvailablePieces.RemoveAt(0);
                        possible_moves.Add(state);
                    }
                }
            }
            return possible_moves;
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

using BlocksConsole.GameLogic.models;
using BlocksConsole.GUI;
using System;

namespace BlocksConsole.Bots
{
    internal class PuzzleSolver
    {
        public GameActions Actions { set; get; }

        public Stack<Game> stack { set; get; }

        public List<Game> visited_states;

        public PuzzleSolver() { }
        public PuzzleSolver(Game initial)
        {
            Actions = new GameActions(initial);
        }
        public Game Solve(Game initialState)
        {
            // Use DFS to try placing pieces until goal state is reached
            return DFSSolve(initialState, 0, GetPossiblePositions(initialState.Board), 0);
        }

        public Game DFSSolve(Game state, int currentPiceIndex, List<KeyValuePair<bool, Position>> possiblePositions, int position_index)
        {
            if (currentPiceIndex >= state.AvailablePieces.Count)
            {
                return state;
            }
            if (currentPiceIndex < 0 )
            {
                return null;
            }

            Game new_state = (Game)state.Clone();

            
            if (state.CheckAllowedCoordinates(possiblePositions[position_index].Value, currentPiceIndex))
            {
                new_state.Board.PlacePiece(state.AvailablePieces[currentPiceIndex], possiblePositions[position_index].Value);
                currentPiceIndex++;
                position_index = 0;
                var display = new Display();
                display.Board(new_state, false);
                return DFSSolve(new_state, currentPiceIndex, GetPossiblePositions(state.Board), position_index);
            }
            else
            {
                position_index++;
                if (position_index >= possiblePositions.Count)
                {
                    return null;
                }
                Game game = DFSSolve(state, currentPiceIndex, possiblePositions, position_index);
                if (game == null)
                {
                    currentPiceIndex--;
                    position_index = 0;
                    game = DFSSolve(new_state, currentPiceIndex, possiblePositions, position_index);
                }
                return game;
            }
        }

        //public List<Game> getChildren(Game game)
        //{
        //    List<Game> children = new List<Game>();
        //    // get the all the posible moves for the piece
        //    for (int i = 0; i < GetPossiblePositions(); i++)
        //    {
        //        children.Add(add_state);
        //    }
        //}

        //public Game dfs(state)
        //{
        //    if(checke all pieces have been placed)
        //       // return game

        //      var neigbors = getChildren(state);
        //    if
        //    foreach neigbor
        //        if (niegbor is visited)
        //                continuew;
        //}

        //private Game DepthFirstSearch()
        //{
        //    // Check if the current game state is a winning state


        //    // Iterate over each piece and each potential position
        //    for (int i = 0; i < Actions.States_Stack.Peek().AvailablePieces.Count; i++)
        //    {
        //        Piece piece = Actions.States_Stack.Peek().AvailablePieces[i];
        //        Game next_state = (Game)Actions.States_Stack.Peek().Clone();

        //        // Attempt to place the piece at every possible position on the board
        //        foreach (Position position in GetPossiblePositions(Actions.States_Stack.Peek().Board))
        //        {
        //            // Clone the game state to maintain immutability in DFS
        //            Game newGame = (Game)Actions.States_Stack.Peek().Clone();
        //            if (next_state.CheckAllowedCoordinates(position, i))
        //            {
        //                next_state.ParentState = (Game)Actions.States_Stack.Peek();
        //                next_state.TryPlaceCurrentPiece(position, Actions.CurrentPieceIndex);
        //                Actions.Current_Position = position;
        //                Actions.PlacePiece(next_state);
        //            }
        //        }
        //    }
        //    if (Actions.States_Stack.Peek().CheckForWinning())
        //    {
        //        return Actions.States_Stack.Peek();
        //    }

        //    // If no solution was found from this path, return null
        //    return null;
        //}

        // Generate all possible positions within the board dimensions
        private List<KeyValuePair<bool, Position>> GetPossiblePositions(GameBoard board)
        {
            List<KeyValuePair<bool, Position>> possible_positions = new List<KeyValuePair<bool, Position>>();
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    possible_positions.Add(new KeyValuePair<bool, Position>( false, new Position(x, y)));
                }
            }
            return possible_positions;
        }
    }
}


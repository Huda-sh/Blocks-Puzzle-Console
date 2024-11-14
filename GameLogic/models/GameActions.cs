using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksConsole.GUI;

namespace BlocksConsole.GameLogic.models
{
    internal class GameActions
    {
        public Stack<Game> States_Stack { get; set; }
        public Position Current_Position { get; set; }
        public int CurrentPieceIndex { get; set; }

        public GameActions(Game Initial_State)
        {
            States_Stack = new Stack<Game>();
            CurrentPieceIndex = 0;
            States_Stack.Push(Initial_State);
        }
        public bool SelectNextPiece(Game game)
        {
            if (game.AvailablePieces.Count == 0)
                return false;
            CurrentPieceIndex = (CurrentPieceIndex + 1) % game.AvailablePieces.Count;
            return true;
        }

        public void PlacePiece(Game state)
        {
            States_Stack.Push(state);
        }

        public void UndoPlacePiece()
        {
            States_Stack.Pop();
            Game game = States_Stack.Peek();
            var selected = game.AvailablePieces[CurrentPieceIndex];
            game.Board.RemovePiece(selected, Current_Position);
        }
    }
}

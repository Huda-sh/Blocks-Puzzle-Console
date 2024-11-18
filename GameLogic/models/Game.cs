using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksConsole.GUI;

namespace BlocksConsole.GameLogic.models
{
    internal class Game : ICloneable
    {
        public Game ParentState { get; set; }
        public GameBoard Board { get; set; }
        public List<Piece> AvailablePieces { get; set; }
        public List<Piece> PlacedPieces { get; set; }
        public Game() { }
        public Game(GameBoard board, List<Piece> pieces)
        {
            Board = board;
            AvailablePieces = new List<Piece>(pieces);
            PlacedPieces = new List<Piece>();
        }

        public bool CheckAllowedCoordinates(Position position, int index)
        {
            if (AvailablePieces.Count == 0)
            {
                return false;
            }
            Piece piece = AvailablePieces[index];
            return CheckAllowedCoordinates(position, piece);
        }

        public bool CheckAllowedCoordinates(Position position, Piece piece)
        {
            return ParentState == null
                ? Board.CanPlacePiece(piece, position)
                : ParentState.Board.CanPlacePiece(piece, position);
        }

        public bool TryPlaceCurrentPiece(Position position, int piece_index)
        {
            if (AvailablePieces.Count == 0)
            {
                return false;
            }
            Piece currentPiece = AvailablePieces[piece_index];
            if (
                ParentState == null
                    ? Board.CanPlacePiece(currentPiece, position)
                    : ParentState.Board.CanPlacePiece(currentPiece, position)
            )
            {
                Board.PlacePiece(currentPiece, position);
                currentPiece.SetAbsolutePosition(position);

                PlacedPieces.Add(currentPiece);
                AvailablePieces.RemoveAt(piece_index);
                if (AvailablePieces.Count > 0)
                {
                    piece_index %= AvailablePieces.Count;
                }
                else
                {
                    piece_index = 0;
                }
                return true;
            }
            return false;
        }

        public bool CheckForWinning()
        {
            return AvailablePieces.Count == 0;
        }

        public object Clone()
        {
            Game cloned_game = new Game();
            cloned_game.Board = (GameBoard) this.Board.Clone();
            cloned_game.AvailablePieces = this.AvailablePieces.ConvertAll<Piece>(piece => (Piece)piece.Clone());
            
            cloned_game.PlacedPieces = this.PlacedPieces.ConvertAll<Piece>(piece =>
                (Piece)piece.Clone()
            );
            cloned_game.ParentState = this.ParentState;
            return cloned_game;
        }
        public override int GetHashCode()
        {
            int PlacedPiecesHash = 17;
            foreach (var piece in PlacedPieces)
            {
                PlacedPiecesHash += 17 * piece.GetHashCode();
            }

            //int available_pieces_hash = 17;
            //foreach (var piece in PlacedPieces)
            //{
            //    available_pieces_hash += 17 * piece.GetHashCode();
            //}
            //int BoardHash = Board.GetHashCode() * 17;
            return PlacedPiecesHash;
            //return PlacedPiecesHash + available_pieces_hash + BoardHash;
        }
    }
}

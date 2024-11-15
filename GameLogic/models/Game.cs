﻿using System;
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
            Game cloned_game = new Game(
                Board = (GameBoard)this.Board.Clone(),
                this.AvailablePieces.ConvertAll<Piece>(piece => (Piece)piece.Clone())
            );
            cloned_game.PlacedPieces = this.PlacedPieces.ConvertAll<Piece>(piece =>
                (Piece)piece.Clone()
            );
            cloned_game.ParentState = this.ParentState;
            return cloned_game;
        }
    }
}

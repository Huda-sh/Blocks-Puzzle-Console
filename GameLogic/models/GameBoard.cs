using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksConsole.GameLogic.models
{
    internal class GameBoard : ICloneable
    {
        public GridCell[,] _Grid;
        public int Width { get; }
        public int Height { get; }

        public GridCell GetCell(Position pos)
        {
            return _Grid[pos.X, pos.Y];
        }

        public GameBoard(int width, int height, int[,] initialShape)
        {
            Width = width;
            Height = height;
            _Grid = new GridCell[height, width];
            Initialize(initialShape);
        }

        public GameBoard(int width, int height, GridCell[,] initialShape)
        {
            Width = width;
            Height = height;
            _Grid = initialShape;
        }

        private void Initialize(int[,] initialShape)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _Grid[y, x] = initialShape[y, x] == 2 ? GridCell.Obstacle : GridCell.Empty;
                }
            }
        }

        public bool IsPositionValid(Position pos)
        {
            if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height)
            {
                return false;
            }

            return _Grid[pos.X, pos.Y] == GridCell.Empty;
        }

        public bool CanPlacePiece(Piece piece, Position origin)
        {
            foreach (var pos in piece.GetAbsolutePositions(origin))
            {
                if (!IsPositionValid(pos))
                {
                    return false;
                }
            }
            return true;
        }

        public void PlacePiece(Piece piece, Position origin)
        {
            foreach (var pos in piece.GetAbsolutePositions(origin))
            {
                _Grid[pos.X, pos.Y] = GridCell.Occupied;
            }
        }

        public void RemovePiece(Piece piece, Position origin)
        {
            foreach (var pos in piece.GetAbsolutePositions(origin))
            {
                _Grid[pos.X, pos.Y] = GridCell.Empty;
            }
        }

        public object Clone()
        {
            GridCell[,] grid = new GridCell[Height, Width];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    grid[i, j] = _Grid[i, j];
                }
            }
            return new GameBoard(this.Width, this.Height, initialShape: grid);
        }

        public override int GetHashCode()
        {
            int grid_hash = 7;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    grid_hash +=  7 * (int) _Grid[i, j];
                }
            }

            return grid_hash + Width * 7 + Height * 7;
        }
    }
}

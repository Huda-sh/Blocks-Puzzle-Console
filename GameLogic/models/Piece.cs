using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;

namespace BlocksConsole.GameLogic.models
{
    internal class Piece : ICloneable
    {
        public List<Block> Blocks { get; }
        public Position AbsolutePosition{ get; set; }

        public Piece(List<Position> relativePositions)
        {
            Blocks = new List<Block>();
            foreach (var pos in relativePositions)
            {
                Blocks.Add(new Block(pos));
            }
        }

        public Piece(List<Block> blocks)
        {
            this.Blocks = blocks;
        }

        public void SetAbsolutePosition(Position origin)
        {
            AbsolutePosition = origin;
        }

        public List<Position> GetAbsolutePositions(Position origin)
        {
            List<Position> absolutePositions = new List<Position>();
            foreach (var block in Blocks)
            {
                absolutePositions.Add(block.GetAbsolutePosition(origin));
            }
            return absolutePositions;
        }

        public object Clone()
        {
            Piece piece = new Piece(Blocks.ConvertAll<Block>(blc => (Block)blc.Clone()));
            if (AbsolutePosition != null )
            {
                piece.AbsolutePosition = AbsolutePosition;
            }
            return piece;
        }
    }
}

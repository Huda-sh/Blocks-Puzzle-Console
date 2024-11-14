using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlocksConsole.GameLogic.Levels
{
    internal class GameData
    {
        public FieldData Field;
        public List<PieceData> Pieces;
    }

    internal class FieldData
    {
        public int width;
        public int height;
        public List<List<int>> Shape;
    }

    internal class PieceData
    {
        public List<BlockData> Blocks;
    }

    [JsonConverter(typeof(BlockDataJsonConverter))]
    public class BlockData
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BlockData(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

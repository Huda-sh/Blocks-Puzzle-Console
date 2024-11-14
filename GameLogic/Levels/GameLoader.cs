using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlocksConsole.GameLogic.models;
using Newtonsoft.Json;

namespace BlocksConsole.GameLogic.Levels
{
    internal class GameLoader
    {
        public Game ImportGame(string file)
        {
            string json = File.ReadAllText(file);

            GameData gameData = JsonConvert.DeserializeObject<GameData>(json)!;

            int[,] initial_shape = new int[gameData.Field.width, gameData.Field.height];
            for (int i = 0; i < gameData.Field.Shape.Count; i++)
            {
                for (int j = 0; j < gameData.Field.Shape.Count; j++)
                {
                    initial_shape[j, i] = gameData.Field.Shape[j][i];
                }
            }

            GameBoard board = new GameBoard(
                gameData.Field.width,
                gameData.Field.height,
                initial_shape
            );

            List<Piece> pieces = new List<Piece>();
            foreach (var pic in gameData.Pieces)
            {
                List<Position> blocks = new List<Position>();
                foreach (var blc in pic.Blocks)
                {
                    Position pos = new Position(blc.X, blc.Y);
                    blocks.Add(pos);
                }
                Piece piece = new Piece(blocks);
                pieces.Add(piece);
            }
            return new Game(board, pieces);
        }
    }
}

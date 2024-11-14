using BlocksConsole.GameLogic;
using BlocksConsole.GameLogic.models;
using Spectre.Console;

namespace BlocksConsole.GUI
{
    internal class Display
    {
        public Display() { }
        private static readonly List<ConsoleColor> PieceColors = new List<ConsoleColor>
    {
        ConsoleColor.Red,
        ConsoleColor.Green,
        ConsoleColor.Blue,
        ConsoleColor.Cyan,
        ConsoleColor.Magenta,
        ConsoleColor.Yellow,
        ConsoleColor.DarkRed,
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkMagenta
    };
        public void Board(Game _game, bool is_initial)
        {
            Write("                                                 ");
            for (int i = 0; i < _game.Board.Width; i++)
            {
                Write(i.ToString() + " ", ConsoleColor.DarkGreen);
            }
            Console.WriteLine();
            for (int i = 0; i < _game.Board.Height; i++)
            {
                Write("                                               " + i.ToString() + " ", ConsoleColor.DarkGreen);
                for (int j = 0; j < _game.Board.Width; j++)
                {
                    Position pos = new Position(j, i);
                    switch (_game.Board.GetCell(new Position(j, i)))
                    {
                        case GridCell.Empty:
                            Write(". ", ConsoleColor.Gray);
                            break;
                        case GridCell.Obstacle:
                            Write("# ", ConsoleColor.DarkBlue);
                            break;
                        case GridCell.Occupied:
                            int pieceIndex = GetPieceIndexAtPosition(_game, pos);
                            ConsoleColor color = pieceIndex >= 0 && pieceIndex < PieceColors.Count
                                ? PieceColors[pieceIndex]
                                : ConsoleColor.DarkYellow;

                            Write(is_initial ? ". " : "O ", color);
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
        private int GetPieceIndexAtPosition(Game game, Position pos)
    {
        for (int k = 0; k < game.PlacedPieces.Count; k++)
        {
            Piece piece = game.PlacedPieces[k];
            foreach (var blockPos in piece.GetAbsolutePositions(piece.AbsolutePosition))
            {
                if (blockPos.Equals(pos))
                {
                    return k; // Return the index of the piece occupying this position
                }
            }
        }
        return -1; // Return -1 if no piece occupies this position
    }

        public static void Write(
            string str,
            ConsoleColor textColor = ConsoleColor.White,
            ConsoleColor backgroundColor = ConsoleColor.Black,
            bool isNewLine = false
        )
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            if (isNewLine)
            {
                Console.WriteLine(str);
            }
            else
            {
                Console.Write(str);
            }
            Console.ResetColor();
        }

        public void Pieces(Game _game, int CurrentPieceIndex)
        {
            Console.WriteLine();
            if (_game.AvailablePieces.Count != 0)
                Write("Available Pieces:", ConsoleColor.DarkMagenta, isNewLine: true);
            // Create the layout
            int rows = _game.AvailablePieces.Count / 6;
            int remaining = _game.AvailablePieces.Count % 6;
            int len = remaining == 0 ? rows : rows + 1;
            Grid grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            Layout[] layouts = new Layout[len];
            int idx = 0;
            for (int i = 0; i < len; i++)
            {
                layouts[i] = new Layout(i.ToString());
                int col_len = ((i == (len - 1)) && remaining != 0) ? remaining : 6;
                Panel[] colss = new Panel[col_len];
                for (int j = 0; j < col_len; j++)
                {
                    Grid piece = new Grid();
                    char[,] shape = GetPieceShape(_game.AvailablePieces[idx]);
                    piece.AddColumn();
                    for (int k = 0; k < shape.GetLength(0); k++)
                    {
                        string str = "";
                        for (int h = 0; h < shape.GetLength(1); h++)
                        {
                            str += shape[k, h] + " ";
                        }
                        if (idx == CurrentPieceIndex)
                        {
                            piece.AddRow(new Text(str, new Style(Color.Purple)));
                        }
                        else
                        {
                            piece.AddRow(new Text(str, new Style(Color.LightYellow3)));
                        }
                    }
                    Panel pnl = new Panel(
                        new Padder(
                            Align.Center(piece, VerticalAlignment.Middle),
                            new Padding(1, 1, 1, 1)
                        )
                    );
                    pnl.Border = BoxBorder.None;
                    colss[j] = pnl;
                    idx++;
                }
                grid.AddRow(colss);
            }

            // Render the layout
            AnsiConsole.Write(grid);
        }

        private static char[,] GetPieceShape(Piece piece)
        {
            // Determine the bounding box for the piece
            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var block in piece.Blocks)
            {
                if (block.RelativePosition.X < minX)
                    minX = block.RelativePosition.X;
                if (block.RelativePosition.X > maxX)
                    maxX = block.RelativePosition.X;
                if (block.RelativePosition.Y < minY)
                    minY = block.RelativePosition.Y;
                if (block.RelativePosition.Y > maxY)
                    maxY = block.RelativePosition.Y;
            }

            // Create a 2D array for displaying the piece
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            char[,] shape = new char[height, width];

            // Initialize with spaces
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    shape[y, x] = ' ';

            // Place the Blocks in the Shape array
            foreach (var block in piece.Blocks)
            {
                int x = block.RelativePosition.X - minX;
                int y = block.RelativePosition.Y - minY;
                shape[y, x] = 'O'; // Represent Blocks with 'O'
                if (block.RelativePosition.X == 0 && block.RelativePosition.Y == 0)
                {
                    shape[y, x] = '*';
                }
            }

            return shape;
        }

        public void Status(MoveState state)
        {
            switch (state)
            {
                case MoveState.Success:
                    Write(
                        "Piece placed successfully!  *\\0/* ",
                        ConsoleColor.DarkGreen,
                        isNewLine: true
                    );
                    break;
                case MoveState.Invalid_Input:
                    Write(
                        "Invalid input. -_- ",
                        backgroundColor: ConsoleColor.DarkRed,
                        isNewLine: true
                    );
                    break;
                case MoveState.New:
                    break;
                case MoveState.Changed_Piece:
                    Write(
                        "Switched to next piece. *\\0/* ",
                        ConsoleColor.DarkYellow,
                        isNewLine: true
                    );
                    break;
                case MoveState.Invalid_Position:
                    Write(
                        "Invalid position! Try again. -_- ",
                        backgroundColor: ConsoleColor.DarkRed,
                        isNewLine: true
                    );
                    break;
                case MoveState.Win:
                    Write(
                        "You are a Winner!! \\ ^ o ^ / ",
                        ConsoleColor.DarkMagenta,
                        isNewLine: true
                    );
                    Write(
                        "enter 'quit' to exit to main menu -_- ",
                        ConsoleColor.Gray,
                        isNewLine: true
                    );
                    break;
                default:
                    break;
            }
        }
    }
}

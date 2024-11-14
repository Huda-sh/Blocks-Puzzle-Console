using BlocksConsole.GameLogic.models;
using BlocksConsole.GUI;
using Spectre.Console;

namespace BlocksConsole
{
    internal class GameController
    {
        private readonly Display _display;
        private GameActions _actions;

        public GameController(Game game, Display display)
        {
            _display = display;
            _actions = new GameActions(game);
        }

        public void Start()
        {
            Console.WriteLine("Starting the Puzzle Game! :D");
            MoveState state = MoveState.New;
            while (true)
            {
                Console.Clear();
                Game next_state;
                if (state == MoveState.Changed_Piece)
                {
                    next_state = (Game)_actions.States_Stack.Peek();
                }
                next_state = (Game)_actions.States_Stack.Peek().Clone();
                Console.WriteLine();
                _display.Board(next_state.ParentState??next_state, next_state.ParentState==null);
                _display.Pieces(next_state, _actions.CurrentPieceIndex);
                _display.Status(state);

                if (state == MoveState.Win)
                {
                    return;
                }

                var rule = new Rule();
                AnsiConsole.Write(rule);

                string option = GetChoosenOption();

                switch (option)
                {
                    case PlayerChoice.NEXT_PIECE:
                        _actions.SelectNextPiece(next_state);
                        state = MoveState.Changed_Piece;
                        continue;

                    case PlayerChoice.UNDO:
                        _actions.UndoPlacePiece();
                        continue;

                    case PlayerChoice.QUIT_TO_MENU:
                        return;

                    case PlayerChoice.CHOOSE_COORDINATES:
                        Position position = GetPosition(next_state);
                        next_state.ParentState = (Game)_actions.States_Stack.Peek();
                        next_state.TryPlaceCurrentPiece(position, _actions.CurrentPieceIndex);
                        _actions.Current_Position = position;
                        _actions.PlacePiece(next_state);
                        state = MoveState.Success;
                        if (next_state.CheckForWinning())
                        {
                            state = MoveState.Win;
                        }
                        continue;
                }
            }
        }

        private Position GetPosition(Game next_state )
        {
            var number = AnsiConsole.Prompt(
                                        new TextPrompt<string>("Enter the X axis index").Validate(
                                            (string n) =>
                                            {
                                                var parts = n.Split(' ');
                                                if (
                                                    parts.Length == 2
                                                    && int.TryParse(parts[0], out int x)
                                                    && int.TryParse(parts[1], out int y)
                                                )
                                                {
                                                    if (
                                                        next_state.CheckAllowedCoordinates(
                                                            new Position(x, y), _actions.CurrentPieceIndex
                                                        )
                                                    )
                                                    {
                                                        return ValidationResult.Success();
                                                    }
                                                    else
                                                    {
                                                        return ValidationResult.Error("Invalid position! Try again. -_- ");
                                                    }
                                                }
                                                else
                                                    return ValidationResult.Error("Invalid input. -_- ");
                                            }
                                        )
                                    );


            var parts = number.Split(' ');
            int.TryParse(parts[0], out int x);
            int.TryParse(parts[1], out int y);
            Position position = new Position(x, y);
            return position;
        }

        private static string GetChoosenOption()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green] Choose an option from below:[/]")
                    .AddChoices(
                        new[]
                        {
                                    PlayerChoice.NEXT_PIECE,
                                    PlayerChoice.UNDO,
                                    PlayerChoice.CHOOSE_COORDINATES,
                                    PlayerChoice.QUIT_TO_MENU,
                        }
                    )
            );
        }
    }
}

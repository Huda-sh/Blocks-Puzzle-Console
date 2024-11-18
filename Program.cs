using BlocksConsole.Bots;
using BlocksConsole.GameLogic.Levels;
using BlocksConsole.GameLogic.models;
using BlocksConsole.GUI;
using Spectre.Console;
namespace BlocksConsole
{
    internal class Programm
    {
        //var font = FigletFont.Load("C:\\Users\\hudas\\source\\repos\\BlocksConsole\\bin\\Debug\\net8.0\\FigletFonts\\Flower Power.flf");
        public static void Main()
        {
            AnsiConsole.Write(
            new FigletText("Blocks Game")
                .Centered()
                .Color(Color.DeepPink1_1));
            Thread.Sleep(1500);

            while (true)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green] Choose an option from below:[/]")
                    .PageSize(10)
                    .AddChoices(new[] { MainMenuChoice.PLAY_GAME, MainMenuChoice.BFS, MainMenuChoice.DFS, MainMenuChoice.DFS_REC, MainMenuChoice.QUIT })
            );

                switch (option)
                {
                    case MainMenuChoice.PLAY_GAME:
                        startGame();
                        break;
                    case MainMenuChoice.BFS:

                        break;
                    case MainMenuChoice.DFS:
                        GameLoader loader = new GameLoader();
                        Game game = loader.ImportGame("field1.json");
                        Game Solution = (new DFS()).Solve(game);
                        if (Solution == null)
                        {
                            Console.WriteLine("No Solution found");
                        }
                        else
                        {
                            Console.WriteLine("Solution!!");
                            var display = new Display();
                            display.Board(Solution, false);
                        }
                        Console.ReadLine();

                        break;
                    case MainMenuChoice.DFS_REC:
                        GameLoader loader2 = new GameLoader();
                        Game game2 = loader2.ImportGame("field1.json");
                        Game Solution2 = (new DFS()).SolveRecursively(game2);
                        if (Solution2 == null)
                        {
                            Console.WriteLine("No Solution found");
                        }
                        else
                        {
                            Console.WriteLine("Solution!!");
                            var display = new Display();
                            display.Board(Solution2, false);
                        }
                        Console.ReadLine();

                        break;
                    case MainMenuChoice.QUIT:
                        Console.WriteLine("Goodbye!!");
                        return;
                }
            }

            void startGame()
            {
                GameLoader loader = new GameLoader();
                Game game = loader.ImportGame("field.json");

                var display = new Display();
                var controller = new GameController(game, display);
                controller.Start();
            }
        }
    }
}
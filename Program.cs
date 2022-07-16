// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using TicTacToe;
using TicTacToe.Models;

const int CPU_VS_CPU_TIMES = 20;
var option = UserOption.CPU1_VS_CPU2;
var results = new ConcurrentBag<string>();

while(option != UserOption.QUIT)
{
  TTTBoard endBoard;
  GameState result;

  Console.Write(@$"
0-Play {CPU_VS_CPU_TIMES} times CPU vs CPU
1-CPU 1 x CPU 2
2-Player x CPU
3-PLayer 1 x Player 2
4-Quit
Option: ");
  option = Enum.TryParse<UserOption>(Console.ReadLine(), out var optionInput) ? optionInput : UserOption.CPU1_VS_CPU2;
  Console.WriteLine();

  if(option == UserOption.PLAY_CPU_VS_CPU_X_TIMES)
  {
    Parallel.For(0,
                 CPU_VS_CPU_TIMES,
                 (i) =>
                 {
                   Console.WriteLine($"Starting the game {i + 1}");
                   endBoard = Logic.StartGame(option);
                   result = Logic.GetGameState(endBoard);

                   results.Add(result switch
                   {
                     GameState.VICTORY_X => $"Player [{TTTBoard.X_SYMBOL}] Victory",
                     GameState.VICTORY_O => $"Player [{TTTBoard.O_SYMBOL}] Victory",
                     GameState.DRAW      => "Draw",
                     _                   => "ERROR",
                   });
                 });

    var index = 0;
    foreach(var resultA in results)
      Console.WriteLine($"Result {++index}: {resultA}");
  }

  if(new[] { UserOption.CPU1_VS_CPU2, UserOption.PLAYER_VS_CPU, UserOption.PLAYER1_VS_PLAYER2 }.Contains(option))
    endBoard = Logic.StartGame(option);
  else
    break;
  result = Logic.GetGameState(endBoard);

  Console.WriteLine();
  Console.ForegroundColor = ConsoleColor.Magenta;
  Console.WriteLine(result switch
  {
    GameState.VICTORY_X => $"Player [{TTTBoard.X_SYMBOL}] Victory",
    GameState.VICTORY_O => $"Player [{TTTBoard.O_SYMBOL}] Victory",
    GameState.DRAW => "Draw",
    _ => "ERROR",
  });
  Console.ResetColor();
  Console.WriteLine();

  var exit = 0;
  while(exit != 1 && exit != 2)
  {
    Console.WriteLine("Play Again?" +
      "\n1.Yes" +
      "\n2.No");
    exit = int.TryParse(Console.ReadLine(), out var resExit) ? resExit : 0;
  }
  Console.WriteLine();
  Console.WriteLine();
  if(exit == 2)
    break;
}

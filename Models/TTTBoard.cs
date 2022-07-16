using System.Text.Json;

namespace TicTacToe.Models;
public class TTTBoard
{
  public const string O_SYMBOL = "o";
  public const string X_SYMBOL = "x";
  public const string EMPTY_SYMBOL = " ";

  public string[][] Values { get; set; } =
  {
    new[] { EMPTY_SYMBOL, EMPTY_SYMBOL, EMPTY_SYMBOL },
    new[] { EMPTY_SYMBOL, EMPTY_SYMBOL, EMPTY_SYMBOL },
    new[] { EMPTY_SYMBOL, EMPTY_SYMBOL, EMPTY_SYMBOL }
  };

  public void Print()
  {
    foreach(var line in Values)
    {
      Console.WriteLine(@" ─────────── ");
      Console.WriteLine($"| {string.Join(" | ", line)} |");
    }
    Console.WriteLine(@" ─────────── ");
  }

  public TTTBoard Copy() => JsonSerializer.Deserialize<TTTBoard>(JsonSerializer.Serialize(this))!;
}

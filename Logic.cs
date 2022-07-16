using System.Runtime.CompilerServices;
using TicTacToe.Models;

namespace TicTacToe;
public class Logic
{
    public static GameState GetGameState(TTTBoard tttBoard)
    {
        var board = tttBoard.Values;

        // Victory by: Diagonal 1
        if(board[0][0] == board[1][1] && board[1][1] == board[2][2])
            switch(board[0][0])
            {
                case TTTBoard.X_SYMBOL: return GameState.VICTORY_X;
                case TTTBoard.O_SYMBOL: return GameState.VICTORY_O;
            }

        // Victory by: Diagonal 2
        if(board[2][0] == board[1][1] && board[1][1] == board[0][2])
            switch(board[2][0])
            {
                case TTTBoard.X_SYMBOL: return GameState.VICTORY_X;
                case TTTBoard.O_SYMBOL: return GameState.VICTORY_O;
            }

        for(var i = 0; i < 3; i++)
        {
            // Victory by: Row
            if(board[i][0] == board[i][1] && board[i][1] == board[i][2])
                switch(board[i][0])
                {
                    case TTTBoard.X_SYMBOL: return GameState.VICTORY_X;
                    case TTTBoard.O_SYMBOL: return GameState.VICTORY_O;
                }
            // Victory by: Column
            if(board[0][i] == board[1][i] && board[1][i] == board[2][i])
                switch(board[0][i])
                {
                    case TTTBoard.X_SYMBOL: return GameState.VICTORY_X;
                    case TTTBoard.O_SYMBOL: return GameState.VICTORY_O;
                }
        }

        var draw = board.SelectMany(x => x).All(x => x != TTTBoard.EMPTY_SYMBOL);

        return draw ? GameState.DRAW : GameState.ONGOING;
    }

    public static (StrongBox<GameState> state, TTTBoard tttBoard) Action(TTTBoard tttBoard, string symbol)
    {
        var board = tttBoard.Values;
        var gameState = GetGameState(tttBoard);
        if(gameState != GameState.ONGOING)
            return (new(gameState), tttBoard);

        var actions = new List<(StrongBox<GameState> state, TTTBoard tttBoard)>();
        for(var i = 0; i < 3; i++)
            for(var j = 0; j < 3; j++)
                if(board[i][j] == TTTBoard.EMPTY_SYMBOL)
                {
                    var copy = tttBoard.Copy();
                    copy.Values[i][j] = symbol;
                    actions.Add((new(GameState.ONGOING), copy));
                }

        if(symbol == TTTBoard.X_SYMBOL)
        {
            foreach(var action in actions)
                action.state.Value = Action(action.tttBoard, TTTBoard.O_SYMBOL).state.Value;

            return actions.MaxBy(x => x.state.Value);
        }
        else
        {
            foreach(var action in actions)
                action.state.Value = Action(action.tttBoard, TTTBoard.X_SYMBOL).state.Value;

            return actions.MinBy(x => x.state.Value);
        }

    }

    public static TTTBoard StartGame(UserOption option)
    {
        var cpuFirst = true;
        var cpuTurn = true;
        var playerTurn = true;

        var board = new TTTBoard();

        if(option != UserOption.PLAY_CPU_VS_CPU_X_TIMES)
            board.Print();

        while(GetGameState(board) == GameState.ONGOING)
        {
            if(cpuTurn && new[] {
                    UserOption.PLAY_CPU_VS_CPU_X_TIMES,
                    UserOption.CPU1_VS_CPU2,
                    UserOption.PLAYER_VS_CPU
                }.Contains(option))
                switch(playerTurn)
                {
                    case true when cpuFirst:
                        {
                            var action = Action(board, TTTBoard.X_SYMBOL);
                            board = action.tttBoard;
                            break;
                        }
                    case false:
                        {
                            var action = Action(board, TTTBoard.O_SYMBOL);
                            board = action.tttBoard;
                            break;
                        }
                }

            if((option == UserOption.PLAYER_VS_CPU && !cpuTurn) || option == UserOption.PLAYER1_VS_PLAYER2)
                while(true)
                {
                    string symbol;
                    if(playerTurn && (option == UserOption.PLAYER1_VS_PLAYER2 || !cpuFirst))
                    {
                        Console.WriteLine($"Player [{TTTBoard.X_SYMBOL}]: ");
                        symbol = TTTBoard.X_SYMBOL;
                    }
                    else
                    {
                        Console.WriteLine($"Player [{TTTBoard.O_SYMBOL}]: ");
                        symbol = TTTBoard.O_SYMBOL;
                    }
                    Console.Write("Line: ");
                    var lin = int.TryParse(Console.ReadLine(), out var resLin) ? resLin : 0;
                    Console.Write("Column: ");
                    var col = int.TryParse(Console.ReadLine(), out var resCol) ? resCol : 0;
                    if(lin is <= 0 or >= 4 ||
                       col is <= 0 or >= 4 ||
                       board.Values[lin - 1][col + 1] != TTTBoard.EMPTY_SYMBOL) continue;
                    board.Values[lin - 1][col + 1] = symbol;
                    break;
                }

            if (option == UserOption.PLAYER_VS_CPU)
                cpuTurn = !cpuTurn;
            
            if(option != UserOption.PLAY_CPU_VS_CPU_X_TIMES)
                board.Print();

            playerTurn = !playerTurn;
        }

        return board;
    }
}


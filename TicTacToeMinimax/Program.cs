using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Program that demonstrates minimax algorithm with a tic-tac-toe game.
/// Adopted from article by Ahmad Abdolsaheb
/// https://medium.freecodecamp.org/how-to-make-your-tic-tac-toe-game-unbeatable-by-using-the-minimax-algorithm-9d690bad4b37
/// </summary>
namespace TicTacToeMinimax
{
    class TicTacToeGame
    {
        private const char HumanPlayer = 'O';
        private const char AiPlayer = 'X';

        public enum GameStatus { HumanWon, AiWon, Tie, MovesAvailable };

        // For counting calls to Minimax
        private int functionCalls = 0;

        public TicTacToeGame()
        {
            // Empty game board
            char[] board = new char[] { '0', '1', '2',
                                        '3', '4', '5',
                                        '6', '7', '8' };

            // Another board configuration to try
            /* char[] board = new char[] { 'O', '1', 'X', 
                                           'X', '4', 'X', 
                                           '6', 'O', 'O' };  */

            char turn = HumanPlayer;
            GameStatus gameStatus = GetGameStatus(board);

            // Keep looping while moves are available
            while (gameStatus == GameStatus.MovesAvailable)
            {
                // Alternate turns
                if (turn == HumanPlayer)
                    turn = AiPlayer;
                else
                    turn = HumanPlayer;

                // Find best move
                functionCalls = 0;
                var bestMove = Minimax(board, turn);

                Console.WriteLine("Best move: " + bestMove);

                board[bestMove.Position] = turn;
                DisplayBoard(board);

                gameStatus = GetGameStatus(board);
            }

            if (gameStatus == GameStatus.HumanWon)
                Console.WriteLine("Human won!");
            else if (gameStatus == GameStatus.AiWon)
                Console.WriteLine("AI won!");
            else
                Console.WriteLine("Tie game.");
        }

        private void DisplayBoard(char[] board)
        {
            Console.WriteLine(functionCalls);
            Console.WriteLine(board[0].ToString() + board[1] + board[2]);
            Console.WriteLine(board[3].ToString() + board[4] + board[5]);
            Console.WriteLine(board[6].ToString() + board[7] + board[8]);
            Console.WriteLine();
        }

        // Returns true if the player has won the game
        private bool IsWinner(char[] board, char player)
        {
            return (board[0] == player && board[1] == player && board[2] == player) ||
                   (board[3] == player && board[4] == player && board[5] == player) ||
                   (board[6] == player && board[7] == player && board[8] == player) ||
                   (board[0] == player && board[3] == player && board[6] == player) ||
                   (board[1] == player && board[4] == player && board[7] == player) ||
                   (board[2] == player && board[5] == player && board[8] == player) ||
                   (board[0] == player && board[4] == player && board[8] == player) ||
                   (board[2] == player && board[4] == player && board[6] == player);
        }

        // Returns a list of available board positions
        private List<int> AvailablePositions(char[] newBoard)
        {
            var available = new List<int>();
            for (int i = 0; i < newBoard.Length; i++)
            {
                if (newBoard[i] != 'O' && newBoard[i] != 'X')
                    available.Add(i);
            }

            return available;
        }

        // Returns move with best score, depending on who the player is
        private Move BestMove(List<Move> moves, char player)
        {           
            if (player == AiPlayer)
            {
                // Find highest score
                return moves.Max();
            }
            else
            {
                // Find lowest score
                return moves.Min();                
            }
        }

        // Returns status of the board
        private GameStatus GetGameStatus(char[] board)
        {
            if (IsWinner(board, HumanPlayer))
                return GameStatus.HumanWon;
            if (IsWinner(board, AiPlayer))
                return GameStatus.AiWon;
            if (AvailablePositions(board).Count == 0)
                return GameStatus.Tie;

            // Still moves available
            return GameStatus.MovesAvailable;
        }

        // Recursively find the best move for the given player
        private Move Minimax(char[] board, char player)
        {
            functionCalls++;
            //DisplayBoard(newBoard);
            
            if (GetGameStatus(board) == GameStatus.HumanWon)
            {
                // AI doesn't want human to win
                return new Move { Score = -10 };
            }
            else if (GetGameStatus(board) == GameStatus.AiWon)
            {
                // Good move for AI
                return new Move { Score = 10 };
            }
            else if (GetGameStatus(board) == GameStatus.Tie)
            {
                // Tie isn't good or bad
                return new Move { Score = 0 };
            }

            // Keep track of all possible moves for available positions
            var moves = new List<Move>();
            
            foreach (var pos in AvailablePositions(board))
            {
                Move move = new Move { Position = pos };
                char label = board[pos];

                // Set the empty spot to the current player
                board[pos] = player;

                // Get the score resulting from the player's opponent moving
                if (player == AiPlayer)
                    move.Score = Minimax(board, HumanPlayer).Score;
                else
                    move.Score = Minimax(board, AiPlayer).Score;

                // Reset the spot to empty
                board[pos] = label;

                moves.Add(move);
            }

            return BestMove(moves, player);
        }       

        static void Main()
        {
            new TicTacToeGame();
        }

        // Represents a move on the game board
        private class Move : IComparable
        {
            public int Score { get; set; }
            public int Position { get; set; }

            // Allow two moves to be compared
            public int CompareTo(object obj)
            {
                if (obj is Move otherMove)
                    return Score.CompareTo(otherMove.Score);
                else
                    return 0;
            }

            public override string ToString()
            {
                return String.Format("Position: {0}, Score: {1}", Position, Score);
            }
        }
    }
}

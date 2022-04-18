using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAI.Model;
using ChessAI.Model.util;

namespace ChessAI
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {


            GameState game = new GameState();
            //ChessNotation chessNotation = new ChessNotation();
            //Console.WriteLine(chessNotation.ToString());
            Console.WriteLine(game.ToString());
            string? userInput;
            while (true)
            {
                string playerToMove = game.whiteToPlay? "White" : "Black";
                Console.WriteLine($"Turn: {playerToMove}");
                var moves = game.GetAllMoves();
                Console.WriteLine(string.Join(", ", moves));
                userInput = Console.ReadLine();
                if (userInput == "undo")
                {
                    game.Undo();
                    Console.WriteLine(game.ToString());
                    continue;
                }
                Move? userMove = Move.ToMove(userInput, game.board);
                //Console.WriteLine(string.Join(", ", moves));
                if (userMove != null && moves.Contains(userMove))
                {

                    game.MakeMove(userMove);
                }
                else
                {
                    Console.WriteLine("invalid syntax or illegal move");
                    continue;
                }
                Console.WriteLine(game.ToString());
                game.LogMoveHistory();
                Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(userMove)}");
                Console.WriteLine(string.Join("\n", game.pieces));

            }








            //var move = new Move(Tuple.Create(1, 0), Tuple.Create(2, 0), game.board);
            //game.MakeMove(move);
            //Console.WriteLine(game.ToString());
            //Console.WriteLine(ChessNotation.GetChessNotation(move));
            //var app = new App();
            //app.InitializeComponent();
            //Console.WriteLine("Hajba Zallou9");
            //app.Run();
            //Console.WriteLine("Hajba Zallou9");
        }
    }
}

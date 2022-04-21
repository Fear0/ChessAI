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

            //Console.WriteLine(Tuple.Create(-1,0).Equals(new Tuple<int,int>(-1,0)));
            GameState game = new GameState();
            //ChessNotation chessNotation = new ChessNotation();
            //Console.WriteLine(chessNotation.ToString());
            Console.WriteLine(game.ToString());
            string? userInput;
            while (true)
            {
                string playerToMove = game.whiteToPlay ? "White" : "Black";

                //Console.Write("Checks or Pins: ");
                //var checkInfo = game.CheckForPinsAndChecks();

              

                Console.WriteLine($"Turn: {playerToMove}");
                var moves = game.GetValidMoves();
                Console.WriteLine(string.Join(", ", moves));


                //Console.Write("Pins: ");
                //Console.WriteLine(String.Join(", ", game.pins)); // log pins
                //Console.Write("Checks: ");
                //Console.WriteLine(String.Join(", ", game.checks)); // log checks


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
                Console.WriteLine("\n");

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

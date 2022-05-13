using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChessAI.Model;
using ChessAI.Model.AI;
using ChessAI.Model.util;
using ChessAI.Model.util.Pieces;

namespace ChessAI
{
    internal class Program
    {


        [STAThread]
        public static void Main(string[] args)
        {
            // d2d4, d7d5, e2e4, d5e4, b1c3, g8f6, c1f4, g7g6, f4e5, f8g7, f1b5, b8c6, d4d5, a7a6, d5c6,
            //RunVsSelf();
            //return;
            //EngineVsEngine(EvaluationFunctions.BoardEvaluationFunction, 3);
            RunVsEngine("negamax",EvaluationFunctions.BoardEvaluationFunction, 4);
            //RandomVsRandom();
            return;
            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            NegaMaxAlphaBetaAgent agent = new NegaMaxAlphaBetaAgent(EvaluationFunctions.BoardEvaluationFunction, 3);
            bool player_one = false; // if human is playing white, then this will be true, else false
            bool player_two = true; // if human is playing black, this will be true, else false
            while (true)
            {
                bool humanTurn = (game.whiteToPlay && player_one) || (!game.whiteToPlay && player_two);
                string playerToMove = game.whiteToPlay ? "White" : "Black";
                if (humanTurn)
                {
                    var moves = game.GetValidMoves();
                    if (game.checkmate)
                    {
                        if (game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Checkmate, Draw");
                        break;

                    }
                    Console.WriteLine(string.Join(", ", moves));
                    //Console.WriteLine(string.Join(", ", moves));
                    string userInput = Console.ReadLine();
                    if (userInput == "undo")
                    {
                        game.Undo();
                        Console.WriteLine(string.Join("\n", game.pieces));
                        Console.WriteLine(game.ToString());
                        continue;
                    }
                    Move? userMove = GameState.FindMoveByUserInput(moves, userInput);
                    //Console.WriteLine(string.Join(", ", moves));
                    if (userMove != null && moves.Contains(userMove))
                    {

                        game.MakeMove(userMove);

                        Console.WriteLine(string.Join("\n", game.pieces));
                    }
                    else
                    {
                        Console.WriteLine("invalid syntax or illegal move");
                        continue;
                    }
                    Console.WriteLine(game.ToString());
                    game.LogMoveHistory();
                    Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(userMove)}");
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine("\n");
                }
                else
                {
                    var bestMove = agent.GetAction(game, game.whiteToPlay);
                    game.MakeMove(bestMove);
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine(game.ToString());
                    Console.WriteLine(bestMove);
                    game.LogMoveHistory();
                    if (game.checkmate)
                    {
                        if (game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Checkmate, Draw");
                        break;

                    }
                    Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(bestMove)}");
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine("\n");
                
            }
            }
            return;


            RunVsSelf();
            //RunVsEngine("negamax",EvaluationFunctions.BoardEvaluationFunction, 5);
            //EngineVsEngine(EvaluationFunctions.BoardEvaluationFunction,3);
            //return;
            //GameState game = new GameState();

            //var copygame = game.DeepCopy();
            //game.pieces.Add(new Queen(0, 0, PieceColor.White)
            //{
            //    status = "captured"
            //}) ;
            //var moves = game.GetValidMoves();
            //game.MakeMove(moves[0]);

            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            //Console.WriteLine(string.Join("\n", game.pieces));

            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(copygame.pieces)));
            //Console.WriteLine(string.Join("\n", copygame.pieces));
            //return;
            ////GameState game = new GameState();
            Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            NegaMaxAlphaBetaAgent negamaxAgent = new NegaMaxAlphaBetaAgent(EvaluationFunctions.BoardEvaluationFunction, 3);
            //var bestMove = negamaxAgent.GetAction(game, true);
            //Console.WriteLine(bestMove);
            //game.MakeMove(bestMove);
            //Console.WriteLine(string.Join("\n", game.pieces));

            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));

            while (true)
            {
                string playerToMove = game.whiteToPlay ? "White" : "Black";



                Console.WriteLine($"Turn: {playerToMove}");
                if (game.whiteToPlay)
                {
                    var bestMove = negamaxAgent.GetAction(game, true);
                    Console.WriteLine(bestMove);
                    game.MakeMove(bestMove);
                    Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
                    if (game.checkmate)
                    {
                        if (!game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Draw by stalemate");
                        break;

                    }
                }
                else
                {
                    var bestMove = negamaxAgent.GetAction(game, false);
                    Console.WriteLine(bestMove);
                    game.MakeMove(bestMove);
                    Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
                    if (game.checkmate)
                    {
                        if (!game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Draw by stalemate");
                        break;

                    }

                }
                //System.Threading.Thread.Sleep(3000);

            }

            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            //var bestMove = negamaxAgent.GetAction(game, true);
            //Console.WriteLine(bestMove);
            //game.MakeMove(bestMove);
            //Console.WriteLine(string.Join("\n", game.pieces));

            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            //bestMove = negamaxAgent.GetAction(game, false);
            //Console.WriteLine(bestMove);
            //game.MakeMove(bestMove);
            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
            //bestMove = negamaxAgent.GetAction(game, true);
            //Console.WriteLine(bestMove);





            //RunVsEngine(EvaluationFunctions.BoardEvaluationFunction,4);
            //RunVsEngine();
            //RandomVsRandom();
            //EngineVsEngine();

            //GameState game = new GameState();
            //Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));    
            /*GameState game = new GameState();
            MinimaxAgent minimaxAgent = new MinimaxAgent(EvaluationFunctions.BoardEvaluationFunction, 4);
            Console.WriteLine(minimaxAgent.GetAction(game));*/


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

        public static void EngineVsEngine(Func<GameState, double> evalFn, int depth)
        {

            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            NegaMaxAlphaBetaAgent minimaxAgent = new NegaMaxAlphaBetaAgent(evalFn, depth);
            bool isMaximizingPlayer = true;
            while (true)
            {
                string playerToMove = game.whiteToPlay ? "White" : "Black";

                //Console.Write("Checks or Pins: ");
                //var checkInfo = game.CheckForPinsAndChecks();



                Console.WriteLine($"Turn: {playerToMove}");
                var action = minimaxAgent.GetAction(game, isMaximizingPlayer);

                if (action != null)
                {
                    Console.WriteLine(action.startPosition + "," + action.endPosition + ", " + action.sourcePieceId + "," + action.targetPieceId + " | " + action);
                    game.MakeMove(action);
                    Console.WriteLine(string.Join("\n", game.pieces));
                    //Console.WriteLine(game.ToString());
                    Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
                }
                if (game.checkmate)
                {
                    if (game.whiteToPlay)
                    {
                        Console.WriteLine("Checkmate, Black wins");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Checkmate, White wins");
                        break;

                    }
                }
                if (game.stalemate)
                {
                    Console.WriteLine("Draw");
                    break;

                }
                //Console.WriteLine("No move to play");
                //break;

                isMaximizingPlayer = !isMaximizingPlayer;
                Thread.Sleep(1000); 
            }

        }


        public static void RandomVsRandom()
        {
            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            RandomAgent randomAgent = new RandomAgent();
            while (true)
            {
                string playerToMove = game.whiteToPlay ? "White" : "Black";

                //Console.Write("Checks or Pins: ");
                //var checkInfo = game.CheckForPinsAndChecks();



                Console.WriteLine($"Turn: {playerToMove}");
                var action = randomAgent.GetAction(game);

                if (action != null)
                {
                    Console.WriteLine(action.startPosition + "," + action.endPosition + ", " + action.sourcePieceId + "," + action.targetPieceId + " | " + action);
                    game.MakeMove(action);
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    //Console.WriteLine("Before Comparison:");
                    //Console.WriteLine(game.ToString());
                    Console.WriteLine(Helpers.BoardToString(Helpers.MapPiecesToBoard(game.pieces)));
                    //Console.WriteLine("After Comparison:");
                }
                if (game.checkmate)
                {
                    if (game.whiteToPlay)
                    {
                        Console.WriteLine("Checkmate, Black wins");
                    }
                    else
                    {
                        Console.WriteLine("Checkmate, White wins");

                    }
                    break;
                }
                if (game.IsDraw())
                {
                    if (game.stalemate)
                    {
                        Console.WriteLine("Draw by stalemate");
                    }
                    else
                    {
                        Console.WriteLine("Draw by insuficient material");
                    }
                    break;

                }
            }

            //System.Threading.Thread.Sleep(1000);
        }



        public static void RunVsSelf()
        {
            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            Console.WriteLine(string.Join("\n", game.pieces));
            string? userInput;
            while (true)
            {
                string playerToMove = game.whiteToPlay ? "White" : "Black";

                //Console.Write("Checks or Pins: ");
                //var checkInfo = game.CheckForPinsAndChecks();



                Console.WriteLine($"Turn: {playerToMove}");
                var moves = game.GetValidMoves();

                if (game.checkmate)
                {
                    if (game.whiteToPlay)
                    {
                        Console.WriteLine("Checkmate, Black wins");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Checkmate, White wins");
                        break;

                    }
                }
                if (game.stalemate)
                {
                    Console.WriteLine("Checkmate, Draw");
                    break;

                }

                Console.WriteLine(string.Join(", ", moves));


                ////Console.Write("Pins: ");
                ////Console.WriteLine(String.Join(", ", game.pins)); // log pins
                ////Console.Write("Checks: ");
                ////Console.WriteLine(String.Join(", ", game.checks)); // log checks


                userInput = Console.ReadLine();
                if (userInput == "undo")
                {
                    game.Undo();
                    Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine(game.ToString());
                    continue;
                }
                Move? userMove = GameState.FindMoveByUserInput(moves, userInput);
                //Console.WriteLine(string.Join(", ", moves));
                if (userMove != null)
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

        }

        public static void RunVsEngine(string agentType, Func<GameState, double> evalFn, int depth)
        {
            GameState game = new GameState();
            Agent agent = null;
            if (agentType == "minimax")
            {
                agent = new MinimaxAgent(evalFn, depth);
            }
            else if (agentType == "negamax")
            {
                agent = new NegaMaxAlphaBetaAgent(evalFn, depth);
            }
            //MinimaxAgent minimaxAgent = new MinimaxAgent(evalFn, depth);
            //Console.WriteLine(minimaxAgent.GetAction(game));



            Console.WriteLine(game.ToString());
            bool humanPlayer = game.whiteToPlay;
            string? userInput;
            bool whiteTurn = true;
            while (true)
            {

                if (whiteTurn)
                {
                    var moves = game.GetValidMoves();
                    if (game.checkmate)
                    {
                        if (game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Checkmate, Draw");
                        break;

                    }

                    Console.WriteLine(string.Join(", ", moves));
                    userInput = Console.ReadLine();
                    if (userInput == "undo")
                    {
                        game.Undo();
                        Console.WriteLine(string.Join("\n", game.pieces));
                        Console.WriteLine(game.ToString());
                        continue;
                    }
                    Move? userMove = GameState.FindMoveByUserInput(moves, userInput);
                    Console.WriteLine(string.Join(", ", moves));
                    if (userMove != null && moves.Contains(userMove))
                    {

                        game.MakeMove(userMove);

                        Console.WriteLine(string.Join("\n", game.pieces));
                    }
                    else
                    {
                        Console.WriteLine("invalid syntax or illegal move");
                        continue;
                    }
                    Console.WriteLine(game.ToString());
                    game.LogMoveHistory();
                    Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(userMove)}");
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine("\n");
                }
                else //engine move, here black
                {
                    var bestMove = agent.GetAction(game, false);
                    game.MakeMove(bestMove);
                    Console.WriteLine(game.ToString());
                    Console.WriteLine(bestMove);
                    game.LogMoveHistory();
                    if (game.checkmate)
                    {
                        if (game.whiteToPlay)
                        {
                            Console.WriteLine("Checkmate, Black wins");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Checkmate, White wins");
                            break;

                        }
                    }
                    if (game.stalemate)
                    {
                        Console.WriteLine("Checkmate, Draw");
                        break;

                    }
                    Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(bestMove)}");
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine("\n");
                }


                whiteTurn = !whiteTurn;
            }
        }


    }
}

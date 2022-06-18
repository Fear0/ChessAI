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
    /// <summary>
    /// This class was used for debugging
    /// </summary>
    internal class Program
    {


        [STAThread]
        public static void Main(string[] args)
        {

            RunVsEngine("negamax",EvaluationType2.BoardEvaluationFunction, 4);


            //b1c3, g8f6, e2e3, g7g6, g1f3, f8g7, f1c4, O-O, e1f1,
            //RunVsEngine("negamax", EvaluationType1.BoardEvaluationFunction, 4);



            //var app = new App();
            //app.InitializeComponent();
            
            //app.Run();
        
        }

        public static void EngineVsEngine(Func<GameState, double> evalFn, int depth)
        {

            GameState game = new GameState();
            Console.WriteLine(game.ToString());
            MinimaxAlphaBetaAgent minimaxAgent = new MinimaxAlphaBetaAgent(evalFn, depth);
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
                agent = new MinimaxAlphaBetaAgent(evalFn, depth);
            }
            else if (agentType == "negamax")
            {
                agent = new NegaMaxAlphaBetaAgent(evalFn, depth);
            }
            else if (agentType == "expectimax")
            {
                agent = new ExpectimaxAgent(evalFn, depth);
            }
            //MinimaxAgent minimaxAgent = new MinimaxAgent(evalFn, depth);
            //Console.WriteLine(minimaxAgent.GetAction(game));



            Console.WriteLine(game.ToString());
            bool humanPlayer = game.whiteToPlay;
            string? userInput;
            bool engineWhite = false;
            while (true)
            {

                if (!engineWhite)
                {
                    Console.WriteLine(game.whiteToPlay ? "White Turn: " : "Black Turn: ");
                    var moves = game.GetValidMoves();
                    //Console.WriteLine(game.currentCastleRights);
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
                        Console.WriteLine("Stalemate, Draw");
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
                    Console.WriteLine(game.whiteToPlay ? "White Turn: " : "Black Turn: ");
                    Console.WriteLine(string.Join(", ", game.GetValidMoves()));
                    //Console.WriteLine("Before: " + game.currentCastleRights);
                    var bestMove = agent.GetAction(game, true);
                    //Console.WriteLine(agent.numberPositions);
                    game.MakeMove(bestMove);
                    Console.WriteLine(game.ToString());
                    Console.WriteLine("After: " + game.currentCastleRights);

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
                        Console.WriteLine("Stalemate, Draw");
                        break;

                    }
                    Console.WriteLine($"Played move: {ChessNotation.GetChessNotation(bestMove)}");
                    //Console.WriteLine(string.Join("\n", game.pieces));
                    Console.WriteLine("\n");
                }


                engineWhite = !engineWhite;
            }
        }


    }
}

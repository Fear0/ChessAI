using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class NegaMaxAlphaBetaAgent : MultiAgentSearchAgent
    {


        private Move nextMove = null;
        public NegaMaxAlphaBetaAgent(Func<GameState, double> evaluationFunction, int depth) : base(evaluationFunction, depth)
        {

        }


        public double NegaMaxAlphaBeta(GameState gameState, int depth, double alpha, double beta, int turn_multiplier)
        {
            if (depth == 0 || gameState.GameOver())
            {
                return turn_multiplier * evaluationFunction(gameState);
            }

            double maxScore = (double)-EvaluationType1.CHECKMATE;
            var legalMoves = gameState.GetValidMoves();

            //TODO change order of moves to leverage the AlphaBeta algorithm

            foreach (var move in legalMoves)
            {

                //var copy = gameState.DeepCopy();
                //Console.WriteLine("Before Move: " + move + ", " + gameState.currentCastleRights);
                gameState.MakeMove(move);
                double score = -1 * NegaMaxAlphaBeta(gameState, depth - 1, -beta, -alpha, -turn_multiplier);
                //gameState = JsonSerializer.Deserialize<GameState>(obj);

                if (score > maxScore)
                {
                    maxScore = score;
                    if (depth == this.DEPTH)
                    {
                        nextMove = move;

                    }
                }
                gameState.Undo();
                //Console.WriteLine("After Undo: " + move + ", " +  gameState.currentCastleRights);
                if (maxScore > alpha)
                {
                    alpha = (double)maxScore;
                }
                if (alpha >= beta)
                {
                    break;
                }
            }
            return maxScore;
        }
        public override Move GetAction(GameState gameState, bool isMaximizingPlayer)
        {

            //Move move = null;
            nextMove = null;
            var bestScore = NegaMaxAlphaBeta(gameState, this.DEPTH, -EvaluationType1.CHECKMATE, EvaluationType1.CHECKMATE, gameState.whiteToPlay ? 1 : -1);
            //Console.WriteLine($"MaxScore: {bestScore}");
            Console.WriteLine(nextMove);
            return nextMove;

            //List<double> moveValueList = new List<double>();
            //foreach (var move in gameState.GetValidMoves())
            //{
            //    gameState.MakeMove(move);
            //    moveValueList.Add()

            //}
            //throw new NotImplementedException();
        }
    }
}

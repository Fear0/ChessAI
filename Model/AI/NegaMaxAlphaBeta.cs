using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class NegaMaxAlphaBetaAgent: MultiAgentSearchAgent
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

            double maxScore = (double) -EvaluationFunctions.CHECKMATE;
            var legalMoves = gameState.GetValidMoves();

            foreach (var move in legalMoves)
            {

                //var copy = gameState.DeepCopy();
                gameState.MakeMove(move);
                double score = -NegaMaxAlphaBeta(gameState, depth - 1, -beta, -alpha, -turn_multiplier);
                //gameState = JsonSerializer.Deserialize<GameState>(obj);
                gameState.Undo();

                if (score > maxScore)
                {
                    maxScore = score;
                    if (depth == this.DEPTH )
                    {
                        nextMove = move;
                      
                    }
                }
                if (maxScore > alpha)
                {
                    alpha = (double) maxScore;
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

            NegaMaxAlphaBeta(gameState, this.DEPTH, -EvaluationFunctions.CHECKMATE, EvaluationFunctions.CHECKMATE, isMaximizingPlayer ? 1 : -1);
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

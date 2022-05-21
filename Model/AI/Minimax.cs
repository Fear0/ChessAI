using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class MinimaxAlphaBetaAgent : MultiAgentSearchAgent
    {


        public MinimaxAlphaBetaAgent(Func<GameState, double> evaluationFunction, int depth = 2) : base(evaluationFunction, depth)
        {

        }


        public double Minimax(GameState state, int depth, double alpha, double beta, bool isMaximizingPlayer)
        {
            if (depth == 0)
            {
                return -evaluationFunction(state);
            }

            List<Move> possibleMoves = state.GetAllMoves();
            

            if (isMaximizingPlayer)
            {
                var bestValue = -EvaluationType2.CHECKMATE;
                foreach (var move in possibleMoves)
                {
                    state.MakeMove(move);
                    bestValue = Math.Max(bestValue, Minimax(state, depth - 1, alpha, beta, !isMaximizingPlayer));
                    state.Undo();
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                    {
                        return bestValue;
                    }
                }
                //Console.WriteLine(bestValue);
                return bestValue;
            }
            else
            {
                var bestValue = EvaluationType2.CHECKMATE;
                foreach (var move in possibleMoves)
                {
                    state.MakeMove(move);
                    bestValue = Math.Min(bestValue, Minimax(state, depth - 1, alpha, beta, !isMaximizingPlayer));
                    state.Undo();
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                    {
                        return bestValue;
                    }
                }
                return bestValue;
            }

        }
        public override Move GetAction(GameState gameState, bool isMaximizingPlayer)
        {

            //black


            var possibleMoves = gameState.GetValidMoves();
            double bestValue = -EvaluationType2.CHECKMATE;
            Move bestMove = null;

            foreach (var move in possibleMoves)
            {
                gameState.MakeMove(move);
                var value = Minimax(gameState, DEPTH - 1, -10000, 10000, !isMaximizingPlayer);
                gameState.Undo();
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;

                }
            }
            return bestMove;


            //throw new NotImplementedException();
        }
    }

}

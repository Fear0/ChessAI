using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class ExpectimaxAgent : MultiAgentSearchAgent
    {

        public ExpectimaxAgent(Func<GameState, double> evaluationFunction, int depth = 2) : base(evaluationFunction, depth)
        {

        }


        public double Expectimax(GameState state, int depth, bool isMaximizingPlayer)
        {
            if (depth == 0)
            {
                return -evaluationFunction(state);
            }

            List<Move> possibleMoves = state.GetAllMoves();

            

            if (isMaximizingPlayer)
            {
                //var bestValue = -EvaluationType2.CHECKMATE;

                List<double> expectimaxValues = new();
                foreach (var move in possibleMoves)
                {
                    state.MakeMove(move);
                    expectimaxValues.Add(Expectimax(state, depth - 1, !isMaximizingPlayer));
                    state.Undo();
                }
                //Console.WriteLine(bestValue);
                return expectimaxValues.Max();
            }
            else
            {
                double expectimaxValue;
                List<double> expectimaxValues = new();
                foreach (var move in possibleMoves)
                {
                    state.MakeMove(move);
                    expectimaxValues.Add(Expectimax(state, depth - 1, !isMaximizingPlayer));
                    state.Undo();
                }

                expectimaxValue = expectimaxValues.Average();
                return expectimaxValue;

            }
        }

        public override Move GetAction(GameState gameState, bool isMaximizingPlayer)
        {

            if (isMaximizingPlayer)
            {

                var possibleMoves = gameState.GetValidMoves();
                double bestValue = -EvaluationType2.CHECKMATE;
                Move bestMove = null;

                foreach (var move in possibleMoves)
                {
                    gameState.MakeMove(move);
                    var value = Expectimax(gameState, DEPTH - 1, !isMaximizingPlayer);
                    gameState.Undo();
                    if (value >= bestValue)
                    {
                        bestValue = value;
                        bestMove = move;

                    }
                }
                return bestMove;
            }

            throw new NotImplementedException();
        }
    }
}

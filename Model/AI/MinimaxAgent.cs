using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class MinimaxAgent : MultiAgentSearchAgent
    {

 
        public MinimaxAgent(Func<GameState, double> evaluationFunction, int depth = 2) : base(evaluationFunction, depth)
        {

        }



        public double minimax(GameState state, bool isMaximizingPlayer, int depth, int turn)
        {
            if (state.GameOver() || depth == 0)
            {
                return  turn * evaluationFunction(state);

            }


            //var successorStates = new List<GameState>();

            //List<decimal> minimaxvalues = new();

            var legalMoves = state.GetValidMoves();

            int nextAgent, nextDepth;

            //foreach (var move in legalMoves)
            //{
            //    successorStates.Add(state.GenerateSuccessorState(move));
            //}

            if (isMaximizingPlayer) //white is always agent 0
            {

                //nextAgent = 1;
                nextDepth = depth-1;
                double bestValue = -1000;
                //List<decimal> minimaxvalues = new();
                foreach (var move in legalMoves)
                {
                    // b d  O(b^d) O(b^(d/2))   max 

                    //var successor = state.GenerateSuccessorState(move);


                    state.MakeMove(move);
                    //Console.WriteLine("State after move before undo: \n" + state);
                    //Console.WriteLine($"Move: {move.startPosition}, {move.endPosition}, {move.pieceMoved}, {move.pieceCaptured}");
                   // Console.WriteLine(String.Join("\n", state.pieces));


                    bestValue = Math.Max(bestValue, minimax(state, !isMaximizingPlayer, nextDepth,-1));

                    state.Undo();
                    //Console.WriteLine("State after undo: \n" + state);
                    //Console.WriteLine($"Move: {move.startPosition}, {move.endPosition}, {move.pieceMoved}, {move.pieceCaptured}");
                    //Console.WriteLine(String.Join("\n", state.pieces));
                    //minimaxvalues.Add(minimax(successor, !agentIndex, nextDepth));

                }
                //Console.WriteLine(string.Join(", ", minimaxvalues));

                return bestValue;

            }
            else
            {
                //nextAgent = 0;
                nextDepth = depth - 1;
                double bestValue = 1000;
                
                //List<decimal> minimaxvalues = new();
                foreach (var move in legalMoves)
                {
                    // b d  O(b^d) O(b^(d/2))   max 

                    //var successor = state.GenerateSuccessorState(move);
                    state.MakeMove(move);
                    //Console.WriteLine("State after move before undo: \n" + state);
                    //Console.WriteLine($"Move: {move.startPosition}, {move.endPosition}, {move.pieceMoved}, {move.pieceCaptured}");
                    //Console.WriteLine(String.Join("\n", state.pieces));
                    //Console.WriteLine(successor);

                    bestValue = Math.Min(bestValue, minimax(state, !isMaximizingPlayer, nextDepth,1));

                    state.Undo();
                    //Console.WriteLine("State after undo: \n" + state);
                    //Console.WriteLine($"Move: {move.startPosition}, {move.endPosition}, {move.pieceMoved}, {move.pieceCaptured}");
                    //Console.WriteLine(String.Join("\n", state.pieces));
                    //minimaxvalues.Add(minimax(successor, !agentIndex, nextDepth));

                }
                //Console.WriteLine(string.Join(", ", minimaxvalues));
                //return minimaxvalues.Min();
                return bestValue;
            }
        }
        public override Move GetAction(GameState gameState, bool isMaximizingPlayer)
        {
            //MinimaxAgent minimaxAgent = new MinimaxAgent(EvaluationFunctions.ScoreEvaluationFunction, 2);

   
            var legalMoves = gameState.GetValidMoves();
            var turn = gameState.whiteToPlay ? -1 : 1;
            //gameState.MakeMove(legalMoves[9]);
            //legalMoves = gameState.GetValidMoves();
            //Console.WriteLine(gameState);
            //List<GameState> successorStates = new List<GameState>();
            double bestValue = 1000;
            Move bestMove = null;
            foreach (var move in legalMoves)
            {
                //var state = gameState;
                gameState.MakeMove(move);
                var value = minimax(gameState, !isMaximizingPlayer, DEPTH-1,turn);
                gameState.Undo();
                if (value < bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }
            //gameState.MakeMove(bestMove);
            //Console.WriteLine(gameState);

            //List<decimal> values = new List<decimal>();
            //foreach (var state in successorStates)
            //{
            //    values.Add(this.minimax(state, false, this.depth));
            //}

            //return legalMoves.ElementAt(values.IndexOf(values.Max()));

            return bestMove;
            //throw new NotImplementedException();
        }


    }
 


}


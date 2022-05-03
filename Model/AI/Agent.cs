using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal abstract class Agent
    {
        //An agent must define a getAction method. Depending on the algorithm, the agent will define a strategy that results in a specific move for a specific state

        protected int index { get; set; }

        public Agent(int index = 0)
        {
            this.index = index;
        }
        
        public abstract Move GetAction(GameState gameState,bool isMaximizingPlayer);
    }


    internal abstract class MultiAgentSearchAgent: Agent
    {
        protected int DEPTH;
        protected Func<GameState, double> evaluationFunction = (gameState) => 0;

      
        protected MultiAgentSearchAgent(Func<GameState, double> evaluationFunction, int depth = 2)
        {
            this.index = 0;
            this.DEPTH = depth;
            this.evaluationFunction = evaluationFunction;
        }

    


    }


}

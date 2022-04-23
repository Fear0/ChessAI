using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal abstract class Agent
    {
        //An agent must define a getAction method, but may also define the following methods which will be called if they exist

        protected int index { get; set; }

        public Agent(int index = 0)
        {
            this.index = index;
        }

        public abstract Move GetAction(int index, GameState gameState);
    }


    internal abstract class MultiAgentSearchAgent: Agent
    {

    }

}

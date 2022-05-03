using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    internal class RandomAgent : Agent
    {
        public RandomAgent(int index=0) : base(index)
        {

        }

        public override Move GetAction(GameState gameState, bool isMaximizingPlayer = true)
        {
            Random r = new Random();
            var validMoves = gameState.GetValidMoves();
            if (validMoves.Count > 0)
            {
                return validMoves[(int)r.Next(validMoves.Count)];
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model
{
    internal class GameState
    {
        public string[,] board = new string[8, 8]
        {
            {"bR", "bB", "bN","bQ", "bK","bN","bB","bR"},
            { "bP","bP","bP","bP","bP","bP","bP","bP"},
            {"--","--","--","--","--","--","--","--" },
           {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
            { "wP","wP","wP","wP","wP","wP","wP","wP"},
            {"wR", "wB", "wN","wQ", "wK","wN","wB","wR"},


        };

        public bool whiteToPlay = true;
        public List<string> moveHisory = new List<string>();

        public GameState()
        {



        }

        public GameState(GameState state)
        {
            this.board = state.board;
            this.whiteToPlay = state.whiteToPlay;
            this.moveHisory = new List<string>();

        }

        public GameState GenerateSuccessorState(Move move)
        {
            return null;
        }

        public void MakeMove(Move move)
        {
            if (move is not null)
            {
                this.board[move.startPosition.Item1, move.startPosition.Item2] = "--";
                this.board[move.endPosition.Item1, move.endPosition.Item2] = move.pieceMoved;
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(0); j++)
                {
                    sb.Append(this.board[i, j] + " ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
     class Knight : Piece
    {

        public Knight(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Knight;
        }

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {
            List<Move> possibleKnightMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';

            // knights move like an L. You can rotate an L 8 different ways.

            if (row - 2 >= 0 && col + 1 <= limit)
            {
                if (board[row - 2, col + 1] == "--" || board[row - 2, col + 1][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 2, col + 1), board));
                }
            }
            if (row - 1 >= 0 && col + 2 <= limit)
            {
                if (board[row - 1, col + 2] == "--" || board[row - 1, col + 2][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 2), board));
                }
            }
            if (row - 1 >= 0 && col - 2 >= 0)
            {
                if (board[row - 1, col - 2] == "--" || board[row - 1, col - 2][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 2), board));
                }
            }
            if (row - 2 >= 0 && col - 1 >= 0)
            {
                if (board[row - 2, col - 1] == "--" || board[row - 2, col - 1][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 2, col - 1), board));
                }
            }
            if (row + 1 <= limit && col + 2 <= limit)
            {
                if (board[row + 1, col + 2] == "--" || board[row + 1, col + 2][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 2), board));
                }
            }
            if (row + 2 <= limit && col + 1 <= limit)
            {
                if (board[row + 2, col + 1] == "--" || board[row + 2, col + 1][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 2, col + 1), board));
                }
            }

            if (row + 2 <= limit && col - 1 >= 0)
            {
                if (board[row + 2, col - 1] == "--" || board[row + 2, col - 1][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 2, col - 1), board));
                }
            }
            if (row + 1 <= limit && col - 2 >= 0)
            {
                if (board[row + 1, col - 2] == "--" || board[row + 1, col - 2][0] == opponent)
                {
                    possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 2), board));
                }
            }

            return possibleKnightMoves;
        }
    }
}

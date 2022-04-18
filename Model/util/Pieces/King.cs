using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class King : Piece
    {

        public King(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.King;
        }

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {

            List<Move> possibleKingMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';

            //a king has 8 possible moves, which are the surrounding squares. (north,south,east,west,northeast,northwest,southeast,southwest)
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-1, 0), Tuple.Create(1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1), Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, 1), Tuple.Create(1, -1) };

            //north
            int i;

            foreach (var direction in directions)
            {
                i = 1;

                if (0 <= row + direction.Item1 * i && row + direction.Item1 * i <= limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
                {
                    if (board[row + direction.Item1 * i, col + direction.Item2 * i] == "--" || board[row + direction.Item1 * i, col + direction.Item2 * i][0] == opponent)
                    {
                        possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i), board));
                    }
                }

            }

            


            return possibleKingMoves;
        }
    }

}

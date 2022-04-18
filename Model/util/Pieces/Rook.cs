using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class Rook : Piece
    {

        public Rook(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Rook;
        }

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {

            List<Move> possibleRookMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';
            char fellow = gamestate.whiteToPlay ? 'w' : 'b';

            // Rooks move horizontally or vertically.
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(0, 1), Tuple.Create(0, -1), Tuple.Create(1, 0), Tuple.Create(-1, 0)};

            // Rooks move horizontally or vertically.
            int i;

            foreach (var direction in directions)
            {
                i = 1;
                if (0 <= row + direction.Item1 * i && row + direction.Item1 * i <= limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
                {

                    while (0 <= row + direction.Item1 * i && row + direction.Item1 * i <= limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
                    {
                        //check if path is blocked by ally piece 
                        if (board[row + direction.Item1 * i, col + direction.Item2 * i][0] == fellow)
                        {
                            break;
                        }
                        if (board[row + direction.Item1 * i, col + direction.Item2 * i] == "--" || board[row + direction.Item1 * i, col + direction.Item2 * i][0] == opponent)
                        {
                            possibleRookMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i), board));

                            // if capture occured, no move along the same path is valid
                            if (board[row + direction.Item1 * i, col + direction.Item2 * i][0] == opponent)
                            {
                                break;
                            }

                        }
                        i++;
                    }
                }
            }




            
            return possibleRookMoves;
        }
    }
}

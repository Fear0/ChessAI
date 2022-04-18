using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class Queen : Piece
    {
        public Queen(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Queen;
        }

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {

            PieceColor color = gamestate.whiteToPlay ? PieceColor.White : PieceColor.Black;
            // actually the color doesnt matter, bishops and rooks are the same regardless the color.
            Rook rook = new Rook(this.location.Item1, this.location.Item2, color);
            Bishop bishop = new Bishop(this.location.Item1, this.location.Item2, color);
            var rookMoves = rook.GetPossibleMoves(gamestate);
            var bishopMoves = bishop.GetPossibleMoves(gamestate);

            return rookMoves.Concat(bishopMoves).ToList();
        }
    }
}

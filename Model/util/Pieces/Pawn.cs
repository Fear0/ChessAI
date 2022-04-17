using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class Pawn : Piece
    {

        public Pawn(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Pawn;
        }

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {
            List<Move> possiblePawnMoves = new List<Move>();
            if (gamestate.whiteToPlay && this.pieceColor == PieceColor.White)
            {
                var location = this.location;
                var row = location.Item1;
                var col = location.Item2;
                var board = gamestate.board;
                if (board[row - 1, col] == "--") { 

                    //move 1 square forward
                    possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col), board));
                    //move 2 squares forward
                    if (row == 6 && board[row-2,col] == "--")
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 2, col), board));
                    }
                }

                //capture left
                if (col > 0)
                {
                    if (board[row - 1, col - 1][0].Equals('b'))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board));
                    }
                }
                
                //capture right
                if (col < 7)
                {
                    if (board[row - 1, col + 1][0].Equals('b'))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 1), board));
                    }
                }
            }
            if (!gamestate.whiteToPlay && this.pieceColor == PieceColor.Black)
            {
                var location = this.location;
                var row = location.Item1;
                var col = location.Item2;
                var board = gamestate.board;
                if (board[row + 1, col] == "--")
                {
                    possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col), board));
                    if (row == 1 && board[row + 2, col] == "--")
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 2, col), board));
                    }
                }

                if (col > 0)
                {
                    if (board[row + 1, col - 1][0].Equals('w'))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 1), board));
                    }
                }
                if (col < 7)
                {
                    if (board[row + 1, col + 1][0].Equals('w'))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 1), board));
                    }
                }
            }

            return possiblePawnMoves;
        }
    }
}

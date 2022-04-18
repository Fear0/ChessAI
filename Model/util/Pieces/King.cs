﻿using System;
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

            //north
            if (row - 1 >= 0)
            {
                if (board[row - 1, col] == "--" || board[row - 1, col][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col), board));
                }
            }

            //south
            if (row + 1 <= limit)
            {
                if (board[row + 1, col] == "--" || board[row + 1, col][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col), board));
                }
            }

            //east
            if (col + 1 <= limit)
            {
                if (board[row, col + 1] == "--" || board[row, col + 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row, col + 1), board));
                }
            }

            //west
            if (col - 1 >= 0)
            {
                if (board[row, col - 1] == "--" || board[row, col - 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row, col - 1), board));
                }
            }

            //northeast
            if (col + 1 <= limit && row - 1 >= 0)
            {
                if (board[row - 1, col + 1] == "--" || board[row - 1, col + 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 1), board));
                }
            }

            //northwest
            if (col - 1 >= 0 && row - 1 >= 0)
            {
                if (board[row - 1, col - 1] == "--" || board[row - 1, col - 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board));
                }
            }

            //southeast
            if (col + 1 <=limit && row + 1 <=limit)
            {
                if (board[row + 1, col + 1] == "--" || board[row + 1, col + 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 1), board));
                }
            }

            //southwest
            if (col - 1 >= 0 && row + 1 <= limit)
            {
                if (board[row + 1, col - 1] == "--" || board[row + 1, col - 1][0] == opponent)
                {
                    possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 1), board));
                }
            }


            return possibleKingMoves;
        }
    }

}
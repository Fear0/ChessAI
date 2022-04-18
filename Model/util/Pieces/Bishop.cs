using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class Bishop : Piece
    {

        public Bishop(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Bishop;
        }


        public override List<Move> GetPossibleMoves(GameState gamestate)
        {
            List<Move> possibleBishopMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';
            char fellow = gamestate.whiteToPlay ? 'w' : 'b';

            // bishops move diagonally. Northeast, Northwest, Southeast and Southwest
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, -1), Tuple.Create(1, 1) };

         
            int i;

            foreach (var direction in directions)
            {
                i = 1;
                if (0 <= row + direction.Item1 * i && row + direction.Item1 * i <=limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
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
                            possibleBishopMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i), board));

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

            //if (row - i >= 0 && col + i <= limit)
            //{

            //    while (row - i >= 0 && col + i <= limit)
            //    {

            //        //check if path is blocked by ally piece 
            //        if (board[row - i, col + i][0] == fellow)
            //        {
            //            break;
            //        }
            //        if (board[row - i, col + i] == "--" || board[row - i, col + i][0] == opponent)
            //        {
            //            possibleBishopMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - i, col + i), board));

            //            // if capture occured, no move along the same path is valid
            //            if (board[row - i, col + i][0] == opponent)
            //            {
            //                break;
            //            }

            //        }
            //        i++;
            //    }
            //}


            //i = 1;


            ////Northwest
            //if (row - i >= 0 && col - i >= 0)
            //{

            //    while (row - i >= 0 && col - i >= 0)
            //    {
            //        if (board[row - i, col - i][0] == fellow)
            //        {
            //            break;
            //        }
            //        if (board[row - i, col - i] == "--" || board[row - i, col - i][0] == opponent)
            //        {
            //            possibleBishopMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - i, col - i), board));

            //            if (board[row - i, col - i][0] == opponent)
            //            {
            //                break;
            //            }

            //        }
            //        i++;
            //    }

            //}

            //i = 1;

            ////Southwest
            //if (row + i <= limit && col - i >= 0)
            //{
            //    while (row + i <= limit && col - i >= 0)
            //    {
            //        if (board[row + i, col - i][0] == fellow)
            //        {
            //            break;
            //        }
            //        if (board[row + i, col - i] == "--" || board[row + i, col - i][0] == opponent)
            //        {
            //            possibleBishopMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + i, col - i), board));

            //            if (board[row + i, col - i][0] == opponent)
            //            {
            //                break;
            //            }

            //        }
            //        i++;
            //    }

            //}

            //i = 1;

            ////Southeast
            //if (row + i <= limit && col + i <= limit)
            //{

            //    while (row + i <= limit && col + i <= limit)
            //    {
            //        if (board[row + i, col + i][0] == fellow)
            //        {
            //            break;
            //        }
            //        if (board[row + i, col + i] == "--" || board[row + i, col + i][0] == opponent)
            //        {
            //            possibleBishopMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + i, col + i), board));

            //            if (board[row + i, col + i][0] == opponent)
            //            {
            //                break;
            //            }

            //        }
            //        i++;
            //    }

            //}

            return possibleBishopMoves;
        }
    }
}

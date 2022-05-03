using ChessAI.Model.util.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util
{
    internal class Helpers
    {


        public static string BoardToString(string[,] board)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("  a  b  c  d  e  f  g  h");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                sb.Append(board.GetLength(0) - i + " ");
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    sb.Append(board[i, j] + " ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public static string[,] MapPiecesToBoard(List<Piece> pieces)
        {
            string[,] board = new string[8, 8]
          {
           {"--","--","--","--","--","--","--","--" },
           {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
           {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
           {"--","--","--","--","--","--","--","--" },

          };

            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].status.Equals("alive"))
                {
                    board[pieces[i].location.Item1, pieces[i].location.Item2] = GetStringFromTypeAndColor(pieces[i].pieceType, pieces[i].pieceColor);
                }
            }
            return board;
        }
        public static string GetStringFromTypeAndColor(PieceType pieceType, PieceColor pieceColor)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wP";
                    }
                    else
                    {
                        return "bP";
                    }
                case PieceType.Knight:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wN";
                    }
                    else
                    {
                        return "bN";
                    }
                case PieceType.Bishop:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wB";
                    }
                    else
                    {
                        return "bB";
                    }
                case PieceType.Rook:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wR";
                    }
                    else
                    {
                        return "bR";
                    }
                case PieceType.King:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wK";
                    }
                    else
                    {
                        return "bK";
                    }
                case PieceType.Queen:
                    if (pieceColor.Equals(PieceColor.White))
                    {
                        return "wQ";
                    }
                    else
                    {
                        return "bQ";
                    }
                default:
                    break;

            }
            return "";
        }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    public abstract class Piece : IComparable<Piece>
    {
        public Tuple<int, int> location { get; set; } //(row,column)
        public PieceType pieceType { get; set; }
        public PieceColor pieceColor { get; set; }

        public decimal score { get; set; }

        public string status { get; set; }

        
        public Piece(int row, int col, PieceColor color)
        {
            this.location = new Tuple<int, int>(row, col);
            //this.pieceType = type;
            this.pieceColor = color;
            this.status = "alive";
        }


        public abstract List<Move> GetPossibleMoves(GameState gamestate);

   
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.Append("(" + location.Item1 + ", " + location.Item2 + ", " + pieceType + ", " + pieceColor + ", " + status + ")").ToString();
        }

        //public int GetMultiplier()
        //{
        //    return (int) pieceColor;
        //}

        public int CompareTo(Piece? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            Piece otherPiece = obj as Piece;
            if (otherPiece != null)
            {
                if (this.location.Equals(obj.location) && this.pieceType.Equals(obj.pieceType) && this.pieceColor.Equals(obj.pieceColor)&& this.status.Equals(obj.status))
                {
                    return 0;
                }
            }
            return -1;
        }


        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal abstract class Piece
    {
        public Tuple<int, int> location { get; set; } = new Tuple<int, int>(0, 0); //(row,column)
        public PieceType pieceType { get; set; }
        public PieceColor pieceColor { get; }

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


    }
}

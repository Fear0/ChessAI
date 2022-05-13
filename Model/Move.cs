using System;
using System.Linq;
using System.Text.RegularExpressions;
using ChessAI.Model.util;
using ChessAI.Model.util.Pieces;

namespace ChessAI.Model
{
    public class Move

    {

        public Tuple<int, int> startPosition; //(row,column)
        public Tuple<int, int> endPosition;

        public int sourcePieceId;

        public int targetPieceId;
        public string pieceMoved { get; }
        public string pieceCaptured { get; }

        public bool is_pawn_promotion = false;

        public bool is_enpassant_move = false;

        public bool is_castle_move = false;

        public Move(Tuple<int, int> startPos, Tuple<int, int> endPos, string[,] board, int source, int target = -1, bool isEnPassant = false, bool isCastle = false)
        {
            startPosition = startPos;
            endPosition = endPos;
            pieceMoved = board[startPos.Item1, startPos.Item2];
            pieceCaptured = board[endPos.Item1, endPos.Item2];
            this.sourcePieceId = source;
            this.targetPieceId = target;
            is_pawn_promotion = (pieceMoved == "wP" && endPosition.Item1 == 0) || (pieceMoved == "bP" && endPosition.Item1 == board.GetLength(0) - 1);
            is_enpassant_move = isEnPassant;
            is_castle_move = isCastle;

            if (is_enpassant_move)
            {
                pieceCaptured = pieceMoved == "wP" ? "bP" : "wP";
            }
        }

   
        public override string ToString()
        {
            if (is_castle_move) // O-O for short castle (ks) and O-O-O for long casle (qs)
            {  
                if (this.endPosition.Item2 == 6)
                {
                    return "O-O";
                }
                else if (this.endPosition.Item2 == 2 )
                {
                    return "O-O-O";
                }
            }
            return ChessNotation.colsToFiles[startPosition.Item2] + ChessNotation.rowsToRanks[startPosition.Item1] + ChessNotation.colsToFiles[endPosition.Item2] + ChessNotation.rowsToRanks[endPosition.Item1];
        }

        public override bool Equals(object? obj)
        {

            if (obj is Move)
            {
                Move other = obj as Move;
                if (other == null) return false;
                return (this.startPosition.Item1 == other.startPosition.Item1) && (startPosition.Item2 == other.startPosition.Item2) && (this.endPosition.Item1 == other.endPosition.Item1) && (this.endPosition.Item2 == other.endPosition.Item2) && (this.pieceMoved.Equals(other.pieceMoved)) && (this.pieceCaptured.Equals(other.pieceCaptured));
            }
            return false;
        }

    }
}
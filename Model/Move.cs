using System;
using System.Text.RegularExpressions;
using ChessAI.Model.util;

namespace ChessAI.Model
{
    public class Move

    {

        public Tuple<int, int> startPosition = Tuple.Create(0, 0); //(row,column)
        public Tuple<int, int> endPosition = Tuple.Create(0, 0);
        public string pieceMoved { get; }
        public string pieceCaptured { get; }

        public Move(Tuple<int, int> startPos, Tuple<int, int> endPos, string[,] board)
        {
            startPosition = startPos;
            endPosition = endPos;
            pieceMoved = board[startPos.Item1, startPos.Item2];
            pieceCaptured = board[endPos.Item1, endPos.Item2];
        }


        public static Move? ToMove(string? input, string[,] board)
        {

            var rx = new Regex(@"[a-h][1-8][a-h][1-8]", RegexOptions.Compiled);
            if (rx.IsMatch(input))
            {
                Tuple<int, int> startPos = Tuple.Create(ChessNotation.ranksToRows[input[1].ToString()], ChessNotation.filesToCols[input[0].ToString()]);
                Tuple<int, int> endPos = Tuple.Create(ChessNotation.ranksToRows[input[3].ToString()], ChessNotation.filesToCols[input[2].ToString()]);


                return new Move(startPos, endPos, board);
            }
            return null;
        }

        public override string ToString()
        {
            return ChessNotation.colsToFiles[startPosition.Item2] + ChessNotation.rowsToRanks[startPosition.Item1] + ChessNotation.colsToFiles[endPosition.Item2] + ChessNotation.rowsToRanks[endPosition.Item1];
        }

    }
}
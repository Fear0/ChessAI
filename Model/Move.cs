using System;

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





    }
}
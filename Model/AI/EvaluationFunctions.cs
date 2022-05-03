using ChessAI.Model.util;
using ChessAI.Model.util.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{
    public class EvaluationFunctions
    {


        public static readonly int CHECKMATE = 1000;
        //public static decimal ScoreEvaluationFunction(GameState state)
        //{
        //    return state.GetScore();
        //}
        static double[,] pawnEvalWhite = new double[8, 8]

        {
                {0.8, 0.8, 0.8, 0.8, 0.8, 0.8, 0.8, 0.8},

               {0.7, 0.7, 0.7, 0.7, 0.7, 0.7, 0.7, 0.7},

               {0.3, 0.3, 0.4, 0.5, 0.5, 0.4, 0.3, 0.3},

               {0.25, 0.25, 0.3, 0.45, 0.45, 0.3, 0.25, 0.25},

               {0.2, 0.2, 0.2, 0.4, 0.4, 0.2, 0.2, 0.2},

               {0.25, 0.15, 0.1, 0.2, 0.2, 0.1, 0.15, 0.25},

               {0.25, 0.3, 0.3, 0.0, 0.0, 0.3, 0.3, 0.25},

               {0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2}
        };

        static double[,] pawnEvalBlack = reverse(pawnEvalWhite);

        static double[,] knightEval = new double[8, 8]

          {
              {0.0, 0.1, 0.2, 0.2, 0.2, 0.2, 0.1, 0.0},
                 {0.1, 0.3, 0.5, 0.5, 0.5, 0.5, 0.3, 0.1},
                 {0.2, 0.5, 0.6, 0.65, 0.65, 0.6, 0.5, 0.2},
                 {0.2, 0.55, 0.65, 0.7, 0.7, 0.65, 0.55, 0.2},
                 {0.2, 0.5, 0.65, 0.7, 0.7, 0.65, 0.5, 0.2},
                 {0.2, 0.55, 0.6, 0.65, 0.65, 0.6, 0.55, 0.2},
                 {0.1, 0.3, 0.5, 0.55, 0.55, 0.5, 0.3, 0.1},
                 {0.0, 0.1, 0.2, 0.2, 0.2, 0.2, 0.1, 0.0}
          };

        static double[,] bishopEvalWhite = new double[8, 8]

            {
                {0.0, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.0},
                 {0.2, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.2},
                 {0.2, 0.4, 0.5, 0.6, 0.6, 0.5, 0.4, 0.2},
                 {0.2, 0.5, 0.5, 0.6, 0.6, 0.5, 0.5, 0.2},
                 {0.2, 0.4, 0.6, 0.6, 0.6, 0.6, 0.4, 0.2},
                 {0.2, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.2},
                 {0.2, 0.5, 0.4, 0.4, 0.4, 0.4, 0.5, 0.2},
                 {0.0, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.0}
            };

        static double[,] bishopEvalBlack = reverse(bishopEvalWhite);

        static double[,] rookEvalWhite = {
            {0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25},
               {0.5, 0.75, 0.75, 0.75, 0.75, 0.75, 0.75, 0.5},
               {0.0, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.0},
               {0.0, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.0},
               {0.0, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.0},
               {0.0, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.0},
               {0.0, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.0},
               {0.25, 0.25, 0.25, 0.5, 0.5, 0.25, 0.25, 0.25}
        }
;

        static double[,] rookEvalBlack = reverse(rookEvalWhite);

        static double[,] queenEval = {
            {0.0, 0.2, 0.2, 0.3, 0.3, 0.2, 0.2, 0.0},
                {0.2, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.2},
                {0.2, 0.4, 0.5, 0.5, 0.5, 0.5, 0.4, 0.2},
                {0.3, 0.4, 0.5, 0.5, 0.5, 0.5, 0.4, 0.3},
                {0.4, 0.4, 0.5, 0.5, 0.5, 0.5, 0.4, 0.3},
                {0.2, 0.5, 0.5, 0.5, 0.5, 0.5, 0.4, 0.2},
                {0.2, 0.4, 0.5, 0.4, 0.4, 0.4, 0.4, 0.2},
                {0.0, 0.2, 0.2, 0.3, 0.3, 0.2, 0.2, 0.0}}
        ;

        static double[,] kingEvalWhite = new double[8, 8] {


            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0},

            {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0},

            {2.0, 2.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.0},

            {2.0, 3.0, 1.0, 0.0, 0.0, 1.0, 3.0, 2.0}
};

        static double[,] kingEvalBlack = reverse(kingEvalWhite);


        static Dictionary<Tuple<PieceType, PieceColor>, double[,]> piece_position_scores = new Dictionary<Tuple<PieceType, PieceColor>, double[,]>()
        {
            {Tuple.Create(PieceType.Pawn,PieceColor.White), pawnEvalWhite },
            {Tuple.Create(PieceType.Pawn,PieceColor.Black), pawnEvalBlack },
            {Tuple.Create(PieceType.Knight,PieceColor.White), knightEval },
            {Tuple.Create(PieceType.Knight,PieceColor.Black), knightEval },
            {Tuple.Create(PieceType.Bishop,PieceColor.White), bishopEvalWhite },
            {Tuple.Create(PieceType.Bishop,PieceColor.Black), bishopEvalBlack },
            {Tuple.Create(PieceType.Rook,PieceColor.White), rookEvalWhite },
            {Tuple.Create(PieceType.Rook,PieceColor.Black), rookEvalBlack },
            {Tuple.Create(PieceType.Queen,PieceColor.White), queenEval },
            {Tuple.Create(PieceType.Queen,PieceColor.Black), kingEvalWhite },
            

        };



        public static Func<GameState, double> ScoreEvaluationFunction => (GameState state) => (double)state.GetScore();


        public static Func<GameState, double> BoardEvaluationFunction = (GameState state) =>
        {

            if (state.checkmate)
            {
                if (state.whiteToPlay)
                {
                    return -1000;
                }
                if (!state.whiteToPlay)
                {
                    return 1000;
                }
            }
            if (state.stalemate)
            {
                return 0;
            }

            double score = 0;

            foreach (var piece in state.pieces)
            {
                if (piece.status == "alive")
                {
                    double piece_position_score = 0;
                    if (piece.pieceType != PieceType.King)
                    {
                        piece_position_score = piece_position_scores[Tuple.Create(piece.pieceType, piece.pieceColor)][piece.location.Item1, piece.location.Item2];
                    }
                    if (piece.pieceColor == PieceColor.White)
                    {
                        score += (double)piece.score + piece_position_score;
                    }
                    if (piece.pieceColor == PieceColor.Black)
                    {
                        score -= (double)piece.score + piece_position_score;
                    }
                }
            }
            return score;
        };
        public static double[,] reverse(double[,] array)
        {
            double[,] reversed = new double[array.GetLength(0), array.GetLength(1)];
            for (int i = array.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    reversed[array.GetLength(0) - 1 - i, j] = array[i, j];
                }
            }
            return reversed;
        }
    }
}

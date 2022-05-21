using ChessAI.Model.util;
using ChessAI.Model.util.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.AI
{


    public class EvaluationType1
    {

        public const double CHECKMATE = 1000;
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


            {0, 0, 0, 0, 0, 0, 0,0},

            {0, 0, 0, 0, 0, 0, 0,0},

            {0, 0, 0, 0, 0, 0, 0,0},

            {0, 0, 0, 0, 0, 0, 0,0},

            { 0, 0, 0, 0, 0, 0, 0,0},

            {0, 0, 0, 0, 0, 0, 0,0},

            {0.2, 0.2, 0.0, 0.0, 0.0, 0.0, 0.2, 0.2},

            {0.2, 0.4, 0.1, 0.0, 0.0, 0.1, 0.4, 0.2}
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
            {Tuple.Create(PieceType.Queen,PieceColor.Black), queenEval },
            {Tuple.Create(PieceType.King,PieceColor.White), kingEvalWhite},
            {Tuple.Create(PieceType.King,PieceColor.Black), kingEvalBlack }


        };



        //public static Func<GameState, double> ScoreEvaluationFunction => (GameState state) => (double)state.GetScore();

        public static Func<GameState, double> ScoreEvaluationFunction = (GameState state) =>
        {

            Dictionary<PieceType, double> pieceValues = new Dictionary<PieceType, double>()
            {
                {PieceType.Pawn,1 },
                {PieceType.King, 0 },
                {PieceType.Knight,3 },
                {PieceType.Bishop,3 },
                {PieceType.Rook,5 },
                {PieceType.Queen,9 }
            };

            if (state.checkmate)
            {

                if (state.whiteToPlay)
                {
                    return -CHECKMATE;
                }
                else
                {
                    return CHECKMATE;
                }
            }
            if (state.stalemate)
            {
                return 0;
            }

            double score = 0;
            PieceColor playerColor = state.whiteToPlay ? PieceColor.White : PieceColor.Black;
            foreach (var piece in state.pieces)
            {
                if (piece.status == "alive")
                {
                    score += pieceValues[piece.pieceType] * (piece.pieceColor == playerColor ? 1 : -1);
                }
            }
            return score;
        };

        public static Func<GameState, double> BoardEvaluationFunction = (GameState state) =>
        {

            if (state.checkmate)
            {
                if (state.whiteToPlay)
                {
                    return -CHECKMATE;
                }
                if (!state.whiteToPlay)
                {
                    return CHECKMATE;
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
                    
                        piece_position_score = piece_position_scores[Tuple.Create(piece.pieceType, piece.pieceColor)][piece.location.Item1, piece.location.Item2];
                        //Console.WriteLine(piece_position_score);
                    
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

    public class EvaluationType2
    {

        public const double CHECKMATE = 9999;

        static double[,] pawnEvalWhite = new double[8, 8]

           {
                {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},

               {5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0,  5.0},

               {1.0,  1.0,  2.0,  3.0,  3.0,  2.0,  1.0,  1.0},

               {0.5,  0.5,  1.0,  2.5,  2.5,  1.0,  0.5,  0.5},

               {0.0,  0.0,  0.0,  2.0,  2.0,  0.0,  0.0,  0.0},

               {0.5, -0.5, -1.0,  0.0,  0.0, -1.0, -0.5,  0.5},

               {0.5,  1.0, 1.0,  -2.0, -2.0,  1.0,  1.0,  0.5},

               {0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0}
           };
        static double[,] pawnEvalBlack = reverse(pawnEvalWhite);

        static double[,] knightEval = new double[8, 8]

          {
              {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0},
                 {-4.0, -2.0, 0.0,  0.0,  0.0,  0.0, -2.0, -4.0},
                 {-3.0,  0.0,  1.0,  1.5,  1.5,  1.0,  0.0, -3.0},
                 {-3.0,  0.5,  1.5,  2.0,  2.0,  1.5,  0.5, -3.0},
                 {-3.0,  0.0,  1.5,  2.0,  2.0,  1.5,  0.0, -3.0},
                 {-3.0,  0.5,  1.0,  1.5,  1.5,  1.0,  0.5, -3.0},
                 {-4.0, -2.0,  0.0,  0.5,  0.5,  0.0, -2.0, -4.0},
                 {-5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0}
          };

        static double[,] bishopEvalWhite = new double[8, 8]

              {
                {-2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0},
                 {-1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0},
                 {-1.0,  0.0,  0.5,  1.0,  1.0,  0.5,  0.0, -1.0},
                 {-1.0,  0.5,  0.5,  1.0,  1.0,  0.5,  0.5, -1.0},
                 {-1.0,  0.0,  1.0,  1.0,  1.0,  1.0,  0.0, -1.0},
                 {-1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0, -1.0},
                 {-1.0,  0.5,  0.0,  0.0,  0.0,  0.0,  0.5, -1.0},
                 {-2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0}
              };

        static double[,] bishopEvalBlack = reverse(bishopEvalWhite);

        static double[,] rookEvalWhite = {
            {  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0},
               {0.5,  1.0,  1.0,  1.0,  1.0,  1.0,  1.0,  0.5},
               {-0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
               {-0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
               {-0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
               {-0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
               {-0.5,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -0.5},
               { 0.0,   0.0, 0.0,  0.5,  0.5,  0.0,  0.0,  0.0}
        };

        static double[,] rookEvalBlack = reverse(rookEvalWhite);

        static double[,] queenEval = {
            {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0},
                {-1.0,  0.0,  0.0,  0.0,  0.0,  0.0,  0.0, -1.0},
                {-1.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0},
                { -0.5,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5},
                { 0.0,  0.0,  0.5,  0.5,  0.5,  0.5,  0.0, -0.5},
                {-1.0,  0.5,  0.5,  0.5,  0.5,  0.5,  0.0, -1.0},
                {-1.0,  0.0,  0.5,  0.0,  0.0,  0.0,  0.0, -1.0},
                {-2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0}};

        static double[,] kingEvalWhite = new double[8, 8] {


            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0},

            {-2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0},

            {-1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0},

            {2.0, 2.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.0},

            {2.0, 3.0, 1.0, 0.0, 0.0, 1.0, 3.0, 2.0}
};

        static double[,] kingEvalBlack = reverse(kingEvalWhite);


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

        public static double GetPieceValue(Piece piece)
        {
            if (piece == null)
            {
                return 0.0;
            }

            double GetAbsoluteValue(Piece piece, int x, int y)
            {
                switch (piece.pieceType)
                {
                    case PieceType.Pawn:
                        return 10 + (piece.pieceColor == PieceColor.White ? pawnEvalWhite[y, x] : pawnEvalBlack[y, x]);
                    case PieceType.Rook:
                        return 50 + (piece.pieceColor == PieceColor.White ? rookEvalWhite[y, x] : rookEvalBlack[y, x]);
                    case PieceType.Knight:
                        return 30 + knightEval[y, x];
                    case PieceType.Bishop:
                        return 30 + (piece.pieceColor == PieceColor.White ? bishopEvalWhite[y, x] : bishopEvalBlack[y, x]);
                    case PieceType.Queen:
                        return 90 + queenEval[y, x];
                    case PieceType.King:
                        return 900 + (piece.pieceColor == PieceColor.White ? kingEvalWhite[y, x] : kingEvalBlack[y, x]);
                    default:
                        return 0;
                }
            };
            double absoluteValue = GetAbsoluteValue(piece, piece.location.Item1, piece.location.Item2);
            return piece.pieceColor == PieceColor.White ? absoluteValue : -absoluteValue;
        }

        public static Func<GameState, double> BoardEvaluationFunction = (GameState state) =>
        {
            double totalEvaluation = 0;
            foreach (var piece in state.pieces)
            {
                if (piece.status == "alive")
                {
                    totalEvaluation += GetPieceValue(piece);
                }
            }
            return totalEvaluation;
        };
    }
}

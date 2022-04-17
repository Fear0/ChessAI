using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAI.Model.util.Pieces;
using ChessAI.Model.util;

namespace ChessAI.Model
{
    internal class GameState
    {
        public string[,] board = new string[8, 8]
        {
            {"bR", "bN", "bB","bQ", "bK","bB","bN","bR"},
            { "bP","bP","bP","bP","bP","bP","bP","bP"},
            {"--","--","--","--","--","--","--","--" },
           {"--","--","--","--","--","--","--","--" },
            {"--","--","--","--","--","--","--","--" },
            {"--","--","--","bQ","--","--","--","--" },
            { "wP","wP","wP","wP","wP","wP","wP","wP"},
            {"wR", "wN", "wB","wQ", "wK","wB","wN","wR"},


        };

        public List<Piece> pieces { get; } = new List<Piece>();
        public bool whiteToPlay;
        public List<Move> moveHisory = new List<Move>();

        public GameState()
        {

            //Pawn pawn = new Pawn(0, 0, util.PieceType.Pawn, util.PieceColor.White);
            //var moves = pawn.GetPossibleMoves(this);
            this.whiteToPlay = true;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                pieces.Add(new Pawn(6, i, PieceColor.White));
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                pieces.Add(new Pawn(1, i, PieceColor.Black));
            }

        }

        public GameState(GameState state)
        {
            this.board = state.board;
            this.whiteToPlay = state.whiteToPlay;
            this.moveHisory = new List<Move>();

        }

        public List<Move> GetAllMoves()
        {
            List<Move> moves = new List<Move>();
            foreach (var piece in pieces)
            {
                if (piece.status == "alive")
                {
                    moves.AddRange(piece.GetPossibleMoves(this));
                }
            }

            return moves;
        }
        public GameState GenerateSuccessorState(Move move)
        {
            return null;
        }

        public void MakeMove(Move move)
        {
            //assume that all moves are valid. Validation will be encapsuled somewhere else
            if (move is not null && this.board[move.startPosition.Item1, move.startPosition.Item2] != "--")
            {
                this.board[move.startPosition.Item1, move.startPosition.Item2] = "--";
                this.board[move.endPosition.Item1, move.endPosition.Item2] = move.pieceMoved;
                this.moveHisory.Add(move);
                var movedPiece = this.GetPieceAtLocation(move.startPosition);
                var capturedPiece = this.GetPieceAtLocation(move.endPosition);
                movedPiece.location = move.endPosition;
                if (capturedPiece != null)
                {
                    capturedPiece.location = Tuple.Create(-1, -1);
                    capturedPiece.status = "captured";
                }
                this.whiteToPlay = !whiteToPlay; //switch turns

                // to do
            }
        }

        public void Undo()
        {
            //undoes the last move
            if (moveHisory.Any())
            {
                var lastMove = moveHisory[moveHisory.Count - 1];
                if (lastMove != null)
                {
                    var startPosition = lastMove.startPosition;
                    var endPosition = lastMove.endPosition;
                    this.board[startPosition.Item1, startPosition.Item2] = lastMove.pieceMoved;
                    this.board[endPosition.Item1, endPosition.Item2] = lastMove.pieceCaptured;
                    var movedPiece = GetPieceAtLocation(endPosition);
                    movedPiece.location = startPosition;
                    var capturedPiece = GetPieceAtLocation(Tuple.Create(-1, -1), lastMove.pieceCaptured);
                    if (capturedPiece != null)
                    {
                        capturedPiece.location = endPosition;
                        capturedPiece.status = "alive";
                    }

                    this.whiteToPlay = !whiteToPlay;
                    this.moveHisory.Remove(lastMove);
                }

            }
        }


        public Piece GetPieceAtLocation(Tuple<int, int> pieceLocation, string pieceString)
        {
            // fetch a specified piece at a particular position, helpful especially with captured piece that share the same position
            foreach (var piece in pieces)
            {

                var location = piece.location;
                var color = pieceString[0];
                var type = pieceString[1];
                if (location.Item1 == pieceLocation.Item1 && location.Item2 == pieceLocation.Item2 && color.Equals(pieceString[0]) && type.Equals(pieceString[1]))
                {
                    return piece;
                }

            }
            return null;
        }
        public Piece GetPieceAtLocation(Tuple<int, int> pieceLocation)
        {
            // fetch the piece at a specific location
            foreach (var piece in pieces)
            {
                if (piece.status == "alive")
                {
                    var location = piece.location;
                    if (location.Item1 == pieceLocation.Item1 && location.Item2 == pieceLocation.Item2)
                    {
                        return piece;
                    }
                }
            }
            return null;

        }


        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("  a  b  c  d  e  f  g  h");
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                sb.Append(this.board.GetLength(0) - i + " ");
                for (int j = 0; j < this.board.GetLength(0); j++)
                {
                    sb.Append(this.board[i, j] + " ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public void LogMoveHistory()
        {
            var sb = new StringBuilder();
            foreach (var move in this.moveHisory)
            {
                sb.Append(move.ToString() + ", ");
            }

            Console.WriteLine($"Move History: {sb.ToString()}");
        }

    }
}

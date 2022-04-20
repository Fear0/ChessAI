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
            {"--","--","--","--","--","--","--","--" },
            { "wP","wP","wP","wP","wP","wP","wP","wP"},
            {"wR", "wN", "wB","wQ", "wK","wB","wN","wR"},


        };

        public List<Piece> pieces { get; } = new List<Piece>();

        public bool whiteToPlay;

        public List<Move> moveHisory = new List<Move>();

        public Tuple<int, int> whiteKingLocation = Tuple.Create(7, 4);

        public Tuple<int, int> blackKingLocation = Tuple.Create(0, 4);


        public bool in_check = false;

        public List<Tuple<int, int, int, int>> pins = new List<Tuple<int, int, int, int>>();  //(row,col,directioncoordinate 1, directioncoordinate 2)

        public List<Tuple<int, int, int, int>> checks = new List<Tuple<int, int, int, int>>();

        public GameState()
        {

            //Pawn pawn = new Pawn(0, 0, util.PieceType.Pawn, util.PieceColor.White);
            //var moves = pawn.GetPossibleMoves(this);
            this.whiteToPlay = true;

            //pawns
            for (int i = 0; i < board.GetLength(0); i++)
            {
                pieces.Add(new Pawn(6, i, PieceColor.White));
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                pieces.Add(new Pawn(1, i, PieceColor.Black));
            }

            //knights
            pieces.Add(new Knight(7, 1, PieceColor.White));
            pieces.Add(new Knight(7, 6, PieceColor.White));
            pieces.Add(new Knight(0, 1, PieceColor.Black));
            pieces.Add(new Knight(0, 6, PieceColor.Black));

            //bishops
            pieces.Add(new Bishop(7, 2, PieceColor.White));
            pieces.Add(new Bishop(7, 5, PieceColor.White));
            pieces.Add(new Bishop(0, 2, PieceColor.Black));
            pieces.Add(new Bishop(0, 5, PieceColor.Black));

            //rooks
            pieces.Add(new Rook(7, 0, PieceColor.White));
            pieces.Add(new Rook(7, 7, PieceColor.White));
            pieces.Add(new Rook(0, 0, PieceColor.Black));
            pieces.Add(new Rook(0, 7, PieceColor.Black));

            //queens
            pieces.Add(new Queen(7, 3, PieceColor.White));
            pieces.Add(new Queen(0, 3, PieceColor.Black));

            //kings
            pieces.Add(new King(7, 4, PieceColor.White));
            pieces.Add(new King(0, 4, PieceColor.Black));

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

            var turn = this.whiteToPlay ? PieceColor.White : PieceColor.Black;
            foreach (var piece in pieces)
            {
                if (piece.status == "alive" && piece.pieceColor == turn)
                {
                    moves.AddRange(piece.GetPossibleMoves(this));
                }
            }

            return moves;
        }


        public List<Move> GetValidMoves()
        {

            List<Move> validMoves = new List<Move>();

            (this.in_check, this.pins, this.checks) = this.CheckForPinsAndChecks();

            int limit = board.GetLength(0) - 1;


            int kingRow, kingCol;

            if (whiteToPlay)
            {

                kingRow = whiteKingLocation.Item1;
                kingCol = whiteKingLocation.Item2;

            }
            else
            {

                kingRow = blackKingLocation.Item1;
                kingCol = blackKingLocation.Item2;

            }

            if (this.in_check)
            {
                if (checks.Count == 1)
                { // check from one piece, so either block or capture the piece or move the king
                    validMoves = this.GetAllMoves();

                    var check = this.checks[0];
                    var checkRow = check.Item1;
                    var checkCol = check.Item2;
                    Piece checkingPiece = GetPieceAtLocation(Tuple.Create(checkRow, checkCol));
                    List<Tuple<int, int>> validSquares = new List<Tuple<int, int>>();

                    // check from a knight. Stop check by capturing the knight, thus its position is a valid square
                    if (checkingPiece.pieceType == PieceType.Knight)
                    {
                        validSquares.Add(Tuple.Create(checkRow, checkCol));
                    }
                    else
                    {
                        // squares between the checked king's position and the checking piece are valid squares because moving to them will block check
                        for (int i = 1; i < board.GetLength(0); i++)
                        {
                            Tuple<int, int> validSquare = Tuple.Create(kingRow + check.Item3 * i, kingRow + check.Item3 * i);
                            validSquares.Add(validSquare);
                            if (validSquare.Item1 == checkRow && validSquare.Item2 == checkCol)
                            {
                                break;
                            }

                        }
                    }
                    // remove the moves that dont block check, capture piece, or move king
                    for (int i = validMoves.Count - 1; i >= 0; i--)
                    {
                        if (validMoves[i].pieceMoved[1] != 'K')
                        {
                            if (!validSquares.Contains(validMoves[i].endPosition))
                            {
                                validMoves.Remove(validMoves[i]);
                            }
                        }
                    }


                }
                else // double check, king has to move
                {
                    validMoves.AddRange(GetPieceAtLocation(Tuple.Create(kingRow, kingCol)).GetPossibleMoves(this));
                }
            }
            else //if no check, all pieces moves are valid
            {
                validMoves = this.GetAllMoves();
            }
            return validMoves;
        }


        // this idea of looking for pins and checks starting from the king instead of iterating through all enemy moves is inspired from Eddie Sharick's Tutorial. 
        public (bool, List<Tuple<int, int, int, int>>, List<Tuple<int, int, int, int>>) CheckForPinsAndChecks()
        {

            pins = new List<Tuple<int, int, int, int>>(); //squares pinned and the direction it is pinned from

            checks = new List<Tuple<int, int, int, int>>(); //squares of the enemy pieces that are applying checks

            in_check = false;

            PieceColor enemyColor, allyColor;

            int startRow, startCol;

            if (whiteToPlay)
            {
                enemyColor = PieceColor.Black;
                allyColor = PieceColor.White;
                startRow = whiteKingLocation.Item1;
                startCol = whiteKingLocation.Item2;

            }
            else
            {
                enemyColor = PieceColor.White;
                allyColor = PieceColor.Black;
                startRow = blackKingLocation.Item1;
                startCol = blackKingLocation.Item2;

            }

            // we will be going through directions, looking for pins or checks.
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-1, 0), Tuple.Create(1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1), Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, 1), Tuple.Create(1, -1) };
            int limit = board.GetLength(0) - 1;

            for (int j = 0; j < directions.Count; j++)
            {
                var direction = directions[j];

                //List<Tuple<int, int, int, int>> possiblePins = new List<Tuple<int,int,int,int>>();
                Tuple<int, int, int, int>? possiblePin = null;

                for (int i = 1; i < board.GetLength(0); i++)
                {
                    int endRow = startRow + direction.Item1 * i;
                    int endCol = startCol + direction.Item2 * i;

                    if (0 <= endRow && endRow <= limit && 0 <= endCol && endCol <= limit)
                    {
                        Piece piece = GetPieceAtLocation(Tuple.Create(endRow, endCol));

                        if (piece != null && piece.pieceColor == allyColor && piece.pieceType != PieceType.King)
                        {
                            //no pin yet -> possible pin
                            if (possiblePin == null)
                            {
                                possiblePin = Tuple.Create(endRow, endCol, direction.Item1, direction.Item2);
                                //possiblePins.Add(possiblePin); 
                            }

                            //second piece can't be pinned
                            else
                            {
                                break;
                            }
                        }
                        else if (piece != null && piece.pieceColor == enemyColor)
                        {
                            var enemyType = piece.pieceType;
                            // 5 possibilities in this annoying conditional
                            // 1.) orthogonally away from king and piece is a rook
                            // 2.) diagonally away from king and piece is a bishop
                            // 3.) 1 square away diagonally from king and piece is a pawn
                            // 4.) any direction and piece is a queen
                            //5.) any direction 1 square away and piece is a king

                            if ((0 <= j && j <= 3 && enemyType == PieceType.Rook) || (4 <= j && j <= 7 && enemyType == PieceType.Bishop)
                                    || (enemyType == PieceType.Queen) || (i == 1 && enemyType == PieceType.King) ||
                                    (i == 1 && enemyType == PieceType.Pawn && ((enemyColor == PieceColor.White && 6 <= j && j <= 7) || (enemyColor == PieceColor.Black && 4 <= j && j <= 5))))
                            {
                                if (possiblePin == null) //no pin so check
                                {
                                    in_check = true;
                                    checks.Add(Tuple.Create(endRow, endCol, direction.Item1, direction.Item2));
                                    break;
                                }
                                else // piece blocking so its a pin
                                {
                                    pins.Add(possiblePin);
                                    break;
                                }
                            }
                            else //enempy piece not applying check
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }


                }

            }

            List<Tuple<int, int>> knightDirections = new List<Tuple<int, int>>() { Tuple.Create(-2, 1), Tuple.Create(-1, 2), Tuple.Create(-1, -2), Tuple.Create(-2, -1), Tuple.Create(1, 2), Tuple.Create(2, 1), Tuple.Create(2, -1), Tuple.Create(1, -2) };

            foreach (var knightDirection in knightDirections)
            {
                var endRow = startRow + knightDirection.Item1;
                var endCol = startCol + knightDirection.Item2;

                if (0 <= endRow && endRow <= limit && 0 <= endCol && endCol <= limit)
                {
                    var checkingPiece = GetPieceAtLocation(Tuple.Create(endRow, endCol));
                    if (checkingPiece != null)
                    {
                        if (checkingPiece.pieceType == PieceType.Knight && checkingPiece.pieceColor == enemyColor)
                        {
                            in_check = true;
                            checks.Add(Tuple.Create(endRow, endCol, knightDirection.Item1, knightDirection.Item2));
                        }
                    }
                }
            }

            return (in_check, pins, checks);
        }





        // to be used later on for AI 
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
                if (movedPiece.pieceType == PieceType.King)
                {
                    if (whiteToPlay && movedPiece.pieceColor == PieceColor.White)
                    {
                        this.whiteKingLocation = movedPiece.location;
                    }
                    if (!whiteToPlay && movedPiece.pieceColor == PieceColor.Black)
                    {
                        this.blackKingLocation = movedPiece.location;
                    }
                }
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
            // fetch a specified piece at a particular position, helpful especially with captured pieces that share the same position
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

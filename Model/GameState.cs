using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAI.Model.util.Pieces;
using ChessAI.Model.util;
using System.Text.RegularExpressions;

namespace ChessAI.Model
{
    public class GameState
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

        //List<GameState> statesLog = new List<GameState>();
        public List<Piece> pieces { get; set; } = new List<Piece>();

        public bool whiteToPlay;

        public List<Move> moveHisory = new List<Move>();

        public Tuple<int, int> whiteKingLocation = Tuple.Create(7, 4);

        public Tuple<int, int> blackKingLocation = Tuple.Create(0, 4);

        public int ids = 0; //indexer for the pieces
        public bool checkmate = false;

        public bool stalemate = false;

        public bool in_check = false;

        public Tuple<int, int> enPassantPossible;

        public List<Tuple<int, int, int, int>> pins = new List<Tuple<int, int, int, int>>();  //(row,col,directioncoordinate 1, directioncoordinate 2)

        public List<Tuple<int, int, int, int>> checks = new List<Tuple<int, int, int, int>>();

        public CastleRights currentCastleRights = new CastleRights(true, true, true, true);

        public List<Tuple<int, int>> enpassant_possible_log = new List<Tuple<int, int>>();

        public List<CastleRights> castle_rights_log = new();



        public GameState()
        {

            this.enpassant_possible_log.Add(enPassantPossible);
            this.castle_rights_log.Add(new CastleRights(this.currentCastleRights.wKs, this.currentCastleRights.wQs, this.currentCastleRights.bKs, this.currentCastleRights.bQs));
            //Pawn pawn = new Pawn(0, 0, util.PieceType.Pawn, util.PieceColor.White);
            //var moves = pawn.GetPossibleMoves(this);
            this.whiteToPlay = true;

            //int ids = 0;
            //pawns
            for (int i = 0; i < board.GetLength(0); i++)
            {

                pieces.Add(new Pawn(6, i, PieceColor.White, ids++));
            }
            for (int i = 0; i < board.GetLength(0); i++)
            {
                pieces.Add(new Pawn(1, i, PieceColor.Black, ids++));
            }

            //knights

            pieces.Add(new Knight(7, 1, PieceColor.White, ids++));

            pieces.Add(new Knight(7, 6, PieceColor.White, ids++));

            pieces.Add(new Knight(0, 1, PieceColor.Black, ids++));

            pieces.Add(new Knight(0, 6, PieceColor.Black, ids++));

            //bishops



            pieces.Add(new Bishop(7, 2, PieceColor.White, ids++));
            pieces.Add(new Bishop(7, 5, PieceColor.White, ids++));
            pieces.Add(new Bishop(0, 2, PieceColor.Black, ids++));
            pieces.Add(new Bishop(0, 5, PieceColor.Black, ids++));

            //rooks



            pieces.Add(new Rook(7, 0, PieceColor.White, ids++));
            pieces.Add(new Rook(7, 7, PieceColor.White, ids++));
            pieces.Add(new Rook(0, 0, PieceColor.Black, ids++));
            pieces.Add(new Rook(0, 7, PieceColor.Black, ids++));

            //queens


            pieces.Add(new Queen(7, 3, PieceColor.White, ids++));
            pieces.Add(new Queen(0, 3, PieceColor.Black, ids++));

            //kings



            pieces.Add(new King(7, 4, PieceColor.White, ids++));
            pieces.Add(new King(0, 4, PieceColor.Black, ids++));

        }

        //public GameState(GameState state)
        //{
        //    this.board = state.board;
        //    this.whiteToPlay = state.whiteToPlay;
        //    this.moveHisory = new List<Move>();

        //}


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



        //If no check. Then return all possible moves of the pieces. Otherwise, trim all the possible moves so it cuts off the moves that dont put the king out of check.
        public List<Move> GetValidMoves()
        {

            List<Move> validMoves = new List<Move>();
            var temp_castle_rights = new CastleRights(this.currentCastleRights.wKs, this.currentCastleRights.wQs,
                                         this.currentCastleRights.bKs, this.currentCastleRights.bQs);
            var checkInfo = this.CheckForPinsAndChecks();
            this.in_check = checkInfo.Item1;
            this.pins = checkInfo.Item2;
            this.checks = checkInfo.Item3;

            int limit = board.GetLength(0) - 1;


            int kingRow, kingCol;

            if (whiteToPlay)  // 
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
                // check from one piece, so either block or capture the piece or move the king
                if (checks.Count == 1) // IsSingleCheck()
                {
                    validMoves = this.GetAllMoves();

                    var check = this.checks[0];
                    var checkRow = check.Item1;
                    var checkCol = check.Item2;
                    Piece checkingPiece = GetPieceAtLocation(Tuple.Create(checkRow, checkCol));
                    List<Tuple<int, int>> validSquares = new List<Tuple<int, int>>();

                    // check from a GameState. Stop check by capturing the knight, thus its position is a valid square
                    if (checkingPiece.pieceType == PieceType.Knight) //
                    {
                        validSquares.Add(Tuple.Create(checkRow, checkCol));
                    }
                    else
                    {
                        // squares between the checked king's position and the checking piece are valid squares because moving to them will block check
                        for (int i = 1; i < board.GetLength(0); i++)
                        {
                            Tuple<int, int> validSquare = Tuple.Create(kingRow + check.Item3 * i, kingCol + check.Item4 * i);
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

                //Console.WriteLine("Castles moves: " + string.Join(", ", castleMoves));
                validMoves.AddRange(whiteToPlay ? GetCastleMoves(whiteKingLocation) : GetCastleMoves(blackKingLocation));

            }


            if (!validMoves.Any())
            {
                if (this.in_check)
                {
                    checkmate = true;
                }
                else
                {
                    this.stalemate = true;
                }
            }
            else
            {
                this.checkmate = false;
                this.stalemate = false;
            }

            this.currentCastleRights = temp_castle_rights;
            return validMoves;
        }


        // this idea of looking for pins and checks starting from the king instead of iterating through all enemy moves is inspired from Eddie Sharick's Tutorial. 
        public (bool, List<Tuple<int, int, int, int>>, List<Tuple<int, int, int, int>>) CheckForPinsAndChecks()
        {

            var pins = new List<Tuple<int, int, int, int>>(); //squares pinned and the direction it is pinned from

            var checks = new List<Tuple<int, int, int, int>>(); //squares of the enemy pieces that are applying checks

            var in_check = false;

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

            //knight checks
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

        public bool IsInCheck()
        {
            if (this.whiteToPlay)
            {
                return IsSquareUnderAttack(this.whiteKingLocation);
            }
            else
            {
                return IsSquareUnderAttack(this.blackKingLocation);
            }
        }

        public bool IsSquareUnderAttack(Tuple<int, int> location)
        {
            //look if an opponent piece can attack the specified square
            //switch turns
            this.whiteToPlay = !this.whiteToPlay;
            var opponentMoves = this.GetAllMoves();
            //switch back turns
            this.whiteToPlay = !this.whiteToPlay;

            foreach (var move in opponentMoves)
            {
                if (move.endPosition.Equals(location))
                {
                    return true;
                }
            }


            return false;
        }


        //get castle moves for a given king's position
        public List<Move> GetCastleMoves(Tuple<int, int> location)
        {
            List<Move> castleMoves = new List<Move>();
            if (IsSquareUnderAttack(location))
            {
                return castleMoves;
            }
            if ((this.whiteToPlay && this.currentCastleRights.wKs) || (!this.whiteToPlay && this.currentCastleRights.bKs))
            {
                castleMoves.AddRange(this.GetKingsideCastleMoves(location));
            }
            if ((this.whiteToPlay && this.currentCastleRights.wQs) || (!this.whiteToPlay && this.currentCastleRights.bQs))
            {
                castleMoves.AddRange(this.GetQueensideCastleMoves(location));

            }

            return castleMoves;
        }

        //get king side castle moves for black and white
        public List<Move> GetKingsideCastleMoves(Tuple<int, int> location)
        {
            List<Move> kingsidecastlemoves = new List<Move>();
            var kingRow = location.Item1;
            var kingCol = location.Item2;
            Piece king = GetPieceAtLocation(location);

            if (kingCol == 4)
            {
                if (this.board[kingRow, kingCol + 1] == "--" && this.board[kingRow, kingCol + 2] == "--" && this.board[kingRow, kingCol + 3][1].Equals('R'))
                {
                    if (!IsSquareUnderAttack(Tuple.Create(kingRow, kingCol + 1)) && !IsSquareUnderAttack(Tuple.Create(kingRow, kingCol + 2)))
                    {

                        kingsidecastlemoves.Add(new Move(location, Tuple.Create(kingRow, kingCol + 2), board, king.id, isCastle: true));

                    }
                }
            }
            return kingsidecastlemoves;
        }


        //get queen side castle moves for black and white
        public List<Move> GetQueensideCastleMoves(Tuple<int, int> location)
        {
            List<Move> queensidecastlemoves = new List<Move>();
            var kingRow = location.Item1;
            var kingCol = location.Item2;
            Piece king = GetPieceAtLocation(location);
            if (kingCol == 4)
            {
                if (this.board[kingRow, kingCol - 1] == "--" && this.board[kingRow, kingCol - 2] == "--" && this.board[kingRow, kingCol - 3] == "--" && this.board[kingRow, kingCol - 4][1].Equals('R'))
                {
                    if (!IsSquareUnderAttack(Tuple.Create(kingRow, kingCol - 1)) && !IsSquareUnderAttack(Tuple.Create(kingRow, kingCol - 2)))
                    {

                        queensidecastlemoves.Add(new Move(location, Tuple.Create(kingRow, kingCol - 2), board, king.id, isCastle: true));

                    }
                }
            }

            return queensidecastlemoves;
        }



        public void UpdateCastleRights(Move move)
        {
            var limit = board.GetLength(0);

            if (move.pieceCaptured == "wR")
            {
                if (move.endPosition.Item2 == limit)
                { // right rook
                    this.currentCastleRights.wKs = false;
                }
                else if (move.endPosition.Item2 == 0) //left rook
                {
                    this.currentCastleRights.wQs = false;
                }
            }
            else if (move.pieceCaptured == "bR")
            {
                if (move.endPosition.Item2 == limit)
                { // right rook
                    this.currentCastleRights.bKs = false;
                }
                else if (move.endPosition.Item2 == 0) //left rook
                {
                    this.currentCastleRights.bQs = false;
                }
            }

            if (move.pieceMoved == "wR")
            {
                if (move.startPosition.Item1 == limit)
                {
                    if (move.startPosition.Item2 == limit)
                    {
                        this.currentCastleRights.wKs = false;
                    }
                    else if (move.startPosition.Item2 == 0)
                    {
                        this.currentCastleRights.wQs = false;
                    }
                }
            }
            else if (move.pieceMoved == "bR")
            {
                if (move.startPosition.Item1 == 0)
                {
                    if (move.startPosition.Item2 == limit)
                    {
                        this.currentCastleRights.bKs = false;
                    }
                    else if (move.startPosition.Item2 == 0)
                    {
                        this.currentCastleRights.bQs = false;
                    }
                }
            }
            if (move.pieceMoved == "wK")
            {
                this.currentCastleRights.wKs = false;
                this.currentCastleRights.wQs = false;
            }
            else if (move.pieceMoved == "bK")
            {
                currentCastleRights.bKs = false;
                currentCastleRights.bQs = false;
            }

        }

        public void MakeMove(Move move)
        {

            if (move != null)
            {
                this.moveHisory.Add(move);
                int movedPieceId = move.sourcePieceId;
                int capturedPieceId = move.targetPieceId;
                if (capturedPieceId != -1)
                {
                    var capturedPiece = GetPieceById(capturedPieceId);
                    capturedPiece.status = "captured";
                }

                var movedPiece = GetPieceById(movedPieceId);
                movedPiece.location = move.endPosition;
                if (movedPiece != null)
                {

                    // if opponent pawn moves 2 squares forward, then en passant capture is possible
                    if (movedPiece.pieceType == PieceType.Pawn && Math.Abs(move.startPosition.Item1 - move.endPosition.Item1) == 2)
                    {
                        this.enPassantPossible = Tuple.Create((move.endPosition.Item1 + move.startPosition.Item1) / 2, move.startPosition.Item2);
                    }
                    else
                    {
                        this.enPassantPossible = Tuple.Create(-1, -1);
                    }

                    if (move.is_castle_move)
                    {
                        if (move.endPosition.Item2 - move.startPosition.Item2 == 2) // a king side castle took place
                        {

                            var rook = GetPieceAtLocation(Tuple.Create(move.endPosition.Item1, move.endPosition.Item2 + 1));
                            rook.location = Tuple.Create(move.endPosition.Item1, move.endPosition.Item2 - 1);

                        }
                        else // a queen side caste took place
                        {

                            var rook = GetPieceAtLocation(Tuple.Create(move.endPosition.Item1, move.endPosition.Item2 - 2));
                            rook.location = Tuple.Create(move.endPosition.Item1, move.endPosition.Item2 + 1);
                        }
                    }
                    if (movedPiece.pieceType == PieceType.King)
                    {
                        if (whiteToPlay && movedPiece.pieceColor == PieceColor.White)
                        {
                            this.whiteKingLocation = move.endPosition;
                        }
                        if (!whiteToPlay && movedPiece.pieceColor == PieceColor.Black)
                        {
                            this.blackKingLocation = move.endPosition;
                        }
                    }
                    if (move.is_pawn_promotion)
                    {
                        movedPiece.status = "captured";
                        var color = movedPiece.pieceColor;
                        if (color == PieceColor.White && this.whiteToPlay)
                        {
                            this.pieces.Add(new Queen(move.endPosition.Item1, move.endPosition.Item2, PieceColor.White, ids++));
                        }
                        else if (color == PieceColor.Black && !this.whiteToPlay)
                        {
                            this.pieces.Add(new Queen(move.endPosition.Item1, move.endPosition.Item2, PieceColor.Black, ids++));
                        }
                    }
                }

                this.UpdateCastleRights(move);

                this.enpassant_possible_log.Add(enPassantPossible);
                this.castle_rights_log.Add(new CastleRights(this.currentCastleRights.wKs, this.currentCastleRights.wQs, this.currentCastleRights.bKs, this.currentCastleRights.bQs));
                this.whiteToPlay = !whiteToPlay; //switch turns
                this.board = Helpers.MapPiecesToBoard(this.pieces);


            }
        }


        public void Undo()
        {

            if (moveHisory.Any())
            {
                this.whiteToPlay = !this.whiteToPlay;
                var lastMove = moveHisory.Last();
                moveHisory.RemoveAt(moveHisory.Count - 1);
                var startPosition = lastMove.startPosition;
                var endPosition = lastMove.endPosition;
                var movedPieceId = lastMove.sourcePieceId;
                var movedPiece = GetPieceById(movedPieceId);
                var capturedPieceId = lastMove.targetPieceId;

                Piece capturedPiece;
                if (capturedPieceId != -1)
                {
                    capturedPiece = GetPieceById(capturedPieceId);
                    capturedPiece.status = "alive";
                }


                movedPiece.location = startPosition;

                if (movedPiece.pieceType == PieceType.King)
                {
                    if (movedPiece.pieceColor == PieceColor.White)
                    {
                        this.whiteKingLocation = startPosition;
                    }
                    else
                    {
                        this.blackKingLocation = startPosition;
                    }
                }


                if (lastMove.is_pawn_promotion)
                {
                    movedPiece.status = "alive";
                    var queen = GetPieceAtLocation(endPosition, "alive", PieceType.Queen, movedPiece.pieceColor);
                    this.pieces.Remove(queen);

                }

                this.enpassant_possible_log.RemoveAt(enpassant_possible_log.Count-1);
                if (enpassant_possible_log.Any())
                {
                    enPassantPossible = enpassant_possible_log[enpassant_possible_log.Count - 1];
                }
                else
                {
                    enPassantPossible = Tuple.Create(-1, -1);
                }


                this.castle_rights_log.RemoveAt(castle_rights_log.Count - 1);

                if (castle_rights_log.Any())
                {
                    currentCastleRights = new CastleRights(castle_rights_log.Last().wKs,castle_rights_log.Last().wQs, castle_rights_log.Last().bKs, castle_rights_log.Last().bQs);
                
                }


                if (lastMove.is_castle_move)
                {
                    if (lastMove.endPosition.Item2 - lastMove.startPosition.Item2 == 2)
                    {
                        var rook = GetPieceAtLocation(Tuple.Create(endPosition.Item1, endPosition.Item2 - 1));
                        rook.location = Tuple.Create(endPosition.Item1, endPosition.Item2 + 1);
                    }
                    else
                    {
                        var rook = GetPieceAtLocation(Tuple.Create(endPosition.Item1, endPosition.Item2 + 1));
                        rook.location = Tuple.Create(endPosition.Item1, endPosition.Item2 - 2);

                    }

                }
                checkmate = false;
                stalemate = false;
                this.board = Helpers.MapPiecesToBoard(pieces);
                //this.whiteToPlay = !this.whiteToPlay;
            }
        }




        public PieceType GetPieceTypeFromString(string piece)
        {
            if (piece.Equals("--"))
            {
                return PieceType.Empty;
            }
            switch (piece[1])
            {
                case 'K':
                    return PieceType.King;

                case 'B':
                    return PieceType.Bishop;

                case 'N':
                    return PieceType.Knight;
                case 'R':
                    return PieceType.Rook;
                case 'Q':
                    return PieceType.Queen;
                case 'P':
                    return PieceType.Pawn;
                default:
                    return PieceType.Empty;
            }
        }

        public Piece GetPieceAtLocation(Tuple<int, int> pieceLocation, string status, PieceType pieceType, PieceColor pieceColor)
        {
            foreach (var piece in pieces)
            {

                var location = piece.location;

                if (location.Item1 == pieceLocation.Item1 && location.Item2 == pieceLocation.Item2 && piece.status == status && piece.pieceType == pieceType && piece.pieceColor == pieceColor)
                {
                    return piece;
                }

            }
            return null;
        }
        public Piece GetPieceAtLocation(Tuple<int, int> pieceLocation, string status, PieceType pieceType)
        {
            // fetch a specified piece at a particular position, helpful especially with captured pieces that share the same position
            foreach (var piece in pieces)
            {

                var location = piece.location;

                if (location.Item1 == pieceLocation.Item1 && location.Item2 == pieceLocation.Item2 && piece.status == status && piece.pieceType == pieceType)
                {
                    return piece;
                }

            }
            return null;
        }

        public Piece GetPieceById(int id)
        {
            foreach (var piece in pieces)
            {
                if (piece.id == id)
                {
                    return piece;

                }
            }
            return null;
        }
        public Piece GetPieceAtLocation(Tuple<int, int> pieceLocation)
        {
            // fetch the piece at a specific location

            //return pieces.Where(piece => piece.status == "alive" && piece.location.Equals(pieceLocation)).FirstOrDefault();



            //Console.WriteLine(string.Join("\n", pieces));
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

        public static Move? FindMoveByUserInput(List<Move> moves, string input)
        {

            foreach (var move in moves)
            {
                if (input == "O-O" && move.pieceMoved[1].Equals('K') && move.endPosition.Item2 == 6)
                {

                    return move;
                }
                else if (input == "O-O-O" && move.pieceMoved[1].Equals('K') && move.endPosition.Item2 == 2)
                {
                    return move;
                }
            }

            var rx = new Regex(@"[a-h][1-8][a-h][1-8]", RegexOptions.Compiled);

            if (rx.IsMatch(input))
            {
                Tuple<int, int> startPos = Tuple.Create(ChessNotation.ranksToRows[input[1].ToString()], ChessNotation.filesToCols[input[0].ToString()]);
                Tuple<int, int> endPos = Tuple.Create(ChessNotation.ranksToRows[input[3].ToString()], ChessNotation.filesToCols[input[2].ToString()]);
                foreach (var move in moves)
                {
                    if (move.startPosition.Equals(startPos) && move.endPosition.Equals(endPos))
                    {
                        return move;
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

        public decimal GetScore()
        {
            // computes a decimal that represents the value of the state. Higher values mean a better state for an agent to be in. 
            // naive
            if (checkmate)
            {
                if (whiteToPlay)
                {
                    return -1000;
                }
                if (!whiteToPlay)
                {
                    return 1000;
                }
            }
            if (stalemate)
            {
                return 0;
            }

            decimal score = 0;


            foreach (var piece in pieces)
            {
                if (piece.status == "alive")
                {
                    if (whiteToPlay && piece.pieceColor == PieceColor.White)
                    {
                        score += piece.score;
                    }
                    else if (!whiteToPlay && piece.pieceColor == PieceColor.Black)
                    {
                        score -= piece.score;
                    }
                }

            }
            return score;
        }
        public bool IsDraw()
        {
            if (stalemate)
            {
                return true;
            }

            //IEnumerable<Piece> alivePieces = from piece in pieces
            //                                 where piece.status == "alive"
            //                                 select piece;

            var alivePieces = pieces.Where(piece => piece.status.Equals("alive")).ToList();
            //Console.WriteLine("alive piece count: "  + alivePieces.Count);

            switch (alivePieces.Count())
            {
                case 2:
                    if (alivePieces.ElementAt(0).pieceType == PieceType.King && alivePieces.ElementAt(1).pieceType == PieceType.King)
                    {
                        return true;
                    }
                    break;
                case 3:
                    IEnumerable<Piece> drawPieces = from piece in alivePieces
                                                    where piece.pieceType == PieceType.King || piece.pieceType == PieceType.Bishop || piece.pieceType == PieceType.Knight
                                                    select piece;
                    if (drawPieces.Count() == 3)
                    {
                        return true;
                    }
                    break;
                default:
                    break;

            }
            return false;
        }
        public bool GameOver() //From white 
        {

            return checkmate || IsDraw();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    public class Pawn : Piece
    {

  
        public Pawn(int row, int col, PieceColor color, int id) : base(row, col, color,id)
        {
            pieceType = PieceType.Pawn;
            score = 1;
        }

        //public void InitializePawn(int row, int col, PieceColor color)
        //{
        //    base.Initialize(row, col, color);
        //    pieceType = PieceType.Pawn;
        //    score = 1;
        //}

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {
            List<Move> possiblePawnMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;

            bool piecePinned = false;

            Tuple<int, int>? pinDirection = null;

            for (int i = gamestate.pins.Count - 1; i >= 0; i--)
            {
                var pin = gamestate.pins[i];
                if (row == pin.Item1 && col == pin.Item2)
                {
                    piecePinned = true;
                    pinDirection = Tuple.Create(pin.Item3, pin.Item4);
                    gamestate.pins.Remove(gamestate.pins[i]); //to avoid duplicates because CheckForChecksAndPins() will be called at each move.
                    break;
                }
            }

            //white pawn move
            if (gamestate.whiteToPlay && this.pieceColor == PieceColor.White)
            {
                //Console.WriteLine(this);
                //Console.WriteLine(gamestate);
                if (board[row - 1, col] == "--")
                {

                    if (!piecePinned || pinDirection.Equals(new Tuple<int, int>(-1, 0)))

                    {
                        //move 1 square forward
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col), board, this.id));
                        //if (row == 5 && col == 4) Console.WriteLine(string.Join(",", possiblePawnMoves));

                        //move 2 squares forward
                        if (row == 6 && board[row - 2, col] == "--")
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 2, col), board, this.id));
                        }
                    }
                }

                //capture left
                if (col > 0)
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(-1, -1)))
                    {

                        if (board[row - 1, col - 1][0].Equals('b'))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row - 1, col - 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board, this.id, target.id));
                            //Console.WriteLine("pawn move here 1: " + new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board, this, target));
                        }
                        if (Tuple.Create(row - 1, col - 1).Equals(gamestate.enPassantPossible) && board[row, col - 1][0].Equals('b'))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row, col - 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board, this.id, target.id, isEnPassant: true));
                            //Console.WriteLine("pawn move here 2: " + new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board, this, target, isEnPassant: true));

                        }
                    }
                }

                //capture right
                if (col < 7)
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(-1, 1)))
                    {
                        if (board[row - 1, col + 1][0].Equals('b'))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row - 1, col + 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 1), board, this.id, target.id));
                        }
                        if (Tuple.Create(row - 1, col + 1).Equals(gamestate.enPassantPossible) && board[row, col + 1][0].Equals('b'))
                        {
                            //Console.WriteLine("hani houni felabyedh");
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row, col + 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 1), board, this.id, target.id, isEnPassant: true));

                        }
                    }
                }
            }

            //black pawn move
            else if (!gamestate.whiteToPlay && this.pieceColor == PieceColor.Black)
            {

                if (board[row + 1, col] == "--")
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(1, 0)))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col), board, this.id));
                        if (row == 1 && board[row + 2, col] == "--")
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 2, col), board, this.id));
                        }
                    }
                }

                //capture left
                if (col > 0)
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(1, -1)))
                    {
                        if (board[row + 1, col - 1][0].Equals('w'))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row + 1, col - 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 1), board, this.id, target.id));
                        }
                        if (Tuple.Create(row + 1, col - 1).Equals(gamestate.enPassantPossible) && board[row, col -1][0].Equals('w'))
                        {

                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row, col - 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 1), board, this.id, target.id, isEnPassant: true));

                        }
                    }
                }

                //capture right
                if (col < 7)
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(1, 1)))
                    {
                        if (board[row + 1, col + 1][0].Equals('w'))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row + 1, col + 1));
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 1), board, this.id, target.id));
                        }
                        if (Tuple.Create(row + 1, col + 1).Equals(gamestate.enPassantPossible) && board[row, col + 1][0].Equals('w'))
                        {
                            //Console.WriteLine("hani houni felak7el");
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row, col + 1) );
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 1), board, this.id, target.id, isEnPassant: true));

                        }
                    }
                }


            }
            //Console.WriteLine("Pawn Move: " + String.Join(",", possiblePawnMoves));
            return possiblePawnMoves;
        }
    }
}

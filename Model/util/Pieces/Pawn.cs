using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    internal class Pawn : Piece
    {

        public Pawn(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.Pawn;
        }

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

            if (gamestate.whiteToPlay && this.pieceColor == PieceColor.White)
            {

                if (board[row - 1, col] == "--")
                {
                    //if (row == 5 && col == 4) Console.WriteLine("9bal");
                  
                    if (!piecePinned || pinDirection.Equals(new Tuple<int, int>(-1, 0)))
                  
                    {
                        //move 1 square forward
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col), board));
                        //if (row == 5 && col == 4) Console.WriteLine(string.Join(",", possiblePawnMoves));

                        //move 2 squares forward
                        if (row == 6 && board[row - 2, col] == "--")
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 2, col), board));
                        }
                    }
                }

                //capture left
                if (col > 0)
                {
                    if (board[row - 1, col - 1][0].Equals('b'))
                    {
                        if (!piecePinned || pinDirection.Equals(Tuple.Create(-1, -1)))
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col - 1), board));
                        }
                    }
                }

                //capture right
                if (col < 7)
                {
                    if (board[row - 1, col + 1][0].Equals('b'))
                    {
                        if (!piecePinned || pinDirection.Equals(Tuple.Create(-1, 1)))
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row - 1, col + 1), board));
                        }
                    }
                }
            }
            if (!gamestate.whiteToPlay && this.pieceColor == PieceColor.Black)
            {

                if (board[row + 1, col] == "--")
                {
                    if (!piecePinned || pinDirection.Equals(Tuple.Create(1, 0)))
                    {
                        possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col), board));
                        if (row == 1 && board[row + 2, col] == "--")
                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 2, col), board));
                        }
                    }
                }

                //capture left
                if (col > 0)
                {
                    if (board[row + 1, col - 1][0].Equals('w'))
                    {

                        if (!piecePinned || pinDirection.Equals(Tuple.Create(1, -1)))
                        {

                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col - 1), board));

                        }
                    }
                }

                //capture right
                if (col < 7)
                {
                    if (board[row + 1, col + 1][0].Equals('w'))
                    {
                        if (!piecePinned || pinDirection.Equals(Tuple.Create(1, 1)))

                        {
                            possiblePawnMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + 1, col + 1), board));
                        }

                    }
                }


            }
            return possiblePawnMoves;
        }
    }
}

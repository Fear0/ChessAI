using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    public class Queen : Piece
    {
   
        public Queen(int row, int col, PieceColor color, int id) : base(row, col, color, id)
        {
            pieceType = PieceType.Queen;
            score = 9;
        }

        //public void InitializeQueen(int row, int col, PieceColor color)
        //{
        //    base.Initialize(row, col, color);
        //    pieceType = PieceType.Queen;
        //    score = 9;

        //}

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {

            /* It looks more elegant to use this commented code because it just generates bishop moves and rook moves. However it is less efficient because
             * it uses 2 while loops instead of 1.

            //Bishop bishop = new Bishop(this.location.Item1, this.location.Item2, color);
            //var rookMoves = rook.GetPossibleMoves(gamestate);
            //Console.WriteLine($"Rook moves:  {string.Join(",", rookMoves)}");
            //var bishopMoves = bishop.GetPossibleMoves(gamestate);
            //Console.WriteLine($"Bishop moves:  {string.Join(",", bishopMoves)}");
            //List<Move> queenMoves = new List<Move>();
            //queenMoves.AddRange(rookMoves);
            //queenMoves.AddRange(bishopMoves);

            */

            List<Move> possibleQueenMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';
            char fellow = gamestate.whiteToPlay ? 'w' : 'b';


            int i; //our index for the moves

            bool piecePinned = false;

            Tuple<int, int>? pinDirection = null;

            for (i = gamestate.pins.Count - 1; i >= 0; i--)
            {
                var pin = gamestate.pins[i];
                if (row == pin.Item1 && col == pin.Item2)
                {
                    piecePinned = true;
                    pinDirection = Tuple.Create(pin.Item3, pin.Item4);
                    //gamestate.pins.Remove(gamestate.pins[i]); //to avoid duplicates because CheckForChecksAndPins() will be called at each move.
                    gamestate.pins.Remove(gamestate.pins[i]);
                    break;
                }
            }


            // queens move in all directions
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, -1), Tuple.Create(1, 1), Tuple.Create(0, 1), Tuple.Create(0, -1), Tuple.Create(1, 0), Tuple.Create(-1, 0) };


            foreach (var direction in directions)
            {
                i = 1;
                if (0 <= row + direction.Item1 * i && row + direction.Item1 * i <= limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
                {

                    while (0 <= row + direction.Item1 * i && row + direction.Item1 * i <= limit && 0 <= col + direction.Item2 * i && col + direction.Item2 * i <= limit)
                    {

                        if (!piecePinned || direction.Equals(pinDirection) || direction.Equals(Tuple.Create(-pinDirection.Item1, -pinDirection.Item2)))
                        {

                            //check if path is blocked by ally piece 
                            if (board[row + direction.Item1 * i, col + direction.Item2 * i][0].Equals(fellow))
                            {
                                break;
                            }
                            if (board[row + direction.Item1 * i, col + direction.Item2 * i] == "--")
                            {
                                possibleQueenMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i), board, this.id));

                                // if capture occured, no move along the same path is valid


                            }
                            else if (board[row + direction.Item1 * i, col + direction.Item2 * i][0].Equals(opponent))
                            {
                                Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i));
                                possibleQueenMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1 * i, col + direction.Item2 * i), board, this.id, target.id));
                                break;
                            }
                        }
                        i++;
                    }
                }

            }



            return possibleQueenMoves;












            //PieceColor color = gamestate.whiteToPlay ? PieceColor.White : PieceColor.Black;
            //// actually the color doesnt matter, bishops and rooks are the same regardless the color.
            ////Rook rook = new Rook();
            ////rook.Initialize(this.location.Item1, this.location.Item2, color);
            //Rook rook = new Rook(this.location.Item1, this.location.Item2, color);
            ////Bishop bishop = new Bishop();
            ////bishop.Initialize(this.location.Item1, this.location.Item2, color);
            //Bishop bishop = new Bishop(this.location.Item1, this.location.Item2, color);
            //var rookMoves = rook.GetPossibleMoves(gamestate);
            //Console.WriteLine($"Rook moves:  {string.Join(",", rookMoves)}");
            //var bishopMoves = bishop.GetPossibleMoves(gamestate);
            //Console.WriteLine($"Bishop moves:  {string.Join(",", bishopMoves)}");
            //List<Move> queenMoves = new List<Move>();
            //queenMoves.AddRange(rookMoves);
            //queenMoves.AddRange(bishopMoves);
            //Console.WriteLine(string.Join(",", queenMoves));
            //foreach (var move in queenMoves)
            //{
            //    move.source.pieceType = PieceType.Queen;
            //    move.source.pieceColor = color;

            //}
        }
    }
}

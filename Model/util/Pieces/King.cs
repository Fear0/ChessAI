using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    public class King : Piece
    {

      
        public King(int row, int col, PieceColor color) : base(row, col, color)
        {
            pieceType = PieceType.King;
        }

        //public void InitializeKing(int row, int col, PieceColor color)
        //{
        //    base.Initialize(row, col, color);
        //    pieceType = PieceType.King;

        //}

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {

            List<Move> possibleKingMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';
            char fellow = gamestate.whiteToPlay ? 'w' : 'b';

            //a king has 8 possible moves, which are the surrounding squares. (north,south,east,west,northeast,northwest,southeast,southwest)
            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-1, 0), Tuple.Create(1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1), Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, 1), Tuple.Create(1, -1) };

            //north
            int i;

            foreach (var direction in directions)
            {
                i = 1;

                var endRow = row + direction.Item1 * i;
                var endCol = col + direction.Item2 * i;

                if (0 <= endRow && endRow <= limit && 0 <= endCol && endCol <= limit)
                {
                    if (board[row + direction.Item1 * i, col + direction.Item2 * i] == "--" || board[row + direction.Item1 * i, col + direction.Item2 * i][0].Equals(opponent))
                    {
                        // we put the king in one of the possible squares
                        if (fellow == 'w')
                        {
                            gamestate.whiteKingLocation = Tuple.Create(endRow, endCol);
                        }
                        else
                        {
                            gamestate.blackKingLocation = Tuple.Create(endRow, endCol);
                        }

                        var checkAndPinLog = gamestate.CheckForPinsAndChecks();

                        if (!checkAndPinLog.Item1) // if putting the king on one of the possible squares doesnt lead to a check, then we have a valid king move
                        {
                            if (board[row + direction.Item1 * i, col + direction.Item2 * i][0].Equals(opponent))
                            {
                                Piece target = gamestate.GetPieceAtLocation(Tuple.Create(endRow, endCol));
                                possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(endRow, endCol), board, this,target));
                            } else
                            {
                                possibleKingMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(endRow, endCol), board, this));
                            }
                        }

                        // reinstate original location
                        if (fellow == 'w')
                        {
                            gamestate.whiteKingLocation = Tuple.Create(row, col);
                        }
                        else
                        {
                            gamestate.blackKingLocation = Tuple.Create(row, col);
                        }
                    }
                }

            }


            return possibleKingMoves;
        }
    }

}

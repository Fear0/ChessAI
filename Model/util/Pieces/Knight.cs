﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util.Pieces
{
    public class Knight : Piece
    {


        public Knight(int row, int col, PieceColor color, int id) : base(row, col, color,id)
        {
            pieceType = PieceType.Knight;
            score = 3;
        }

        //public void InitializeKnight(int row, int col, PieceColor color)
        //{
        //    base.Initialize(row, col, color);
        //    pieceType = PieceType.Knight;
        //    score = 3;
        //}

        public override List<Move> GetPossibleMoves(GameState gamestate)
        {
            List<Move> possibleKnightMoves = new List<Move>();

            var location = this.location;
            var row = location.Item1;
            var col = location.Item2;
            var board = gamestate.board;
            int limit = board.GetLength(0) - 1;
            char opponent = gamestate.whiteToPlay ? 'b' : 'w';

            bool piecePinned = false;


            for (int i = gamestate.pins.Count - 1; i >= 0; i--)
            {
                var pin = gamestate.pins[i];
                if (row == pin.Item1 && col == pin.Item2)
                {
                    piecePinned = true;
                    gamestate.pins.Remove(gamestate.pins[i]); //to avoid duplicates because CheckForChecksAndPins() will be called at each move.
                    break;
                }
            }

            List<Tuple<int, int>> directions = new List<Tuple<int, int>>() { Tuple.Create(-2, 1), Tuple.Create(-1, 2), Tuple.Create(-1, -2), Tuple.Create(-2, -1), Tuple.Create(1, 2), Tuple.Create(2, 1), Tuple.Create(2, -1), Tuple.Create(1, -2) };
            // knights move like an L. You can rotate an L 8 different ways.

            foreach (var direction in directions)
            {
                if (0 <= row + direction.Item1 && row + direction.Item1 <= limit && 0 <= col + direction.Item2 && col + direction.Item2 <= limit)
                {
                    if (!piecePinned)
                    {
                        if (board[row + direction.Item1, col + direction.Item2] == "--")
                        {
                            possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1, col + direction.Item2), board,this.id));
                        }
                        if (board[row + direction.Item1, col + direction.Item2][0].Equals(opponent))
                        {
                            Piece target = gamestate.GetPieceAtLocation(Tuple.Create(row + direction.Item1, col + direction.Item2));
                            possibleKnightMoves.Add(new Move(Tuple.Create(row, col), Tuple.Create(row + direction.Item1, col + direction.Item2), board, this.id, target.id));

                        }
                    }
                }
            }


            return possibleKnightMoves;
        }
    }
}

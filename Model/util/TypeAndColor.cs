﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util
{
    public enum PieceType
    {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn,
        Empty
    }

    public enum PieceColor
    {
        Black = -1,
        White = 1
    }
}

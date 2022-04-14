using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util
{
    internal class ChessNotation
    {
        public static Dictionary<string, int> ranksToRows => new Dictionary<string, int>() {

            {"1", 7},
            {"2", 6},
            { "3",5},
            { "4",4},
            { "5",3},
            { "6",2},
            { "7",1},
            { "8",0}


            };
        public static Dictionary<int, string> rowsToRanks => ranksToRows.ToDictionary(x => x.Value, x => x.Key);

        public static Dictionary<string, int> filesToCols => new Dictionary<string, int>()
        {
            {"a", 0},
            {"b", 1},
            { "c",2},
            { "d",3},
            { "e",4},
            { "f",5},
            { "g",6},
            { "h",7}
        };

        public static Dictionary<int, string> colsToFiles => filesToCols.ToDictionary(x => x.Value, x => x.Key);

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in colsToFiles)

        //        sb.Append(item.ToString());

        //    return sb.ToString();
        //}

        /// <summary>
        /// gets the corresponding chess position using ranks and files
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetChessPosition(int row, int col)
        {
            return colsToFiles[col] + rowsToRanks[row];
        }

        /// <summary>
        /// uses ranks and files to log the move
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static string GetChessNotation(Move move)
        {

            var startPosition = move.startPosition;
            var endPosition = move.endPosition;
            return new StringBuilder().Append(GetChessPosition(startPosition.Item1, startPosition.Item2) + GetChessPosition(endPosition.Item1, endPosition.Item2)).ToString();
        }


    }
}

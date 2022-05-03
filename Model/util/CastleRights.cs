using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.Model.util
{
    public class CastleRights
    {
        public bool wKs { get; set; }
        public bool wQs { get; set; }
        public bool bKs { get; set; }
        public bool bQs { get; set; }

        public CastleRights(bool wks, bool wqs, bool bks, bool bqs)
        {
            this.wKs = wks;
            this.wQs = wqs;
            this.bKs = bks;
            this.bQs = bqs;
        }
    }
}

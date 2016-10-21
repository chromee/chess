using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    class PieceFundation
    {
        public playerColorType playerColor;
        public pieceType pieceType;

        public class MovePattern
        {
            public int horizontal;
            public int vertical;
        }

        public List<MovePattern> movePattern = new List<MovePattern>();

        public PieceFundation(pieceType firstpieceType, playerColorType pColor)
        {
            playerColor = pColor;
            pieceType = firstpieceType;
        }

        public void setMovePattern(int horizontalMove, int verticalMove)
        {
            MovePattern mp = new MovePattern();
            mp.horizontal = horizontalMove;
            mp.vertical = verticalMove;
            movePattern.Add(mp);
        }
    }
}

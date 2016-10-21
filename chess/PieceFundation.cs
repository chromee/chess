using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class PieceFundation
    {
        public playerColorType playerColor;
        public pieceType pieceType;
        public int HorizontalPosition;
        public int VerticalPosition;

        public PictureBox Picture = new PictureBox();
        public bool moveFlag;

        public PieceFundation(pieceType pType, playerColorType pColor, int hor, int ver, PictureBox pic)
        {
            playerColor = pColor;
            pieceType = pType;
            HorizontalPosition = hor;
            VerticalPosition = ver;
            Picture = pic;
            moveFlag = false;
        }

        public class MovePattern
        {
            public int horizontal;
            public int vertical;
        }

        public List<MovePattern> movePattern = new List<MovePattern>();

        public void setMovePattern(int horizontalMove, int verticalMove)
        {
            MovePattern mp = new MovePattern();
            mp.horizontal = horizontalMove;
            mp.vertical = verticalMove;
            movePattern.Add(mp);
        }
    }
}

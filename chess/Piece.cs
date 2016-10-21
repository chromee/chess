using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class Piece
    {
        public playerColorType playerColor { get; set; }
        public pieceType pieceType { get; set; }
        public int HorizontalPosition;      //配列で使う値
        public int VerticalPosition;           //配列で使う値
        public bool moveFlag;
        public PictureBox Picture = new PictureBox();

        public Piece(pieceType pType, playerColorType pColor, int hor, int ver, PictureBox pic)
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

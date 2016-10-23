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
        public Cross position;
        public Image image;
        public bool moveFlag;

        private Board board;


        public Piece(pieceType pType, playerColorType pColor, Cross c, Board b)
        {
            playerColor = pColor;
            pieceType = pType;
            position = c;
            moveFlag = false;
           
            board = b;
            setPiecePos();
        }

        private void setPiecePos()
        {
            var re = new System.Text.RegularExpressions.Regex(@"[^*a-z_]");
            var pTypeStr = re.Replace(pieceType.ToString(), "");
            //image = System.Drawing.Image.FromFile($@"D:\Program Files\OneDrive\VisualProject\private\chess\chess\piece\{pColor}_{pTypeStr}.png");
            image = System.Drawing.Image.FromFile($@"D:\Program Files\OneDrive\VisualProject\private\chess\chess\piece\{pTypeStr}.png");
            board.square[position.x, position.y].button.BackgroundImage = image;
        }

        //移動パターン
        public List<Cross> movePattern = new List<Cross>();
        public void setMovePattern(int horMove, int verMove)
        {
            movePattern.Add(new Cross(horMove, verMove));
        }
    }
}

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
        public Vector2 position;
        public Image image;
        public bool moveFlag;

        private Board board;


        public Piece(pieceType pType, playerColorType pColor, Vector2 c, Board b)
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
            //image = System.Drawing.Image.FromFile($@"D:\Program Files\OneDrive\VisualProject\private\chess\chess\piece\{pTypeStr}.png");
            image = System.Drawing.Image.FromFile($@"C:\Users\odk\OneDrive\VisualProject\private\chess\chess\piece\{pTypeStr}.png");
            board.square[position.x, position.y].button.BackgroundImage = image;
        }

        //移動パターン
        public List<Vector2> movePattern = new List<Vector2>();
        public void setMovePattern(int horMove, int verMove)
        {
            movePattern.Add(new Vector2(horMove, verMove));
        }
    }
}

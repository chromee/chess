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
        public pieceColorType pieceColor { get; set; }
        public pieceType pieceType { get; set; }
        public Vector2 position;
        public Image image;
        public List<Vector2> movePatterns = new List<Vector2>();

        private Board board;


        public Piece(pieceType pType, pieceColorType pColor, Vector2 c, Board b)
        {
            pieceColor = pColor;
            pieceType = pType;
            position = c;

            board = b;
            setPiecePos();
        }

        private void setPiecePos()
        {
            var re = new System.Text.RegularExpressions.Regex(@"[^*a-z_]");
            image = System.Drawing.Image.FromFile($@"D:\Program Files\OneDrive\VisualProject\private\chess\chess\piece\{pieceColor}_{pieceType}.png");
            //image = System.Drawing.Image.FromFile($@"C:\Users\odk\OneDrive\VisualProject\private\chess\chess\piece\{pieceColor}_{pieceType}.png");
            if (board.square[position.x, position.y] != null)
            {
                board.square[position.x, position.y].button.BackgroundImage = image;
            }

        }

        //移動パターン
        
        public void setMovePattern(int horMove, int verMove)
        {
            movePatterns.Add(new Vector2(horMove, verMove));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

enum pieceType { king, queen, rook, bishop, knight, pawn }
enum playerColorType { white, black }

namespace chess
{
    class PieceSet
    {
        playerColorType playerColor;
        Panel panel;
        Form form;

        private int pieceSize = 60;
        private int piecePadding = 75;
        private int horizonPos = 0;
        private int verPos = 0;

        public static Piece king;
        //PieceFundation queen;
        //PieceFundation rook;
        //PieceFundation bishop;
        //PieceFundation knight;
        //PieceFundation pawn;

        public PieceSet(playerColorType playerCol, Panel boardPanel, Form f)
        {
            playerColor = playerCol;
            panel = boardPanel;
            form = f;
            setPiece();
        }

        private void setPiece()
        {
            PictureBox kingPic = new PictureBox();
            setPictureSetting(kingPic);
            king = new Piece(pieceType.king, playerColor, horizonPos, verPos, kingPic);
            king.setMovePattern(-1, 1);
            king.setMovePattern(0, 1);
            king.setMovePattern(1, 1);
            king.setMovePattern(1, 0);
            king.setMovePattern(-1, 0);
            king.setMovePattern(-1, -1);
            king.setMovePattern(0, -1);
            king.setMovePattern(-1, 1);

        }

        private void setPictureSetting(PictureBox pic)
        {
            pic.Height = pieceSize;
            pic.Width = pieceSize;
            pic.Top = piecePadding;
            pic.Left = piecePadding;
            pic.Image = System.Drawing.Image.FromFile(@"D:\Program Files\OneDrive\VisualProject\private\chess\chess\piece\black_king.png");
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.Transparent;
            pic.Click += new EventHandler(piece_Click);
            panel.Controls.Add(pic);
            //form.Controls.Add(pic);
        }

        /// <summary>
        /// イベント関数
        /// </summary>

        private void piece_Click(object sender, EventArgs e)
        {
            king.moveFlag = true;
        }
    }
}

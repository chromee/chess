﻿using System;
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
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var imageDirectory = currentDirectory.Remove(currentDirectory.Length -10) + $@"\piece\{pieceColor}_{pieceType}.png";
            image = System.Drawing.Image.FromFile(imageDirectory);
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

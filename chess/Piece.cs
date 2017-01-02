using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class Piece
    {
        public PieceColor pieceColor;
        public PieceType pieceType;
        public Vector2 position;
        public Image image;
        public List<Vector2> movePatterns = new List<Vector2>();


        public Piece(PieceType pType, PieceColor pColor, Vector2 c)
        {
            pieceColor = pColor;
            pieceType = pType;
            position = c;

            setPiecePos();
        }

        private void setPiecePos()
        {
            setPieceImage();
            if (Board.squares[position.x, position.y] != null)
            {
                Board.squares[position.x, position.y].button.BackgroundImage = image;
            }
        }

        public void setPieceImage()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var imageDirectory = currentDirectory.Remove(currentDirectory.Length - 10) + $@"\piece\{pieceColor}_{pieceType}.png";
            image = System.Drawing.Image.FromFile(imageDirectory);
        }

        //移動パターン設定
        public void setMovePattern(int horMove, int verMove)
        {
            movePatterns.Add(new Vector2(horMove, verMove));
        }
        
        public void move(Square moveSquare)
        {
            Board.squares[position.x, position.y].button.BackgroundImage = null;
            position = new Vector2(moveSquare.position.x, moveSquare.position.y);
            moveSquare.button.BackgroundImage = image;
            moveSquare.button.BackgroundImageLayout = ImageLayout.Zoom;

            //ポーンは最初だけ２マス進める(チェスの仕様)
            if (pieceType == PieceType.pawn && movePatterns.Count == 2)
                movePatterns.RemoveAt(1);

            //ポーンが一番奥まできたらクイーンになるやつ
            if (pieceType == PieceType.pawn && pieceColor == PieceColor.white && position.y == 8)
                pawnChangeToQueen();
            if (pieceType == PieceType.pawn && pieceColor == PieceColor.black && position.y == 1)
                pawnChangeToQueen();
        }

        public void pawnChangeToQueen()
        {
            pieceType = PieceType.queen;
            setPieceImage();
            Board.squares[position.x, position.y].button.BackgroundImage = image;
            for (int i = 1; i < 9; i++)
            {
                setMovePattern(-1 * i, 0 * i);
                setMovePattern(-1 * i, 1 * i);
                setMovePattern(0 * i, 1 * i);
                setMovePattern(1 * i, 1 * i);
                setMovePattern(1 * i, 0 * i);
                setMovePattern(1 * i, -1 * i);
                setMovePattern(0 * i, -1 * i);
                setMovePattern(-1 * i, -1 * i);
            }
        }

        public bool isEnemy(Piece piece)
        {
            return pieceColor != piece.pieceColor;
        }
        
    }
}

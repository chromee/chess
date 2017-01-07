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
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (position != null)
                    Board.squares[position.x, position.y].button.BackgroundImage = null;
                if(value.IsInsideBoard())
                    Board.squares[value.x, value.y].button.BackgroundImage = image;

                position = value;
                if (pieceType == PieceType.pawn && ReachLastLine(value))
                    PawnChangeToQueen();
            }
        }
        public Image image;
        public List<Vector2> movePatterns = new List<Vector2>();


        public Piece(PieceType pType, PieceColor pColor, Vector2 pos)
        {
            pieceColor = pColor;
            pieceType = pType;
            setImage();
            Position = pos;
        }

        private void setImage()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var imageDirectory = currentDirectory.Remove(currentDirectory.Length - 10) + $@"\piece\{pieceColor}_{pieceType}.png";
            image = System.Drawing.Image.FromFile(imageDirectory);
        }

        private void PawnChangeToQueen()
        {
            pieceType = PieceType.queen;
            setImage();
            Board.squares[Position.x, Position.y].button.BackgroundImage = image;
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

        private bool ReachLastLine(Vector2 pos)
        {
            return (pieceColor == PieceColor.white && pos.y == 8) || (pieceColor == PieceColor.black && pos.y == 1);
        }

        #region public関数
        public void move(Square moveSquare)
        {
            Position = new Vector2(moveSquare.position.x, moveSquare.position.y);

            //ポーンは最初だけ２マス進めるやつ
            if (pieceType == PieceType.pawn && movePatterns.Count == 2)
                movePatterns.RemoveAt(1);
        }

        public void setMovePattern(int horMove, int verMove)
        {
            movePatterns.Add(new Vector2(horMove, verMove));
        }

        public bool isEnemy(Piece piece)
        {
            return pieceColor != piece.pieceColor;
        }

        public bool isExist(Vector2 pos)
        {
            return pos.x == Position.x && pos.y == Position.y;
        }

        public bool isLongMoveable()
        {
            return pieceType == PieceType.bishop || pieceType == PieceType.queen || pieceType == PieceType.rook;
        }
        #endregion

    }
}

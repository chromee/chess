using System;
using System.Collections.Generic;
using System.Linq;
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
                if (value.IsInsideBoard())
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
            SetImage();
            Position = pos;
        }

        private void SetImage()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var imageDirectory = currentDirectory.Remove(currentDirectory.Length - 10) + $@"\piece\{pieceColor}_{pieceType}.png";
            image = System.Drawing.Image.FromFile(imageDirectory);
        }

        private void PawnChangeToQueen()
        {
            pieceType = PieceType.queen;
            SetImage();
            Board.squares[Position.x, Position.y].SetImage(image);
            movePatterns.Clear();
            for (int i = 1; i < 9; i++)
            {
                SetMovePattern(-1 * i, 0 * i);
                SetMovePattern(-1 * i, 1 * i);
                SetMovePattern(0 * i, 1 * i);
                SetMovePattern(1 * i, 1 * i);
                SetMovePattern(1 * i, 0 * i);
                SetMovePattern(1 * i, -1 * i);
                SetMovePattern(0 * i, -1 * i);
                SetMovePattern(-1 * i, -1 * i);
            }
        }

        private bool ReachLastLine(Vector2 pos)
        {
            return (pieceColor == PieceColor.white && pos.y == 8) || (pieceColor == PieceColor.black && pos.y == 1);
        }


        #region 移動まわり
        public void Move(Vector2 pos)
        {
            Position = pos;

            var enemy = Board.pieces.Find(piece => piece.position == pos && IsEnemy(piece));
            if (enemy != null)
                enemy.position = new Vector2(0, 0);

            //ポーンは最初だけ２マス進めるやつ
            if (pieceType == PieceType.pawn && movePatterns.Count == 2)
                movePatterns.RemoveAt(1);
        }

        public void ApplyMoveableSquares()
        {
            Board.ResetMoveableSquares();
            if (pieceType == PieceType.pawn)
                ApplyPawnMoveableSquares();
            else
            {
                foreach (var movePattern in movePatterns)
                {
                    int moveToPosX = Position.x + movePattern.x;
                    int moveToPosY = Position.y + movePattern.y;
                    bool inBoard = Vector2.IsInsideBoard(moveToPosX, moveToPosY);
                    if (inBoard)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                }
                ApplyUnMoveablequares();
            }
        }

        public void ApplyMoveableSquares(List<Piece> pieces)
        {
            Board.ResetMoveableSquares();
            if (pieceType == PieceType.pawn)
                ApplyPawnMoveableSquares(pieces);
            else
            {
                foreach (var movePattern in movePatterns)
                {
                    int moveToPosX = Position.x + movePattern.x;
                    int moveToPosY = Position.y + movePattern.y;
                    bool inBoard = Vector2.IsInsideBoard(moveToPosX, moveToPosY);
                    if (inBoard)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                }
                ApplyUnMoveablequares();
            }
        }

        private void ApplyPawnMoveableSquares()
        {
            int moveDirection = pieceColor == PieceColor.white ? 1 : -1;

            foreach (var movePattern in movePatterns)
            {
                int moveToPosX = Position.x + movePattern.x;
                int moveToPosY = Position.y + movePattern.y;

                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard)
                {
                    //正面の1,2マスに駒が存在しない場合のみ移動可能領域に設定
                    bool isPieceOnFront = Board.pieces
                        .Any(p => p.Position == new Vector2(Position.x, Position.y + moveDirection));
                    bool isPieceOnTwoFront = Board.pieces
                        .Any(p => p.Position == new Vector2(Position.x, Position.y + moveDirection * 2));
                    if (!isPieceOnFront && Math.Abs(movePattern.y) == 1)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                    if (!isPieceOnFront && !isPieceOnTwoFront && Math.Abs(movePattern.y) == 2)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                }
            }

            //ポーンの斜めに敵駒があったら斜め方向のマスを移動可能領域に追加
            bool isPieceOnRightFront = Board.pieces
                    .Any(p => p.Position == new Vector2(Position.x + 1, Position.y + moveDirection) && IsEnemy(p));
            bool isPieceOnLeftFront = Board.pieces
                    .Any(p => p.Position == new Vector2(Position.x - 1, Position.y + moveDirection) && IsEnemy(p));
            if (isPieceOnRightFront)
                Board.squares[Position.x + 1, Position.y + moveDirection].ToMoveable();
            if (isPieceOnLeftFront)
                Board.squares[Position.x - 1, Position.y + moveDirection].ToMoveable();
        }

        private void ApplyPawnMoveableSquares(List<Piece> pieces)
        {
            int moveDirection = pieceColor == PieceColor.white ? 1 : -1;

            foreach (var movePattern in movePatterns)
            {
                int moveToPosX = Position.x + movePattern.x;
                int moveToPosY = Position.y + movePattern.y;

                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard)
                {
                    //正面の1,2マスに駒が存在しない場合のみ移動可能領域に設定
                    bool isPieceOnFront = pieces
                        .Any(p => p.Position == new Vector2(Position.x, Position.y + moveDirection));
                    bool isPieceOnTwoFront = pieces
                        .Any(p => p.Position == new Vector2(Position.x, Position.y + moveDirection * 2));
                    if (!isPieceOnFront && Math.Abs(movePattern.y) == 1)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                    if (!isPieceOnFront && !isPieceOnTwoFront && Math.Abs(movePattern.y) == 2)
                        Board.squares[moveToPosX, moveToPosY].ToMoveable();
                }
            }

            //ポーンの斜めに敵駒があったら斜め方向のマスを移動可能領域に追加
            bool isPieceOnRightFront = pieces
                    .Any(p => p.Position == new Vector2(Position.x + 1, Position.y + moveDirection) && IsEnemy(p));
            bool isPieceOnLeftFront = pieces
                    .Any(p => p.Position == new Vector2(Position.x - 1, Position.y + moveDirection) && IsEnemy(p));
            if (isPieceOnRightFront)
                Board.squares[Position.x + 1, Position.y + moveDirection].ToMoveable();
            if (isPieceOnLeftFront)
                Board.squares[Position.x - 1, Position.y + moveDirection].ToMoveable();
        }

        private void ApplyUnMoveablequares()
        {
            var moveableSquares = Board.GetMoveableSquares();

            if (IsLongMoveable())     //bishop rook queen
            {
                foreach (var moveableSquare in moveableSquares)
                {
                    if (moveableSquare.IsMoveable)
                    {
                        Piece pieceOnMoveSquare = Board.pieces.Find(p => p.Position == moveableSquare.position);

                        if (pieceOnMoveSquare != null)
                        {
                            Vector2 relativePos = new Vector2(pieceOnMoveSquare.Position.x - Position.x, pieceOnMoveSquare.Position.y - Position.y);

                            #region 縦方向
                            if (relativePos.x == 0)
                            {
                                if (relativePos.y > 0)
                                    for (int i = pieceOnMoveSquare.Position.y; i < 9; i++)
                                        Board.squares[pieceOnMoveSquare.Position.x, i].ToUnMoveable();
                                else if (relativePos.y < 0)
                                    for (int i = pieceOnMoveSquare.Position.y; i > 0; i--)
                                        Board.squares[pieceOnMoveSquare.Position.x, i].ToUnMoveable();
                            }
                            #endregion
                            #region 横方向
                            else if (relativePos.y == 0)
                            {
                                if (relativePos.x > 0)
                                    for (int i = pieceOnMoveSquare.Position.x; i < 9; i++)
                                        Board.squares[i, pieceOnMoveSquare.Position.y].ToUnMoveable();
                                else if (relativePos.x < 0)
                                    for (int i = pieceOnMoveSquare.Position.x; i > 0; i--)
                                        Board.squares[i, pieceOnMoveSquare.Position.y].ToUnMoveable();
                            }
                            #endregion
                            #region 右斜め上方向
                            else if ((relativePos.x > 0 && relativePos.y > 0) || (relativePos.x < 0 && relativePos.y < 0))
                            {
                                if (relativePos.x > 0 && relativePos.y > 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x < pieceOnMoveSquare.Position.y)
                                        for (int y = pieceOnMoveSquare.Position.y; y < 9; y++)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y + i].ToUnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x < 9; x++)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y + i].ToUnMoveable();
                                            i++;
                                        }
                                }
                                else if (relativePos.x < 0 && relativePos.y < 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x > pieceOnMoveSquare.Position.y)
                                        for (int y = pieceOnMoveSquare.Position.y; y > 0; y--)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y - i].ToUnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x > 0; x--)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y - i].ToUnMoveable();
                                            i++;
                                        }
                                }
                            }
                            #endregion
                            #region 左斜め上方向
                            else if ((relativePos.x < 0 && relativePos.y > 0) || (relativePos.x > 0 && relativePos.y < 0))
                            {
                                //左斜め上からのboardの中線(1,8) => (8,1)のラインが判定の分かれ目
                                //左上に上る場合，中線より左側はx=0がy=9より先にくる，中線より右側はy=9がx=0より先に来る。
                                if (relativePos.x < 0 && relativePos.y > 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x + pieceOnMoveSquare.Position.y < 9)
                                        for (int x = pieceOnMoveSquare.Position.x; x > 0; x--)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y + i].ToUnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int y = pieceOnMoveSquare.Position.y; y < 9; y++)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y + i].ToUnMoveable();
                                            i++;
                                        }
                                }
                                else if (relativePos.x > 0 && relativePos.y < 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x + pieceOnMoveSquare.Position.y < 9)
                                        for (int y = pieceOnMoveSquare.Position.y; y > 0; y--)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y - i].ToUnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x < 9; x++)
                                        {
                                            Board.squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y - i].ToUnMoveable();
                                            i++;
                                        }
                                }
                            }
                            #endregion

                            if (IsEnemy(pieceOnMoveSquare))
                                Board.squares[pieceOnMoveSquare.Position.x, pieceOnMoveSquare.Position.y].ToMoveable();
                        }
                    }
                }
            }
            else  //king, knight, pawn
            {
                foreach (var moveableSquare in moveableSquares)
                {
                    bool isExistAlly = Board.pieces.Any(piece => piece.Position == moveableSquare.position && !IsEnemy(piece));
                    if (isExistAlly)
                        moveableSquare.ToUnMoveable();
                }
            }
        }

        #endregion

        #region public関数
        public void SetMovePattern(int horMove, int verMove)
        {
            movePatterns.Add(new Vector2(horMove, verMove));
        }

        public bool IsEnemy(Piece piece)
        {
            return pieceColor != piece.pieceColor;
        }

        public bool IsLongMoveable()
        {
            return pieceType == PieceType.bishop || pieceType == PieceType.queen || pieceType == PieceType.rook;
        }

        public bool IsAlive()
        {
            return position != Vector2.Zero();
        }

        public int GetTypePoint()
        {
            switch (pieceType)
            {
                case PieceType.pawn:
                    return 1;
                case PieceType.knight:
                    return 3;
                case PieceType.rook:
                    return 5;
                case PieceType.bishop:
                    return 5;
                case PieceType.queen:
                    return 10;
                case PieceType.king:
                    return 100;
                default:
                    return 0;
            }
        }
        #endregion

    }
}

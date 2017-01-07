using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reactive.Linq;

namespace chess
{
    class Board
    {
        Form form;

        public static List<Piece> pieces = new List<Piece>();
        public static Square[,] squares = new Square[9, 9];

        private PieceColor turnPlayerColor;
        private Label turnLabel = new Label();

        Piece beforeSelectedPiece = null;

        public Board(Form f)
        {
            form = f;
            turnPlayerColor = PieceColor.white;
            CreateBoard();
            SetLabel();
            SetPieces();
        }

        private void Square_Click(object sender, EventArgs e)
        {
            for (int selectedX = 1; selectedX < 9; selectedX++)
            {
                for (int selectedY = 1; selectedY < 9; selectedY++)
                {
                    var selectedSquare = squares[selectedX, selectedY];
                    if (sender.Equals(selectedSquare.button))
                        SelectSquare(selectedSquare);
                }
            }
        }

        #region"盤面初期化まわり"
        private int squareSize = 70;
        private int boardTopPadding = 70;
        private int boardLeftPadding = 20;

        private void CreateBoard()
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = new Button();
                    squares[x, y] = new Square(new Vector2(x, y), btn);
                    SetSquareButtonProperties(btn, x, y);
                    form.Controls.Add(btn);
                }
            }
        }

        private void SetSquareButtonProperties(Button btn, int x, int y)
        {
            btn.Top = squareSize * 8 - squareSize * y + boardTopPadding;
            btn.Left = squareSize * x + boardLeftPadding;
            btn.Width = squareSize;
            btn.Height = squareSize;
            if ((x + y) % 2 == 0)
                btn.BackColor = Color.LightYellow;
            else
                btn.BackColor = Color.Tan;
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Click += new EventHandler(Square_Click);
        }

        private void SetPieces()
        {
            PieceSet whitePieces = new PieceSet(PieceColor.white);
            pieces.AddRange(whitePieces.pieces);
            PieceSet blackPieces = new PieceSet(PieceColor.black);
            pieces.AddRange(blackPieces.pieces);
        }

        private void SetLabel()
        {
            turnLabel.Top = 0;
            turnLabel.Left = 230;
            turnLabel.Width = 300;
            turnLabel.Height = 75;
            turnLabel.Text = $"{turnPlayerColor} turn";
            turnLabel.TextAlign = ContentAlignment.MiddleCenter;
            turnLabel.Font = new Font(turnLabel.Font.OriginalFontName, 30);
            form.Controls.Add(turnLabel);
        }
        #endregion

        #region "駒の移動まわり"
        private void SelectSquare(Square selectedSquare)
        {
            var selectedPiece = pieces.Find(p => p.Position.x == selectedSquare.position.x && p.Position.y == selectedSquare.position.y);

            bool isSelectPiece = selectedPiece != null;
            bool isSelectSquare = selectedPiece == null;
            bool isSelectedPiece = beforeSelectedPiece != null;

            //pieace選択
            if (isSelectPiece && !isSelectedPiece && selectedPiece.pieceColor == turnPlayerColor)
            {
                beforeSelectedPiece = selectedPiece;
                ApplyMoveableSquares(selectedPiece);
            }
            //pieace選択 -> square選択
            else if (isSelectedPiece && isSelectSquare)
            {
                if (selectedSquare.isMoveable)
                {
                    beforeSelectedPiece.move(selectedSquare);
                    changeTurn(beforeSelectedPiece);
                    beforeSelectedPiece = null;
                    ResetMoveableSquares();
                }
            }
            //pieace選択 -> 敵pieace選択
            else if (isSelectedPiece && selectedPiece.pieceColor != beforeSelectedPiece.pieceColor)
            {
                if (selectedSquare.isMoveable)
                {
                    selectedPiece.Position = new Vector2(0, 0);
                    beforeSelectedPiece.move(selectedSquare);

                    if (selectedPiece.pieceType == PieceType.king)
                        WinJudge(turnPlayerColor);
                    else
                    {
                        changeTurn(beforeSelectedPiece);
                        ResetMoveableSquares();
                        beforeSelectedPiece = null;
                    }
                }
            }
            //pieace選択 -> 味方pieace選択
            else if (isSelectedPiece && !beforeSelectedPiece.isEnemy(selectedPiece))
            {
                ResetMoveableSquares();
                ApplyMoveableSquares(selectedPiece);
                beforeSelectedPiece = selectedPiece;
            }
        }

        private void ApplyMoveableSquares(Piece selectedPiece)
        {
            if (selectedPiece.pieceType == PieceType.pawn)
                ApplyPawnMoveableSquares(selectedPiece);
            else
            {
                foreach (var movePattern in selectedPiece.movePatterns)
                {
                    int moveToPosX = selectedPiece.Position.x + movePattern.x;
                    int moveToPosY = selectedPiece.Position.y + movePattern.y;
                    bool inBoard = Vector2.IsInsideBoard(moveToPosX, moveToPosY);
                    if (inBoard)
                        squares[moveToPosX, moveToPosY].Moveable();
                }
                ApplyUnMoveablequares(selectedPiece);
            }
        }

        private void ApplyPawnMoveableSquares(Piece selectedPawn)
        {
            int moveDirection = selectedPawn.pieceColor == PieceColor.white ? 1 : -1;

            foreach (var movePattern in selectedPawn.movePatterns)
            {
                int moveToPosX = selectedPawn.Position.x + movePattern.x;
                int moveToPosY = selectedPawn.Position.y + movePattern.y;

                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard)
                {
                    //正面の1,2マスに駒が存在しない場合のみ移動可能領域に設定
                    bool isPieceOnFront = pieces
                        .Any(p => p.Position == new Vector2(selectedPawn.Position.x, selectedPawn.Position.y + moveDirection));
                    bool isPieceOnTwoFront = pieces
                            .Any(p => p.Position == new Vector2(selectedPawn.Position.x, selectedPawn.Position.y + moveDirection * 2));
                    if (!isPieceOnFront && Math.Abs(movePattern.y) == 1)
                        squares[moveToPosX, moveToPosY].Moveable();
                    if (!isPieceOnFront && !isPieceOnTwoFront && Math.Abs(movePattern.y) == 2)
                        squares[moveToPosX, moveToPosY].Moveable();
                }
            }

            //ポーンの斜めに敵駒があったら斜め方向のマスを移動可能領域に追加
            bool isPieceOnRightFront = pieces
                    .Any(p => (p.Position == new Vector2(selectedPawn.Position.x + 1, selectedPawn.Position.y + moveDirection) && selectedPawn.isEnemy(p)));
            bool isPieceOnLeftFront = pieces
                    .Any(p => (p.Position == new Vector2(selectedPawn.Position.x - 1, selectedPawn.Position.y + moveDirection) && selectedPawn.isEnemy(p)));
            if (isPieceOnRightFront)
                squares[selectedPawn.Position.x + 1, selectedPawn.Position.y + moveDirection].Moveable();
            if (isPieceOnLeftFront)
                squares[selectedPawn.Position.x - 1, selectedPawn.Position.y + moveDirection].Moveable();
        }

        private void ApplyUnMoveablequares(Piece selectedPiece)
        {
            var moveableSquares = GetMoveableSquares();

            if (selectedPiece.isLongMoveable())     //bishop rook queen
            {
                foreach (var moveableSquare in moveableSquares)
                {
                    if (moveableSquare.isMoveable)
                    {
                        Piece pieceOnMoveSquare = pieces.Find(p => p.Position == moveableSquare.position);

                        if (pieceOnMoveSquare != null)
                        {
                            Vector2 relativePos = new Vector2(pieceOnMoveSquare.Position.x - selectedPiece.Position.x, pieceOnMoveSquare.Position.y - selectedPiece.Position.y);

                            #region 縦方向
                            if (relativePos.x == 0)
                            {
                                if (relativePos.y > 0)
                                    for (int i = pieceOnMoveSquare.Position.y; i < 9; i++)
                                        squares[pieceOnMoveSquare.Position.x, i].UnMoveable();
                                else if (relativePos.y < 0)
                                    for (int i = pieceOnMoveSquare.Position.y; i > 0; i--)
                                        squares[pieceOnMoveSquare.Position.x, i].UnMoveable();
                            }
                            #endregion
                            #region 横方向
                            else if (relativePos.y == 0)
                            {
                                if (relativePos.x > 0)
                                    for (int i = pieceOnMoveSquare.Position.x; i < 9; i++)
                                        squares[i, pieceOnMoveSquare.Position.y].UnMoveable();
                                else if (relativePos.x < 0)
                                    for (int i = pieceOnMoveSquare.Position.x; i > 0; i--)
                                        squares[i, pieceOnMoveSquare.Position.y].UnMoveable();
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
                                            squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y + i].UnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x < 9; x++)
                                        {
                                            squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y + i].UnMoveable();
                                            i++;
                                        }
                                }
                                else if (relativePos.x < 0 && relativePos.y < 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x > pieceOnMoveSquare.Position.y)
                                        for (int y = pieceOnMoveSquare.Position.y; y > 0; y--)
                                        {
                                            squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y - i].UnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x > 0; x--)
                                        {
                                            squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y - i].UnMoveable();
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
                                            squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y + i].UnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int y = pieceOnMoveSquare.Position.y; y < 9; y++)
                                        {
                                            squares[pieceOnMoveSquare.Position.x - i, pieceOnMoveSquare.Position.y + i].UnMoveable();
                                            i++;
                                        }
                                }
                                else if (relativePos.x > 0 && relativePos.y < 0)
                                {
                                    var i = 0;
                                    if (pieceOnMoveSquare.Position.x + pieceOnMoveSquare.Position.y < 9)
                                        for (int y = pieceOnMoveSquare.Position.y; y > 0; y--)
                                        {
                                            squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y - i].UnMoveable();
                                            i++;
                                        }
                                    else
                                        for (int x = pieceOnMoveSquare.Position.x; x < 9; x++)
                                        {
                                            squares[pieceOnMoveSquare.Position.x + i, pieceOnMoveSquare.Position.y - i].UnMoveable();
                                            i++;
                                        }
                                }
                            }
                            #endregion

                            if (pieceOnMoveSquare.isEnemy(selectedPiece))
                                squares[pieceOnMoveSquare.Position.x, pieceOnMoveSquare.Position.y].Moveable();
                        }
                    }
                }
            }
            else  //king, knight, pawn
            {
                foreach (var moveableSquare in moveableSquares)
                {
                    bool isExistAlly = pieces.Any(piece => piece.Position == moveableSquare.position && !piece.isEnemy(selectedPiece));
                    if (isExistAlly)
                        moveableSquare.UnMoveable();
                }
            }
        }

        private void ResetMoveableSquares()
        {
            var moveableSquares = GetMoveableSquares();
            foreach (var square in moveableSquares)
                square.UnMoveable();
        }

        private List<Square> GetMoveableSquares()
        {
            var moveableSquares = new List<Square>();
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    if (squares[x, y].isMoveable)
                        moveableSquares.Add(squares[x, y]);
                }
            }
            return moveableSquares;
        }

        #endregion

        #region "ゲームシステムまわり"
        private void changeTurn(Piece beforeSelectedPiece)
        {
            turnPlayerColor = beforeSelectedPiece.pieceColor == PieceColor.white ? PieceColor.black : PieceColor.white;
            turnLabel.Text = $"{turnPlayerColor} turn";
        }

        private void WinJudge(PieceColor winPlayerColor)
        {
            string JudgeText = winPlayerColor == PieceColor.white ? "白の勝ち" : "黒の勝ち";
            MessageBox.Show($"{JudgeText}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            resetBoard();
        }

        private void resetBoard()
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = squares[x, y].button;
                    if ((x + y) % 2 == 0)
                        btn.BackColor = Color.LightYellow;
                    else
                        btn.BackColor = Color.Tan;
                    btn.BackgroundImage = null;
                    btn.BackgroundImageLayout = ImageLayout.Zoom;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            pieces.Clear();
            SetPieces();
        }

        private void message(string text)
        {
            MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}

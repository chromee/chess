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
        Form firstForm;
        Panel panel;

        public PieceSet pieceSet;

        Label[] labelHorizontal;
        Label[] labelVertical;
        public static Square[,] square;

        private pieceColor turnPlayerColor;
        private Label turnLabel;

        public Board(Panel p, Form form, Label L)
        {
            panel = p;
            firstForm = form;
            turnLabel = L;
            setPanelSetting(panel);
            createBoard();
            turnPlayerColor = pieceColor.white;
        }

        public void setPieces(PieceSet p)
        {
            pieceSet = p;
        }

        /// <summary>
        /// イベント関数
        /// </summary>
        Piece beforeSelectedPiece = null;
        List<Square> canMoveSquares = new List<Square>();
        private void board_Click(object sender, EventArgs e)
        {
            Observable.Return(1).Subscribe(n =>
            {
                MessageBox.Show($"{n}やぞ");
            });

            for (int selectedX = 1; selectedX < 9; selectedX++)
            {
                for (int selectedY = 1; selectedY < 9; selectedY++)
                {
                    var selectedSquare = square[selectedX, selectedY];
                    if (sender.Equals(selectedSquare.button))
                    {
                        var selectedPiece = pieceSet.pieces.Find(p => p.position.x == selectedX && p.position.y == selectedY);

                        bool isSelectPiece = selectedPiece != null;
                        bool isSelectSquare = selectedPiece == null;
                        bool isSelectedPiece = beforeSelectedPiece != null;

                        //pieace選択
                        if (isSelectPiece && !isSelectedPiece && selectedPiece.pieceColor == turnPlayerColor)
                        {
                            beforeSelectedPiece = selectedPiece;
                            setCanMoveSquares(selectedPiece);
                        }
                        //pieace選択 -> square選択
                        else if (isSelectedPiece && isSelectSquare)
                        {
                            bool isSelectCanMoveSquare = canMoveSquares.Any(s => s.position == selectedSquare.position);
                            if (isSelectCanMoveSquare)
                            {
                                movePiece(beforeSelectedPiece, selectedSquare);
                                changeTurn(beforeSelectedPiece);
                                beforeSelectedPiece = null;
                                resetCanMoveSquares(canMoveSquares);
                            }
                        }
                        //pieace選択 -> 敵pieace選択
                        else if (isSelectedPiece && selectedPiece.pieceColor != beforeSelectedPiece.pieceColor)
                        {
                            bool isSelectCanMoveSquare = canMoveSquares.Any(s => s.position == selectedSquare.position);
                            if (isSelectCanMoveSquare)
                            {
                                selectedPiece.position = new Vector2(0, 0);
                                movePiece(beforeSelectedPiece, selectedSquare);
                                if (selectedPiece.pieceType == pieceType.king)
                                {
                                    string winPlayer = turnPlayerColor == pieceColor.white ? "白の勝ち" : "黒の勝ち";
                                    MessageBox.Show($"{winPlayer}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    resetBoard();
                                }
                                else
                                {
                                    changeTurn(beforeSelectedPiece);
                                    resetCanMoveSquares(canMoveSquares);
                                    beforeSelectedPiece = null;
                                }
                            }
                        }
                        //pieace選択 -> 味方pieace選択
                        else if (isSelectedPiece && selectedPiece.pieceColor == beforeSelectedPiece.pieceColor)
                        {
                            resetCanMoveSquares(canMoveSquares);
                            setCanMoveSquares(selectedPiece);
                            beforeSelectedPiece = selectedPiece;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// プライベート関数
        /// </summary>

        private void setPanelSetting(Panel boardPanel)
        {
            boardPanel.Size = firstForm.Size;
            boardPanel.Top = 80;
            boardPanel.Left = 25;
            boardPanel.AutoScrollPosition = firstForm.AutoScrollPosition;
            firstForm.Controls.Add(boardPanel);
        }

        private int squareSize = 70;
        private int squarePadding = 70;

        private void createBoard()
        {
            square = new Square[9, 9];
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = new Button();
                    square[x, y] = new Square(new Vector2(x, y), btn);
                    setSquareButton(btn, x, y);
                    setHorizontalLabel();
                    setVerticalLabel();
                    panel.Controls.Add(btn);
                }
            }
        }

        private void setSquareButton(Button btn, int x, int y)
        {
            btn.Top = squareSize * 8 - squareSize * y;
            btn.Left = squareSize * x - squareSize + squarePadding;
            btn.Width = squareSize;
            btn.Height = squareSize;
            if ((x + y) % 2 == 0)
                btn.BackColor = Color.LightYellow;
            else
                btn.BackColor = Color.Tan;
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Click += new EventHandler(board_Click);
        }

        private void setHorizontalLabel()
        {
            labelHorizontal = new Label[9];
            for (int i = 1; i < labelHorizontal.Length; i++)
            {
                // ラベルのインスタンスを生成
                labelHorizontal[i] = new Label();
                // プロパティを設定
                labelHorizontal[i].Left = squareSize * i;
                labelHorizontal[i].Top = squareSize * 8;
                labelHorizontal[i].Width = squareSize;
                labelHorizontal[i].Height = squareSize;
                labelHorizontal[i].BorderStyle = BorderStyle.FixedSingle;
                labelHorizontal[i].BackColor = Color.White;
                labelHorizontal[i].BorderStyle = BorderStyle.None;
                labelHorizontal[i].TextAlign = ContentAlignment.MiddleCenter;
                labelHorizontal[i].Text = (i).ToString();
                // フォームに追加する
                panel.Controls.Add(labelHorizontal[i]);
            }
        }

        private void setVerticalLabel()
        {
            labelVertical = new Label[9];
            for (int j = 1; j < labelVertical.Length; j++)
            {
                // ラベルのインスタンスを生成
                labelVertical[j] = new Label();
                // プロパティを設定
                labelVertical[j].Left = 0;
                labelVertical[j].Top = squareSize * 8 - squareSize * j;
                labelVertical[j].Width = squareSize;
                labelVertical[j].Height = squareSize;
                labelVertical[j].BorderStyle = BorderStyle.FixedSingle;
                labelVertical[j].BackColor = Color.White;
                labelVertical[j].BorderStyle = BorderStyle.None;
                labelVertical[j].TextAlign = ContentAlignment.MiddleCenter;
                labelVertical[j].Text = (j).ToString();
                // フォームに追加する
                panel.Controls.Add(labelVertical[j]);
            }
        }

        private void setCanMoveSquares(Piece selectedPiece)
        {
            foreach (var movePattern in selectedPiece.movePatterns)
            {
                int moveToPosX = selectedPiece.position.x + movePattern.x;
                int moveToPosY = selectedPiece.position.y + movePattern.y;
                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard && selectedPiece.pieceType != pieceType.pawn)
                {
                    canMoveSquares.Add(square[moveToPosX, moveToPosY]);
                    square[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
                }
                if (selectedPiece.pieceType == pieceType.pawn)
                {
                    pawnMoveSquares(selectedPiece, movePattern.x, movePattern.y);
                }
            }
            removeCannotMovesquares(selectedPiece);
        }

        private void pawnMoveSquares(Piece selectedPiece, int movePatternX, int movePatternY)
        {
            int moveToPosX = selectedPiece.position.x + movePatternX;
            int moveToPosY = selectedPiece.position.y + movePatternY;

            int moveDirection = 0;
            if (selectedPiece.pieceColor == pieceColor.white)
                moveDirection = 1;
            else if (selectedPiece.pieceColor == pieceColor.black)
                moveDirection = -1;

            //ポーンの斜めに敵駒があったら斜め方向のマスを移動可能領域に追加
            bool isPieceOnRightFront = pieceSet.pieces
                    .Any(p => (p.position.x == selectedPiece.position.x + 1 && p.position.y == selectedPiece.position.y + moveDirection && p.pieceColor != selectedPiece.pieceColor));
            bool isPieceOnLeftFront = pieceSet.pieces
                    .Any(p => (p.position.x == selectedPiece.position.x - 1 && p.position.y == selectedPiece.position.y + moveDirection && p.pieceColor != selectedPiece.pieceColor));
            if (isPieceOnRightFront)
            {
                canMoveSquares.Add(square[selectedPiece.position.x + 1, selectedPiece.position.y + moveDirection]);
                square[selectedPiece.position.x + 1, selectedPiece.position.y + moveDirection].button.BackColor = Color.Salmon;
            }
            if (isPieceOnLeftFront)
            {
                canMoveSquares.Add(square[selectedPiece.position.x - 1, selectedPiece.position.y + moveDirection]);
                square[selectedPiece.position.x - 1, selectedPiece.position.y + moveDirection].button.BackColor = Color.Salmon;
            }

            //正面の駒が存在する場合は正面のマスを移動可能領域から除外
            bool isPieceOnFront = pieceSet.pieces
                    .Any(p => (p.position.x == selectedPiece.position.x && p.position.y == selectedPiece.position.y + moveDirection));
            bool isPieceOnTwoFront = pieceSet.pieces
                    .Any(p => (p.position.x == selectedPiece.position.x && p.position.y == selectedPiece.position.y + moveDirection * 2));
            if (!isPieceOnFront && (movePatternY == 1 || movePatternY == -1))
            {
                canMoveSquares.Add(square[moveToPosX, moveToPosY]);
                square[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
            }
            if (!isPieceOnFront && !isPieceOnTwoFront && (movePatternY == 2 || movePatternY == -2))
            {
                canMoveSquares.Add(square[moveToPosX, moveToPosY]);
                square[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
            }
        }

        private void removeCannotMovesquares(Piece selectedPiece)
        {
            //クイーンとかの一気に移動できる駒が駒を飛び越えて移動できるのをなんとかする
            List<Square> removeSquares = new List<Square>();
            if (selectedPiece.pieceType == pieceType.bishop || selectedPiece.pieceType == pieceType.queen || selectedPiece.pieceType == pieceType.rook)
            {
                foreach (var canMoveSquare in canMoveSquares)
                {
                    Piece PieceOnMoveSquare = pieceSet.pieces.Find(p => p.position.x == canMoveSquare.position.x && p.position.y == canMoveSquare.position.y);

                    if (PieceOnMoveSquare != null)
                    {
                        Vector2 relativePos = new Vector2(PieceOnMoveSquare.position.x - selectedPiece.position.x, PieceOnMoveSquare.position.y - selectedPiece.position.y);

                        //縦方向
                        if (relativePos.x == 0)
                        {
                            if (relativePos.y > 0)
                                for (int i = PieceOnMoveSquare.position.y; i < 9; i++)
                                    removeSquares.Add(square[PieceOnMoveSquare.position.x, i]);
                            else if (relativePos.y < 0)
                                for (int i = PieceOnMoveSquare.position.y; i > 0; i--)
                                    removeSquares.Add(square[PieceOnMoveSquare.position.x, i]);
                        }
                        //横方向
                        else if (relativePos.y == 0)
                        {
                            if (relativePos.x > 0)
                                for (int i = PieceOnMoveSquare.position.x; i < 9; i++)
                                    removeSquares.Add(square[i, PieceOnMoveSquare.position.y]);
                            else if (relativePos.x < 0)
                                for (int i = PieceOnMoveSquare.position.x; i > 0; i--)
                                    removeSquares.Add(square[i, PieceOnMoveSquare.position.y]);
                        }
                        //右斜め上方向
                        else if ((relativePos.x > 0 && relativePos.y > 0) || (relativePos.x < 0 && relativePos.y < 0))
                        {
                            if (relativePos.x > 0 && relativePos.y > 0)
                            {
                                var i = 0;
                                if (PieceOnMoveSquare.position.x < PieceOnMoveSquare.position.y)
                                    for (int y = PieceOnMoveSquare.position.y; y < 9; y++)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x + i, PieceOnMoveSquare.position.y + i]);
                                        i++;
                                    }
                                else
                                    for (int x = PieceOnMoveSquare.position.x; x < 9; x++)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x + i, PieceOnMoveSquare.position.y + i]);
                                        i++;
                                    }
                            }
                            else if (relativePos.x < 0 && relativePos.y < 0)
                            {
                                var i = 0;
                                if (PieceOnMoveSquare.position.x > PieceOnMoveSquare.position.y)
                                    for (int y = PieceOnMoveSquare.position.y; y > 0; y--)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x - i, PieceOnMoveSquare.position.y - i]);
                                        i++;
                                    }
                                else
                                    for (int x = PieceOnMoveSquare.position.x; x > 0; x--)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x - i, PieceOnMoveSquare.position.y - i]);
                                        i++;
                                    }
                            }
                        }
                        //左斜め上方向
                        else if ((relativePos.x < 0 && relativePos.y > 0) || (relativePos.x > 0 && relativePos.y < 0))
                        {
                            //左斜め上からのboardの中線(1,8) => (8,1)のラインが判定の分かれ目
                            //左上に上る場合，中線より左側はx=0がy=9より先にくる，中線より右側はy=9がx=0より先に来る。
                            if (relativePos.x < 0 && relativePos.y > 0)
                            {
                                var i = 0;
                                if (PieceOnMoveSquare.position.x + PieceOnMoveSquare.position.y < 9)
                                    for (int x = PieceOnMoveSquare.position.x; x > 0; x--)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x - i, PieceOnMoveSquare.position.y + i]);
                                        i++;
                                    }
                                else
                                    for (int y = PieceOnMoveSquare.position.y; y < 9; y++)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x - i, PieceOnMoveSquare.position.y + i]);
                                        i++;
                                    }
                            }
                            else if (relativePos.x > 0 && relativePos.y < 0)
                            {
                                var i = 0;
                                if (PieceOnMoveSquare.position.x + PieceOnMoveSquare.position.y < 9)
                                    for (int y = PieceOnMoveSquare.position.y; y > 0; y--)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x + i, PieceOnMoveSquare.position.y - i]);
                                        i++;
                                    }
                                else
                                    for (int x = PieceOnMoveSquare.position.x; x < 9; x++)
                                    {
                                        removeSquares.Add(square[PieceOnMoveSquare.position.x + i, PieceOnMoveSquare.position.y - i]);
                                        i++;
                                    }
                            }
                        }
                        if (PieceOnMoveSquare.pieceColor != selectedPiece.pieceColor)
                            removeSquares.Remove(square[PieceOnMoveSquare.position.x, PieceOnMoveSquare.position.y]);
                    }
                }
            }
            else  //king, knight, pawn
            {
                foreach (var canMoveSquare in canMoveSquares)
                {
                    if (pieceSet.pieces.Any(p => p.position.x == canMoveSquare.position.x && p.position.y == canMoveSquare.position.y && p.pieceColor == selectedPiece.pieceColor))
                        removeSquares.Add(canMoveSquare);
                }
            }

            //MessageBox.Show($"{}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            foreach (var removeSquare in removeSquares)
            {
                //MessageBox.Show($"{removeSquare.position.x}, {removeSquare.position.y}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                removeSquare.resetColor();
                canMoveSquares.Remove(removeSquare);
            }
            removeSquares.Clear();
        }

        private void resetCanMoveSquares(List<Square> resetSquares)
        {
            foreach (var square in resetSquares)
            {
                square.resetColor();
            }
            resetSquares.Clear();
        }

        private void movePiece(Piece movePiece, Square selectedSquare)
        {
            square[movePiece.position.x, movePiece.position.y].button.BackgroundImage = null;
            movePiece.position = new Vector2(selectedSquare.position.x, selectedSquare.position.y);
            selectedSquare.button.BackgroundImage = movePiece.image;
            selectedSquare.button.BackgroundImageLayout = ImageLayout.Zoom;

            //ポーンは最初だけ２マス進める(チェスの仕様)
            if (movePiece.pieceType == pieceType.pawn && movePiece.movePatterns.Count == 2)
            {
                movePiece.movePatterns.RemoveAt(1);
            }

            //ポーンが一番奥まできたらクイーンになるやつ
            if (movePiece.pieceType == pieceType.pawn && movePiece.pieceColor == pieceColor.white && movePiece.position.y == 8)
            {
                pawnChangeToQueen(movePiece);
            }
            if (movePiece.pieceType == pieceType.pawn && movePiece.pieceColor == pieceColor.black && movePiece.position.y == 0)
            {
                pawnChangeToQueen(movePiece);
            }
        }

        private void changeTurn(Piece beforeSelectedPiece)
        {
            turnPlayerColor = beforeSelectedPiece.pieceColor == pieceColor.white ? pieceColor.black : pieceColor.white;
            turnLabel.Text = $"{turnPlayerColor} turn";
        }

        private void pawnChangeToQueen(Piece pawn)
        {
            pawn.pieceType = pieceType.queen;
            pawn.setPieceImage();
            square[pawn.position.x, pawn.position.y].button.BackgroundImage = pawn.image;
            for (int i = 1; i < 9; i++)
            {
                pawn.setMovePattern(-1 * i, 0 * i);
                pawn.setMovePattern(-1 * i, 1 * i);
                pawn.setMovePattern(0 * i, 1 * i);
                pawn.setMovePattern(1 * i, 1 * i);
                pawn.setMovePattern(1 * i, 0 * i);
                pawn.setMovePattern(1 * i, -1 * i);
                pawn.setMovePattern(0 * i, -1 * i);
                pawn.setMovePattern(-1 * i, -1 * i);
            }
        }

        private void resetBoard()
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = square[x, y].button;
                    if ((x + y) % 2 == 0)
                        btn.BackColor = Color.LightYellow;
                    else
                        btn.BackColor = Color.Tan;
                    btn.BackgroundImage = null;
                    btn.BackgroundImageLayout = ImageLayout.Zoom;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            pieceSet.pieces.Clear();
            pieceSet.setPieces(pieceColor.white);
            pieceSet.setPieces(pieceColor.black);
            pieceSet.setPiecesMovePattern();
            turnPlayerColor = pieceColor.white;
        }
    }
}

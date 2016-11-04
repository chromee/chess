using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    class Board
    {
        Form firstForm;
        Panel panel;
        TextBox textBox1;
        TextBox textBox2;

        public PieceSet pieceSet;

        Label[] labelHorizontal;
        Label[] labelVertical;
        public Square[,] BoardSquare;

        public Board(Panel p, Form form, TextBox tb1, TextBox tb2)
        {
            panel = p;
            firstForm = form;
            setPanelSetting(panel);
            createBoard();

            textBox1 = tb1;
            textBox2 = tb2;
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
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var selectedSquare = BoardSquare[x, y];
                    if (sender.Equals(selectedSquare.button))
                    {
                        var piece = pieceSet.pieces.Find(p => p.position.x == x && p.position.y == y);

                        bool isSelectPiece = piece != null;
                        bool isSelectSquare = piece == null;
                        bool isSelectedPiece = beforeSelectedPiece != null;

                        //pieace選択
                        if (isSelectPiece && !isSelectedPiece)
                        {
                            beforeSelectedPiece = piece;
                            setCanMoveSquares(piece, x, y);
                        }
                        //pieace選択 -> square選択
                        else if (isSelectedPiece && isSelectSquare)
                        {
                            bool isSelectCanMoveSquare = canMoveSquares.Any(s => s.position == selectedSquare.position);
                            if (isSelectCanMoveSquare)
                            {
                                movePiece(beforeSelectedPiece, selectedSquare);
                                beforeSelectedPiece = null;
                                resetCanMoveSquares(canMoveSquares);
                            }
                        }
                        //pieace選択 -> 敵pieace選択
                        else if (isSelectedPiece && piece.pieceColor != beforeSelectedPiece.pieceColor)
                        {
                            bool isSelectCanMoveSquare = canMoveSquares.Any(s => s.position == selectedSquare.position);
                            if (isSelectCanMoveSquare)
                            {
                                piece.position = new Vector2(0, 0);
                                movePiece(beforeSelectedPiece, selectedSquare);
                                resetCanMoveSquares(canMoveSquares);
                                beforeSelectedPiece = null;
                            }
                        }
                        //pieace選択 -> 味方pieace選択
                        else if (isSelectedPiece && piece.pieceColor == beforeSelectedPiece.pieceColor)
                        {
                            resetCanMoveSquares(canMoveSquares);
                            setCanMoveSquares(piece, x, y);
                            beforeSelectedPiece = piece;
                        }

                        //デバッグ関係
                        string pInfo = $"pieceInfo : ";
                        if (piece != null && beforeSelectedPiece != null)
                        {
                            pInfo = $"{piece.pieceColor}_{piece.pieceType}, before : {beforeSelectedPiece.pieceColor}_{beforeSelectedPiece.pieceType}";
                        }
                        //textBox1.Text = $"{selectedSquare.position.x}, {selectedSquare.position.y}";
                        //textBox1.Text = $"{square.GetLength(0)}, {square.GetLength(1)}";

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

        private void createBoard()
        {
            BoardSquare = new Square[9, 9];
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = new Button();
                    BoardSquare[x, y] = new Square(new Vector2(x, y), btn);
                    setSquareButtonSetting(btn, x, y);
                    setHorizontalLabel();
                    setVerticalLabel();
                    panel.Controls.Add(btn);
                }
            }
        }

        private int squareSize = 70;
        private int squarePadding = 70;

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

        private void setSquareButtonSetting(Button btn, int x, int y)
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

        private void movePiece(Piece movePiece, Square selectedSquare)
        {
            BoardSquare[movePiece.position.x, movePiece.position.y].button.BackgroundImage = null;
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

        private void pawnChangeToQueen(Piece pawn)
        {
            pawn.pieceType = pieceType.queen;
            pawn.setPieceImage();
            BoardSquare[pawn.position.x, pawn.position.y].button.BackgroundImage = pawn.image;
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

        private void setCanMoveSquares(Piece selectedPiece, int x, int y)
        {
            foreach (var movePattern in selectedPiece.movePatterns)
            {
                int moveToPosX = x + movePattern.x;
                int moveToPosY = y + movePattern.y;
                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard && selectedPiece.pieceType != pieceType.pawn)
                {
                    canMoveSquares.Add(BoardSquare[moveToPosX, moveToPosY]);
                    BoardSquare[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
                }
                pawnMoveSquares(selectedPiece, moveToPosX, moveToPosY);               
                removeCannotMovesquares(selectedPiece, x, y);
            }

        }

        private void pawnMoveSquares(Piece selectedPiece, int moveToPosX, int moveToPosY)
        {
            //ポーンは斜め前の敵だけ倒せるやつ
            if (selectedPiece.pieceType == pieceType.pawn)
            {
                int moveDirection = 0;
                if (selectedPiece.pieceColor == pieceColor.white)
                    moveDirection = 1;
                else if (selectedPiece.pieceColor == pieceColor.black)
                    moveDirection = -1;

                bool isPieceOnRightFront = pieceSet.pieces
                        .Any(p => (p.position.x == selectedPiece.position.x + 1 && p.position.y == selectedPiece.position.y + moveDirection && p.pieceType == selectedPiece.pieceType));
                bool isPieceOnLeftFront = pieceSet.pieces
                        .Any(p => (p.position.x == selectedPiece.position.x - 1 && p.position.y == selectedPiece.position.y + moveDirection && p.pieceType == selectedPiece.pieceType));
                bool isPieceOnFront = pieceSet.pieces
                        .Any(p => (p.position.x == selectedPiece.position.x && p.position.y == selectedPiece.position.y + moveDirection));

                if (isPieceOnRightFront)
                {
                    canMoveSquares.Add(BoardSquare[selectedPiece.position.x + 1, selectedPiece.position.y + moveDirection]);
                    BoardSquare[selectedPiece.position.x + 1, selectedPiece.position.y + moveDirection].button.BackColor = Color.Salmon;
                }
                if (isPieceOnLeftFront)
                {
                    canMoveSquares.Add(BoardSquare[selectedPiece.position.x - 1, selectedPiece.position.y + moveDirection]);
                    BoardSquare[selectedPiece.position.x - 1, selectedPiece.position.y + moveDirection].button.BackColor = Color.Salmon;
                }
                if (!isPieceOnFront)
                {
                    canMoveSquares.Add(BoardSquare[moveToPosX, moveToPosY]);
                    BoardSquare[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
                }
            }
        }

        private void removeCannotMovesquares(Piece selectedPiece, int x, int y)
        {
            //クイーンとかの一気に移動できる駒が駒を飛び越えて移動できるのをなんとかする
            List<Square> removeSquares = new List<Square>();
            if (selectedPiece.pieceType == pieceType.bishop || selectedPiece.pieceType == pieceType.queen || selectedPiece.pieceType == pieceType.rook)
            {
                foreach (var canMoveSquare in canMoveSquares)
                {
                    var PieceOnMoveSquare = pieceSet.pieces.Find(p => p.position.x == canMoveSquare.position.x && p.position.y == canMoveSquare.position.y);

                    if (PieceOnMoveSquare != null)
                    {
                        Vector2 relativePos = new Vector2(PieceOnMoveSquare.position.x - selectedPiece.position.x, PieceOnMoveSquare.position.y - selectedPiece.position.y);

                        //縦方向
                        if (relativePos.x == 0)
                        {
                            if (relativePos.y > 0)
                            {
                                for (int i = PieceOnMoveSquare.position.y; i < 9; i++)
                                {
                                    removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x, i]);
                                }
                            }
                            else if (relativePos.y < 0)
                            {
                                for (int i = PieceOnMoveSquare.position.y; i > 0; i--)
                                {
                                    removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x, i]);
                                }
                            }
                            if (PieceOnMoveSquare.pieceColor != selectedPiece.pieceColor)
                                removeSquares.Remove(BoardSquare[PieceOnMoveSquare.position.x, PieceOnMoveSquare.position.y]);
                        }
                        //横方向
                        else if (relativePos.y == 0)
                        {
                            if (relativePos.x > 0)
                            {
                                for (int i = PieceOnMoveSquare.position.x; i < 9; i++)
                                {
                                    removeSquares.Add(BoardSquare[i, PieceOnMoveSquare.position.y]);
                                }
                            }
                            else if (relativePos.x < 0)
                            {
                                for (int i = PieceOnMoveSquare.position.x; i > 0; i--)
                                {
                                    removeSquares.Add(BoardSquare[i, PieceOnMoveSquare.position.y]);
                                }
                            }
                            if (PieceOnMoveSquare.pieceColor != selectedPiece.pieceColor)
                                removeSquares.Remove(BoardSquare[PieceOnMoveSquare.position.x, PieceOnMoveSquare.position.y]);
                        }
                        //右斜め上方向
                        else if ((relativePos.x > 0 && relativePos.y > 0) || (relativePos.x < 0 && relativePos.y < 0))
                        {
                            if (relativePos.x > 0 && relativePos.y > 0)
                            {
                                var lowerPos = PieceOnMoveSquare.position.x > PieceOnMoveSquare.position.y ? PieceOnMoveSquare.position.x : PieceOnMoveSquare.position.y;
                                for (int i = lowerPos; i < 9; i++)
                                {
                                    removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x + i - lowerPos, PieceOnMoveSquare.position.y + i - lowerPos]);
                                }
                            }
                            else if (relativePos.x < 0 && relativePos.y < 0)
                            {
                                var lowerPos = PieceOnMoveSquare.position.x < PieceOnMoveSquare.position.y ? PieceOnMoveSquare.position.x : PieceOnMoveSquare.position.y;
                                for (int i = lowerPos; i > 0; i--)
                                {
                                    removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x - i + lowerPos, PieceOnMoveSquare.position.y - i + lowerPos]);
                                }
                            }
                            if (PieceOnMoveSquare.pieceColor != selectedPiece.pieceColor)
                                removeSquares.Remove(BoardSquare[PieceOnMoveSquare.position.x, PieceOnMoveSquare.position.y]);
                        }
                        //左斜め上方向
                        else if ((relativePos.x < 0 && relativePos.y > 0) || (relativePos.x > 0 && relativePos.y < 0))
                        {
                            //左斜め上からのboardの中線(1,8) => (8,1)のラインが判定の分かれ目
                            //左上に上る場合，中線より左側はx=0がy=9より先にくる，中線より右側はy=9がx=0より先に来る。
                            if (relativePos.x < 0 && relativePos.y > 0)
                            {
                                //中線の左側が x + y < 9
                                if (PieceOnMoveSquare.position.x + PieceOnMoveSquare.position.y < 9)
                                {
                                    var j = 0;
                                    for (int i = PieceOnMoveSquare.position.x; i > 0; i--)
                                    {
                                        removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x - j, PieceOnMoveSquare.position.y + j]);
                                    }
                                }
                                else
                                {
                                    var j = 0;
                                    for (int i = PieceOnMoveSquare.position.y; i > 0; i--)
                                    {
                                        removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x + j, PieceOnMoveSquare.position.y - j]);
                                    }
                                }
                            }
                            else if (relativePos.x > 0 && relativePos.y < 0)
                            {
                                var lowerPos = PieceOnMoveSquare.position.x < PieceOnMoveSquare.position.y ? PieceOnMoveSquare.position.x : PieceOnMoveSquare.position.y;
                                for (int i = lowerPos; i > 0; i--)
                                {
                                    removeSquares.Add(BoardSquare[PieceOnMoveSquare.position.x - i + lowerPos, PieceOnMoveSquare.position.y - i + lowerPos]);
                                }
                            }
                            if (PieceOnMoveSquare.pieceColor != selectedPiece.pieceColor)
                                removeSquares.Remove(BoardSquare[PieceOnMoveSquare.position.x, PieceOnMoveSquare.position.y]);
                        }
                    }
                }
            }

            foreach (var removeSquare in removeSquares)
            {
                removeSquare.resetColor();
                canMoveSquares.Remove(removeSquare);
            }
        }

        private void resetCanMoveSquares(List<Square> resetSquares)
        {
            foreach (var square in resetSquares)
            {
                square.resetColor();
            }
            resetSquares.Clear();
        }

    }











}

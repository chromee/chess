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
                        textBox1.Text = $"{selectedSquare.position.x}, {selectedSquare.position.y}";
                        //textBox1.Text = $"{square.GetLength(0)}, {square.GetLength(1)}";
                        textBox2.Text = $"pieceInfo : {pInfo}";
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
        }

        private void setCanMoveSquares(Piece selectedPiece, int x, int y)
        {
            foreach (var movePattern in selectedPiece.movePatterns)
            {
                int moveToPosX = x + movePattern.x;
                int moveToPosY = y + movePattern.y;
                bool inBoard = moveToPosX > 0 && moveToPosX < 9 && moveToPosY > 0 && moveToPosY < 9;
                if (inBoard)
                {
                    canMoveSquares.Add(BoardSquare[moveToPosX, moveToPosY]);
                    BoardSquare[moveToPosX, moveToPosY].button.BackColor = Color.Salmon;
                }
            }

            List<Square> removeSquares = new List<Square>();
            foreach (var square in canMoveSquares)
            {
                var PieceOnCanMoveSquare = pieceSet.pieces.Find(p => p.position.x == square.position.x && p.position.y == square.position.y);
                if (PieceOnCanMoveSquare != null && PieceOnCanMoveSquare.pieceColor == selectedPiece.pieceColor)
                {
                    Vector2 relativePos = new Vector2(PieceOnCanMoveSquare.position.x - selectedPiece.position.x, PieceOnCanMoveSquare.position.y - selectedPiece.position.y);
                }
                else if (PieceOnCanMoveSquare != null && PieceOnCanMoveSquare.pieceColor != selectedPiece.pieceColor)
                {

                }
            }
            foreach (var square in removeSquares)
            {
                resetSquareColor(square);
                canMoveSquares.Remove(square);
            }
        }

        private void resetCanMoveSquares(List<Square> resetSquares)
        {
            foreach (var square in resetSquares)
            {
                resetSquareColor(square);
            }
            resetSquares.Clear();
        }

        private void resetSquareColor(Square square)
        {
            if ((square.position.x + square.position.y) % 2 == 0)
                square.button.BackColor = Color.LightYellow;
            else
                square.button.BackColor = Color.Tan;
        }

        /// <summary>
        /// プライベートクラス
        /// </summary>

        public class Square
        {
            public Vector2 position;
            public Button button;

            public Square(Vector2 pos, Button btn)
            {
                position = new Vector2(pos.x, pos.y);
                button = btn;
            }
        }

    }











}

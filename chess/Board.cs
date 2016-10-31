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
        public Square[,] square;

        public Board(Panel p, Form form, TextBox textBox1, TextBox tb2)
        {
            panel = p;
            firstForm = form;
            setPanelSetting(panel);
            createBoard();

            this.textBox1 = textBox1;
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
        Square beforeSelectedSquare = null;
        List<Square> canMoveSquares = new List<Square>();
        private void board_Click(object sender, EventArgs e)
        {
            for (int hor = 1; hor < 9; hor++)
            {
                for (int ver = 1; ver < 9; ver++)
                {
                    var selectedSquare = square[hor, ver];
                    if (sender.Equals(selectedSquare.button))
                    {
                        var piece = pieceSet.pieces.Find(p => p.position.x == hor && p.position.y == ver);

                        //pieace -> square
                        if (piece != null && beforeSelectedPiece == null)
                        {
                            beforeSelectedPiece = piece;
                            foreach (var movePattern in piece.movePatterns)
                            {
                                if(square[movePattern.x,movePattern.y]!=null)
                                {
                                    canMoveSquares.Add(square[movePattern.x, movePattern.y]);
                                    foreach (var canMoveSquare in canMoveSquares)
                                    {
                                        canMoveSquare.button.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
                        else if (beforeSelectedPiece != null && piece == null)
                        {
                            beforeSelectedPiece.position = new Vector2(hor, ver);
                            beforeSelectedSquare.button.BackgroundImage = null;

                            selectedSquare.button.BackgroundImage = beforeSelectedPiece.image;
                            selectedSquare.button.BackgroundImageLayout = ImageLayout.Zoom;
                            beforeSelectedPiece = null;
                        }

                        //pieace -> pieace
                        if (beforeSelectedPiece != null && piece.pieceColor != beforeSelectedPiece.pieceColor)
                        {
                            piece.position = new Vector2(0, 0);
                            beforeSelectedPiece.position = new Vector2(hor, ver);
                            beforeSelectedSquare.button.BackgroundImage = null;

                            selectedSquare.button.BackgroundImage = beforeSelectedPiece.image;
                            selectedSquare.button.BackgroundImageLayout = ImageLayout.Zoom;
                            beforeSelectedPiece = null;
                        }
                        else if (beforeSelectedPiece != null && piece.pieceColor == beforeSelectedPiece.pieceColor)
                        {
                            beforeSelectedPiece = piece;
                        }


                        textBox1.Text = $"{hor}, {ver}";
                        if (piece != null && beforeSelectedPiece != null)
                        {
                            var pInfo = $"{piece.pieceColor}_{piece.pieceType}, before : {beforeSelectedPiece.pieceColor}_{beforeSelectedPiece.pieceType}";
                            textBox2.Text = $"{hor}, {ver}　pieceInfo : {pInfo}";
                        }
                        beforeSelectedSquare = square[hor, ver];
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
            boardPanel.Top = 50;
            boardPanel.Left = 50;
            boardPanel.AutoScrollPosition = firstForm.AutoScrollPosition;
            firstForm.Controls.Add(boardPanel);
        }

        private void createBoard()
        {
            square = new Square[9, 9];
            for (int vertical = 1; vertical < 9; vertical++)
            {
                for (int horizontal = 1; horizontal < 9; horizontal++)
                {
                    var btn = new Button();
                    square[horizontal, vertical] = new Square(horizontal, vertical, btn);
                    setSquareButtonSetting(btn, horizontal, vertical);
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

        private void setSquareButtonSetting(Button btn, int horizontal, int vertical)
        {
            btn.Top = squareSize * 8 - squareSize * vertical;
            btn.Left = squareSize * horizontal - squareSize + squarePadding;
            btn.Width = squareSize;
            btn.Height = squareSize;
            if ((horizontal + vertical) % 2 == 0)
                btn.BackColor = Color.LightYellow;
            else
                btn.BackColor = Color.Tan;
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            //btn.Text = $"{horizontal}, {vertical}";
            btn.Click += new EventHandler(board_Click);
        }

        /// <summary>
        /// プライベートクラス
        /// </summary>

        public class Square
        {
            public int horizontal;
            public int vertical;
            public Button button;

            public Square(int hor, int ver, Button btn)
            {
                horizontal = hor;
                vertical = ver;
                button = btn;
            }
        }

    }











}

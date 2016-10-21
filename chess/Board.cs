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
        TextBox textBox;




        public Board(Panel p, Form form, TextBox textBox)
        {
            panel = p;
            firstForm = form;
            this.textBox = textBox;
        }

        Label[] labelHorizontal;
        Label[] labelVertical;
        SquarePosition[,] square;

        public void setBoardPanelSetting(Panel boardPanel)
        {
            boardPanel.Size = firstForm.Size;
            boardPanel.Top = 50;
            boardPanel.Left = 50;
            boardPanel.AutoScrollPosition = firstForm.AutoScrollPosition;
            firstForm.Controls.Add(boardPanel);
        }

        public void createBoard()
        {
            square = new SquarePosition[9, 9];

            for (int horizontal = 1; horizontal < 9; horizontal++)
            {
                for (int vertical = 1; vertical < 9; vertical++)
                {
                    var btn = new Button();
                    square[horizontal, vertical] = new SquarePosition(horizontal, vertical, btn);
                    setButtonSetting(btn, horizontal, vertical);
                    setHorizontalLabel();
                    setVerticalLabel();
                    panel.Controls.Add(btn);
                    //firstForm.Controls.Add(btn);
                }
            }
        }

        /// <summary>
        /// イベント関数
        /// </summary>

        private int pieceMoveDist = 70;

        private void board_Click(object sender, EventArgs e)
        {
            for (int hor = 1; hor < 9; hor++)
            {
                for (int ver = 1; ver < 9; ver++)
                {
                    if (sender.Equals(square[hor, ver].button))
                    {
                        int si = int.Parse(labelHorizontal[hor].Text);
                        int sj = int.Parse(labelVertical[ver].Text);
                        //lblCell[i, j].Text = (si * sj).ToString();
                        textBox.Text = $"{si}, {sj}";
                    }
                }
            }
            if (PieceSet.king.moveFlag)
            {
                PieceSet.king.Picture.Top += pieceMoveDist;
                PieceSet.king.Picture.Left += pieceMoveDist;
                PieceSet.king.moveFlag = false;
            }
        }

        /// <summary>
        /// プライベート関数
        /// </summary>

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
                labelHorizontal[i].Top = 0;
                labelHorizontal[i].Width = squareSize;
                labelHorizontal[i].Height = squareSize;
                labelHorizontal[i].BorderStyle = BorderStyle.FixedSingle;
                labelHorizontal[i].BackColor = Color.LightGray;
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
                labelVertical[j].Top = squareSize * j;
                labelVertical[j].Width = squareSize;
                labelVertical[j].Height = squareSize;
                labelVertical[j].BorderStyle = BorderStyle.FixedSingle;
                labelVertical[j].BackColor = Color.LightGray;
                labelVertical[j].TextAlign = ContentAlignment.MiddleCenter;
                labelVertical[j].Text = (j).ToString();
                // フォームに追加する
                panel.Controls.Add(labelVertical[j]);
            }
        }

        private void setButtonSetting(Button btn, int horizontal, int vertical)
        {
            btn.Name = $"square_{horizontal}_{vertical}";
            btn.Top = squareSize * horizontal - squareSize + squarePadding;
            btn.Left = squareSize * vertical - squareSize + squarePadding;
            btn.Width = squareSize;
            btn.Height = squareSize;
            if ((horizontal + vertical) % 2 == 0)
                btn.BackColor = Color.LightYellow;
            else
                btn.BackColor = Color.SaddleBrown;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Text = $"{horizontal}, {vertical}";
            btn.Click += new EventHandler(board_Click);
        }

        /// <summary>
        /// プライベートクラス
        /// </summary>

        private class SquarePosition
        {
            public int horizontal;
            public int vertical;
            public Button button;

            public SquarePosition(int hor, int ver, Button btn)
            {
                horizontal = hor;
                vertical = ver;
                button = btn;
            }
        }

    }











}

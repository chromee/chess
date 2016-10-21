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
        TextBox debugText;

        private int squareButtonSize = 80;
        

        public Board(Form form, TextBox textBox)
        {
            firstForm = form;
            debugText = textBox;
        }

        SquarePosition[,] square;

        public void setBoardPanelSetting(Panel boardPanel)
        {
            boardPanel.Size = firstForm.Size;
            boardPanel.Top = 50;
            boardPanel.Left = 50;
            boardPanel.AutoScrollPosition = firstForm.AutoScrollPosition;
            firstForm.Controls.Add(boardPanel);
        }

        public void createBoard(Panel boardPanel)
        {
            square = new SquarePosition[9, 9];

            for (int horizontal = 1; horizontal < 9; horizontal++)
            {
                for (int vertical = 1; vertical < 9; vertical++)
                {
                    var btn = new Button();
                    square[horizontal, vertical] = new SquarePosition(horizontal, vertical, btn);
                    setButtonSetting(btn, horizontal, vertical);
                    boardPanel.Controls.Add(btn);
                    //firstForm.Controls.Add(btn);
                }
            }
        }

        /// <summary>
        /// イベント関数
        /// </summary>

        private int pieceMoveDist = 80;

        private void board_Click(object sender, EventArgs e)
        {
            if(Piece.king.moveFlag)
            {
                Piece.king.Picture.Top += pieceMoveDist;
                Piece.king.Picture.Left += pieceMoveDist;
                Piece.king.moveFlag = false;
            }
        }

        /// <summary>
        /// プライベート関数
        /// </summary>

        private void setButtonSetting(Button btn, int horizontal, int vertical)
        {
            btn.Name = $"square_{horizontal}_{vertical}";
            btn.Top = squareButtonSize * horizontal - squareButtonSize;
            btn.Left = squareButtonSize * vertical - squareButtonSize;
            btn.Width = squareButtonSize;
            btn.Height = squareButtonSize;
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

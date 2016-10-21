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

        public Board(Form form, TextBox textBox)
        {
            firstForm = form;
            debugText = textBox;
        }

        class Position
        {
            public int horizontal;
            public int vertical;
            public Button button;

            public Position(int hor, int ver, Button btn)
            {
                horizontal = hor;
                vertical = ver;
                button = btn;
            }
        }

        Position[,] square;

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
            square = new Position[9, 9];

            for (int horizontal = 1; horizontal < 9; horizontal++)
            {
                for (int vertical = 1; vertical < 9; vertical++)
                {
                    var btn = new Button();
                    square[horizontal, vertical] = new Position(horizontal, vertical, btn);
                    setButtonSetting(btn, horizontal, vertical);
                    boardPanel.Controls.Add(btn);
                }
            }
        }

        /// <summary>
        /// プライベート関数
        /// </summary>

        private void setButtonSetting(Button btn, int horizontal, int vertical)
        {
            int squareButtonSize = firstForm.Size.Height / 10;
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
            btn.Click += new EventHandler(board_Clickq);
        }

        /// <summary>
        /// イベント関数
        /// </summary>

        private void board_Clickq(object sender, EventArgs e)
        {
            //debugText.Text = boardButton.Where(bList => bList.Where(v => sender.Equals(v)).
        }
    }











}

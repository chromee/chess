using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    class Square
    {
        public Vector2 position;
        public Button button;

        public Square(Vector2 pos, Button btn)
        {
            position = new Vector2(pos.x, pos.y);
            button = btn;
        }

        public void resetColor()
        {
            if ((position.x + position.y) % 2 == 0)
                button.BackColor = Color.LightYellow;
            else
                button.BackColor = Color.Tan;
        }
    }
}

using System.Drawing;
using System.Windows.Forms;

namespace chess
{
    class Square
    {
        public Vector2 position;
        public Button button;
        public bool isMoveable = false;

        public Square(Vector2 pos, Button btn)
        {
            position = new Vector2(pos.x, pos.y);
            isMoveable = false;
            button = btn;
        }

        public void SetImage(Image img)
        {
            button.BackgroundImage = img;
        }

        public void resetColor()
        {
            if ((position.x + position.y) % 2 == 0)
                button.BackColor = Color.LightYellow;
            else
                button.BackColor = Color.Tan;
        }

        public void Moveable()
        {
            isMoveable = true;
            button.BackColor = Color.Salmon;
        }

        public void UnMoveable()
        {
            isMoveable = false;
            resetColor();
        }
    }
}

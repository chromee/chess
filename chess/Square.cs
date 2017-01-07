using System.Drawing;
using System.Windows.Forms;

namespace chess
{
    class Square
    {
        public Vector2 position;
        public Button button;
        public bool isMoveable;
        public bool IsMoveable
        {
            get { return isMoveable; }
            set
            {
                isMoveable = value;
                if (isMoveable)
                    button.BackColor = Color.Salmon;
                else
                    ResetColor();
            }
        }

        public Square(Vector2 pos, Button btn)
        {
            position = new Vector2(pos.x, pos.y);
            button = btn;
            IsMoveable = false;
        }

        public void SetImage(Image img)
        {
            button.BackgroundImage = img;
        }

        public void ToMoveable()
        {
            IsMoveable = true;
        }

        public void ToUnMoveable()
        {
            IsMoveable = false;
        }

        private void ResetColor()
        {
            if ((position.x + position.y) % 2 == 0)
                button.BackColor = Color.LightYellow;
            else
                button.BackColor = Color.Tan;
        }
    }
}

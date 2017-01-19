using System.Drawing;
using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
        {
            PieceSet.randomLevel = 0;
        }

        private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
        {
            PieceSet.randomLevel = 1;
        }

        private void radioButton3_CheckedChanged(object sender, System.EventArgs e)
        {
            PieceSet.randomLevel = 2;
        }

        private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
        {
            Board.VSAI = true;
        }
        private void radioButton5_CheckedChanged(object sender, System.EventArgs e)
        {
            Board.VSAI = false;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            radioButton1.Dispose();
            radioButton2.Dispose();
            radioButton3.Dispose();
            radioButton4.Dispose();
            radioButton5.Dispose();
            button1.Dispose();
            panel1.Dispose();
            panel2.Dispose();
            this.BackgroundImage = null;
            Board board = new Board(this);
        }





    }
}

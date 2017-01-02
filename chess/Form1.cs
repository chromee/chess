using System.Windows.Forms;
using System.Reactive;


namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = this.Size;

            Board board = new Board(this);
        }
    }
}

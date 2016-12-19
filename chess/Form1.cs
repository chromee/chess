using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Panel panel = new Panel();
            Board board = new Board(panel, this, label1);
            PieceSet PieceSet = new PieceSet();
            board.setPieces(PieceSet);
        }
    }
}

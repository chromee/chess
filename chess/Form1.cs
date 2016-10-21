using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Panel panel = new Panel();
            Board board = new Board(panel, this, textBox1);
            board.setBoardPanelSetting(panel);
            PieceSet blackPiece = new PieceSet(playerColorType.black, panel, this);
            board.createBoard();
            //Piece whitePiece = new Piece(playerColorType.white, boardPanel, this);

        }
    }
}

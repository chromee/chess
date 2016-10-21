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
            Board board = new Board(this, textBox1);
            Piece whitePiece = new Piece(playerColorType.white);
            Piece blackPiece = new Piece(playerColorType.black);

            Panel boardPanel = new Panel();
            board.setBoardPanelSetting(boardPanel);
            board.createBoard(boardPanel);
        }
    }
}

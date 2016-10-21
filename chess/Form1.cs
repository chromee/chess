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


            Panel boardPanel = new Panel();
            board.setBoardPanelSetting(boardPanel);
            Piece blackPiece = new Piece(playerColorType.black, boardPanel, this);
            board.createBoard(boardPanel);
            //Piece whitePiece = new Piece(playerColorType.white, boardPanel, this);

        }
    }
}

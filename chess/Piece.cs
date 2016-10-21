using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

enum pieceType { king, queen, rook, bishop, knight, pawn }
enum playerColorType { white, black }

namespace chess
{
    class Piece
    {
        playerColorType playerColor;

        PieceFundation king;
        PieceFundation queen;
        PieceFundation rook;
        PieceFundation bishop;
        PieceFundation knight;
        PieceFundation pawn;

        public Piece(playerColorType playerCol)
        {
            playerColor = playerCol;
        }

        private void setPiece()
        {
            king = new PieceFundation(pieceType.king, playerColor);
            king.setMovePattern(-1, 1);
            king.setMovePattern(0, 1);
            king.setMovePattern(1, 1);
            king.setMovePattern(1, 0);
            king.setMovePattern(-1, 0);
            king.setMovePattern(-1, -1);
            king.setMovePattern(0, -1);
            king.setMovePattern(-1, 1);
        }

        
        
    }
}

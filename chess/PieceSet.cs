using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

enum pieceType { king, queen, rook, bishop, knight, pawn }
enum playerColorType { white, black }

namespace chess
{
    class PieceSet
    {
        Form form;
        Board board;
        TextBox textBox;

        public List<Piece> pieces = new List<Piece>(); //左のポーン→右のルーク

        public PieceSet(Form f, Board b, TextBox t)
        {
            form = f;
            board = b;
            textBox = t;
            setPieces(playerColorType.white);
            setPieces(playerColorType.black);
        }

        private void setPieces(playerColorType pCol)
        {
            int pawnLine = 0;
            int otherLine = 0;
            if (pCol == playerColorType.white)
            {
                pawnLine = 2;
                otherLine = 1;
            }
            else if (pCol == playerColorType.black)
            {
                pawnLine = 7;
                otherLine = 8;
            }

            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Piece(pieceType.pawn, pCol, new Vector2(i,pawnLine), board));
            }
            pieces.Add(new Piece(pieceType.rook,    pCol, new Vector2(1, otherLine), board));
            pieces.Add(new Piece(pieceType.knight, pCol, new Vector2(2, otherLine), board));
            pieces.Add(new Piece(pieceType.bishop, pCol, new Vector2(3, otherLine), board));
            pieces.Add(new Piece(pieceType.queen,  pCol, new Vector2(4, otherLine), board));
            pieces.Add(new Piece(pieceType.king,     pCol, new Vector2(5, otherLine), board));
            pieces.Add(new Piece(pieceType.bishop, pCol, new Vector2(6, otherLine), board));
            pieces.Add(new Piece(pieceType.knight,  pCol, new Vector2(7, otherLine), board));
            pieces.Add(new Piece(pieceType.rook,     pCol, new Vector2(8, otherLine), board));


        }

    }
}

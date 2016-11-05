using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

enum pieceType { king, queen, rook, bishop, knight, pawn }
enum pieceColor { white, black }

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
            setPieces(pieceColor.white);
            setPieces(pieceColor.black);
            setPiecesMovePattern();
        }

        public void setPieces(pieceColor pCol)
        {
            int pawnLine = 0;
            int otherLine = 0;
            if (pCol == pieceColor.white)
            {
                pawnLine = 2;
                otherLine = 1;
            }
            else if (pCol == pieceColor.black)
            {
                pawnLine = 7;
                otherLine = 8;
            }

            for (int i = 1; i < 9; i++)
            {
                pieces.Add(new Piece(pieceType.pawn, pCol, new Vector2(i, pawnLine), board));
            }
            pieces.Add(new Piece(pieceType.rook, pCol, new Vector2(1, otherLine), board));
            pieces.Add(new Piece(pieceType.knight, pCol, new Vector2(2, otherLine), board));
            pieces.Add(new Piece(pieceType.bishop, pCol, new Vector2(3, otherLine), board));
            pieces.Add(new Piece(pieceType.queen, pCol, new Vector2(4, otherLine), board));
            pieces.Add(new Piece(pieceType.king, pCol, new Vector2(5, otherLine), board));
            pieces.Add(new Piece(pieceType.bishop, pCol, new Vector2(6, otherLine), board));
            pieces.Add(new Piece(pieceType.knight, pCol, new Vector2(7, otherLine), board));
            pieces.Add(new Piece(pieceType.rook, pCol, new Vector2(8, otherLine), board));
        }

        public void setPiecesMovePattern()
        {
            var kings = pieces.Where(p => p.pieceType == pieceType.king);
            foreach (var king in kings)
            {
                king.setMovePattern(-1, 0);
                king.setMovePattern(-1, 1);
                king.setMovePattern(0, 1);
                king.setMovePattern(1, 1);
                king.setMovePattern(1, 0);
                king.setMovePattern(1, -1);
                king.setMovePattern(0, -1);
                king.setMovePattern(-1, -1);
            }

            var queens = pieces.Where(p => p.pieceType == pieceType.queen);
            foreach (var queen in queens)
            {
                for (int i = 1; i < 9; i++)
                {
                    queen.setMovePattern(-1 * i, 0 * i);
                    queen.setMovePattern(-1 * i, 1 * i);
                    queen.setMovePattern(0 * i, 1 * i);
                    queen.setMovePattern(1 * i, 1 * i);
                    queen.setMovePattern(1 * i, 0 * i);
                    queen.setMovePattern(1 * i, -1 * i);
                    queen.setMovePattern(0 * i, -1 * i);
                    queen.setMovePattern(-1 * i, -1 * i);
                }
            }

            var rooks = pieces.Where(p => p.pieceType == pieceType.rook);
            foreach (var rook in rooks)
            {
                for (int i = 1; i < 9; i++)
                {
                    rook.setMovePattern(1 * i, 0);
                    rook.setMovePattern(-1 * i, 0);
                    rook.setMovePattern(0, 1 * i);
                    rook.setMovePattern(0, -1 * i);
                }
            }

            var bishops = pieces.Where(p => p.pieceType == pieceType.bishop);
            foreach (var bishop in bishops)
            {
                for (int i = 1; i < 9; i++)
                {
                    bishop.setMovePattern(1 * i, 1 * i);
                    bishop.setMovePattern(1 * i, -1 * i);
                    bishop.setMovePattern(-1 * i, 1 * i);
                    bishop.setMovePattern(-1 * i, -1 * i);
                }
            }

            var knights = pieces.Where(p => p.pieceType == pieceType.knight);
            foreach (var knight in knights)
            {
                knight.setMovePattern(1, 2);
                knight.setMovePattern(-1, 2);
                knight.setMovePattern(1, -2);
                knight.setMovePattern(-1, -2);
                knight.setMovePattern(2, 1);
                knight.setMovePattern(2, -1);
                knight.setMovePattern(-2, 1);
                knight.setMovePattern(-2, -1);
            }

            var pawns = pieces.Where(p => p.pieceType == pieceType.pawn);
            foreach (var pawn in pawns)
            {
                int direction = pawn.pieceColor == pieceColor.white ? 1 : -1;
                pawn.setMovePattern(0, 1 * direction);
                pawn.setMovePattern(0, 2 * direction);
            }
        }

    }
}

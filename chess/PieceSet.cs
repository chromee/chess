using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

enum pieceType
{
    white_king, white_queen, white_rook1, white_rook2, white_bishop1, white_bishop2, white_knight1, white_knight2, white_pawn1, white_pawn2, white_pawn3, white_pawn4, white_pawn5, white_pawn6, white_pawn7, white_pawn8,
    black_king, black_queen, black_rook1, black_rook2, black_bishop1, black_bishop2, black_knight1, black_knight2, black_pawn1, black_pawn2, black_pawn3, black_pawn4, black_pawn5, black_pawn6, black_pawn7, black_pawn8
}
enum playerColorType { white, black }

namespace chess
{
    class PieceSet
    {

        Form form;
        Board board;
        TextBox textBox;

        public Dictionary<pieceType, Cross> firstPiecePosition = new Dictionary<pieceType, Cross>();
        public List<Piece> pieces = new List<Piece>();
        public Dictionary<Cross, Piece> piecePosMap = new Dictionary<Cross, Piece>();

        public PieceSet(Form f, Board b, TextBox t)
        {
            form = f;
            board = b;
            textBox = t;
            setFirstPiecePos(playerColorType.white);
            setPiece(playerColorType.white);
            setFirstPiecePos(playerColorType.black);
            setPiece(playerColorType.black);
        }

        private void setPiece(playerColorType playerColor)
        {
            foreach (var pieceAndPos in firstPiecePosition)
            {
                var p = new Piece(pieceAndPos.Key, playerColor, pieceAndPos.Value, board);
                pieces.Add(p);
                //piecePosMap.Add(pieceAndPos.Value, p);
            }
            //king.setMovePattern(-1, 1);
            //king.setMovePattern(0, 1);
            //king.setMovePattern(1, 1);
            //king.setMovePattern(1, 0);
            //king.setMovePattern(-1, 0);
            //king.setMovePattern(-1, -1);
            //king.setMovePattern(0, -1);
            //king.setMovePattern(-1, 1);
        }

        private void setFirstPiecePos(playerColorType pCol)
        {
            int ver1 = 0;
            int ver2 = 0;
            if (pCol == playerColorType.white)
            {
                ver1 = 1;
                ver2 = 2;
                firstPiecePosition.Add(pieceType.white_king, new Cross(4, ver1));
                firstPiecePosition.Add(pieceType.white_queen, new Cross(5, ver1));
                firstPiecePosition.Add(pieceType.white_bishop1, new Cross(3, ver1));
                firstPiecePosition.Add(pieceType.white_bishop2, new Cross(6, ver1));
                firstPiecePosition.Add(pieceType.white_rook1, new Cross(1, ver1));
                firstPiecePosition.Add(pieceType.white_rook2, new Cross(8, ver1));
                firstPiecePosition.Add(pieceType.white_knight1, new Cross(2, ver1));
                firstPiecePosition.Add(pieceType.white_knight2, new Cross(7, ver1));
                firstPiecePosition.Add(pieceType.white_pawn1, new Cross(1, ver2));
                firstPiecePosition.Add(pieceType.white_pawn2, new Cross(2, ver2));
                firstPiecePosition.Add(pieceType.white_pawn3, new Cross(3, ver2));
                firstPiecePosition.Add(pieceType.white_pawn4, new Cross(4, ver2));
                firstPiecePosition.Add(pieceType.white_pawn5, new Cross(5, ver2));
                firstPiecePosition.Add(pieceType.white_pawn6, new Cross(6, ver2));
                firstPiecePosition.Add(pieceType.white_pawn7, new Cross(7, ver2));
                firstPiecePosition.Add(pieceType.white_pawn8, new Cross(8, ver2));
            }
            else if (pCol == playerColorType.black)
            {
                ver1 = 8;
                ver2 = 7;
                firstPiecePosition.Add(pieceType.black_king, new Cross(4, ver1));
                firstPiecePosition.Add(pieceType.black_queen, new Cross(5, ver1));
                firstPiecePosition.Add(pieceType.black_bishop1, new Cross(3, ver1));
                firstPiecePosition.Add(pieceType.black_bishop2, new Cross(6, ver1));
                firstPiecePosition.Add(pieceType.black_rook1, new Cross(1, ver1));
                firstPiecePosition.Add(pieceType.black_rook2, new Cross(8, ver1));
                firstPiecePosition.Add(pieceType.black_knight1, new Cross(2, ver1));
                firstPiecePosition.Add(pieceType.black_knight2, new Cross(7, ver1));
                firstPiecePosition.Add(pieceType.black_pawn1, new Cross(1, ver2));
                firstPiecePosition.Add(pieceType.black_pawn2, new Cross(2, ver2));
                firstPiecePosition.Add(pieceType.black_pawn3, new Cross(3, ver2));
                firstPiecePosition.Add(pieceType.black_pawn4, new Cross(4, ver2));
                firstPiecePosition.Add(pieceType.black_pawn5, new Cross(5, ver2));
                firstPiecePosition.Add(pieceType.black_pawn6, new Cross(6, ver2));
                firstPiecePosition.Add(pieceType.black_pawn7, new Cross(7, ver2));
                firstPiecePosition.Add(pieceType.black_pawn8, new Cross(8, ver2));
            }
        }

    }
}

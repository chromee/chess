using System.Collections.Generic;
using System.Linq;

public enum PieceType { king, queen, rook, bishop, knight, pawn }
public enum PieceColor { white, black }

namespace chess
{
    class PieceSet
    {
        private PieceColor pieceColor;
        private List<Piece> pieces = new List<Piece>();

        public PieceSet(PieceColor pColor)
        {
            pieceColor = pColor;
            setPieces();
            setPiecesMovePattern();
        }

        public List<Piece> getPieces()
        {
            return pieces;
        }

        private void setPieces()
        {
            int pawnLine = 0;
            int otherLine = 0;
            if (pieceColor == PieceColor.white)
            {
                pawnLine = 2;
                otherLine = 1;
            }
            else if (pieceColor == PieceColor.black)
            {
                pawnLine = 7;
                otherLine = 8;
            }

            for (int i = 1; i < 9; i++)
            {
                pieces.Add(new Piece(PieceType.pawn, pieceColor, new Vector2(i, pawnLine)));
            }
            pieces.Add(new Piece(PieceType.rook, pieceColor, new Vector2(1, otherLine)));
            pieces.Add(new Piece(PieceType.knight, pieceColor, new Vector2(2, otherLine)));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, new Vector2(3, otherLine)));
            pieces.Add(new Piece(PieceType.queen, pieceColor, new Vector2(4, otherLine)));
            pieces.Add(new Piece(PieceType.king, pieceColor, new Vector2(5, otherLine)));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, new Vector2(6, otherLine)));
            pieces.Add(new Piece(PieceType.knight, pieceColor, new Vector2(7, otherLine)));
            pieces.Add(new Piece(PieceType.rook, pieceColor, new Vector2(8, otherLine)));
        }

        private void setPiecesMovePattern()
        {
            var kings = pieces.Where(p => p.pieceType == PieceType.king);
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

            var queens = pieces.Where(p => p.pieceType == PieceType.queen);
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

            var rooks = pieces.Where(p => p.pieceType == PieceType.rook);
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

            var bishops = pieces.Where(p => p.pieceType == PieceType.bishop);
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

            var knights = pieces.Where(p => p.pieceType == PieceType.knight);
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

            var pawns = pieces.Where(p => p.pieceType == PieceType.pawn);
            foreach (var pawn in pawns)
            {
                int direction = pawn.pieceColor == PieceColor.white ? 1 : -1;
                pawn.setMovePattern(0, 1 * direction);
                pawn.setMovePattern(0, 2 * direction);
            }
        }

    }
}

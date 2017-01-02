using System.Collections.Generic;
using System.Linq;
using System;

public enum PieceType { king, queen, rook, bishop, knight, pawn }
public enum PieceColor { white, black }

namespace chess
{
    class PieceSet
    {
        private PieceColor pieceColor;
        private List<Piece> pieces = new List<Piece>();
        public int randomLevel = 0;

        public PieceSet(PieceColor pColor)
        {
            pieceColor = pColor;
            switch (randomLevel)
            {
                default:
                    setPieces();
                    break;
                case 1:
                    setPiecesRandomLevel1();
                    break;
                case 2:
                    setPiecesRandomLevel2();
                    break;
            }
            setPiecesMovePattern();
        }

        public List<Piece> getPieces()
        {
            return pieces;
        }

        private void setPieces()
        {
            int pawnLine = pieceColor == PieceColor.white ? 2 : 7;
            int otherLine = pieceColor == PieceColor.white ? 1 : 8;

            for (int i = 1; i < 9; i++)
                pieces.Add(new Piece(PieceType.pawn, pieceColor, new Vector2(i, pawnLine)));

            pieces.Add(new Piece(PieceType.rook, pieceColor, new Vector2(1, otherLine)));
            pieces.Add(new Piece(PieceType.knight, pieceColor, new Vector2(2, otherLine)));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, new Vector2(3, otherLine)));
            pieces.Add(new Piece(PieceType.queen, pieceColor, new Vector2(4, otherLine)));
            pieces.Add(new Piece(PieceType.king, pieceColor, new Vector2(5, otherLine)));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, new Vector2(6, otherLine)));
            pieces.Add(new Piece(PieceType.knight, pieceColor, new Vector2(7, otherLine)));
            pieces.Add(new Piece(PieceType.rook, pieceColor, new Vector2(8, otherLine)));
        }

        private void setPiecesRandomLevel1()
        {
            int pawnLine = pieceColor == PieceColor.white ? 2 : 7;
            int otherLine = pieceColor == PieceColor.white ? 1 : 8;

            var positions = new List<Vector2>();
            var x_postions = Enumerable.Range(1, 8).ToArray();
            foreach (var x in x_postions)
            {
                positions.Add(new Vector2(x, pawnLine));
                positions.Add(new Vector2(x, otherLine));
            }

            Random R = new Random();
            for (int i = 1; i < 9; i++)
                pieces.Add(new Piece(PieceType.pawn, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.rook, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.knight, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.queen, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.king, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.knight, pieceColor, positions.Pop(R.Next(positions.Count))));
            pieces.Add(new Piece(PieceType.rook, pieceColor, positions.Pop(R.Next(positions.Count))));
        }

        private void setPiecesRandomLevel2()
        {
            for (int i = 1; i < 9; i++)
                pieces.Add(new Piece(PieceType.pawn, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.rook, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.knight, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.queen, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.king, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.knight, pieceColor, PieceNotExistPos()));
            pieces.Add(new Piece(PieceType.rook, pieceColor, PieceNotExistPos()));
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

        public bool isExistPiecePosition(Vector2 position)
        {
            return this.pieces.Any(p => p.isExist(position)) || Board.pieces.Any(p => p.isExist(position));
        }

        public Vector2 PieceNotExistPos()
        {
            while (true)
            {
                Vector2 pos = Vector2.Random(1, 9);
                if (!isExistPiecePosition(pos))
                    return pos;
            }
        }

    }
}

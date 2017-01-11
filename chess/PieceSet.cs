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
        public List<Piece> pieces = new List<Piece>();
        public int randomLevel = 0;

        public PieceSet(PieceColor pColor)
        {
            pieceColor = pColor;
            switch (randomLevel)
            {
                default:
                    SetPieces();
                    break;
                case 1:
                    SetPiecesRandomLevel1();
                    break;
                case 2:
                    SetPiecesRandomLevel2();
                    break;
            }
            SetPiecesMovePattern();
        }

        private void SetPieces()
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

        private void SetPiecesRandomLevel1()
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

        private void SetPiecesRandomLevel2()
        {
            for (int i = 1; i < 9; i++)
                pieces.Add(new Piece(PieceType.pawn, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.rook, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.knight, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.queen, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.king, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.bishop, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.knight, pieceColor, GetPieceNotExistPos()));
            pieces.Add(new Piece(PieceType.rook, pieceColor, GetPieceNotExistPos()));
        }

        private void SetPiecesMovePattern()
        {
            var kings = pieces.Where(p => p.pieceType == PieceType.king);
            foreach (var king in kings)
            {
                king.SetMovePattern(-1, 0);
                king.SetMovePattern(-1, 1);
                king.SetMovePattern(0, 1);
                king.SetMovePattern(1, 1);
                king.SetMovePattern(1, 0);
                king.SetMovePattern(1, -1);
                king.SetMovePattern(0, -1);
                king.SetMovePattern(-1, -1);
            }

            var queens = pieces.Where(p => p.pieceType == PieceType.queen);
            foreach (var queen in queens)
            {
                for (int i = 1; i < 9; i++)
                {
                    queen.SetMovePattern(-1 * i, 0 * i);
                    queen.SetMovePattern(-1 * i, 1 * i);
                    queen.SetMovePattern(0 * i, 1 * i);
                    queen.SetMovePattern(1 * i, 1 * i);
                    queen.SetMovePattern(1 * i, 0 * i);
                    queen.SetMovePattern(1 * i, -1 * i);
                    queen.SetMovePattern(0 * i, -1 * i);
                    queen.SetMovePattern(-1 * i, -1 * i);
                }
            }

            var rooks = pieces.Where(p => p.pieceType == PieceType.rook);
            foreach (var rook in rooks)
            {
                for (int i = 1; i < 9; i++)
                {
                    rook.SetMovePattern(1 * i, 0);
                    rook.SetMovePattern(-1 * i, 0);
                    rook.SetMovePattern(0, 1 * i);
                    rook.SetMovePattern(0, -1 * i);
                }
            }

            var bishops = pieces.Where(p => p.pieceType == PieceType.bishop);
            foreach (var bishop in bishops)
            {
                for (int i = 1; i < 9; i++)
                {
                    bishop.SetMovePattern(1 * i, 1 * i);
                    bishop.SetMovePattern(1 * i, -1 * i);
                    bishop.SetMovePattern(-1 * i, 1 * i);
                    bishop.SetMovePattern(-1 * i, -1 * i);
                }
            }

            var knights = pieces.Where(p => p.pieceType == PieceType.knight);
            foreach (var knight in knights)
            {
                knight.SetMovePattern(1, 2);
                knight.SetMovePattern(-1, 2);
                knight.SetMovePattern(1, -2);
                knight.SetMovePattern(-1, -2);
                knight.SetMovePattern(2, 1);
                knight.SetMovePattern(2, -1);
                knight.SetMovePattern(-2, 1);
                knight.SetMovePattern(-2, -1);
            }

            var pawns = pieces.Where(p => p.pieceType == PieceType.pawn);
            foreach (var pawn in pawns)
            {
                int direction = pawn.pieceColor == PieceColor.white ? 1 : -1;
                pawn.SetMovePattern(0, 1 * direction);
                pawn.SetMovePattern(0, 2 * direction);
            }
        }

        private bool IsExistPiecePosition(Vector2 position)
        {
            return this.pieces.Any(p => p.Position == position) || Board.pieces.Any(p => p.Position == position);
        }

        private Vector2 GetPieceNotExistPos()
        {
            while (true)
            {
                Vector2 pos = Vector2.Random(1, 9);
                if (!IsExistPiecePosition(pos))
                    return pos;
            }
        }

    }
}

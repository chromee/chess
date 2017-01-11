using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace chess
{
    class AI
    {
        private PieceColor pieceType;

        public AI(PieceColor pt)
        {
            pieceType = pt;
        }

        private List<Move> CheckMoves(PieceColor color)
        {
            List<Move> moves = new List<Move>();
            int moveIndex = 0;

            var controllPieces = Board.pieces.Where(piece => piece.pieceColor == color);
            var enemyPieces = Board.pieces.Where(piece => piece.pieceColor != color);
            foreach (var piece in controllPieces)
            {
                //移動先に敵駒がいればプラス
                piece.ApplyMoveableSquares();
                var controllPieceMoveableSquares = Board.GetMoveableSquares();
                foreach (var moveableSquare in controllPieceMoveableSquares)
                {
                    moves.Add(new Move(piece, moveableSquare.position, 0));

                    var killableEnemy = Board.pieces.Find(p => p.Position == moveableSquare.position && piece.IsEnemy(p));
                    if (killableEnemy != null)
                        moves[moveIndex].point += killableEnemy.GetTypePoint();

                    //移動先が敵駒の移動可能範囲ならマイナス
                    foreach (var enemy in enemyPieces)
                    {
                        enemy.ApplyMoveableSquares();
                        var enemyMoveableSquares = Board.GetMoveableSquares();
                        bool IsKilledEnemy = enemyMoveableSquares.Any(square => square.position == moveableSquare.position);
                        if (IsKilledEnemy)
                            moves[moveIndex].point -= piece.GetTypePoint();
                    }
                }
                moveIndex++;
            }
            return moves;
        }

        private Move DicideMove(List<Move> moves)
        {
            Move bestMove = moves.FindMax(move => move.point);
            return bestMove;
        }

        public void move()
        {
            var moves = CheckMoves(pieceType);
            var move = DicideMove(moves);
            move.Execute();
        }

    }
}

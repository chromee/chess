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

            var controllPieces = Board.pieces.Where(piece => piece.pieceColor == color && piece.IsAlive());
            var enemyPieces = Board.pieces.Where(piece => piece.pieceColor != color);
            foreach (var piece in controllPieces)
            {
                piece.ApplyMoveableSquares();
                var controllPieceMoveableSquares = Board.GetMoveableSquares();
                foreach (var moveableSquare in controllPieceMoveableSquares)
                {
                    moves.Add(new Move(piece, moveableSquare.position, 0));
                    var pieceCorrectPos = piece.Position;
                    piece.Position = moveableSquare.position;

                    #region 移動先に敵駒がいればプラス
                    var killableEnemy = Board.pieces.Find(p => p.Position == moveableSquare.position && piece.IsEnemy(p));
                    if (killableEnemy != null)
                    {
                        moves[moveIndex].point += killableEnemy.GetTypePoint();
                        //message($"killable:{killableEnemy.Position.x.ToString()},  {killableEnemy.Position.y.ToString()}");
                    }
                    #endregion

                    #region 移動先が敵駒の移動可能範囲ならマイナス
                    foreach (var enemy in enemyPieces)
                    {
                        enemy.ApplyMoveableSquares();
                        var enemyMoveableSquares = Board.GetMoveableSquares();
                        bool IsKilledEnemy = enemyMoveableSquares.Any(square => square.position == moveableSquare.position);
                        if (IsKilledEnemy)
                        {
                            moves[moveIndex].point -= piece.GetTypePoint();
                            //message($"killed:{enemy.pieceType.ToString()}, pos:{moveableSquare.position.x}, {moveableSquare.position.y}");
                        }
                    }
                    #endregion
                    piece.Position = pieceCorrectPos;
                    moveIndex++;
                }
                //message($"result:{piece.pieceType}, ({piece.Position.x}.{piece.Position.y}), {moves[moveIndex].point.ToString()}");
            }
            return moves;
        }

        private Move DicideMove(List<Move> moves)
        {
            int maxPoint = moves.Max(move => move.point);
            Move bestMove = moves.Where(move => move.point == maxPoint).RandomAt();
            //message(bestMove.point.ToString());
            return bestMove;
        }

        public void move()
        {
            var moves = CheckMoves(pieceType);
            var move = DicideMove(moves);
            move.Execute();
        }

        private void message(string text)
        {
            MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}

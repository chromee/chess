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
                //移動先に敵駒がいればプラス
                piece.ApplyMoveableSquares();
                var controllPieceMoveableSquares = Board.GetMoveableSquares();
                foreach (var moveableSquare in controllPieceMoveableSquares)
                {
                    moves.Add(new Move(piece, moveableSquare.position, 0));

                    var killableEnemy = Board.pieces.Find(p => p.Position == moveableSquare.position && piece.IsEnemy(p));
                    if (killableEnemy != null)
                    {
                        moves[moveIndex].point += killableEnemy.GetTypePoint();
                        message($"killable:{killableEnemy.Position.x.ToString()},  {killableEnemy.Position.y.ToString()}");
                        message($"add:{moves[moveIndex].point.ToString()}");
                    }

                    var vertualPieces = new List<Piece>(Board.pieces);
                    var a = vertualPieces.Find(p => p == piece);
                    a.Position = moveableSquare.position;
                    //移動先が敵駒の移動可能範囲ならマイナス
                    foreach (var enemy in enemyPieces)
                    {
                        enemy.ApplyMoveableSquares(vertualPieces);
                        var enemyMoveableSquares = Board.GetMoveableSquares();
                        bool IsKilledEnemy = enemyMoveableSquares.Any(square => square.position == moveableSquare.position);
                        if (IsKilledEnemy)
                        {
                            moves[moveIndex].point -= piece.GetTypePoint();
                            message($"killed:{enemy.pieceType.ToString()}");
                            message($"sub:{moves[moveIndex].point.ToString()}");
                        }
                    }
                }
                message($"result:{piece.pieceType}, ({piece.Position.x}.{piece.Position.y}), {moves[moveIndex].point.ToString()}");
                moveIndex++;
            }
            return moves;
        }

        private Move DicideMove(List<Move> moves)
        {
            Move bestMove = moves.FindMax(move => move.point);
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

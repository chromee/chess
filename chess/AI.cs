using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace chess
{
    class AI
    {
        private PieceColor pieceColor;
        private Board board;
        public bool canKillKing = false;

        public AI(PieceColor pc, Board b)
        {
            pieceColor = pc;
            board = b;
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
                        if (killableEnemy.pieceType == PieceType.king)
                            canKillKing = true;
                    }
                    #endregion

                    #region 移動先が敵駒の移動可能範囲ならマイナス
                    foreach (var enemy in enemyPieces)
                    {
                        enemy.ApplyMoveableSquares();
                        var enemyMoveableSquares = Board.GetMoveableSquares();
                        foreach (var enemyMoveableSquare in enemyMoveableSquares)
                        {
                            var killedAlly = Board.pieces.Find(p => p.Position == enemyMoveableSquare.position);
                            if (killedAlly != null)
                                moves[moveIndex].point -= killedAlly.GetTypePoint();
                        }
                    }
                    #endregion
                    piece.Position = pieceCorrectPos;
                    moveIndex++;
                }
            }
            return moves;
        }

        private Move DicideMove(List<Move> moves)
        {
            int maxPoint = moves.Max(move => move.point);
            Move bestMove = moves.Where(move => move.point == maxPoint).RandomAt();
            return bestMove;
        }

        public void move()
        {
            var moves = CheckMoves(pieceColor);
            var move = DicideMove(moves);
            move.Execute();
            if (canKillKing)
                board.Win(pieceColor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reactive.Linq;

namespace chess
{
    class Board
    {
        Form form;

        public static List<Piece> pieces = new List<Piece>();
        public static Square[,] squares = new Square[9, 9];

        public static bool VSAI = true;
        public AI ai = new AI(PieceColor.black);

        private PieceColor turnPlayerColor;
        private Label turnLabel = new Label();

        Piece beforeSelectedPiece = null;

        public Board(Form f)
        {
            form = f;
            turnPlayerColor = PieceColor.white;
            CreateBoard();
            SetLabel();
            SetPieces();
        }

        private void Square_Click(object sender, EventArgs e)
        {
            for (int selectedX = 1; selectedX < 9; selectedX++)
            {
                for (int selectedY = 1; selectedY < 9; selectedY++)
                {
                    var selectedSquare = squares[selectedX, selectedY];
                    if (sender.Equals(selectedSquare.button))
                        SelectSquare(selectedSquare);
                }
            }
        }

        #region"盤面初期化まわり"
        private int squareSize = 70;
        private int boardTopPadding = 70;
        private int boardLeftPadding = 20;

        private void CreateBoard()
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = new Button();
                    squares[x, y] = new Square(new Vector2(x, y), btn);
                    SetSquareButtonProperties(btn, x, y);
                    form.Controls.Add(btn);
                }
            }
        }

        private void SetSquareButtonProperties(Button btn, int x, int y)
        {
            btn.Top = squareSize * 8 - squareSize * y + boardTopPadding;
            btn.Left = squareSize * x + boardLeftPadding;
            btn.Width = squareSize;
            btn.Height = squareSize;
            if ((x + y) % 2 == 0)
                btn.BackColor = Color.LightYellow;
            else
                btn.BackColor = Color.Tan;
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Click += new EventHandler(Square_Click);
        }

        private void SetPieces()
        {
            PieceSet whitePieces = new PieceSet(PieceColor.white);
            pieces.AddRange(whitePieces.pieces);
            PieceSet blackPieces = new PieceSet(PieceColor.black);
            pieces.AddRange(blackPieces.pieces);
        }

        private void SetLabel()
        {
            turnLabel.Top = 0;
            turnLabel.Left = 230;
            turnLabel.Width = 300;
            turnLabel.Height = 75;
            turnLabel.Text = $"{turnPlayerColor} turn";
            turnLabel.TextAlign = ContentAlignment.MiddleCenter;
            turnLabel.Font = new Font(turnLabel.Font.OriginalFontName, 30);
            form.Controls.Add(turnLabel);
        }
        #endregion

        #region "駒の移動まわり"
        private void SelectSquare(Square selectedSquare)
        {
            var selectedPiece = pieces.Find(p => p.Position.x == selectedSquare.position.x && p.Position.y == selectedSquare.position.y);

            bool isSelectPiece = selectedPiece != null;
            bool isSelectSquare = selectedPiece == null;
            bool isSelectedPiece = beforeSelectedPiece != null;

            #region pieace選択
            if (isSelectPiece && !isSelectedPiece && selectedPiece.pieceColor == turnPlayerColor)
            {
                beforeSelectedPiece = selectedPiece;
                selectedPiece.ApplyMoveableSquares();
            }
            #endregion
            #region pieace選択 -> square選択
            else if (isSelectedPiece && isSelectSquare)
            {
                if (selectedSquare.IsMoveable)
                {
                    beforeSelectedPiece.Move(selectedSquare.position);
                    ChangeTurn(beforeSelectedPiece);
                    beforeSelectedPiece = null;
                    ResetMoveableSquares();
                }
            }
            #endregion
            #region pieace選択 -> 敵pieace選択
            else if (isSelectedPiece && selectedPiece.pieceColor != beforeSelectedPiece.pieceColor)
            {
                if (selectedSquare.IsMoveable)
                {
                    beforeSelectedPiece.Move(selectedSquare.position);

                    if (selectedPiece.pieceType == PieceType.king)
                        WinJudge(turnPlayerColor);
                    else
                    {
                        ChangeTurn(beforeSelectedPiece);
                        ResetMoveableSquares();
                        beforeSelectedPiece = null;
                    }
                }
            }
            #endregion
            #region pieace選択 -> 味方pieace選択
            else if (isSelectedPiece && !beforeSelectedPiece.IsEnemy(selectedPiece))
            {
                selectedPiece.ApplyMoveableSquares();
                beforeSelectedPiece = selectedPiece;
            }
            #endregion
        }

        public static void ResetMoveableSquares()
        {
            var moveableSquares = GetMoveableSquares();
            foreach (var square in moveableSquares)
                square.ToUnMoveable();
        }

        public static List<Square> GetMoveableSquares()
        {
            var moveableSquares = new List<Square>();
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    if (squares[x, y].IsMoveable)
                        moveableSquares.Add(squares[x, y]);
                }
            }
            return moveableSquares;
        }

        #endregion

        #region "ゲームシステムまわり"
        private void ChangeTurn(Piece beforeSelectedPiece)
        {
            turnPlayerColor = turnPlayerColor == PieceColor.white ? PieceColor.black : PieceColor.white;
            turnLabel.Text = $"{turnPlayerColor} turn";
            if(VSAI)
            {
                ai.move();
                turnPlayerColor = turnPlayerColor == PieceColor.white ? PieceColor.black : PieceColor.white;
                turnLabel.Text = $"{turnPlayerColor} turn";
            }
        }

        private void WinJudge(PieceColor winPlayerColor)
        {
            string JudgeText = winPlayerColor == PieceColor.white ? "白の勝ち" : "黒の勝ち";
            MessageBox.Show($"{JudgeText}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetBoard();
        }

        private void ResetBoard()
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    var btn = squares[x, y].button;
                    if ((x + y) % 2 == 0)
                        btn.BackColor = Color.LightYellow;
                    else
                        btn.BackColor = Color.Tan;
                    btn.BackgroundImage = null;
                    btn.BackgroundImageLayout = ImageLayout.Zoom;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            pieces.Clear();
            SetPieces();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    class Move
    {
        public Piece piece;
        public Vector2 position;
        public int point;

        public Move(Piece pie, Vector2 pos, int p)
        {
            piece = pie;
            position = pos;
            point = p;
        }

        public void Execute()
        {
            piece.Move(position);
        }
    }
}

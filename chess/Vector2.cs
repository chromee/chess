using System;
using System.Linq;
using System.Collections.Generic;

namespace chess
{
    public class Vector2
    {
        public int x;
        public int y;
        public Vector2(int hor, int ver)
        {
            x = hor;
            y = ver;
        }
        #region 演算子のオーバーライド
        public static bool operator ==(Vector2 c1, Vector2 c2)
        {
            //nullの確認（構造体のようにNULLにならない型では不要）
            //両方nullか（参照元が同じか）
            //(c1 == c2)とすると、無限ループ
            if (object.ReferenceEquals(c1, c2))
            {
                return true;
            }
            //どちらかがnullか
            //(c1 == null)とすると、無限ループ
            if (((object)c1 == null) || ((object)c2 == null))
            {
                return false;
            }

            return (c1.x == c2.x) && (c1.y == c2.y);
        }
        public static bool operator !=(Vector2 c1, Vector2 c2)
        {
            return !(c1 == c2);
            //(c1 != c2)とすると、無限ループ
        }
        #endregion

        public static Vector2 Random(int min, int max)
        {
            Random R = new Random();
            Vector2 vec = new Vector2(R.Next(min, max), R.Next(min, max));
            return vec;
        }

        public bool IsSamePos(Vector2 vec)
        {
            return vec.x == x && vec.y == y;
        }
        public bool IsSamePos(int x_in, int y_in)
        {
            return x_in == x && y_in == y;
        }

        public bool IsInsideBoard()
        {
            return x > 0 && x < 9 && y > 0 && y < 9;
        }
        public static bool IsInsideBoard(int x, int y)
        {
            return x > 0 && x < 9 && y > 0 && y < 9;
        }
    }
}

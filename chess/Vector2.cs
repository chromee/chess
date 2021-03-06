﻿using System;

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
            if (object.ReferenceEquals(c1, c2))
                return true;
            if (((object)c1 == null) || ((object)c2 == null))
                return false;
            
            return (c1.x == c2.x) && (c1.y == c2.y);
        }
        public static bool operator !=(Vector2 c1, Vector2 c2)
        {
            return !(c1 == c2);
        }
        #endregion

        public static Vector2 Random(int min, int max)
        {
            Random R = new Random();
            Vector2 vec = new Vector2(R.Next(min, max), R.Next(min, max));
            return vec;
        }

        public bool IsInsideBoard()
        {
            return x > 0 && x < 9 && y > 0 && y < 9;
        }
        public static bool IsInsideBoard(int x, int y)
        {
            return x > 0 && x < 9 && y > 0 && y < 9;
        }

        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
    }
}

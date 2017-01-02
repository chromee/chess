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

        public static Vector2 Random(int min, int max)
        {
            Random R = new Random();
            Vector2 vec = new Vector2(R.Next(min, max), R.Next(min, max));
            return vec;
        }
    }
}

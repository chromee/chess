using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public static class ListExtensions
    {
        /// <summary>
        /// 先頭にあるオブジェクトを削除せずに返します
        /// </summary>
        public static T Peek<T>(this IList<T> self)
        {
            return self[0];
        }

        /// <summary>
        /// indexのオブジェクトを返してから削除
        /// </summary>
        public static T Pop<T>(this IList<T> self, int index)
        {
            var result = self[index];
            self.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// 末尾にオブジェクトを追加します
        /// </summary>
        public static void Push<T>(this IList<T> self, T item)
        {
            self.Insert(0, item);
        }

        public struct IndexedItem2<T>
        {
            public T Element { get; }
            public int X { get; }
            public int Y { get; }
            internal IndexedItem2(T element, int x, int y)
            {
                this.Element = element;
                this.X = x;
                this.Y = y;
            }
        }

        //--- 拡張メソッド
        public static IEnumerable<IndexedItem2<T>> WithIndex<T>(this T[,] self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            for (int x = 0; x < self.GetLength(0); x++)
                for (int y = 0; y < self.GetLength(1); y++)
                    yield return new IndexedItem2<T>(self[x, y], x, y);
        }
    }
}

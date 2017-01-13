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
        /// indexのオブジェクトを返してから削除
        /// </summary>
        public static T Pop<T>(this IList<T> self, int index)
        {
            var result = self[index];
            self.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// 最大値を持つ要素を返します
        /// </summary>
        public static TSource FindMax<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
        {
            return self.First(c => selector(c).Equals(self.Max(selector)));
        }

        static Random _Rand = new Random();
        public static T RandomAt<T>(this IEnumerable<T> ie)
        {
            return ie.OrderBy(x => _Rand.Next()).First();
        }
    }
}

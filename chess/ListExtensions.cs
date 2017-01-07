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
    }
}

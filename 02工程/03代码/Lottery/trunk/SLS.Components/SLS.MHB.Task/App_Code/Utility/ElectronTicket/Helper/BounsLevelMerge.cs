using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.MHB.Task.Helper
{
    /// <summary>
    /// 拼合恒鹏上海开奖信息中的奖等字符串，用与 Linq 的 Group By 中的自定义聚合。
    /// 用途：聚合、拼合 Group By 中的字符串
    /// </summary>
    public static class BounsLevelMerge
    {
        public static string Merge<TSource>(this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in source)
            {
                if (sb.ToString() != "")
                {
                    sb.Append(",");
                }

                sb.Append(selector(row));
            }

            return sb.ToString();
        }
    }
}

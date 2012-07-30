using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToMd5(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = System.Text.Encoding.Default.GetBytes(source);
            var hash = md5.ComputeHash(bs);

            StringBuilder s = new StringBuilder();
            foreach (var item in hash) {
                s.Append(item.ToString("x"));
            }

            return s.ToString();
        }
        /// <summary>
        /// 输出中文的是和否
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToChinease(this bool source)
        {
            if (source == true)
                return "是";
            else
                return "否";
        }
    }
}

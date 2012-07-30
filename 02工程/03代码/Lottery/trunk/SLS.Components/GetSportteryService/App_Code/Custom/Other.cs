using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GetSporttery
{
    class Other
    {

       /// <summary>
        /// Match转数组
       /// </summary>
       /// <param name="matchs"></param>
       /// <returns></returns>
        public static string[] MatchToArray(MatchCollection matchs)
        {
            string[] temp = new string[matchs.Count];
            int i = 0;
            foreach (Match match in matchs)
            {
                temp[i++] = match.Value;
            }

            return temp;
           
        }

        /// <summary>
        /// 得到子字符串的开始位置与长度不包括结束的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <param name="IStart"></param>
        /// <param name="ILen"></param>
        public static void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
        {
            int IEnd = -1;
            IStart = str.ToLower().IndexOf(strStart.ToLower());
            if (IStart != -1)
            {
                IEnd = str.ToLower().IndexOf(strEnd.ToLower(), IStart);                
                if (IEnd == -1)
                {
                    IStart = 0;
                    ILen = str.Length;
                }
                else
                {
                    ILen = IEnd - IStart;
                }
            }
            else
            {
                IStart = 0;
                ILen = 0;
            }
        }

        public static bool IsDate(string date)
        {
            return Regex.Match(date, @"^\d{4}-\d{1,2}-\d{1,2}$").Success;
        }
    }
}

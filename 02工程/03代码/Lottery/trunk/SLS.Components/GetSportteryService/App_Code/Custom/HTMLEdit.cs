using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shove.HTML;
using System.IO;
using System.Net;
using System.Collections;

namespace GetSporttery
{
    class HTMLEdit
    {
        public HTMLEdit()
        {

        }

        /// <summary>
        /// 只得到body元素
        /// </summary>
        /// <param name="str_Htmls"></param>        
        /// <returns></returns>
        public static string OnlyBody(string str_Htmls)
        {
            try
            {
                if (string.IsNullOrEmpty(str_Htmls))
                    return "";
                str_Htmls = str_Htmls.Replace("&amp;amp;nbsp;", "");
                return HTML.StandardizationHTML(str_Htmls, true, true, true);
            }
            catch (Exception ex)
            {
                new GetSportteryService.Log("Sporttery").Write("HTMLEdit 行：28附近：调用：“OnlyBody”时发生错误：" + ex.Message);                
                throw (ex);
            }
        }


        /// <summary>
        /// 分隔字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitString(string str, string separator)
        {
            string tmp = str;
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            int i = 0;
            int pos = tmp.IndexOf(separator);
            while (pos != -1)
            {
                ht.Add(i, tmp.Substring(0, pos));
                tmp = tmp.Substring(pos + separator.Length);
                pos = tmp.IndexOf(separator);
                i++;
            }
            ht.Add(i, tmp);
            string[] array = new string[ht.Count];
            for (int j = 0; j < ht.Count; j++)
                array[j] = ht[j].ToString();

            return array;
        }

                
    }

}

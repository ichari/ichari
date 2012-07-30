using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Data;

namespace SLS.AutomaticOpenLottery.Task
{
    public class PF
    {
        // 获取 Url 返回的 Html 流
        public static string Post(string Url, string RequestString, int TimeoutSeconds)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            if (TimeoutSeconds > 0)
            {
                request.Timeout = 1000 * TimeoutSeconds;
            }
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(RequestString);

            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();

            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));

            return sr.ReadToEnd();
        }

        // 获取 Url 返回的 Html 流
        public static string GetHtml(string Url, string encodeing, int TimeoutSeconds)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "www.svnhost.cn";
                if (TimeoutSeconds > 0)
                {
                    request.Timeout = 1000 * TimeoutSeconds;
                }
                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(encodeing));
                    string html = reader.ReadToEnd();
                    return html;
                }
                else
                {
                    return "";
                }
            }
            catch (SystemException)
            {
                return "";
            }
        }

        public static string GetHtml(string Url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "www.svnhost.cn";
                request.Timeout = 1200000;//120秒
                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
                    string html = reader.ReadToEnd();
                    if (html.Length < 100)
                    {
                        return null;
                    }
                    else
                    {
                        return html;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (SystemException ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (request != null)
                {
                    request = null;
                }
            }
        }

        //获取开奖号码
        public static DataTable GetDataTable(DataTable dt, int Type2)
        {
            DataColumn newDC0 = new DataColumn("Order", System.Type.GetType("System.String"));
            DataColumn newDC1 = new DataColumn("LotteryID", System.Type.GetType("System.String"));
            DataColumn newDC2 = new DataColumn("LotteryName", System.Type.GetType("System.String"));
            DataColumn newDC3 = new DataColumn("IsuseName", System.Type.GetType("System.String"));
            DataColumn newDC4 = new DataColumn("WinLotteryNumber", System.Type.GetType("System.String"));
            DataColumn newDC5 = new DataColumn("LotteryTypeID", System.Type.GetType("System.String"));
            DataColumn newDC6 = new DataColumn("LotteryType2", System.Type.GetType("System.String"));
            DataColumn newDC7 = new DataColumn("LotteryType2Name", System.Type.GetType("System.String"));

            DataTable dtType2 = new DataTable();
            dtType2.Columns.Add(newDC0);
            dtType2.Columns.Add(newDC1);
            dtType2.Columns.Add(newDC2);
            dtType2.Columns.Add(newDC3);
            dtType2.Columns.Add(newDC4);
            dtType2.Columns.Add(newDC5);
            dtType2.Columns.Add(newDC6);
            dtType2.Columns.Add(newDC7);

            DataRow[] Rows;

            if (Type2 == 3)
            {
                Rows = dt.Select("LotteryType2 = " + Type2 + "and LotteryID <> " + SLS.Lottery.ZCDC.ID.ToString(), "EndTime desc");
            }
            else
            {
                Rows = dt.Select("LotteryType2 = " + Type2, "EndTime desc");
            }

            foreach (DataRow dr in Rows)
            {
                DataRow dr1 = dtType2.NewRow();
                dr1[0] = dr[0].ToString();
                dr1[1] = dr[1].ToString();
                dr1[2] = dr[2].ToString();
                dr1[3] = dr[3].ToString();
                dr1[4] = new SLS.Lottery()[int.Parse(dr[1].ToString())].ShowNumber(dr[4].ToString());
                dr1[5] = dr[5].ToString();
                dr1[6] = dr[6].ToString();
                dr1[7] = dr[7].ToString();

                dtType2.Rows.Add(dr1);
            }

            return dtType2;
        }

        //正则表达式替换通用方法
        public static string RegexReplace(string StrReplace, string strRegex, string NewStr)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return regex.Replace(StrReplace, NewStr);
        }

        //正则表达式
        public static string[] strRegex(string Str, string StrRegex)
        {
            Regex regex = new Regex(StrRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(Str);

            string[] strings = new string[m.Length];
            int i = 0;

            while (m.Success && i < m.Length)
            {
                strings[i] = m.Value;
                m = m.NextMatch();
                i++;
            }

            return strings;
        }
        private class ReplaceKey
        {
            public string[,] Key;

            public ReplaceKey()
            {
                Key = new string[2, 84];

                Key[0, 0] = "00"; Key[1, 0] = "A";
                Key[0, 1] = "11"; Key[1, 1] = "B";
                Key[0, 2] = "22"; Key[1, 2] = "H";
                Key[0, 3] = "33"; Key[1, 3] = "e";
                Key[0, 4] = "44"; Key[1, 4] = "F";
                Key[0, 5] = "55"; Key[1, 5] = "E";
                Key[0, 6] = "66"; Key[1, 6] = "M";
                Key[0, 7] = "77"; Key[1, 7] = "z";
                Key[0, 8] = "88"; Key[1, 8] = "I";
                Key[0, 9] = "99"; Key[1, 9] = "b";

                Key[0, 10] = "10"; Key[1, 10] = "K";
                Key[0, 11] = "20"; Key[1, 11] = "L";
                Key[0, 12] = "30"; Key[1, 12] = "C";
                Key[0, 13] = "40"; Key[1, 13] = "N";
                Key[0, 14] = "50"; Key[1, 14] = "l";
                Key[0, 15] = "60"; Key[1, 15] = "n";
                Key[0, 16] = "70"; Key[1, 16] = "m";
                Key[0, 17] = "80"; Key[1, 17] = "R";
                Key[0, 18] = "90"; Key[1, 18] = "a";

                Key[0, 19] = "01"; Key[1, 19] = "T";
                Key[0, 20] = "21"; Key[1, 20] = "U";
                Key[0, 21] = "31"; Key[1, 21] = "j";
                Key[0, 22] = "41"; Key[1, 22] = "W";
                Key[0, 23] = "51"; Key[1, 23] = "X";
                Key[0, 24] = "61"; Key[1, 24] = "V";
                Key[0, 25] = "71"; Key[1, 25] = "Z";
                Key[0, 26] = "81"; Key[1, 26] = "S";
                Key[0, 27] = "91"; Key[1, 27] = "J";

                Key[0, 28] = "02"; Key[1, 28] = "c";
                Key[0, 29] = "12"; Key[1, 29] = "d";
                Key[0, 30] = "32"; Key[1, 30] = "D";
                Key[0, 31] = "42"; Key[1, 31] = "f";
                Key[0, 32] = "52"; Key[1, 32] = "G";
                Key[0, 33] = "62"; Key[1, 33] = "h";
                Key[0, 34] = "72"; Key[1, 34] = "i";
                Key[0, 35] = "82"; Key[1, 35] = "w";
                Key[0, 36] = "92"; Key[1, 36] = "k";

                Key[0, 37] = "03"; Key[1, 37] = "O";
                Key[0, 38] = "13"; Key[1, 38] = "Q";
                Key[0, 39] = "23"; Key[1, 39] = "P";
                Key[0, 40] = "43"; Key[1, 40] = "o";
                Key[0, 41] = "53"; Key[1, 41] = "p";
                Key[0, 42] = "63"; Key[1, 42] = "x";
                Key[0, 43] = "73"; Key[1, 43] = "t";
                Key[0, 44] = "83"; Key[1, 44] = "s";
                Key[0, 45] = "93"; Key[1, 45] = "r";

                Key[0, 46] = "04"; Key[1, 46] = "u";
                Key[0, 47] = "14"; Key[1, 47] = "v";
                Key[0, 48] = "24"; Key[1, 48] = "Y";
                Key[0, 49] = "34"; Key[1, 49] = "q";
                Key[0, 50] = "54"; Key[1, 50] = "y";
                Key[0, 51] = "64"; Key[1, 51] = "g";
                Key[0, 52] = "74"; Key[1, 52] = "!";
                Key[0, 53] = "84"; Key[1, 53] = "@";
                Key[0, 54] = "94"; Key[1, 54] = "#";

                Key[0, 55] = "05"; Key[1, 55] = "$";
                Key[0, 56] = "15"; Key[1, 56] = "%";
                Key[0, 57] = "25"; Key[1, 57] = "^";
                Key[0, 58] = "35"; Key[1, 58] = "&";
                Key[0, 59] = "45"; Key[1, 59] = "*";
                Key[0, 60] = "65"; Key[1, 60] = "(";
                Key[0, 61] = "75"; Key[1, 61] = ")";
                Key[0, 62] = "85"; Key[1, 62] = "_";
                Key[0, 63] = "95"; Key[1, 63] = "-";

                Key[0, 64] = "06"; Key[1, 64] = "+";
                Key[0, 65] = "16"; Key[1, 65] = "=";
                Key[0, 66] = "26"; Key[1, 66] = "|";
                Key[0, 67] = "36"; Key[1, 67] = "\\";
                Key[0, 68] = "46"; Key[1, 68] = "<";
                Key[0, 69] = "56"; Key[1, 69] = ">";
                Key[0, 70] = "76"; Key[1, 70] = ",";
                Key[0, 71] = "86"; Key[1, 71] = ".";
                Key[0, 72] = "96"; Key[1, 72] = "?";

                Key[0, 73] = "07"; Key[1, 73] = "/";
                Key[0, 74] = "17"; Key[1, 74] = "[";
                Key[0, 75] = "27"; Key[1, 75] = "]";
                Key[0, 76] = "37"; Key[1, 76] = "{";
                Key[0, 77] = "47"; Key[1, 77] = "}";
                Key[0, 78] = "57"; Key[1, 78] = ":";
                Key[0, 79] = "67"; Key[1, 79] = ";";
                Key[0, 80] = "87"; Key[1, 80] = "\"";
                Key[0, 81] = "97"; Key[1, 81] = "\'";

                Key[0, 82] = "08"; Key[1, 82] = "`";
                Key[0, 83] = "18"; Key[1, 83] = "~";
            }
        }
    }
}

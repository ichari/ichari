using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Net;
using GetSporttery;

using Shove.Database;

namespace GetSportteryService
{
    class Clutch
    {
        public static string ConnectionString = "";
        public static string ConnectionStringInformation = "";
        public string Source = "";
        public static string PorXY = "";
        public static int Port = 80;

        int n = 1;
        Basketball Basket = new Basketball();

        Log Clutchini = new Log(System.AppDomain.CurrentDomain.BaseDirectory + "Clutch.ini");
        Log log = new Log("Sporttery");

        #region 进度显示
        private void PrintThrendInfo(string Info)
        {
            lock (Clutchini)
            {
                try
                {
                    Clutchini.Write("足彩 " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + Info);
                    n++;
                    if (n >= 1000)
                    {
                        n = 1;
                    }
                }
                catch (SystemException ex)
                {
                    Clutchini.Write("足彩 " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + ex.Message + ex.StackTrace);
                }
            }
        }

        private void PrintThrendInfoBascket(string Info)
        {
            lock (Clutchini)
            {
                try
                {
                    Clutchini.Write("篮彩 " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + Info);
                    n++;
                    if (n >= 1000)
                    {
                        n = 1;
                    }
                }
                catch (SystemException ex)
                {
                    Clutchini.Write("篮彩 " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + ex.Message + ex.StackTrace);
                }
            }
        }

        private void PrintThrendInfo(string Info, string type)
        {
            lock (Clutchini)
            {
                try
                {
                    Clutchini.Write(type + " " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + Info);
                    n++;
                    if (n >= 1000)
                    {
                        n = 1;
                    }
                }
                catch (SystemException ex)
                {
                    Clutchini.Write(type + " " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + ex.Message + ex.StackTrace);
                }
            }
        }

        #endregion

        #region 得到html

        public string GetHtml(string Url, bool IsGB)
        {
            HttpWebResponse hwrs = null;
            StreamReader reader = null;
            HttpWebRequest hwr = null;

            try
            {
                hwr = (HttpWebRequest)HttpWebRequest.Create(Url);      //建立HttpWebRequest對象
                hwr.Timeout = 120000;                                                  //定義服務器超時時間
                hwr.UseDefaultCredentials = true;                                      //啟用網關認証

                try
                {
                    hwrs = (HttpWebResponse)hwr.GetResponse();              //取得回應
                }
                catch
                {
                    return null;
                }

                //判断HTTP响应状态 
                if (hwrs.StatusCode != HttpStatusCode.OK)
                {
                    PrintThrendInfo("GetHtml(string Url):抓取页面\"" + Url + "\"出错,非致命错误请重试,当前错误：" + hwrs.StatusCode.ToString());
                    return null;
                }
                else
                {
                    if (IsGB)
                    {
                        reader = new StreamReader(hwrs.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                    }
                    else
                    {
                        reader = new StreamReader(hwrs.GetResponseStream(), Encoding.Default);
                    }
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch (SystemException)
            {
                return null;
            }
            finally
            {
                if (hwrs != null)
                {
                    hwrs.Close();
                    hwrs = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (hwr != null)
                {
                    hwr = null;
                }
            }
        }

        public string get_html(string Url)
        {
            HttpWebResponse hwrs = null;
            StreamReader reader = null;
            HttpWebRequest hwr = null;

            try
            {
                hwr = (HttpWebRequest)HttpWebRequest.Create(Url);      //建立HttpWebRequest對象
                hwr.Timeout = 120000;                                                  //定義服務器超時時間
                //  WebProxy proxy = new WebProxy(PorXY, Port);
                //proxy.Credentials = new NetworkCredential("f3210316", "6978233");      //用戶名,密碼
                hwr.UseDefaultCredentials = true;                                      //啟用網關認証
                //    hwr.Proxy = proxy;

                try
                {
                    hwrs = (HttpWebResponse)hwr.GetResponse();              //取得回應
                }
                catch
                {
                    // MessageBox.Show("无法连接代理！");
                    return null;
                }

                //判断HTTP响应状态 
                if (hwrs.StatusCode != HttpStatusCode.OK)
                {
                    PrintThrendInfo("GetHtml(string Url):抓取页面\"" + Url + "\"出错,非致命错误请重试,当前错误：" + hwrs.StatusCode.ToString());
                    return null;
                }
                else
                {
                    reader = new StreamReader(hwrs.GetResponseStream(), Encoding.Default);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch (SystemException)
            {
                //Log.HandleException("行：3004附近：", "GetHtml(string Url):抓取页面\"" + Url + "\"出错,错误：" + ex.Message);
                return null;
            }
            finally
            {
                if (hwrs != null)
                {
                    hwrs.Close();
                    hwrs = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (hwr != null)
                {
                    hwr = null;
                }
            }
        }

        public string GetHtml(string Url, string EncodeStr)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                //request.UserAgent = "www.svnhost.cn";
                request.Timeout = 1200000;//120秒
                //request.AllowAutoRedirect = false;
                //    WebProxy proxy = new WebProxy(PorXY, Port);                                   //定義一個網關對象
                request.UseDefaultCredentials = true;                                      //啟用網關認証
                //   request.Proxy = proxy;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();              //取得回應
                }
                catch
                {
                    //MessageBox.Show("无法连接代理！");
                    return null;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    string html = reader.ReadToEnd();
                    if (html.Length < 100)
                    {
                        PrintThrendInfo("GetHtml(string Url,string EncodeStr):抓取页面\"" + Url + "\"出错,页面为空值.");
                        return null;
                    }
                    else
                    {
                        return html;
                    }
                }
                else
                {
                    PrintThrendInfo("GetHtml(string Url,string EncodeStr):抓取页面\"" + Url + "\"出错,非致命错误请重试,当前错误：" + response.StatusCode.ToString() + response.StatusDescription);
                    return null;
                }
            }
            catch (SystemException ex)
            {
                ErrLog.HandleException("行：3064附近：", "GetHtml(string Url,string EncodeStr):抓取页面\"" + Url + "\"出错,错误：" + ex.Message);
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

        #endregion 得到html

        #region 过关奖金

        public void GetPassRate()
        {
            switch (Source)
            {
                case "betzc":
                    GetbetzcPassRate();
                    break;

                case "500wan":
                    Get500wanPassRate();
                    break;

                case "Sporttery":
                    GetSportteryPassRate();
                    break;
            }
        }

        public void GetbetzcPassRate()
        {
            int Count = 0;
            string[,] ArryStrUrl = GetStrUrl();
            string Html = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetPassRateHtml(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Gethhad_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //比分
                        sb.Append(Getcrs_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //总进球数
                        sb.Append(Getttg_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //半登场胜平负
                        sb.Append(Gethafu_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }
            PrintThrendInfo("过关奖金抓取：完成过关奖金抓取数据抓取.");
        }

        public void Get500wanPassRate()
        {
            int Count = 0;
            string[,] ArryStrUrl = Get500wanStrUrl();
            string Html = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetPassRateHtml(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Get500wanhhad_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //比分
                        sb.Append(Get500wancrs_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //总进球数
                        sb.Append(Get500wanttg_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //半登场胜平负
                        sb.Append(Get500wanhafu_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }
            PrintThrendInfo("过关奖金抓取：完成过关奖金抓取数据抓取.");
        }

        public void GetSportteryPassRate()
        {
            int Count = 0;
            string[,] ArryStrUrl = GetSportteryStrUrl();
            string Html = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetPassRateHtml(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(GetSportteryhhad_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //比分
                        sb.Append(GetSportterycrs_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //总进球数
                        sb.Append(GetSportteryttg_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //半登场胜平负
                        sb.Append(GetSportteryhafu_listCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }
            PrintThrendInfo("过关奖金抓取：完成过关奖金抓取数据抓取.");
        }

        /// <summary>
        /// 得到URL和标题
        /// </summary>
        /// <returns></returns>
        private string[,] GetStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://buy.betzc.com/jczq/spfgg.html";
            strUrl[0, 1] = "胜平负";
            strUrl[1, 0] = "http://buy.betzc.com/jczq/bfgg.html";
            strUrl[1, 1] = "比分";
            strUrl[2, 0] = "http://buy.betzc.com/jczq/jqsgg.html";
            strUrl[2, 1] = "总进球数";
            strUrl[3, 0] = "http://buy.betzc.com/jczq/bqcgg.html";
            strUrl[3, 1] = "半全场胜平负";

            return strUrl;
        }

        /// <summary>
        /// 得到URL和标题
        /// </summary>
        /// <returns></returns>
        private string[,] Get500wanStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://trade.500wan.com/jczq/index.php?playid=269";
            strUrl[0, 1] = "胜平负";
            strUrl[1, 0] = "http://trade.500wan.com/jczq/index.php?playid=271";
            strUrl[1, 1] = "比分";
            strUrl[2, 0] = "http://trade.500wan.com/jczq/index.php?playid=270";
            strUrl[2, 1] = "总进球数";
            strUrl[3, 0] = "http://trade.500wan.com/jczq/index.php?playid=272";
            strUrl[3, 1] = "半全场胜平负";

            return strUrl;
        }

        /// <summary>
        /// 得到URL和标题
        /// </summary>
        /// <returns></returns>
        private string[,] GetSportteryStrUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://info.sporttery.cn/lotterygame/hhad_list.php";
            strUrl[0, 1] = "胜平负";
            strUrl[1, 0] = "http://info.sporttery.cn/lotterygame/crs_list.php";
            strUrl[1, 1] = "比分";
            strUrl[2, 0] = "http://info.sporttery.cn/lotterygame/ttg_list.php";
            strUrl[2, 1] = "总进球数";
            strUrl[3, 0] = "http://info.sporttery.cn/lotterygame/hafu_list.php";
            strUrl[3, 1] = "半全场胜平负";

            return strUrl;
        }

        /// <summary>
        /// 得到过关奖金Html
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetPassRateHtml(string strUrl, string title)
        {
            string Html = string.Empty;
            //PrintThrendInfo(title + "抓取：开始" + title + "数据抓取.");
            //PrintThrendInfo(title + "抓取：目标地址\"" + strUrl + "\"");
            Html = GetHtml(strUrl, false);          //得到html页面内容
            Html = HTMLEdit.OnlyBody(Html);
            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfo(title + "抓取：没有获取到Html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            //PrintThrendInfo(title + "抓取：读取页面\"" + strUrl + "\"成功.开始分析页面数据...");
            return Html.Replace("&amp;amp;nbsp;", "");
        }

        /// <summary>
        /// 取Html标签中元素
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string getHtmlText(string str, int i)
        {
            try
            {
                if (i == 1)
                {
                    str = str.Split('>')[str.Split('>').Length - 1];
                }
                else
                {
                    str = str.Split('>')[str.Split('>').Length - i].Split('<')[0];
                }
            }
            catch
            {
                return "";
            }

            return TrimSpaceletter(str);
        }

        /// <summary>
        /// 去除前后空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string TrimSpaceletter(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"^\s*|\s*$", ""); ;
        }

        /// <summary>
        /// 过滤得到日期格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetDate(string str)
        {
            return System.Text.RegularExpressions.Regex.Match(str, @"\d{4}-\d{2}-\d{2}").Value;
        }

        /// <summary>
        /// 过滤得到时间格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string GetTime(string str)
        {
            return System.Text.RegularExpressions.Regex.Match(str, @"\d{2}:\d{2}").Value;
        }

        /// <summary>
        /// 过滤得到时间格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string GetDateTime(string str)
        {
            return System.Text.RegularExpressions.Regex.Match(str, @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}").Value;
        }

        /// <summary>
        /// 得到胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private string Gethhad_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string id = string.Empty;
            string str_win = string.Empty;
            string str_flat = string.Empty;
            string str_lose = string.Empty;
            string str_date = string.Empty;
            string LoseBall = "0";
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_PassRate set IsHhad = 0 ; ");
            Other.GetStrScope(Html, "<TR CLASS=\"trlblue\">", "</TBODY", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            if (ILen == 0)
            {
                log.Write("胜平负 Gethhad_listCmd 方法 Other.GetStrScope(Html, \"<TR CLASS=\"trlblue\">\", \"</TBODY\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            string[] days = Html.Split(new string[] { "<TR CLASS=\"trlblue\">" }, StringSplitOptions.RemoveEmptyEntries);
            string table = string.Empty;
            string[] rows = null;
            string[] tds = null;
            string date = string.Empty;
            string LastDate = string.Empty;
            string GameColor = string.Empty;

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
                date = GetDate(rows[0]);

                for (int j = 1; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 12)
                    {
                        MatchNumber = getHtmlText(tds[0], 2);
                        Game = getHtmlText(tds[1], 1).Replace(" ", "");
                        GameColor = tds[1].Substring(tds[1].IndexOf("#"), 7);
                        MainTeam = getHtmlText(tds[3], 2);
                        GuestTeam = getHtmlText(tds[4], 2);
                        id = Count.ToString();
                        str_date = date + " " + GetTime(getHtmlText(tds[2], 2));

                        if (!string.IsNullOrEmpty(str_date) && (Shove._Convert.StrToDateTime(str_date, DateTime.Now.ToString()).CompareTo(Shove._Convert.StrToDateTime(LastDate, DateTime.Now.ToString())) < 0) && MatchNumber.IndexOf("001") < 0)
                        {
                            str_date = Shove._Convert.StrToDateTime(str_date, DateTime.Now.ToString()).AddDays(1).ToString();
                        }

                        str_win = getHtmlText(tds[6], 3);
                        str_flat = getHtmlText(tds[7], 3);
                        str_lose = getHtmlText(tds[8], 3);
                        LoseBall = getHtmlText(tds[5], 1);
                        LoseBall = LoseBall == "" ? "0" : LoseBall;

                        LastDate = str_date;

                        sb.Append(" exec P_spfgg '" + MainTeam + "','" + GuestTeam + "','" + Game + "','" + MatchNumber + "'," + str_win + "," + str_flat + "," + str_lose + "," + LoseBall + "," + ",'" + str_date + "' 0, 0, 0, 0, ''; ");

                        sb.Append("update T_PassRate set GameColor = '" + GameColor + "' where MatchNumber= '" + MatchNumber + "' ; ");
                        sb.Append("update T_Match set GameColor = '" + GameColor + "', MainLoseBall = '" + LoseBall + "' where MainTeam = '" + MainTeam + "' and GuestTeam = '" + GuestTeam + "' and dateadd(d, 4, StopSellingTime) > GETDATE() and MatchNumber= '" + MatchNumber + "' ; ");

                        Count++;
                    }
                }

            }

            sb.Append(" delete T_PassRate where StopSellTime < dateadd(day, -1,getdate()); ");
            return sb.ToString();
        }

        /// <summary>
        /// 得到比分
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Getcrs_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTable = null;
            string table = string.Empty;
            string[] trs = null;
            string[] tds = null;

            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsCrs = 0 ; ");

            #region 定义对应字段

            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string S1_0 = string.Empty;
            string S2_0 = string.Empty;
            string S2_1 = string.Empty;
            string S3_0 = string.Empty;
            string S3_1 = string.Empty;
            string S3_2 = string.Empty;
            string S4_0 = string.Empty;
            string S4_1 = string.Empty;
            string S4_2 = string.Empty;
            string S5_0 = string.Empty;
            string S5_1 = string.Empty;
            string S5_2 = string.Empty;
            string SOther = string.Empty;
            string P0_0 = string.Empty;
            string P1_1 = string.Empty;
            string P2_2 = string.Empty;
            string P3_3 = string.Empty;
            string POther = string.Empty;
            string F0_1 = string.Empty;
            string F0_2 = string.Empty;
            string F1_2 = string.Empty;
            string F0_3 = string.Empty;
            string F1_3 = string.Empty;
            string F2_3 = string.Empty;
            string F0_4 = string.Empty;
            string F1_4 = string.Empty;
            string F2_4 = string.Empty;
            string F0_5 = string.Empty;
            string F1_5 = string.Empty;
            string F2_5 = string.Empty;
            string FOther = string.Empty;

            #endregion 定义对应字段

            Other.GetStrScope(Html, "<TR CLASS=\"trlblue\">", "<FORM ID=\"initform\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            if (Html.IndexOf("<TABLE WIDTH=\"100%\" CELLSPACING=\"1\" CELLPADDING=\"2\" BORDER=\"0\" BGCOLOR=\"#ebebeb\" CLASS=\"trtwoborder\">") >= 0)
            {
                HtmlTable = Html.Split(new string[] { "<TABLE WIDTH=\"100%\" CELLSPACING=\"1\" CELLPADDING=\"2\" BORDER=\"0\" BGCOLOR=\"#ebebeb\" CLASS=\"trtwoborder\">" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                log.Write("足彩比分抓取 Gethhad_listCmd 方法 if (Html.IndexOf(\"<TABLE WIDTH=\"100%\" CELLS…… 返回 -1");
                return "";
            }

            try
            {
                MatchNumber = getHtmlText(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[1], 2);
                Game = getHtmlText(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[2], 1);
                MainTeam = getHtmlText(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[4], 2);
                GuestTeam = getHtmlText(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[5], 2);

                Stopselltime = GetDate(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[3]) + "-" + getHtmlText(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[3], 2) + ":00";
            }
            catch
            {
            }

            for (int i = 1; i < HtmlTable.Length; i++)
            {
                Other.GetStrScope(HtmlTable[i], "<TBODY>", "</TBODY>", out IStart, out ILen);
                if (ILen != 0)
                {
                    table = HtmlTable[i].Substring(IStart, ILen);
                    trs = table.Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                    #region 赋值

                    //主胜
                    tds = trs[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    S1_0 = getHtmlText(tds[1], 4);
                    S2_0 = getHtmlText(tds[2], 4);
                    S2_1 = getHtmlText(tds[3], 4);
                    S3_0 = getHtmlText(tds[4], 4);
                    S3_1 = getHtmlText(tds[5], 4);
                    S3_2 = getHtmlText(tds[6], 4);
                    S4_0 = getHtmlText(tds[7], 4);
                    S4_1 = getHtmlText(tds[8], 4);
                    S4_2 = getHtmlText(tds[9], 4);
                    S5_0 = getHtmlText(tds[10], 4);
                    S5_1 = getHtmlText(tds[11], 4);
                    S5_2 = getHtmlText(tds[12], 4);
                    SOther = getHtmlText(tds[13], 4);

                    //主平
                    tds = trs[1].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    P0_0 = getHtmlText(tds[1], 4);
                    P1_1 = getHtmlText(tds[2], 4);
                    P2_2 = getHtmlText(tds[3], 4);
                    P3_3 = getHtmlText(tds[4], 4);
                    POther = getHtmlText(tds[5], 4);

                    //主负
                    tds = trs[2].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    F0_1 = getHtmlText(tds[1], 4);
                    F0_2 = getHtmlText(tds[2], 4);
                    F1_2 = getHtmlText(tds[3], 4);
                    F0_3 = getHtmlText(tds[4], 4);
                    F1_3 = getHtmlText(tds[5], 4);
                    F2_3 = getHtmlText(tds[6], 4);
                    F0_4 = getHtmlText(tds[7], 4);
                    F1_4 = getHtmlText(tds[8], 4);
                    F2_4 = getHtmlText(tds[9], 4);
                    F0_5 = getHtmlText(tds[10], 4);
                    F1_5 = getHtmlText(tds[11], 4);
                    F2_5 = getHtmlText(tds[12], 4);
                    FOther = getHtmlText(tds[13], 4);

                    #endregion 赋值

                    sb.Append(" exec P_Bfgg '");
                    sb.Append(MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "'," + S1_0 + "," + S2_0 + "," + S2_1 + "," + S3_0 + "," + S3_1 + ",");
                    sb.Append(S3_2 + "," + S4_0 + "," + S4_1 + "," + S4_2 + "," + S5_0 + "," + S5_1 + ",");
                    sb.Append(S5_2 + "," + SOther + "," + P0_0 + "," + P1_1 + "," + P2_2 + "," + P3_3 + ",");
                    sb.Append(POther + "," + F0_1 + "," + F0_2 + "," + F1_2 + "," + F0_3 + "," + F1_3 + ",");
                    sb.Append(F2_3 + "," + F0_4 + "," + F1_4 + "," + F2_4 + "," + F0_5 + "," + F1_5 + "," + F2_5 + "," + FOther + ", 0, ''; ");

                    try
                    {
                        MatchNumber = getHtmlText(HtmlTable[i].Substring(HtmlTable[i].IndexOf("</TBODY>")).Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[1], 2);
                        Game = getHtmlText(HtmlTable[i].Substring(HtmlTable[i].IndexOf("</TBODY>")).Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[2], 1);
                        MainTeam = getHtmlText(HtmlTable[i].Substring(HtmlTable[i].IndexOf("</TBODY>")).Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[4], 2);
                        GuestTeam = getHtmlText(HtmlTable[i].Substring(HtmlTable[i].IndexOf("</TBODY>")).Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[5], 2);
                        Stopselltime = GetDate(HtmlTable[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[3]) + "-" + getHtmlText(HtmlTable[i].Substring(HtmlTable[i].IndexOf("</TBODY>")).Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries)[3], 2) + ":00";
                    }
                    catch { }

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 总进球数SQL命令
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Getttg_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string id = string.Empty;
            string in0 = string.Empty;
            string in1 = string.Empty;
            string in2 = string.Empty;
            string in3 = string.Empty;
            string in4 = string.Empty;
            string in5 = string.Empty;
            string in6 = string.Empty;
            string in7 = string.Empty;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsTtg = 0 ; ");

            Other.GetStrScope(Html, "<TR CLASS=\"trlblue\">", "<FORM ID=\"initform\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩总进球数 Getttg_listCmd 方法 Other.GetStrScope(Html, \"<TR CLASS=\"trlblue\">\", \"<FORM ID=\"initform\"\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);
            HtmlTr = SplitString(Html, "</TR>");
            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 15)
                {
                    id = Count.ToString();
                    in0 = getHtmlText(HtmlTd[4], 3);
                    in1 = getHtmlText(HtmlTd[5], 3);
                    in2 = getHtmlText(HtmlTd[6], 3);
                    in3 = getHtmlText(HtmlTd[7], 3);
                    in4 = getHtmlText(HtmlTd[8], 3);
                    in5 = getHtmlText(HtmlTd[9], 3);
                    in6 = getHtmlText(HtmlTd[10], 3);
                    in7 = getHtmlText(HtmlTd[11], 3);
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 1);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTr[i + 1], 3);

                    Stopselltime = GetDate(HtmlTd[2]) + "-" + getHtmlText(HtmlTd[2], 4) + " " + getHtmlText(HtmlTd[2], 3) + ":00";

                    sb.Append(" exec P_Jqsgg '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "'," + in0 + "," + in1 + ",");
                    sb.Append(in2 + "," + in3 + "," + in4 + "," + in5 + "," + in6 + "," + in7 + ", 0, '';");
                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 半全场胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Gethafu_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string id = string.Empty;
            string SS = string.Empty;
            string SP = string.Empty;
            string SF = string.Empty;
            string PS = string.Empty;
            string PP = string.Empty;
            string PF = string.Empty;
            string FS = string.Empty;
            string FP = string.Empty;
            string FF = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsHafu = 0 ; ");

            Count = 0;

            Other.GetStrScope(Html, "<TR CLASS=\"trlblue\">", "<FORM ID=\"initform\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩半全场胜平负 Gethafu_listCmd 方法 Other.GetStrScope(Html, \"<TR CLASS=\"trlblue\">\", \"<FORM ID=\"initform\"\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "</TR>");

            for (int i = 1; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 16)
                {
                    id = Count.ToString();
                    SS = getHtmlText(HtmlTd[4], 3);
                    SP = getHtmlText(HtmlTd[5], 3);
                    SF = getHtmlText(HtmlTd[6], 3);
                    PS = getHtmlText(HtmlTd[7], 3);
                    PP = getHtmlText(HtmlTd[8], 3);
                    PF = getHtmlText(HtmlTd[9], 3);
                    FS = getHtmlText(HtmlTd[10], 3);
                    FP = getHtmlText(HtmlTd[11], 3);
                    FF = getHtmlText(HtmlTd[12], 3);
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 1);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTr[i + 1], 3);

                    Stopselltime = GetDate(HtmlTd[2]) + "-" + getHtmlText(HtmlTd[2], 4) + " " + getHtmlText(HtmlTd[2], 3) + ":00";

                    sb.Append(" exec P_Bqcgg '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "'," + SS + "," + SP + "," + SF + ",");
                    sb.Append(PS + "," + PP + "," + PF + "," + FS + "," + FP + "," + FF + ", 0, '';");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private string Get500wanhhad_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string str_win = string.Empty;
            string str_flat = string.Empty;
            string str_lose = string.Empty;
            string str_date = string.Empty;
            string LoseBall = "0";
            string MatchNumber = string.Empty;
            string TempMatchNumber = "";
            string Game = string.Empty;
            string GameColor = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_PassRate set IsHhad = 0 ; ");
            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            if (ILen == 0)
            {
                log.Write("胜平负 Get500wanhhad_listCmd 方法 Other.GetStrScope(Html, \"<div class=\"dc_hs\" <div class=\"dc_r\"\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string table = string.Empty;
            string[] rows = null;
            string[] tds = null;
            string LastDate = string.Empty;
            string EuropeSSP = "";
            string EuropePSP = "";
            string EuropeFSP = "";

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 14)
                    {
                        MatchNumber = getHtmlText(tds[0], 2);
                        Game = getHtmlText(tds[1], 2);
                        GameColor = RegexString(tds[1], "background[^>]*>")[0];
                        GameColor = GameColor.Substring(GameColor.IndexOf("#"), GameColor.IndexOf(";") - GameColor.IndexOf("#"));
                        MainTeam = getHtmlText(tds[3], 2);
                        GuestTeam = getHtmlText(tds[5], 4);
                        str_date = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        TempMatchNumber = MatchNumber;

                        str_win = getHtmlText(tds[8], 2);
                        str_flat = getHtmlText(tds[9], 2);
                        str_lose = getHtmlText(tds[10], 2);
                        EuropeSSP = getHtmlText(tds[6], 15);
                        EuropePSP = getHtmlText(tds[6], 13);
                        EuropeFSP = getHtmlText(tds[6], 11);
                        LoseBall = getHtmlText(tds[4], 2).Replace("+", "");
                        LoseBall = LoseBall == "" ? "0" : LoseBall;

                        if (string.IsNullOrEmpty(str_win))
                        {
                            str_win = "0";
                        }

                        if (string.IsNullOrEmpty(str_flat))
                        {
                            str_flat = "0";
                        }

                        if (string.IsNullOrEmpty(str_lose))
                        {
                            str_lose = "0";
                        }

                        sb.Append(" exec P_spfgg '" + MainTeam + "','" + GuestTeam + "','" + Game + "','" + MatchNumber + "'," + str_win + "," + str_flat + "," + str_lose + "," + LoseBall + ",'" + MatchDate + "','" + str_date + "', '" + EuropeSSP + "','" + EuropePSP + "','" + EuropeFSP + "', 0, ''; ");

                        sb.Append("update T_PassRate set GameColor = '" + GameColor + "' where MatchNumber= '" + MatchNumber + "' ; ");
                        sb.Append("update T_Match set GameColor = '" + GameColor + "', MainLoseBall = '" + LoseBall + "' where MainTeam = '" + MainTeam + "' and GuestTeam = '" + GuestTeam + "' and dateadd(d, 4, StopSellingTime) > GETDATE() and MatchNumber= '" + MatchNumber + "' ; ");

                        Count++;
                    }
                }

            }

            sb.Append(" delete T_PassRate where StopSellTime < dateadd(day, -1,getdate()); ");
            return sb.ToString();
        }

        /// <summary>
        /// 得到比分
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wancrs_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string table = string.Empty;
            string[] rows = null;
            string date = string.Empty;
            string[] tds = null;
            string[] trs = null;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsCrs = 0 ; ");

            #region 定义对应字段

            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string S1_0 = string.Empty;
            string S2_0 = string.Empty;
            string S2_1 = string.Empty;
            string S3_0 = string.Empty;
            string S3_1 = string.Empty;
            string S3_2 = string.Empty;
            string S4_0 = string.Empty;
            string S4_1 = string.Empty;
            string S4_2 = string.Empty;
            string S5_0 = string.Empty;
            string S5_1 = string.Empty;
            string S5_2 = string.Empty;
            string SOther = string.Empty;
            string P0_0 = string.Empty;
            string P1_1 = string.Empty;
            string P2_2 = string.Empty;
            string P3_3 = string.Empty;
            string POther = string.Empty;
            string F0_1 = string.Empty;
            string F0_2 = string.Empty;
            string F1_2 = string.Empty;
            string F0_3 = string.Empty;
            string F1_3 = string.Empty;
            string F2_3 = string.Empty;
            string F0_4 = string.Empty;
            string F1_4 = string.Empty;
            string F2_4 = string.Empty;
            string F0_5 = string.Empty;
            string F1_5 = string.Empty;
            string F2_5 = string.Empty;
            string FOther = string.Empty;

            #endregion 定义对应字段

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            rows = Html.Split(new string[] { "ZID" }, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j < rows.Length; j++)
            {
                tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    MatchNumber = getHtmlText(tds[0], 2);
                    Game = getHtmlText(tds[1], 2);
                    MainTeam = getHtmlText(tds[3], 2);
                    GuestTeam = getHtmlText(tds[4], 4);
                    Stopselltime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(tds[2]) + ":00";
                }
                catch { }

                table = rows[j];

                Other.GetStrScope(table, "<TBODY>", "</TBODY>", out IStart, out ILen);

                if (ILen != 0)
                {
                    table = table.Substring(IStart, ILen);
                    trs = table.Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                    #region 赋值

                    //主胜
                    tds = trs[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    S1_0 = getHtmlText(tds[1], 3);
                    S2_0 = getHtmlText(tds[2], 3);
                    S2_1 = getHtmlText(tds[3], 3);
                    S3_0 = getHtmlText(tds[4], 3);
                    S3_1 = getHtmlText(tds[5], 3);
                    S3_2 = getHtmlText(tds[6], 3);
                    S4_0 = getHtmlText(tds[7], 3);
                    S4_1 = getHtmlText(tds[8], 3);
                    S4_2 = getHtmlText(tds[9], 3);
                    S5_0 = getHtmlText(tds[10], 3);
                    S5_1 = getHtmlText(tds[11], 3);
                    S5_2 = getHtmlText(tds[12], 3);
                    SOther = getHtmlText(tds[0], 3);

                    //主平
                    tds = trs[1].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    P0_0 = getHtmlText(tds[1], 3);
                    P1_1 = getHtmlText(tds[2], 3);
                    P2_2 = getHtmlText(tds[3], 3);
                    P3_3 = getHtmlText(tds[4], 3);
                    POther = getHtmlText(tds[0], 3);

                    //主负
                    tds = trs[2].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    F0_1 = getHtmlText(tds[1], 3);
                    F0_2 = getHtmlText(tds[2], 3);
                    F1_2 = getHtmlText(tds[3], 3);
                    F0_3 = getHtmlText(tds[4], 3);
                    F1_3 = getHtmlText(tds[5], 3);
                    F2_3 = getHtmlText(tds[6], 3);
                    F0_4 = getHtmlText(tds[7], 3);
                    F1_4 = getHtmlText(tds[8], 3);
                    F2_4 = getHtmlText(tds[9], 3);
                    F0_5 = getHtmlText(tds[10], 3);
                    F1_5 = getHtmlText(tds[11], 3);
                    F2_5 = getHtmlText(tds[12], 3);
                    FOther = getHtmlText(tds[0], 3);

                    if (string.IsNullOrEmpty(S1_0))
                    {
                        S1_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_0))
                    {
                        S2_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_1))
                    {
                        S2_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_0))
                    {
                        S3_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_1))
                    {
                        S3_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_2))
                    {
                        S3_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_0))
                    {
                        S4_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_1))
                    {
                        S4_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_2))
                    {
                        S4_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_0))
                    {
                        S5_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_1))
                    {
                        S5_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_2))
                    {
                        S5_2 = "0";
                    }

                    if (string.IsNullOrEmpty(SOther))
                    {
                        SOther = "0";
                    }

                    if (string.IsNullOrEmpty(P0_0))
                    {
                        P0_0 = "0";
                    }

                    if (string.IsNullOrEmpty(P1_1))
                    {
                        P1_1 = "0";
                    }

                    if (string.IsNullOrEmpty(P2_2))
                    {
                        P2_2 = "0";
                    }

                    if (string.IsNullOrEmpty(P3_3))
                    {
                        P3_3 = "0";
                    }

                    if (string.IsNullOrEmpty(POther))
                    {
                        POther = "0";
                    }

                    if (string.IsNullOrEmpty(F0_1))
                    {
                        F0_1 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_2))
                    {
                        F0_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_2))
                    {
                        F1_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_3))
                    {
                        F0_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_3))
                    {
                        F1_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_3))
                    {
                        F2_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_4))
                    {
                        F0_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_4))
                    {
                        F1_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_4))
                    {
                        F2_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_5))
                    {
                        F0_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_5))
                    {
                        F1_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_5))
                    {
                        F2_5 = "0";
                    }

                    if (string.IsNullOrEmpty(FOther))
                    {
                        FOther = "0";
                    }

                    #endregion 赋值

                    sb.Append(" exec P_Bfgg '");
                    sb.Append(MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + S1_0 + "," + S2_0 + "," + S2_1 + "," + S3_0 + "," + S3_1 + ",");
                    sb.Append(S3_2 + "," + S4_0 + "," + S4_1 + "," + S4_2 + "," + S5_0 + "," + S5_1 + ",");
                    sb.Append(S5_2 + "," + SOther + "," + P0_0 + "," + P1_1 + "," + P2_2 + "," + P3_3 + ",");
                    sb.Append(POther + "," + F0_1 + "," + F0_2 + "," + F1_2 + "," + F0_3 + "," + F1_3 + ",");
                    sb.Append(F2_3 + "," + F0_4 + "," + F1_4 + "," + F2_4 + "," + F0_5 + "," + F1_5 + "," + F2_5 + "," + FOther + ", 0, ''; ");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 总进球数SQL命令
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wanttg_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string in0 = string.Empty;
            string in1 = string.Empty;
            string in2 = string.Empty;
            string in3 = string.Empty;
            string in4 = string.Empty;
            string in5 = string.Empty;
            string in6 = string.Empty;
            string in7 = string.Empty;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsTtg = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩总进球数 Getttg_listCmd 方法 Other.GetStrScope(Html, \"<DIV CLASS=\"dc_hs\" <DIV CLASS=\"dc_r\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);
            HtmlTr = SplitString(Html, "</TR>");
            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 15)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTd[4], 4);
                    Stopselltime = GetDateTime(HtmlTd[2].Substring(HtmlTd[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(HtmlTd[2]) + ":00";

                    in0 = getHtmlText(HtmlTd[6], 3);
                    in1 = getHtmlText(HtmlTd[7], 3);
                    in2 = getHtmlText(HtmlTd[8], 3);
                    in3 = getHtmlText(HtmlTd[9], 3);
                    in4 = getHtmlText(HtmlTd[10], 3);
                    in5 = getHtmlText(HtmlTd[11], 3);
                    in6 = getHtmlText(HtmlTd[12], 3);
                    in7 = getHtmlText(HtmlTd[13], 3);

                    if (string.IsNullOrEmpty(in0))
                    {
                        in0 = "0";
                    }

                    if (string.IsNullOrEmpty(in1))
                    {
                        in1 = "0";
                    }

                    if (string.IsNullOrEmpty(in2))
                    {
                        in2 = "0";
                    }

                    if (string.IsNullOrEmpty(in3))
                    {
                        in3 = "0";
                    }

                    if (string.IsNullOrEmpty(in4))
                    {
                        in4 = "0";
                    }

                    if (string.IsNullOrEmpty(in5))
                    {
                        in5 = "0";
                    }

                    if (string.IsNullOrEmpty(in6))
                    {
                        in6 = "0";
                    }

                    if (string.IsNullOrEmpty(in7))
                    {
                        in7 = "0";
                    }

                    sb.Append(" exec P_Jqsgg '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + in0 + "," + in1 + ",");
                    sb.Append(in2 + "," + in3 + "," + in4 + "," + in5 + "," + in6 + "," + in7 + ", 0, '';");
                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 半全场胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wanhafu_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string SS = string.Empty;
            string SP = string.Empty;
            string SF = string.Empty;
            string PS = string.Empty;
            string PP = string.Empty;
            string PF = string.Empty;
            string FS = string.Empty;
            string FP = string.Empty;
            string FF = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_PassRate set IsHafu = 0 ; ");
            Count = 0;

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩总进球数 Getttg_listCmd 方法 Other.GetStrScope(Html, \"<DIV CLASS=\"dc_hs\" <DIV CLASS=\"dc_r\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "</TR>");

            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 16)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTd[4], 4);
                    Stopselltime = GetDateTime(HtmlTd[2].Substring(HtmlTd[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(HtmlTd[2]) + ":00";

                    SS = getHtmlText(HtmlTd[6], 3);
                    SP = getHtmlText(HtmlTd[7], 3);
                    SF = getHtmlText(HtmlTd[8], 3);
                    PS = getHtmlText(HtmlTd[9], 3);
                    PP = getHtmlText(HtmlTd[10], 3);
                    PF = getHtmlText(HtmlTd[11], 3);
                    FS = getHtmlText(HtmlTd[12], 3);
                    FP = getHtmlText(HtmlTd[13], 3);
                    FF = getHtmlText(HtmlTd[14], 3);

                    if (string.IsNullOrEmpty(SS))
                    {
                        SS = "0";
                    }

                    if (string.IsNullOrEmpty(SP))
                    {
                        SP = "0";
                    }

                    if (string.IsNullOrEmpty(SF))
                    {
                        SF = "0";
                    }

                    if (string.IsNullOrEmpty(PS))
                    {
                        PS = "0";
                    }

                    if (string.IsNullOrEmpty(PP))
                    {
                        PP = "0";
                    }

                    if (string.IsNullOrEmpty(PF))
                    {
                        PF = "0";
                    }

                    if (string.IsNullOrEmpty(FS))
                    {
                        FS = "0";
                    }

                    if (string.IsNullOrEmpty(FP))
                    {
                        FP = "0";
                    }

                    if (string.IsNullOrEmpty(FF))
                    {
                        FF = "0";
                    }

                    sb.Append(" exec P_Bqcgg  '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + SS + "," + SP + "," + SF + ",");
                    sb.Append(PS + "," + PP + "," + PF + "," + FS + "," + FP + "," + FF + ", 0, '';");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public void GetOkoooInformation()
        {
            //澳客资讯
            string Html = GetPassRateHtml("http://www.okooo.com/Lottery06/SportterySoccer/SportterySoccerIndex.php", "胜平负");

            string MatchNumber = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {
                //Other.GetStrScope(Html, "<DIV CLASS=\"pt_list1\">", "<DIV CLASS=\"pagemain pt_list3 \">", out IStart, out ILen);

                Html = Html.Substring(Html.IndexOf("ID=\"table1\""));
                Html = Html.Substring(0, Html.IndexOf("</TABLE>"));
            }
            catch
            { }

            if (Html.Length < 1)
            {
                return;
            }

            string[] days = Html.Split(new string[] { "<TABLE" }, StringSplitOptions.RemoveEmptyEntries);
            string table = string.Empty;
            string[] rows = null;
            string[] tds = null;
            string OkoooID = "";
            string EuropeSSP = "";
            string EuropePSP = "";
            string EuropeFSP = "";
            string GameColor = string.Empty;
            string Day = "";

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                string Work = "";

                try
                {
                    Work = rows[0].Substring(rows[0].IndexOf("星期") + 2, 1);
                    Day = GetDate(rows[0].Substring(rows[0].IndexOf("星期") + 2, 20)).Replace("-", "");
                }
                catch
                {
                    continue;
                }

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);

                    if (tds.Length == 9)
                    {
                        try
                        {
                            MatchNumber = "周" + Work + getHtmlText(tds[0], 5);
                            OkoooID = tds[3].Substring(tds[3].IndexOf("/1/") + 3).Substring(0, tds[3].Substring(tds[3].IndexOf("/1/") + 3).IndexOf("/"));
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            GameColor = RegexString(tds[0], "#\\S{6}")[0];
                        }
                        catch
                        {
                        }

                        long InforMationID = 0;
                        long InforMationMainTeamID = 0;
                        long InforMationGuestTeamID = 0;
                        long InforMationMatchTypeID = 0;

                        try
                        {
                            EuropeSSP = getHtmlText(tds[4], 6);
                            EuropePSP = getHtmlText(tds[4], 4);
                            EuropeFSP = getHtmlText(tds[4], 2);
                        }
                        catch
                        {
                            continue;
                        }

                        try
                        {
                            DataTable dt = Shove.Database.MSSQL.Select(Clutch.ConnectionStringInformation, "select ID, HostTeamID, QuestTeamID, MatchTypeID from T_Matchs where OkoooMatchID = " + OkoooID, null);

                            if ((dt != null) && (dt.Rows.Count == 1))
                            {
                                InforMationID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0);
                                InforMationMainTeamID = Shove._Convert.StrToLong(dt.Rows[0]["HostTeamID"].ToString(), 0);
                                InforMationGuestTeamID = Shove._Convert.StrToLong(dt.Rows[0]["QuestTeamID"].ToString(), 0);
                                InforMationMatchTypeID = Shove._Convert.StrToLong(dt.Rows[0]["MatchTypeID"].ToString(), 0);
                            }
                        }
                        catch
                        {

                        }
                        sb.Append("update T_PassRate set GameColor = '" + GameColor + "', EuropeSSP = '" + EuropeSSP + "', EuropePSP = '" + EuropePSP + "', EuropeFSP = '" + EuropeFSP + "', InforMationID = " + InforMationID.ToString() + ", InforMationMainTeamID = " + InforMationMainTeamID.ToString() + ", InforMationGuestTeamID =" + InforMationGuestTeamID.ToString() + ", InforMationMatchTypeID = " + InforMationMatchTypeID.ToString() + " where MatchNumber= '" + MatchNumber + "' ; ");
                        sb.Append("exec P_CompensationRateAdd " + OkoooID + ",'" + Day + "','" + MatchNumber + "', 0, ''; ");

                        sb.Append("update T_singleRate set GameColor = '" + GameColor + "', EuropeSSP = '" + EuropeSSP + "', EuropePSP = '" + EuropePSP + "', EuropeFSP = '" + EuropeFSP + "', InforMationID = " + InforMationID.ToString() + ", InforMationMainTeamID = " + InforMationMainTeamID.ToString() + ", InforMationGuestTeamID =" + InforMationGuestTeamID.ToString() + ", InforMationMatchTypeID = " + InforMationMatchTypeID.ToString() + " where MatchNumber= '" + MatchNumber + "' ; ");
                    }
                }

            }

            ExecSql(sb, 1, "http://www.okooo.com/Lottery06/SportterySoccer/SportterySoccerIndex.php");
        }

        /// <summary>
        /// 胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private string GetSportteryhhad_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string id = string.Empty;
            string str_win = string.Empty;
            string str_flat = string.Empty;
            string str_lose = string.Empty;
            string str_date = string.Empty;
            string GameColor = string.Empty;
            string LoseBall = "0";
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string EuropeSSP = "";
            string EuropePSP = "";
            string EuropeFSP = "";

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_PassRate set IsHhad = 0 ; ");
            Other.GetStrScope(Html, "<TABLE CLASS=\"tabContent", "ID=\"match_list\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            HtmlTr = Html.Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tr in HtmlTr)
            {
                HtmlTd = tr.Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                if (HtmlTd.Length == 9)
                {
                    LoseBall = "0";         //初始值

                    MatchNumber = getHtmlText(HtmlTd[0], 1);
                    Game = getHtmlText(HtmlTd[1], 2);
                    GameColor = RegexString(HtmlTd[1], "bgcolor[^>]*>")[0];
                    GameColor = GameColor.Substring(GameColor.IndexOf("#"), GameColor.LastIndexOf("\"") - GameColor.IndexOf("#"));
                    MainTeam = Regex.Replace(HtmlTd[2].Split('>')[2].Split('<')[0], @"\s|(\(.+\))", "");
                    GuestTeam = HtmlTd[2].Split('>')[4].Split('<')[0];
                    id = Count.ToString();
                    if (HtmlTd[2].IndexOf(")<STRONG>") != -1)
                    {
                        LoseBall = SplitString(HtmlTd[2], ")<STRONG>")[0].Split('(')[1].Replace("+", "");
                    }
                    str_date = DateTime.Now.Year.ToString() + "-" + getHtmlText(HtmlTd[3], 1) + ":00";
                    str_win = getHtmlText(HtmlTd[4], 1);
                    str_flat = getHtmlText(HtmlTd[5], 1);
                    str_lose = getHtmlText(HtmlTd[6], 1);

                    sb.Append(" exec P_spfgg '" + MainTeam + "','" + GuestTeam + "','" + Game + "','" + MatchNumber + "'," + str_win + "," + str_flat + "," + str_lose + "," + LoseBall + "," + ",'" + str_date + "', '" + EuropeSSP + "','" + EuropePSP + "','" + EuropeFSP + "', 0, ''; ");

                    sb.Append("update T_PassRate set GameColor = '" + GameColor + "' where MatchNumber= '" + MatchNumber + "' ; ");
                    sb.Append("update V_Match set GameColor = '" + GameColor + "' where MainTeam like '" + MainTeam + "' + '%' and GuestTeam like '" + GuestTeam + "' + '%' and StopSellingTime > GETDATE() and MatchNumber= '" + MatchNumber + "' ; ");

                    Count++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 比分
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string GetSportterycrs_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlDiv = null;
            string[] HtmlTd = null;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsCrs = 0 ; ");

            #region 定义对应字段

            string MatchNumber = string.Empty;
            string S1_0 = string.Empty;
            string S2_0 = string.Empty;
            string S2_1 = string.Empty;
            string S3_0 = string.Empty;
            string S3_1 = string.Empty;
            string S3_2 = string.Empty;
            string S4_0 = string.Empty;
            string S4_1 = string.Empty;
            string S4_2 = string.Empty;
            string S5_0 = string.Empty;
            string S5_1 = string.Empty;
            string S5_2 = string.Empty;
            string SOther = string.Empty;
            string P0_0 = string.Empty;
            string P1_1 = string.Empty;
            string P2_2 = string.Empty;
            string P3_3 = string.Empty;
            string POther = string.Empty;
            string F0_1 = string.Empty;
            string F0_2 = string.Empty;
            string F1_2 = string.Empty;
            string F0_3 = string.Empty;
            string F1_3 = string.Empty;
            string F2_3 = string.Empty;
            string F0_4 = string.Empty;
            string F1_4 = string.Empty;
            string F2_4 = string.Empty;
            string F0_5 = string.Empty;
            string F1_5 = string.Empty;
            string F2_5 = string.Empty;
            string FOther = string.Empty;

            #endregion 定义对应字段

            Other.GetStrScope(Html, "<DIV CLASS=\"mainBody", "ID=\"match_list\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            HtmlDiv = Html.Split(new string[] { "</DIV>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string td in HtmlDiv)
            {
                HtmlTd = SplitString(td, "</TD>");

                if (HtmlTd.Length == 38)
                {
                    #region 赋值
                    MatchNumber = getHtmlText(HtmlTd[0], 1);
                    S1_0 = getHtmlText(HtmlTd[2], 1);
                    S2_0 = getHtmlText(HtmlTd[3], 1);
                    S2_1 = getHtmlText(HtmlTd[4], 1);
                    S3_0 = getHtmlText(HtmlTd[5], 1);
                    S3_1 = getHtmlText(HtmlTd[6], 1);
                    S3_2 = getHtmlText(HtmlTd[7], 1);
                    S4_0 = getHtmlText(HtmlTd[8], 1);
                    S4_1 = getHtmlText(HtmlTd[9], 1);
                    S4_2 = getHtmlText(HtmlTd[10], 1);
                    S5_0 = getHtmlText(HtmlTd[11], 1);
                    S5_1 = getHtmlText(HtmlTd[12], 1);
                    S5_2 = getHtmlText(HtmlTd[13], 1);
                    SOther = getHtmlText(HtmlTd[14], 1);

                    P0_0 = getHtmlText(HtmlTd[17], 1);
                    P1_1 = getHtmlText(HtmlTd[18], 1);
                    P2_2 = getHtmlText(HtmlTd[19], 1);
                    P3_3 = getHtmlText(HtmlTd[20], 1);
                    POther = getHtmlText(HtmlTd[21], 1);

                    F0_1 = getHtmlText(HtmlTd[23], 1);
                    F0_2 = getHtmlText(HtmlTd[24], 1);
                    F1_2 = getHtmlText(HtmlTd[25], 1);
                    F0_3 = getHtmlText(HtmlTd[26], 1);
                    F1_3 = getHtmlText(HtmlTd[27], 1);
                    F2_3 = getHtmlText(HtmlTd[28], 1);
                    F0_4 = getHtmlText(HtmlTd[29], 1);
                    F1_4 = getHtmlText(HtmlTd[30], 1);
                    F2_4 = getHtmlText(HtmlTd[31], 1);
                    F0_5 = getHtmlText(HtmlTd[32], 1);
                    F1_5 = getHtmlText(HtmlTd[33], 1);
                    F2_5 = getHtmlText(HtmlTd[34], 1);
                    FOther = getHtmlText(HtmlTd[35], 1);

                    #endregion 赋值

                    sb.Append(" exec pro_crs_list '");
                    sb.Append(MatchNumber + "'," + S1_0 + "," + S2_0 + "," + S2_1 + "," + S3_0 + "," + S3_1 + ",");
                    sb.Append(S3_2 + "," + S4_0 + "," + S4_1 + "," + S4_2 + "," + S5_0 + "," + S5_1 + ",");
                    sb.Append(S5_2 + "," + SOther + "," + P0_0 + "," + P1_1 + "," + P2_2 + "," + P3_3 + ",");
                    sb.Append(POther + "," + F0_1 + "," + F0_2 + "," + F1_2 + "," + F0_3 + "," + F1_3 + ",");
                    sb.Append(F2_3 + "," + F0_4 + "," + F1_4 + "," + F2_4 + "," + F0_5 + "," + F1_5 + "," + F2_5 + "," + FOther + "; ");

                    Count++;
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// 半场胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string GetSportteryhafu_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string SS = string.Empty;
            string SP = string.Empty;
            string SF = string.Empty;
            string PS = string.Empty;
            string PP = string.Empty;
            string PF = string.Empty;
            string FS = string.Empty;
            string FP = string.Empty;
            string FF = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_PassRate set IsHafu = 0 ; ");
            Count = 0;

            Other.GetStrScope(Html, "<TABLE CLASS=\"tabContent", "ID=\"match_list\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "</TR>");

            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 14)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    SS = getHtmlText(HtmlTd[4], 1);
                    SP = getHtmlText(HtmlTd[5], 1);
                    SF = getHtmlText(HtmlTd[6], 1);
                    PS = getHtmlText(HtmlTd[7], 1);
                    PP = getHtmlText(HtmlTd[8], 1);
                    PF = getHtmlText(HtmlTd[9], 1);
                    FS = getHtmlText(HtmlTd[10], 1);
                    FP = getHtmlText(HtmlTd[11], 1);
                    FF = getHtmlText(HtmlTd[12], 1);

                    sb.Append(" exec pro_hafu_list '" + MatchNumber + "'," + SS + "," + SP + "," + SF + ",");
                    sb.Append(PS + "," + PP + "," + PF + "," + FS + "," + FP + "," + FF + ";");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 总进球数
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string GetSportteryttg_listCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string in0 = string.Empty;
            string in1 = string.Empty;
            string in2 = string.Empty;
            string in3 = string.Empty;
            string in4 = string.Empty;
            string in5 = string.Empty;
            string in6 = string.Empty;
            string in7 = string.Empty;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_PassRate set IsTtg = 0 ; ");

            Other.GetStrScope(Html, "<TABLE CLASS=\"tabContent", "ID=\"match_list\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);
            HtmlTr = SplitString(Html, "</TR>");
            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 13)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    in0 = getHtmlText(HtmlTd[4], 1);
                    in1 = getHtmlText(HtmlTd[5], 1);
                    in2 = getHtmlText(HtmlTd[6], 1);
                    in3 = getHtmlText(HtmlTd[7], 1);
                    in4 = getHtmlText(HtmlTd[8], 1);
                    in5 = getHtmlText(HtmlTd[9], 1);
                    in6 = getHtmlText(HtmlTd[10], 1);
                    in7 = getHtmlText(HtmlTd[11], 1);

                    sb.Append(" exec pro_ttg_list '" + MatchNumber + "'," + in0 + "," + in1 + ",");
                    sb.Append(in2 + "," + in3 + "," + in4 + "," + in5 + "," + in6 + "," + in7 + ";");

                    Count++;
                }
            }

            return sb.ToString();
        }

        #endregion 过关奖金

        #region 单场奖金

        public void GetSingleRate()
        {
            switch (Source)
            {
                case "500wan":
                    Get500wanSingleRate();
                    break;

                case "Sporttery":
                    GetSportterySingleRate();
                    break;
            }
        }

        private void Get500wanSingleRate()
        {
            int Count = 0;
            string[,] ArryStrUrl = Get500wanSingleUrl();
            string Html = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetSingleRateHtml(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);

                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Get500wanhhad_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //比分
                        sb.Append(Get500wancrs_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //总进球数
                        sb.Append(Get500wanttg_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //半登场胜平负
                        sb.Append(Get500wanhafu_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            PrintThrendInfo("单场奖金抓取：完成单场奖金数据抓取.");
        }

        private void GetSportterySingleRate()
        {
            int Count = 0;
            string[,] ArryStrUrl = GetSingleUrl();
            string Html = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetSingleRateHtml(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);

                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Gethhad_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //比分
                        sb.Append(Getcrs_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //总进球数
                        sb.Append(Getttg_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //半登场胜平负
                        sb.Append(Gethafu_vpCmd(Html, out Count));
                        ExecSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            PrintThrendInfo("单场奖金抓取：完成单场奖金数据抓取.");
        }

        /// <summary>
        /// 得到单场奖金的URL
        /// </summary>
        /// <returns></returns>
        private string[,] GetSingleUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://info.sporttery.cn/football/hhad_vp.php";
            strUrl[0, 1] = "胜平负";
            strUrl[1, 0] = "http://info.sporttery.cn/football/crs_vp.php";
            strUrl[1, 1] = "比分";
            strUrl[2, 0] = "http://info.sporttery.cn/football/ttg_vp.php";
            strUrl[2, 1] = "总进球数";
            strUrl[3, 0] = "http://info.sporttery.cn/football/hafu_vp.php";
            strUrl[3, 1] = "半全场胜平负";

            return strUrl;
        }

        /// <summary>
        /// 得到URL和标题
        /// </summary>
        /// <returns></returns>
        private string[,] Get500wanSingleUrl()
        {
            string[,] strUrl = new string[4, 4];
            strUrl[0, 0] = "http://trade.500wan.com/jczq/index.php?playid=269&g=1";
            strUrl[0, 1] = "胜平负";
            strUrl[1, 0] = "http://trade.500wan.com/jczq/index.php?playid=271&g=1";
            strUrl[1, 1] = "比分";
            strUrl[2, 0] = "http://trade.500wan.com/jczq/index.php?playid=270&g=1";
            strUrl[2, 1] = "总进球数";
            strUrl[3, 0] = "http://trade.500wan.com/jczq/index.php?playid=272&g=1";
            strUrl[3, 1] = "半全场胜平负";

            return strUrl;
        }

        /// <summary>
        /// 单场过关奖金
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetSingleRateHtml(string strUrl, string title)
        {
            string Html = string.Empty;
            //PrintThrendInfo(title + "抓取：开始" + title + "数据抓取.");
            //PrintThrendInfo(title + "抓取：目标地址\"" + strUrl + "\"");
            Html = GetHtml(strUrl, false);          //得到html页面内容
            Html = HTMLEdit.OnlyBody(Html);
            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfo(title + "抓取：没有获取到Html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            //PrintThrendInfo(title + "抓取：读取页面\"" + strUrl + "\"成功.开始分析页面数据...");
            return Html.Replace("&amp;amp;nbsp;", "");
        }

        /// <summary>
        /// 得到胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private string Gethhad_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string id = string.Empty;
            string str_win = string.Empty;
            string str_flat = string.Empty;
            string str_lose = string.Empty;
            string str_date = string.Empty;
            string LoseBall = "0";
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string UpTime = string.Empty;
            string GameColor = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_singleRate set IsHhad = 0 ; ");

            try
            {
                Other.GetStrScope(Html, "ID=\"jumpTable\"", "ID=\"match_counts2\"", out IStart, out ILen);
            }
            catch
            {
                log.Write("足彩单场胜平负（ID=\"jumpTable\" ID=\"match_counts2\"）错误");

                return "";
            }

            if (ILen != 0)
            {
                if (Html.IndexOf("数据更新时间") != -1)
                {
                    UpTime = DateTime.Now.Year.ToString() + "-" + SplitString(Html, "数据更新时间：")[1].Split('<')[0].Trim();
                }
                else
                {
                    log.Write("足彩单场胜平负没有抓取到数据更新时间");
                }
            }
            else
            {
                return "";
            }

            Html = Html.Substring(IStart, ILen);
            HtmlTr = Html.Split(new string[] { "<TR CLASS=\"tr1\">", "<TR>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tr in HtmlTr)
            {
                HtmlTd = tr.Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                if (HtmlTd.Length == 8)
                {
                    LoseBall = "0";         //初始值

                    MatchNumber = HtmlTd[0].Split('>')[HtmlTd[0].Split('>').Length - 1];
                    Game = HtmlTd[1].Split('>')[HtmlTd[1].Split('>').Length - 2].Split('<')[0];
                    MainTeam = Regex.Replace(HtmlTd[2].Split('>')[2].Split('<')[0], @"\s|(\(.+\))", "");
                    GuestTeam = HtmlTd[2].Split('>')[4].Split('<')[0];

                    if (HtmlTd[2].IndexOf(")<STRONG>") != -1)
                    {
                        LoseBall = SplitString(HtmlTd[2], ")<STRONG>")[0].Split('(')[1].Replace("+", "");
                    }
                    str_date = HtmlTd[3].Split('>')[HtmlTd[3].Split('>').Length - 1];
                    str_win = HtmlTd[4].IndexOf("尚无投注") == -1 ? HtmlTd[4].Split('>')[1].Split('<')[0] : "0";
                    str_flat = HtmlTd[5].IndexOf("尚无投注") == -1 ? HtmlTd[5].Split('>')[1].Split('<')[0] : "0";
                    str_lose = HtmlTd[6].IndexOf("尚无投注") == -1 ? HtmlTd[6].Split('>')[1].Split('<')[0] : "0";
                    Count++;

                    sb.Append(" exec P_spfdg '" + MainTeam + "','" + GuestTeam + "','" + Game + "','" + MatchNumber + "'," + str_win + "," + str_flat + "," + str_lose + "," + LoseBall + "," + ",'" + str_date + "', 0, 0, 0, 0, ''; ");

                    sb.Append("update T_singleRate set GameColor = '" + GameColor + "' where MatchNumber= '" + MatchNumber + "' ; ");
                    sb.Append("update T_Match set GameColor = '" + GameColor + "', MainLoseBall = '" + LoseBall + "' where MainTeam = '" + MainTeam + "' and GuestTeam = '" + GuestTeam + "' and dateadd(d, 4, StopSellingTime) > GETDATE() and MatchNumber= '" + MatchNumber + "' ; ");
                }
            }

            sb.Append(" delete T_singleRate where StopSellTime < dateadd(day, -1,getdate()); ");

            return sb.ToString();
        }

        /// <summary>
        /// 得到比分
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Getcrs_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlDiv = null;
            string[] HtmlTd = null;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_singleRate set IsCrs = 0 ; ");

            #region 定义对应字段

            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string S1_0 = string.Empty;
            string S2_0 = string.Empty;
            string S2_1 = string.Empty;
            string S3_0 = string.Empty;
            string S3_1 = string.Empty;
            string S3_2 = string.Empty;
            string S4_0 = string.Empty;
            string S4_1 = string.Empty;
            string S4_2 = string.Empty;
            string S5_0 = string.Empty;
            string S5_1 = string.Empty;
            string S5_2 = string.Empty;
            string SOther = string.Empty;
            string P0_0 = string.Empty;
            string P1_1 = string.Empty;
            string P2_2 = string.Empty;
            string P3_3 = string.Empty;
            string POther = string.Empty;
            string F0_1 = string.Empty;
            string F0_2 = string.Empty;
            string F1_2 = string.Empty;
            string F0_3 = string.Empty;
            string F1_3 = string.Empty;
            string F2_3 = string.Empty;
            string F0_4 = string.Empty;
            string F1_4 = string.Empty;
            string F2_4 = string.Empty;
            string F0_5 = string.Empty;
            string F1_5 = string.Empty;
            string F2_5 = string.Empty;
            string FOther = string.Empty;

            #endregion 定义对应字段

            try
            {
                Other.GetStrScope(Html, "ID=\"jumpTable\"", "ID=\"match_counts2\"", out IStart, out ILen);
            }
            catch
            {
                log.Write("足彩单场比分（ID=\"jumpTable\"ID=\"match_counts2\"）错误");

                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlDiv = Html.Split(new string[] { "</DIV>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string td in HtmlDiv)
            {
                HtmlTd = SplitString(td, "</TD>");

                if (HtmlTd.Length == 35)
                {
                    try
                    {
                        MatchNumber = getHtmlText(HtmlTd[0], 15);
                        Game = getHtmlText(HtmlTd[0], 11);
                        MainTeam = getHtmlText(HtmlTd[0], 8).Replace("VS", "V").Split('V')[0].Trim();
                        GuestTeam = getHtmlText(HtmlTd[0], 8).Replace("VS", "V").Split('V')[1].Trim();

                        Stopselltime = "20" + getHtmlText(HtmlTd[0], 5).Split('：')[1].Trim() + ":00";
                    }
                    catch { }

                    #region 赋值

                    S1_0 = Getcrs_vpRate(HtmlTd[1]);
                    S2_0 = Getcrs_vpRate(HtmlTd[2]);
                    S2_1 = Getcrs_vpRate(HtmlTd[3]);
                    S3_0 = Getcrs_vpRate(HtmlTd[4]);
                    S3_1 = Getcrs_vpRate(HtmlTd[5]);
                    S3_2 = Getcrs_vpRate(HtmlTd[6]);
                    S4_0 = Getcrs_vpRate(HtmlTd[7]);
                    S4_1 = Getcrs_vpRate(HtmlTd[8]);
                    S4_2 = Getcrs_vpRate(HtmlTd[9]);
                    S5_0 = Getcrs_vpRate(HtmlTd[10]);
                    S5_1 = Getcrs_vpRate(HtmlTd[11]);
                    S5_2 = Getcrs_vpRate(HtmlTd[12]);
                    SOther = Getcrs_vpRate(HtmlTd[13]);

                    P0_0 = Getcrs_vpRate(HtmlTd[15]);
                    P1_1 = Getcrs_vpRate(HtmlTd[16]);
                    P2_2 = Getcrs_vpRate(HtmlTd[17]);
                    P3_3 = Getcrs_vpRate(HtmlTd[18]);
                    POther = Getcrs_vpRate(HtmlTd[19]);

                    F0_1 = Getcrs_vpRate(HtmlTd[21]);
                    F0_2 = Getcrs_vpRate(HtmlTd[22]);
                    F1_2 = Getcrs_vpRate(HtmlTd[23]);
                    F0_3 = Getcrs_vpRate(HtmlTd[24]);
                    F1_3 = Getcrs_vpRate(HtmlTd[25]);
                    F2_3 = Getcrs_vpRate(HtmlTd[26]);
                    F0_4 = Getcrs_vpRate(HtmlTd[27]);
                    F1_4 = Getcrs_vpRate(HtmlTd[28]);
                    F2_4 = Getcrs_vpRate(HtmlTd[29]);
                    F0_5 = Getcrs_vpRate(HtmlTd[30]);
                    F1_5 = Getcrs_vpRate(HtmlTd[31]);
                    F2_5 = Getcrs_vpRate(HtmlTd[32]);
                    FOther = Getcrs_vpRate(HtmlTd[33]);

                    if (string.IsNullOrEmpty(S1_0))
                    {
                        S1_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_0))
                    {
                        S2_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_1))
                    {
                        S2_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_0))
                    {
                        S3_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_1))
                    {
                        S3_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_2))
                    {
                        S3_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_0))
                    {
                        S4_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_1))
                    {
                        S4_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_2))
                    {
                        S4_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_0))
                    {
                        S5_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_1))
                    {
                        S5_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_2))
                    {
                        S5_2 = "0";
                    }

                    if (string.IsNullOrEmpty(SOther))
                    {
                        SOther = "0";
                    }

                    if (string.IsNullOrEmpty(P0_0))
                    {
                        P0_0 = "0";
                    }

                    if (string.IsNullOrEmpty(P1_1))
                    {
                        P1_1 = "0";
                    }

                    if (string.IsNullOrEmpty(P2_2))
                    {
                        P2_2 = "0";
                    }

                    if (string.IsNullOrEmpty(P3_3))
                    {
                        P3_3 = "0";
                    }

                    if (string.IsNullOrEmpty(POther))
                    {
                        POther = "0";
                    }

                    if (string.IsNullOrEmpty(F0_1))
                    {
                        F0_1 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_2))
                    {
                        F0_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_2))
                    {
                        F1_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_3))
                    {
                        F0_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_3))
                    {
                        F1_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_3))
                    {
                        F2_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_4))
                    {
                        F0_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_4))
                    {
                        F1_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_4))
                    {
                        F2_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_5))
                    {
                        F0_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_5))
                    {
                        F1_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_5))
                    {
                        F2_5 = "0";
                    }

                    if (string.IsNullOrEmpty(FOther))
                    {
                        FOther = "0";
                    }

                    #endregion 赋值

                    sb.Append(" exec P_Bfdg '");
                    sb.Append(MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "'," + S1_0 + "," + S2_0 + "," + S2_1 + "," + S3_0 + "," + S3_1 + ",");
                    sb.Append(S3_2 + "," + S4_0 + "," + S4_1 + "," + S4_2 + "," + S5_0 + "," + S5_1 + ",");
                    sb.Append(S5_2 + "," + SOther + "," + P0_0 + "," + P1_1 + "," + P2_2 + "," + P3_3 + ",");
                    sb.Append(POther + "," + F0_1 + "," + F0_2 + "," + F1_2 + "," + F0_3 + "," + F1_3 + ",");
                    sb.Append(F2_3 + "," + F0_4 + "," + F1_4 + "," + F2_4 + "," + F0_5 + "," + F1_5 + "," + F2_5 + "," + FOther + ", 0, ''; ");

                    Count++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 单场总进球数SQL命令
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Getttg_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string in0 = string.Empty;
            string in1 = string.Empty;
            string in2 = string.Empty;
            string in3 = string.Empty;
            string in4 = string.Empty;
            string in5 = string.Empty;
            string in6 = string.Empty;
            string in7 = string.Empty;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_singleRate set IsTtg = 0 ; ");

            try
            {
                Other.GetStrScope(Html, "ID=\"jumpTable\"", "ID=\"match_counts2\"", out IStart, out ILen);
            }
            catch
            {
                log.Write("足彩单场进球（ID=\"jumpTable\"ID=\"match_counts2\"）错误");

                return "";
            }

            Html = Html.Substring(IStart, ILen);
            HtmlTr = Html.Split(new string[] { "<TR CLASS=\"tr1\">", "<TR>" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = HtmlTr[i].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                if (HtmlTd.Length == 13)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[2], 3);
                    GuestTeam = getHtmlText(HtmlTd[2], 6);
                    Stopselltime = "20" + getHtmlText(HtmlTd[3], 1) + ":00";

                    in0 = GettvpRate(HtmlTd[4]);
                    in1 = GettvpRate(HtmlTd[5]);
                    in2 = GettvpRate(HtmlTd[6]);
                    in3 = GettvpRate(HtmlTd[7]);
                    in4 = GettvpRate(HtmlTd[8]);
                    in5 = GettvpRate(HtmlTd[9]);
                    in6 = GettvpRate(HtmlTd[10]);
                    in7 = GettvpRate(HtmlTd[11]);

                    sb.Append(" exec P_Jqsdg '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "','" + DateTime.Now.ToString() + "'," + in0 + "," + in1 + ",");
                    sb.Append(in2 + "," + in3 + "," + in4 + "," + in5 + "," + in6 + "," + in7 + ", 0, '';");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 半场胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Gethafu_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string Stopselltime = string.Empty;
            string SS = string.Empty;
            string SP = string.Empty;
            string SF = string.Empty;
            string PS = string.Empty;
            string PP = string.Empty;
            string PF = string.Empty;
            string FS = string.Empty;
            string FP = string.Empty;
            string FF = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_singleRate set IsHafu = 0 ; ");
            Count = 0;

            try
            {
                Other.GetStrScope(Html, "jumpTable", "ID=\"match_counts2\"", out IStart, out ILen);
            }
            catch
            {
                log.Write("足彩单场半全场胜平负（jumpTable ID=\"match_counts2\"）错误");

                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = Html.Split(new string[] { "<TR CLASS=\"tr1\">", "<TR>" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 14)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[2], 3);
                    GuestTeam = getHtmlText(HtmlTd[2], 6);
                    Stopselltime = "20" + getHtmlText(HtmlTd[3], 1) + ":00";

                    SS = GettvpRate(HtmlTd[4]);
                    SP = GettvpRate(HtmlTd[5]);
                    SF = GettvpRate(HtmlTd[6]);
                    PS = GettvpRate(HtmlTd[7]);
                    PP = GettvpRate(HtmlTd[8]);
                    PF = GettvpRate(HtmlTd[9]);
                    FS = GettvpRate(HtmlTd[10]);
                    FP = GettvpRate(HtmlTd[11]);
                    FF = GettvpRate(HtmlTd[12]);

                    sb.Append(" exec P_Bqcdg  '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + Stopselltime + "'," + SS + "," + SP + "," + SF + ",");
                    sb.Append(PS + "," + PP + "," + PF + "," + FS + "," + FP + "," + FF + ", 0, '';");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 得到胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private string Get500wanhhad_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            Count = 0;

            string str_win = string.Empty;
            string str_flat = string.Empty;
            string str_lose = string.Empty;
            string str_date = string.Empty;
            string MatchDate = string.Empty;
            string LoseBall = "0";
            string MatchNumber = string.Empty;
            string TempMatchNumber = "";
            string Game = string.Empty;
            string GameColor = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_singleRate set IsHhad = 0 ; ");
            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);
            Html = Html.Substring(IStart, ILen);

            if (ILen == 0)
            {
                log.Write("胜平负 Get500wanhhad_listCmd 方法 Other.GetStrScope(Html, \"<div class=\"dc_hs\" <div class=\"dc_r\"\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            string[] days = Html.Split(new string[] { "<DIV CLASS=\"dc_hs\"" }, StringSplitOptions.RemoveEmptyEntries);
            string table = string.Empty;
            string[] rows = null;
            string[] tds = null;
            string LastDate = string.Empty;
            string EuropeSSP = "";
            string EuropePSP = "";
            string EuropeFSP = "";

            for (int i = 0; i < days.Length; i++)
            {
                rows = days[i].Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < rows.Length; j++)
                {
                    tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tds.Length == 14)
                    {
                        MatchNumber = getHtmlText(tds[0], 2);
                        Game = getHtmlText(tds[1], 2);
                        GameColor = RegexString(tds[1], "background[^>]*>")[0];
                        GameColor = GameColor.Substring(GameColor.IndexOf("#"), GameColor.IndexOf(";") - GameColor.IndexOf("#"));
                        MainTeam = getHtmlText(tds[3], 2);
                        GuestTeam = getHtmlText(tds[5], 4);
                        str_date = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                        MatchDate = GetDateTime(tds[2]) + ":00";

                        TempMatchNumber = MatchNumber;

                        str_win = getHtmlText(tds[8], 2);
                        str_flat = getHtmlText(tds[9], 2);
                        str_lose = getHtmlText(tds[10], 2);
                        EuropeSSP = getHtmlText(tds[6], 15);
                        EuropePSP = getHtmlText(tds[6], 13);
                        EuropeFSP = getHtmlText(tds[6], 11);
                        LoseBall = getHtmlText(tds[4], 2).Replace("+", "");
                        LoseBall = LoseBall == "" ? "0" : LoseBall;

                        if (string.IsNullOrEmpty(str_win))
                        {
                            str_win = "0";
                        }

                        if (string.IsNullOrEmpty(str_flat))
                        {
                            str_flat = "0";
                        }

                        if (string.IsNullOrEmpty(str_lose))
                        {
                            str_lose = "0";
                        }

                        sb.Append(" exec P_spfdg '" + MainTeam + "','" + GuestTeam + "','" + Game + "','" + MatchNumber + "'," + str_win + "," + str_flat + "," + str_lose + "," + LoseBall + ",'" + MatchDate + "','" + str_date + "', '" + EuropeSSP + "','" + EuropePSP + "','" + EuropeFSP + "', 0, ''; ");

                        sb.Append("update T_singleRate set GameColor = '" + GameColor + "', MainLoseBall = '" + LoseBall + "' where MatchNumber= '" + MatchNumber + "' ; ");

                        Count++;
                    }
                }

            }

            sb.Append(" delete T_singleRate where StopSellTime < dateadd(day, -1,getdate()); ");
            return sb.ToString();
        }

        /// <summary>
        /// 得到比分
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wancrs_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string table = string.Empty;
            string[] rows = null;
            string date = string.Empty;
            string[] tds = null;
            string[] trs = null;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_singleRate set IsCrs = 0 ; ");

            #region 定义对应字段

            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string S1_0 = string.Empty;
            string S2_0 = string.Empty;
            string S2_1 = string.Empty;
            string S3_0 = string.Empty;
            string S3_1 = string.Empty;
            string S3_2 = string.Empty;
            string S4_0 = string.Empty;
            string S4_1 = string.Empty;
            string S4_2 = string.Empty;
            string S5_0 = string.Empty;
            string S5_1 = string.Empty;
            string S5_2 = string.Empty;
            string SOther = string.Empty;
            string P0_0 = string.Empty;
            string P1_1 = string.Empty;
            string P2_2 = string.Empty;
            string P3_3 = string.Empty;
            string POther = string.Empty;
            string F0_1 = string.Empty;
            string F0_2 = string.Empty;
            string F1_2 = string.Empty;
            string F0_3 = string.Empty;
            string F1_3 = string.Empty;
            string F2_3 = string.Empty;
            string F0_4 = string.Empty;
            string F1_4 = string.Empty;
            string F2_4 = string.Empty;
            string F0_5 = string.Empty;
            string F1_5 = string.Empty;
            string F2_5 = string.Empty;
            string FOther = string.Empty;

            #endregion 定义对应字段

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            Html = Html.Substring(IStart, ILen);

            rows = Html.Split(new string[] { "ZID" }, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 1; j < rows.Length; j++)
            {
                tds = rows[j].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    MatchNumber = getHtmlText(tds[0], 2);
                    Game = getHtmlText(tds[1], 2);
                    MainTeam = getHtmlText(tds[3], 2);
                    GuestTeam = getHtmlText(tds[4], 4);
                    Stopselltime = GetDateTime(tds[2].Substring(tds[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(tds[2]) + ":00";
                }
                catch { }

                table = rows[j];

                Other.GetStrScope(table, "<TBODY>", "</TBODY>", out IStart, out ILen);

                if (ILen != 0)
                {
                    table = table.Substring(IStart, ILen);
                    trs = table.Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                    #region 赋值

                    //主胜
                    tds = trs[0].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    S1_0 = getHtmlText(tds[1], 3);
                    S2_0 = getHtmlText(tds[2], 3);
                    S2_1 = getHtmlText(tds[3], 3);
                    S3_0 = getHtmlText(tds[4], 3);
                    S3_1 = getHtmlText(tds[5], 3);
                    S3_2 = getHtmlText(tds[6], 3);
                    S4_0 = getHtmlText(tds[7], 3);
                    S4_1 = getHtmlText(tds[8], 3);
                    S4_2 = getHtmlText(tds[9], 3);
                    S5_0 = getHtmlText(tds[10], 3);
                    S5_1 = getHtmlText(tds[11], 3);
                    S5_2 = getHtmlText(tds[12], 3);
                    SOther = getHtmlText(tds[0], 3);

                    //主平
                    tds = trs[1].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    P0_0 = getHtmlText(tds[1], 3);
                    P1_1 = getHtmlText(tds[2], 3);
                    P2_2 = getHtmlText(tds[3], 3);
                    P3_3 = getHtmlText(tds[4], 3);
                    POther = getHtmlText(tds[0], 3);

                    //主负
                    tds = trs[2].Split(new string[] { "</TD>" }, StringSplitOptions.RemoveEmptyEntries);
                    F0_1 = getHtmlText(tds[1], 3);
                    F0_2 = getHtmlText(tds[2], 3);
                    F1_2 = getHtmlText(tds[3], 3);
                    F0_3 = getHtmlText(tds[4], 3);
                    F1_3 = getHtmlText(tds[5], 3);
                    F2_3 = getHtmlText(tds[6], 3);
                    F0_4 = getHtmlText(tds[7], 3);
                    F1_4 = getHtmlText(tds[8], 3);
                    F2_4 = getHtmlText(tds[9], 3);
                    F0_5 = getHtmlText(tds[10], 3);
                    F1_5 = getHtmlText(tds[11], 3);
                    F2_5 = getHtmlText(tds[12], 3);
                    FOther = getHtmlText(tds[0], 3);

                    #endregion 赋值

                    if (string.IsNullOrEmpty(S1_0))
                    {
                        S1_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_0))
                    {
                        S2_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S2_1))
                    {
                        S2_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_0))
                    {
                        S3_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_1))
                    {
                        S3_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S3_2))
                    {
                        S3_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_0))
                    {
                        S4_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_1))
                    {
                        S4_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S4_2))
                    {
                        S4_2 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_0))
                    {
                        S5_0 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_1))
                    {
                        S5_1 = "0";
                    }

                    if (string.IsNullOrEmpty(S5_2))
                    {
                        S5_2 = "0";
                    }

                    if (string.IsNullOrEmpty(SOther))
                    {
                        SOther = "0";
                    }

                    if (string.IsNullOrEmpty(P0_0))
                    {
                        P0_0 = "0";
                    }

                    if (string.IsNullOrEmpty(P1_1))
                    {
                        P1_1 = "0";
                    }

                    if (string.IsNullOrEmpty(P2_2))
                    {
                        P2_2 = "0";
                    }

                    if (string.IsNullOrEmpty(P3_3))
                    {
                        P3_3 = "0";
                    }

                    if (string.IsNullOrEmpty(POther))
                    {
                        POther = "0";
                    }

                    if (string.IsNullOrEmpty(F0_1))
                    {
                        F0_1 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_2))
                    {
                        F0_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_2))
                    {
                        F1_2 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_3))
                    {
                        F0_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_3))
                    {
                        F1_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_3))
                    {
                        F2_3 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_4))
                    {
                        F0_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_4))
                    {
                        F1_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_4))
                    {
                        F2_4 = "0";
                    }

                    if (string.IsNullOrEmpty(F0_5))
                    {
                        F0_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F1_5))
                    {
                        F1_5 = "0";
                    }

                    if (string.IsNullOrEmpty(F2_5))
                    {
                        F2_5 = "0";
                    }

                    if (string.IsNullOrEmpty(FOther))
                    {
                        FOther = "0";
                    }

                    sb.Append(" exec P_Bfdg '");
                    sb.Append(MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + S1_0 + "," + S2_0 + "," + S2_1 + "," + S3_0 + "," + S3_1 + ",");
                    sb.Append(S3_2 + "," + S4_0 + "," + S4_1 + "," + S4_2 + "," + S5_0 + "," + S5_1 + ",");
                    sb.Append(S5_2 + "," + SOther + "," + P0_0 + "," + P1_1 + "," + P2_2 + "," + P3_3 + ",");
                    sb.Append(POther + "," + F0_1 + "," + F0_2 + "," + F1_2 + "," + F0_3 + "," + F1_3 + ",");
                    sb.Append(F2_3 + "," + F0_4 + "," + F1_4 + "," + F2_4 + "," + F0_5 + "," + F1_5 + "," + F2_5 + "," + FOther + ", 0, ''; ");

                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 总进球数SQL命令
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wanttg_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string in0 = string.Empty;
            string in1 = string.Empty;
            string in2 = string.Empty;
            string in3 = string.Empty;
            string in4 = string.Empty;
            string in5 = string.Empty;
            string in6 = string.Empty;
            string in7 = string.Empty;
            Count = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append(" update T_singleRate set IsTtg = 0 ; ");

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩总进球数 Getttg_listCmd 方法 Other.GetStrScope(Html, \"<DIV CLASS=\"dc_hs\" <DIV CLASS=\"dc_r\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);
            HtmlTr = SplitString(Html, "</TR>");
            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 15)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTd[4], 4);
                    Stopselltime = GetDateTime(HtmlTd[2].Substring(HtmlTd[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(HtmlTd[2]) + ":00";

                    in0 = getHtmlText(HtmlTd[6], 3);
                    in1 = getHtmlText(HtmlTd[7], 3);
                    in2 = getHtmlText(HtmlTd[8], 3);
                    in3 = getHtmlText(HtmlTd[9], 3);
                    in4 = getHtmlText(HtmlTd[10], 3);
                    in5 = getHtmlText(HtmlTd[11], 3);
                    in6 = getHtmlText(HtmlTd[12], 3);
                    in7 = getHtmlText(HtmlTd[13], 3);

                    if (string.IsNullOrEmpty(in0))
                    {
                        in0 = "0";
                    }

                    if (string.IsNullOrEmpty(in1))
                    {
                        in1 = "0";
                    }

                    if (string.IsNullOrEmpty(in2))
                    {
                        in2 = "0";
                    }

                    if (string.IsNullOrEmpty(in3))
                    {
                        in3 = "0";
                    }

                    if (string.IsNullOrEmpty(in4))
                    {
                        in4 = "0";
                    }

                    if (string.IsNullOrEmpty(in5))
                    {
                        in5 = "0";
                    }

                    if (string.IsNullOrEmpty(in6))
                    {
                        in6 = "0";
                    }

                    if (string.IsNullOrEmpty(in7))
                    {
                        in7 = "0";
                    }

                    sb.Append(" exec P_Jqsdg '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + in0 + "," + in1 + ",");
                    sb.Append(in2 + "," + in3 + "," + in4 + "," + in5 + "," + in6 + "," + in7 + ", 0, '';");
                    Count++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 半全场胜平负
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        private string Get500wanhafu_vpCmd(string Html, out int Count)
        {
            int IStart = 0;
            int ILen = 0;
            string[] HtmlTr = null;
            string[] HtmlTd = null;
            string MatchNumber = string.Empty;
            string Game = string.Empty;
            string MainTeam = string.Empty;
            string GuestTeam = string.Empty;
            string MatchDate = string.Empty;
            string Stopselltime = string.Empty;
            string SS = string.Empty;
            string SP = string.Empty;
            string SF = string.Empty;
            string PS = string.Empty;
            string PP = string.Empty;
            string PF = string.Empty;
            string FS = string.Empty;
            string FP = string.Empty;
            string FF = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(" update T_singleRate set IsHafu = 0 ; ");
            Count = 0;

            Other.GetStrScope(Html, "<DIV CLASS=\"dc_hs\"", "<DIV CLASS=\"dc_r\"", out IStart, out ILen);

            if (ILen == 0)
            {
                log.Write("足彩总进球数 Getttg_listCmd 方法 Other.GetStrScope(Html, \"<DIV CLASS=\"dc_hs\" <DIV CLASS=\"dc_r\", out IStart, out ILen); 返回ILen为：0");
                return "";
            }

            Html = Html.Substring(IStart, ILen);

            HtmlTr = SplitString(Html, "</TR>");

            for (int i = 0; i < HtmlTr.Length; i++)
            {
                HtmlTd = SplitString(HtmlTr[i], "</TD>");
                if (HtmlTd.Length == 16)
                {
                    MatchNumber = getHtmlText(HtmlTd[0], 2);
                    Game = getHtmlText(HtmlTd[1], 2);
                    MainTeam = getHtmlText(HtmlTd[3], 2);
                    GuestTeam = getHtmlText(HtmlTd[4], 4);
                    Stopselltime = GetDateTime(HtmlTd[2].Substring(HtmlTd[2].IndexOf("match_time"))) + ":00";
                    MatchDate = GetDateTime(HtmlTd[2]) + ":00";

                    SS = getHtmlText(HtmlTd[6], 3);
                    SP = getHtmlText(HtmlTd[7], 3);
                    SF = getHtmlText(HtmlTd[8], 3);
                    PS = getHtmlText(HtmlTd[9], 3);
                    PP = getHtmlText(HtmlTd[10], 3);
                    PF = getHtmlText(HtmlTd[11], 3);
                    FS = getHtmlText(HtmlTd[12], 3);
                    FP = getHtmlText(HtmlTd[13], 3);
                    FF = getHtmlText(HtmlTd[14], 3);

                    if (string.IsNullOrEmpty(SS))
                    {
                        SS = "0";
                    }

                    if (string.IsNullOrEmpty(SP))
                    {
                        SP = "0";
                    }

                    if (string.IsNullOrEmpty(SF))
                    {
                        SF = "0";
                    }

                    if (string.IsNullOrEmpty(PS))
                    {
                        PS = "0";
                    }

                    if (string.IsNullOrEmpty(PP))
                    {
                        PP = "0";
                    }

                    if (string.IsNullOrEmpty(PF))
                    {
                        PF = "0";
                    }

                    if (string.IsNullOrEmpty(FS))
                    {
                        FS = "0";
                    }

                    if (string.IsNullOrEmpty(FP))
                    {
                        FP = "0";
                    }

                    if (string.IsNullOrEmpty(FF))
                    {
                        FF = "0";
                    }

                    sb.Append(" exec P_Bqcdg  '" + MatchNumber + "','" + Game + "','" + MainTeam + "','" + GuestTeam + "','" + MatchDate + "','" + Stopselltime + "'," + SS + "," + SP + "," + SF + ",");
                    sb.Append(PS + "," + PP + "," + PF + "," + FS + "," + FP + "," + FF + ", 0, '';");

                    Count++;
                }
            }

            return sb.ToString();
        }

        private string Getcrs_vpRate(string Td)
        {
            return Td.IndexOf("尚无投注") == -1 ? SplitString(SplitString(Td, "</LABEL>")[1], "<IMG")[0] : "0";
        }

        /// <summary>
        /// 返回rate
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string GettvpRate(string td)
        {
            return td.IndexOf("尚无投注") == -1 ? SplitString(td, ">")[1].Split('<')[0] : "0";
        }

        #endregion 单场奖金

        #region 篮球

        public void GetPassRateBasket()
        {
            switch (Source)
            {
                case "sporttery":
                    GetsportteryPassRateBasket();
                    break;

                case "500wan":
                    Get500wanPassRateBasket();
                    break;
            }
        }

        /// <summary>
        /// 篮球过关奖金
        /// </summary>
        public void GetsportteryPassRateBasket()
        {
            String Html = string.Empty;
            string strUrl = string.Empty;
            StringBuilder sb = new StringBuilder();
            int Count = 0;

            string[,] ArryStrUrl = Basket.GetPassRateStrUrl();

            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetAllHtmlBasket(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Basket.Getmnl_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //让分胜负
                        sb.Append(Basket.Gethdc_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //胜负分差
                        sb.Append(Basket.Getwnm_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //大小分
                        sb.Append(Basket.Gethilo_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            ExecEndBasket("过关奖金");
        }

        /// <summary>
        /// 篮球过关奖金
        /// </summary>
        public void Get500wanPassRateBasket()
        {
            String Html = string.Empty;
            string strUrl = string.Empty;
            StringBuilder sb = new StringBuilder();
            int Count = 0;

            string[,] ArryStrUrl = Basket.Get500WanPassRateStrUrl();

            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetAllHtmlBasket(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Basket.Get500Wanmnl_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //让分胜负
                        sb.Append(Basket.Get500Wanhdc_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //胜负分差
                        sb.Append(Basket.Get500Wanwnm_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //大小分
                        sb.Append(Basket.Get500Wanhilo_list(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            ExecEndBasket("过关奖金");
        }

        public void GetSingleRateBasket()
        {
            switch (Source)
            {
                case "sporttery":
                    GetsportterySingleRateBasket();
                    break;

                case "500wan":
                    Get500wanSingleRateBasket();
                    break;
            }
        }

        /// <summary>
        /// 单场奖金
        /// </summary>
        public void GetsportterySingleRateBasket()
        {
            String Html = string.Empty;
            string strUrl = string.Empty;
            StringBuilder sb = new StringBuilder();
            int Count = 0;

            string[,] ArryStrUrl = Basket.GetSingleRateStrUrl();

            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetAllHtmlBasket(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜平负
                        sb.Append(Basket.Getmnl_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //让分胜负
                        sb.Append(Basket.Gethdc_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //胜负分差
                        sb.Append(Basket.Getwnm_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //大小分
                        sb.Append(Basket.Gethilo_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            ExecEndBasket("单场奖金");
        }

        /// <summary>
        /// 篮球过关奖金
        /// </summary>
        public void Get500wanSingleRateBasket()
        {
            String Html = string.Empty;
            string strUrl = string.Empty;
            StringBuilder sb = new StringBuilder();
            int Count = 0;

            string[,] ArryStrUrl = Basket.Get500WanSingleRateStrUrl();

            for (int i = 0; i < ArryStrUrl.GetLength(0); i++)
            {
                Html = GetAllHtmlBasket(ArryStrUrl[i, 0], ArryStrUrl[i, 1]);
                switch (i)
                {
                    case 0:                                            //胜负
                        sb.Append(Basket.Get500Wanmnl_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[0, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 1:                                             //让分胜负
                        sb.Append(Basket.Get500Wanhdc_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[1, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 2:                                             //胜负分差
                        sb.Append(Basket.Get500Wanwnm_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[2, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                    case 3:                                             //大小分
                        sb.Append(Basket.Get500Wanhilo_vp(Html, out Count));
                        ExecBasketRateSql(sb, Count, ArryStrUrl[3, 1]);
                        sb.Remove(0, sb.Length);
                        break;
                }
            }

            ExecEndBasket("过关奖金");
        }

        #endregion 篮球

        #region 得到赛果开奖

        public void GetMatchResult()
        {
            int IPage = 1;

            while (IPage < 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    string Html = GetMatchResultHtml(IPage.ToString(), DateTime.Now.AddDays(i * -1).ToString("yyyy-MM-dd"));

                    if (string.IsNullOrEmpty(Html))
                    {
                        break;
                    }

                    string MatchNumber = "";

                    string Result = "";
                    string SPFResult = "";
                    string SPFBonus = "";
                    string BQCResult = "";
                    string BQCBonus = "";
                    string ZJQSResult = "";
                    string ZJQSBonus = "";
                    string ZQBFResult = "";
                    string ZQBFBonus = "";

                    int IStart = 0;
                    int ILen = 0;

                    string[] rows = null;
                    string[] tds = null;

                    StringBuilder sb = new StringBuilder();

                    Other.GetStrScope(Html, "<DIV CLASS=\"lea_list\">", "<DIV CLASS=\"ld_bottom have_b\">", out IStart, out ILen);
                    Html = Html.Substring(IStart, ILen);

                    rows = Html.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);

                    if (rows.Length < 10)
                    {
                        break;
                    }

                    for (int j = 0; j < rows.Length; j++)
                    {
                        tds = rows[j].Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                        if (tds.Length != 20)
                        {
                            continue;
                        }

                        MatchNumber = getHtmlText(tds[0], 1);
                        Result = getHtmlText(tds[6], 1).Substring(getHtmlText(tds[6], 1).IndexOf(')') + 1);
                        SPFResult = getHtmlText(tds[8], 1);
                        SPFBonus = getHtmlText(tds[9], 2);
                        ZJQSResult = getHtmlText(tds[11], 1);
                        ZJQSBonus = getHtmlText(tds[12], 2);
                        ZQBFResult = getHtmlText(tds[14], 1);
                        ZQBFBonus = getHtmlText(tds[15], 2);
                        BQCResult = getHtmlText(tds[17], 1);
                        BQCBonus = getHtmlText(tds[18], 2);

                        sb.Append("update T_Match set Result = '" + Result + "', SPFResult = '" + SPFResult + "', SPFBonus = '" + SPFBonus + "', BQCResult = '" + BQCResult + "', BQCBonus = '" + BQCBonus + "', ZJQSResult = '" + ZJQSResult + "', ZJQSBonus = '" + ZJQSBonus + "', ZQBFResult ='" + ZQBFResult + "', ZQBFBonus = '" + ZQBFBonus + "', IsOpened = 1 where MatchNumber= '" + MatchNumber + "' and StopSellingTime between dateadd(d, -4, getdate()) and getdate() ; ");
                    }

                    int Row = 0;

                    try
                    {
                        Row = MsSql.ExecuteNonQuery(sb.ToString(), this.GetType().Name);
                    }
                    catch (Exception e)
                    {
                        log.Write(e.Message + " SQL语句：" + sb.ToString());

                        return;
                    }
                }

                IPage++;
            }
        }

        public void GetMatchBasketResult()
        {
            int IPage = 1;

            while (IPage < 4)
            {
                for (int i = 0; i < 3; i++)
                {
                    string Html = GetMatchBasketResultHtml(IPage.ToString(), DateTime.Now.AddDays(i * -1).ToString("yyyy-MM-dd"));

                    if (string.IsNullOrEmpty(Html))
                    {
                        break;
                    }

                    string MatchNumber = "";

                    string SFResult = "";
                    string SFBonus = "";
                    string RFSFResult = "";
                    string RFSFBonus = "";
                    string SFCResult = "";
                    string SFCBonus = "";
                    string DXResult = "";
                    string DXBonus = "";
                    string Result = "";

                    int IStart = 0;
                    int ILen = 0;

                    string[] rows = null;
                    string[] tds = null;

                    StringBuilder sb = new StringBuilder();

                    Other.GetStrScope(Html, "<DIV CLASS=\"lea_list\">", "<DIV CLASS=\"ld_bottom have_b\">", out IStart, out ILen);
                    Html = Html.Substring(IStart, ILen);

                    rows = Html.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);

                    if (rows.Length < 10)
                    {
                        break;
                    }

                    for (int j = 0; j < rows.Length; j++)
                    {
                        tds = rows[j].Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);

                        if (tds.Length != 21)
                        {
                            continue;
                        }

                        MatchNumber = getHtmlText(tds[0], 1);

                        try
                        {
                            SFResult = getHtmlText(tds[7], 1);
                            SFBonus = getHtmlText(tds[8], 2);
                            RFSFResult = getHtmlText(tds[11], 1);
                            RFSFBonus = getHtmlText(tds[12], 2);
                            SFCResult = getHtmlText(tds[14], 1);
                            SFCBonus = getHtmlText(tds[15], 2);
                            DXResult = getHtmlText(tds[18], 1);
                            DXBonus = getHtmlText(tds[19], 2);

                            Result = getHtmlText(tds[6], 1);

                            if (string.IsNullOrEmpty(SFResult) && !string.IsNullOrEmpty(Result))
                            {
                                if (Shove._Convert.StrToInt(Result.Split(':')[0], 0) > Shove._Convert.StrToInt(Result.Split(':')[1], 0))
                                {
                                    SFResult = "主负";
                                }
                                else
                                {
                                    SFResult = "主胜";
                                }
                            }

                            if (string.IsNullOrEmpty(SFCResult) && !string.IsNullOrEmpty(SFCBonus))
                            {
                                SFCResult = GetRFSFResult(Result);
                            }

                            if (string.IsNullOrEmpty(SFResult) && !string.IsNullOrEmpty(SFBonus))
                            {
                                SFResult = GetSFResult(Result);
                            }
                        }
                        catch { }

                        sb.Append("update T_MatchBasket set SFResult = '" + SFResult + "', SFBonus = '" + SFBonus + "', RFSFResult = '" + RFSFResult + "', RFSFBonus = '" + RFSFBonus + "', SFCResult = '" + SFCResult + "', SFCBonus ='" + SFCBonus + "', DXResult = '" + DXResult + "', DXBonus = '" + DXBonus + "', Result = '" + Result + "', IsOpened = 1 where MatchNumber= '" + MatchNumber + "' and StopSellingTime between dateadd(d, -4, getdate()) and getdate() ; ");
                    }

                    int Row = 0;

                    try
                    {
                        Row = MsSql.ExecuteNonQuery(sb.ToString(), this.GetType().Name);
                    }
                    catch (Exception e)
                    {
                        log.Write(e.Message + " SQL语句：" + sb.ToString());

                        return;
                    }
                }

                IPage++;
            }
        }

        /// <summary>
        /// 得到赛果及开奖Html
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        private string GetMatchResultHtml(string Page, string Day)
        {
            string strUrl = "";
            string Html = "";

            strUrl = "http://zx.500wan.com/jczq/kaijiang.php?all=0&page=" + Page + "&d=" + Day;
            Html = GetHtml(strUrl, false);          //得到html页面内容

            //Html = HTMLEdit.OnlyBody(Html);

            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfo("赛结开奖抓取：没有获取到html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            return Html;
        }

        /// <summary>
        /// 得到赛果及开奖Html
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        private string GetMatchBasketResultHtml(string Page, string Day)
        {
            string strUrl = "";
            string Html = "";

            strUrl = "http://zx.500wan.com/jclq/kaijiang.php?ggid=0&all=0&page=" + Page + "&d=" + Day;
            Html = GetHtml(strUrl, false);          //得到html页面内容

            //Html = HTMLEdit.OnlyBody(Html);

            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfo("赛结开奖抓取：没有获取到html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            return Html;
        }

        #endregion

        #region 得到赔率数据
        public void GetCompensationRate()
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select OkoooMatchID from T_CompensationRate where [Day] >= YEAR(GETDATE()) * 10000 + MONTH(GETDATE()) * 100 + Day(GETDATE())", null);

            if (dt == null)
            {
                new Log("System").Write("数据库读取错误: GetCompensationRate");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            string HTML = "";
            string[] rows = null;

            string Average99_S = "";
            string Average99_P = "";
            string Average99_F = "";
            string Willhill_S = "";
            string Willhill_P = "";
            string Willhill_F = "";
            string Lad_S = "";
            string Lad_P = "";
            string Lad_F = "";
            string Bet365_S = "";
            string Bet365_P = "";
            string Bet365_F = "";
            string Macau_S = "";
            string Macau_P = "";
            string Macau_F = "";

            string[] Str = new string[3];
            string StrRegex = @"(\d){1,5}[.]\d\d";

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    HTML = GetPassRateHtml("http://www.okooo.com/match/1/" + dr["OkoooMatchID"].ToString() + "/odds/", "获取" + dr["OkoooMatchID"].ToString() + "的欧赔");

                    HTML = HTML.Substring(HTML.IndexOf("99家平均"), HTML.IndexOf("最小值") - HTML.IndexOf("99家平均"));
                }
                catch
                {
                    continue;
                }

                rows = HTML.Split(new string[] { "</TR>" }, StringSplitOptions.RemoveEmptyEntries);

                Average99_S = "0";
                Average99_P = "0";
                Average99_F = "0";
                Willhill_S = "0";
                Willhill_P = "0";
                Willhill_F = "0";
                Lad_S = "0";
                Lad_P = "0";
                Lad_F = "0";
                Bet365_S = "0";
                Bet365_P = "0";
                Bet365_F = "0";
                Macau_S = "0";
                Macau_P = "0";
                Macau_F = "0";

                string tempstr = "";

                int i = 0;

                foreach (string str in rows)
                {
                    try
                    {
                        tempstr = str.Substring(str.IndexOf("okooo"));
                    }
                    catch
                    {
                        continue;
                    }

                    if (str.IndexOf("99家平均") >= 0)
                    {
                        Str = RegexNumber(tempstr, StrRegex);

                        Average99_S = Str[0];
                        Average99_P = Str[1];
                        Average99_F = Str[2];

                        i++;
                    }

                    if (str.IndexOf("威廉.希尔") >= 0)
                    {
                        Str = RegexNumber(tempstr, StrRegex);

                        Willhill_S = Str[0];
                        Willhill_P = Str[1];
                        Willhill_F = Str[2];

                        i++;
                    }

                    if (str.IndexOf("立博") >= 0)
                    {
                        Str = RegexNumber(tempstr, StrRegex);

                        Lad_S = Str[0];
                        Lad_P = Str[1];
                        Lad_F = Str[2];

                        i++;
                    }

                    if (str.IndexOf("Bet365") >= 0)
                    {
                        Str = RegexNumber(tempstr, StrRegex);

                        Bet365_S = Str[0];
                        Bet365_P = Str[1];
                        Bet365_F = Str[2];

                        i++;
                    }

                    if (str.IndexOf("澳门彩票") >= 0)
                    {
                        Str = RegexNumber(tempstr, StrRegex);

                        Macau_S = Str[0];
                        Macau_P = Str[1];
                        Macau_F = Str[2];

                        i++;
                    }

                    if (i >= 5)
                    {
                        break;
                    }
                }

                sb.Append("exec P_CompensationRateEdit " + dr["OkoooMatchID"].ToString() + ",'','', " + Average99_S + ", " + Average99_P + ", " + Average99_F + ", " + Willhill_S + ", " + Willhill_P + ", " + Willhill_F + ", " + Lad_S + ", " + Lad_P + ", " + Lad_F + ", " + Bet365_S + ", " + Bet365_P + ", " + Bet365_F + ", " + Macau_S + ", " + Macau_P + ", " + Macau_F + ", 0, ''; ");
            }

            ExecSql(sb, 1, "http://data.okooo.com/MatchInfo/MatchReportLatestOdds.php");
        }
        //正则表达式
        private string[] RegexNumber(string Str, string StrRegex)
        {
            Regex regex = new Regex(StrRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(Str);

            string[] strings = new string[3];
            int i = 0;

            while (m.Success && i < 3)
            {
                strings[i] = m.Value;
                i++;

                m = m.NextMatch();
            }

            return strings;
        }

        #endregion

        #region 将赔率数据写入文件,针对不同赔率公司
        public void WriteToFile()
        {
            string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!AppDir.EndsWith("\\"))
            {
                AppDir += "\\";
            }

            Shove._IO.IniFile ini = new Shove._IO.IniFile(AppDir + "Config.ini");

            //第一步：读取最近一期的SP值
            DataTable dt = Shove.Database.MSSQL.Select(ConnectionString, "select * from T_CompensationRate where [Day] >= YEAR(GETDATE()) * 10000 + MONTH(GETDATE()) * 100 + Day(GETDATE())", null);

            StringBuilder sb_Average99 = new StringBuilder();
            StringBuilder sb_Willhill = new StringBuilder();
            StringBuilder sb_Lad = new StringBuilder();
            StringBuilder sb_Bet365 = new StringBuilder();
            StringBuilder sb_Macau = new StringBuilder();

            //第二步：将结果写入文件中

            if (dt != null && dt.Rows.Count > 0)
            {
                sb_Average99.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sb_Average99.Append("<xml>");

                sb_Willhill.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sb_Willhill.Append("<xml>");

                sb_Lad.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sb_Lad.Append("<xml>");

                sb_Bet365.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sb_Bet365.Append("<xml>");

                sb_Macau.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?> ");
                sb_Macau.Append("<xml>");

                foreach (DataRow rw in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {

                        switch (col.ColumnName)
                        {
                            #region 让球胜平负的sp值

                            case "Average99_S":
                                sb_Average99.Append("<m id=\"" + rw["MatchID"] + "\" win=\"" + rw["Average99_S"].ToString() + "\" ");
                                break;

                            case "Average99_P":
                                sb_Average99.Append("draw=\"" + rw["Average99_P"].ToString() + "\" ");
                                break;

                            case "Average99_F":
                                sb_Average99.Append("lost=\"" + rw["Average99_F"].ToString() + "\" />");
                                break;

                            #endregion

                            #region 总进球数的sp值

                            case "Willhill_S":
                                sb_Willhill.Append("<m id=\"" + rw["MatchID"] + "\" win=\"" + rw["Willhill_S"].ToString() + "\" ");
                                break;

                            case "Willhill_P":
                                sb_Willhill.Append("draw=\"" + rw["Willhill_P"].ToString() + "\" ");
                                break;

                            case "Willhill_F":
                                sb_Willhill.Append("lost=\"" + rw["Willhill_F"].ToString() + "\" />");
                                break;

                            #endregion

                            #region 比分的sp值

                            case "Lad_S":
                                sb_Lad.Append("<m id=\"" + rw["MatchID"] + "\" win=\"" + rw["Lad_S"].ToString() + "\" ");
                                break;

                            case "Lad_P":
                                sb_Lad.Append("draw=\"" + rw["Lad_P"].ToString() + "\" ");
                                break;

                            case "Lad_F":
                                sb_Lad.Append("lost=\"" + rw["Lad_F"].ToString() + "\" />");
                                break;

                            #endregion

                            #region 半全场胜平负的sp值

                            case "Bet365_S":
                                sb_Bet365.Append("<m id=\"" + rw["MatchID"] + "\" win=\"" + rw["Bet365_S"].ToString() + "\" ");
                                break;

                            case "Bet365_P":
                                sb_Bet365.Append("draw=\"" + rw["Bet365_P"].ToString() + "\" ");
                                break;

                            case "Bet365_F":
                                sb_Bet365.Append("lost=\"" + rw["Bet365_F"].ToString() + "\" />");
                                break;

                            #endregion

                            #region 半全场胜平负的sp值

                            case "Macau_S":
                                sb_Macau.Append("<m id=\"" + rw["MatchID"] + "\" win=\"" + rw["Macau_S"].ToString() + "\" ");
                                break;

                            case "Macau_P":
                                sb_Macau.Append("draw=\"" + rw["Macau_P"].ToString() + "\" ");
                                break;

                            case "Macau_F":
                                sb_Macau.Append("lost=\"" + rw["Macau_F"].ToString() + "\" />");
                                break;

                            #endregion
                        }
                    }
                }

                sb_Average99.Append("</xml>");
                sb_Willhill.Append("</xml>");
                sb_Lad.Append("</xml>");
                sb_Bet365.Append("</xml>");
                sb_Macau.Append("</xml>");

                WriteSPToJsFile("Average99.xml", sb_Average99.ToString());
                WriteSPToJsFile("Willhill.xml", sb_Willhill.ToString());
                WriteSPToJsFile("Lad.xml", sb_Lad.ToString());
                WriteSPToJsFile("Bet365.xml", sb_Bet365.ToString());
                WriteSPToJsFile("Macau.xml", sb_Macau.ToString());
            }
        }

        public void WriteSPToJsFile(string FileName, string Message)
        {
            string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!AppDir.EndsWith("\\"))
            {
                AppDir += "\\";
            }

            Shove._IO.IniFile ini = new Shove._IO.IniFile(AppDir + "Config.ini");

            string PathName = ini.Read("Config", "PathName");

            if (PathName == "")
            {
                return;
            }

            if (!Directory.Exists(PathName))
            {
                try
                {
                    Directory.CreateDirectory(PathName);
                }
                catch
                {
                    throw;
                }
            }

            FileName = PathName + @"\" + FileName;

            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));

                try
                {
                    writer.WriteLine(Message);
                }
                catch { }

                writer.Close();
            }
        }
        #endregion

        #region 公用

        /// <summary>
        /// 执行足球sql语句
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Count"></param>
        /// <param name="title"></param>
        private void ExecSql(StringBuilder sb, int Count, string title)
        {
            int Row = 0;
            try
            {
                Row = MsSql.ExecuteNonQuery(sb.ToString(), this.GetType().Name, MsSql.ConnectionString);
            }
            catch (Exception e)
            {
                log.Write(e.Message + " SQL语句：" + sb.ToString()); return;
            }

            if (Row > -1)
            {
                PrintThrendInfo(title + "抓取：数据写入成功." + Count + "条");
                PrintThrendInfo(title + "抓取：完成" + title + "数据抓取.");
            }
            else
            {
                PrintThrendInfo(title + "抓取：未完成" + title + "数据抓取,数据更新时出错.");
            }
        }

        /// <summary>
        /// 执行篮球sql语句
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Count"></param>
        /// <param name="title"></param>
        private void ExecBasketSql(StringBuilder sb, int Count, string title)
        {
            int Row = 0;
            try
            {
                Row = MsSql.ExecuteNonQuery(sb.ToString(), this.GetType().Name, MsSql.ConnectionString);
            }
            catch (Exception e)
            {
                log.Write(e.Message + " SQL语句：" + sb.ToString()); return;
            }

            if (Row > -1)
            {
                PrintThrendInfoBascket(title + "抓取：数据写入成功." + Count + "条");
                PrintThrendInfoBascket(title + "抓取：完成" + title + "数据抓取.");
            }
            else
            {
                PrintThrendInfoBascket(title + "抓取：未完成" + title + "数据抓取,数据更新时出错.");
            }
        }

        /// <summary>
        /// 执行篮球sql语句
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="Count"></param>
        /// <param name="title"></param>
        private void ExecBasketRateSql(StringBuilder sb, int Count, string title)
        {
            int Row = 0;

            try
            {
                Row = MsSql.ExecuteNonQuery(sb.ToString(), this.GetType().Name, MsSql.ConnectionString);
            }
            catch (Exception e)
            {
                log.Write(e.Message + " SQL语句：" + sb.ToString()); return;
            }

            if (Row > -1)
            {
                PrintThrendInfoBascket(title + "抓取：数据写入成功." + Count + "条");
                PrintThrendInfoBascket(title + "抓取：完成" + title + "数据抓取.");
            }
            else
            {
                PrintThrendInfoBascket(title + "抓取：未完成" + title + "数据抓取,数据更新时出错.");
            }
        }

        /// <summary>
        /// 字符串分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private string[] SplitString(string str, string separator)
        {
            return HTMLEdit.SplitString(str, separator);
        }

        /// <summary>
        /// 得到Html
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetAllHtml(string strUrl, string title)
        {
            string Html = string.Empty;
            PrintThrendInfo(title + "抓取：开始" + title + "数据抓取.");
            PrintThrendInfo(title + "抓取：目标地址\"" + strUrl + "\"");
            Html = GetHtml(strUrl, false);          //得到html页面内容
            Html = HTMLEdit.OnlyBody(Html);
            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfo(title + "抓取：没有获取到Html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            PrintThrendInfo(title + "抓取：读取页面\"" + strUrl + "\"成功.开始分析页面数据...");
            return Html.Replace("&amp;amp;nbsp;", "");
        }

        /// <summary>
        /// 得到Html
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetAllHtmlBasket(string strUrl, string title)
        {
            string Html = string.Empty;
            PrintThrendInfoBascket(title + "抓取：开始" + title + "数据抓取.");
            PrintThrendInfoBascket(title + "抓取：目标地址\"" + strUrl + "\"");
            Html = GetHtml(strUrl, false);          //得到html页面内容
            Html = HTMLEdit.OnlyBody(Html);
            if (Html == null || Html.Length < 100)
            {
                PrintThrendInfoBascket(title + "抓取：没有获取到Html页面\"" + strUrl + "\"请重试.");
                return "";
            }

            PrintThrendInfoBascket(title + "抓取：读取页面\"" + strUrl + "\"成功.开始分析页面数据...");
            return Html.Replace("&amp;amp;nbsp;", "");
        }


        /// <summary>
        /// 最后写入的提示
        /// </summary>
        /// <param name="title"></param>
        private void ExecEnd(string title)
        {
            PrintThrendInfo(title + "：完成" + title + "数据抓取.");
        }

        /// <summary>
        /// 篮球最后写入的提示
        /// </summary>
        /// <param name="title"></param>
        private void ExecEndBasket(string title)
        {
            PrintThrendInfoBascket(title + "：完成" + title + "数据抓取.");
        }

        //正则表达式
        public static string[] RegexString(string Str, string StrRegex)
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

        private string GetRFSFResult(string BFResult)
        {
            int Main = 0;
            int Guest = 0;

            if (BFResult.Split(':').Length > 1)
            {
                Main = Shove._Convert.StrToInt(BFResult.Split(':')[1], 0);
                Guest = Shove._Convert.StrToInt(BFResult.Split(':')[0], 0);
            }

            int Poor = Guest - Main;

            if (Poor > 25)
            {
                return "客26+";
            }

            if (Poor > 20)
            {
                return "客21-25";
            }

            if (Poor > 15)
            {
                return "客16-20";
            }

            if (Poor > 10)
            {
                return "客11-15";
            }

            if (Poor > 5)
            {
                return "客6-10";
            }

            if (Poor > 0)
            {
                return "客1-5";
            }

            if (Poor > -5)
            {
                return "主1-5";
            }

            if (Poor > -10)
            {
                return "主6-10";
            }

            if (Poor > -15)
            {
                return "主11-25";
            }

            if (Poor > -20)
            {
                return "主16-20";
            }

            if (Poor > -25)
            {
                return "主21-25";
            }

            if (Poor < -25)
            {
                return "主26+";
            }

            return "";
        }

        private string GetSFResult(string BFResult)
        {
            int Main = 0;
            int Guest = 0;

            if (BFResult.Split(':').Length > 1)
            {
                Main = Shove._Convert.StrToInt(BFResult.Split(':')[1], 0);
                Guest = Shove._Convert.StrToInt(BFResult.Split(':')[0], 0);
            }

            if (Guest > Main)
            {
                return "主负";
            }
            else
            {
                return "主胜";
            }
        }

        #endregion

    }
}

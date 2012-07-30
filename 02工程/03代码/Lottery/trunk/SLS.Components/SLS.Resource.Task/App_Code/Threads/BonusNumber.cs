using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Shove.Database;

namespace SLS.Resource.Task
{

    /// <summary>
    /// 奖期类
    /// </summary>
    class BonusNumber
    {
        private static Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");


        #region 十一运夺金

        //十一运夺金每期的开奖时间间隔为15+7＝22分钟
        private static int intervalMin_SYYDJ = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_SYYDJ = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_SYYDJ(string ConnectionString, string LotteryID)
        {
            //先获取间隔时间
            intervalMin_SYYDJ = Shove._Convert.StrToInt(ini.Read("Options", "IntervalMin"), 18);

            //如果上期的开了奖的结期时间加上22分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_SYYDJ.AddMinutes(intervalMin_SYYDJ) < DateTime.Now)
            {
                new Log("System").Write("十一运夺金：" + lastIssueEndTime_SYYDJ.ToString());
                new Log("System").Write("十一运夺金：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();


                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=62 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                           .Append(dr["Name"].ToString())
                           .Append("&nbsp;WORD_ONE:")
                           .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                           .Append("<td class='hui12' style='padding-left:10px'>")
                           .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                           .Append("</td></td>")
                           .Append("</tr></table>")
                           .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_SYYDJ = DateTime.Now;

                            //获取开奖列表
                            sb.Append("var bonusNumbers = \"");

                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_SYYDJ.js").Write(sb.ToString());
                }

            }
        }

        #endregion

        #region 江西时时彩

        //江西时时彩每期的开奖时间间隔为10分钟
        private static int intervalMin_JXSSC = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_JXSSC = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_JXSSC(string ConnectionString, string LotteryID)
        {

            //如果上期的开了奖的结期时间加上10分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_JXSSC.AddMinutes(intervalMin_JXSSC) < DateTime.Now)
            {
                new Log("System").Write("江西时时彩：" + lastIssueEndTime_JXSSC.ToString());
                new Log("System").Write("江西时时彩：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();

                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryId=61 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");
                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                            .Append(dr["Name"].ToString())
                            .Append("&nbsp;WORD_ONE:")
                            .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                            .Append("<td class='hui12' style='padding-left:10px'>")
                            .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                            .Append("</td></td>")
                            .Append("</tr></table>")
                            .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_JXSSC = DateTime.Now;

                            sb.Append("var bonusNumbers = \"");
                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_JXSSC.js").Write(sb.ToString());
                }
            }

        }


        #endregion

        #region 11选5

        //十一运夺金每期的开奖时间间隔为15+7＝22分钟
        private static int intervalMin_11X5 = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_11X5 = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_11X5(string ConnectionString, string LotteryID)
        {
            //先获取间隔时间
            intervalMin_11X5 = Shove._Convert.StrToInt(ini.Read("Options", "IntervalMin"), 18);

            //如果上期的开了奖的结期时间加上22分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_11X5.AddMinutes(intervalMin_11X5) < DateTime.Now)
            {
                new Log("System").Write("11选5：" + lastIssueEndTime_11X5.ToString());
                new Log("System").Write("11选5：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();


                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=70 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                           .Append(dr["Name"].ToString())
                           .Append("&nbsp;WORD_ONE:")
                           .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                           .Append("<td class='hui12' style='padding-left:10px'>")
                           .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                           .Append("</td></td>")
                           .Append("</tr></table>")
                           .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_11X5 = DateTime.Now;

                            //获取开奖列表
                            sb.Append("var bonusNumbers = \"");

                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_JX11X5.js").Write(sb.ToString());
                }

            }
        }

        #endregion

        #region 重庆时时彩

        //江西时时彩每期的开奖时间间隔为10分钟
        private static int intervalMin_CQSSC = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_CQSSC = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_CQSSC(string ConnectionString, string LotteryID)
        {

            //如果上期的开了奖的结期时间加上10分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_CQSSC.AddMinutes(intervalMin_CQSSC) < DateTime.Now)
            {
                new Log("System").Write("重庆时时彩：" + lastIssueEndTime_CQSSC.ToString());
                new Log("System").Write("重庆时时彩：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();

                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryId=28 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");
                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                            .Append(dr["Name"].ToString())
                            .Append("&nbsp;WORD_ONE:")
                            .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                            .Append("<td class='hui12' style='padding-left:10px'>")
                            .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                            .Append("</td></td>")
                            .Append("</tr></table>")
                            .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_CQSSC = DateTime.Now;

                            sb.Append("var bonusNumbers = \"");
                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_CQSSC.js").Write(sb.ToString());
                }
            }

        }


        #endregion

        #region 快赢481

        //河南快赢481每期的开奖时间间隔为10分钟
        private static int intervalMin_KY481 = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_KY481 = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_KY481(string ConnectionString, string LotteryID)
        {

            //如果上期的开了奖的结期时间加上10分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_KY481.AddMinutes(intervalMin_KY481) < DateTime.Now)
            {
                new Log("System").Write("河南快赢481：" + lastIssueEndTime_KY481.ToString());
                new Log("System").Write("河南快赢481：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();

                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryId=68 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");
                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                            .Append(dr["Name"].ToString())
                            .Append("&nbsp;WORD_ONE:")
                            .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                            .Append("<td class='hui12' style='padding-left:10px'>")
                            .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                            .Append("</td></td>")
                            .Append("</tr></table>")
                            .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_KY481 = DateTime.Now;

                            sb.Append("var bonusNumbers = \"");
                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_KY481.js").Write(sb.ToString());
                }
            }

        }


        #endregion

        #region 河南11选5

        //十一运夺金每期的开奖时间间隔为15+7＝22分钟
        private static int intervalMin_HN11X5 = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_HN11X5 = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_HN11X5(string ConnectionString, string LotteryID)
        {
            //先获取间隔时间
            intervalMin_HN11X5 = Shove._Convert.StrToInt(ini.Read("Options", "IntervalMin"), 18);

            //如果上期的开了奖的结期时间加上22分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_HN11X5.AddMinutes(intervalMin_HN11X5) < DateTime.Now)
            {
                new Log("System").Write("河南11选5：" + lastIssueEndTime_HN11X5.ToString());
                new Log("System").Write("河南11选5：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();


                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=77 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                           .Append(dr["Name"].ToString())
                           .Append("&nbsp;WORD_ONE:")
                           .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                           .Append("<td class='hui12' style='padding-left:10px'>")
                           .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                           .Append("</td></td>")
                           .Append("</tr></table>")
                           .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_HN11X5 = DateTime.Now;

                            //获取开奖列表
                            sb.Append("var bonusNumbers = \"");

                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_HN11X5.js").Write(sb.ToString());
                }

            }
        }

        #endregion

        #region 广东11选5

        //十一运夺金每期的开奖时间间隔为15+7＝22分钟
        private static int intervalMin_GD11X5 = 1;

        //上一期的结束时间       
        private static DateTime lastIssueEndTime_GD11X5 = DateTime.MinValue;

        //获取上期的开奖号码
        public static void GetLastWinNumber_GD11X5(string ConnectionString, string LotteryID)
        {
            //先获取间隔时间
            intervalMin_GD11X5 = Shove._Convert.StrToInt(ini.Read("Options", "IntervalMin"), 1);

            //如果上期的开了奖的结期时间加上22分钟，小于当前时间，那么就去数据库读取新的一期开奖号
            if (lastIssueEndTime_GD11X5.AddMinutes(intervalMin_GD11X5) < DateTime.Now)
            {
                new Log("System").Write("11选5：" + lastIssueEndTime_GD11X5.ToString());
                new Log("System").Write("11选5：" + DateTime.Now.ToString() + "执行");

                StringBuilder sb = new StringBuilder();


                DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=78 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = 1;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (i == 1)
                        {
                            //最新开奖期号和奖号
                            sb.Append("var bonusNumber = \"<table cellspacing='5' cellpadding='0' style='text-align: center;'><tr><td  height='25' class='hui12' style='font-weight: bold;'>")
                           .Append(dr["Name"].ToString())
                           .Append("&nbsp;WORD_ONE:")
                           .Append(FormatWinNumber(LotteryID, dr["WinLotteryNumber"].ToString()))
                           .Append("<td class='hui12' style='padding-left:10px'>")
                           .Append(DataBindIsuseCount(ConnectionString, Shove._Convert.StrToInt(LotteryID, 0)))
                           .Append("</td></td>")
                           .Append("</tr></table>")
                           .AppendLine("\";");

                            //更新上期开了奖的结期时间
                            lastIssueEndTime_GD11X5 = DateTime.Now;

                            //获取开奖列表
                            sb.Append("var bonusNumbers = \"");

                        }

                        sb.Append("<tr>")
                        .Append("<td height=\\\"22\\\" align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"blue12\\\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"hui12\\\">")
                        .Append(Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("HH:mm"))
                        .Append("</td>")
                        .Append("<td align=\\\"center\\\" bgcolor=\\\"#FFFFFF\\\" class=\\\"red2\\\">")
                        .Append(dr["WinLotteryNumber"].ToString())
                        .Append("</td>")
                        .Append("</tr>");

                        if (i % 10 == 0 && i != dt.Rows.Count)
                        {
                            sb.Append("|");
                        }

                        i++;

                    }

                    sb.AppendLine("\";");

                    new JsFile(@ini.Read("Options", "JsFilePath"), "BonusNumber_GD11X5.js").Write(sb.ToString());
                }

            }
        }

        #endregion

        #region 胜负彩
        public static void MakeSFCFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select ID, Name, StartTime, EndTime from T_Isuses where LotteryID = 74 and IsOpened = 0 and EndTime > Getdate()");

            if (dt == null)
            {
                new Log("System").Write("获取胜负彩的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            DataSet ds = new DataSet();
            DataTable dtTeam = new DataTable();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbSQL = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                ds.Clear();

                sb.Append("<xml>");
                sb.Append("<head expect=\"" + dr["Name"].ToString() + "\" starttime=\"" + dr["StartTime"].ToString() + "\" EndTime=\"" + dr["EndTime"].ToString() + "\"/>");

                string URL = "http://www.500wan.com/static/public/sfc/daigou/xml/" + dr["Name"].ToString() + ".xml";

                try
                {
                    ds.ReadXml(URL);
                }
                catch
                {
                    new Log("System").Write("获取胜负彩赔率数据出现错误");

                    continue;
                }

                if (ds == null)
                {
                    return;
                }

                if (ds.Tables.Count < 2)
                {
                    return;
                }

                dtTeam = MSSQL.Select(ConnectionString, "select ID, IsuseID, No, HostTeam, QuestTeam, [DateTime],  lostScale, drawScale, winScale from T_IsuseForSFC where IsuseID=" + dr["ID"].ToString() + " order by no ");

                if (dtTeam.Rows.Count < 1)
                {
                    continue;
                }

                string plurl = "";
                string win = "";
                string draw = "";
                string lost = "";
                int winScale = 0;
                int drawScale = 0;
                int lostScale = 0;

                for (int i = 0; i < dtTeam.Rows.Count; i++)
                {
                    plurl = ds.Tables[1].Rows[i]["plurl"].ToString().Replace("&nbsp;",",");

                    win = "";
                    draw = "";
                    lost = "";

                    if (!string.IsNullOrEmpty(plurl))
                    {
                        if (plurl.Split(',').Length == 3)
                        {
                            win = plurl.Split(',')[0];
                            draw = plurl.Split(',')[1];
                            lost = plurl.Split(',')[2];
                        }

                        sbSQL.AppendLine("update T_IsuseForSFC set win=" + win + ", draw=" + draw + ", lost=" + lost + " where id=" + dtTeam.Rows[i]["ID"].ToString() + " ;");
                    }

                    winScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0);
                    drawScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0);
                    lostScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0);

                    sb.Append("<row ordernum=\"" + dtTeam.Rows[i]["no"].ToString() + "\" hometeam=\"" + dtTeam.Rows[i]["HostTeam"].ToString() + "\" QuestTeam=\"" + dtTeam.Rows[i]["QuestTeam"].ToString() + "\" resultscore=\"" + dtTeam.Rows[i]["DateTime"].ToString() + "\" winScale=\"" + (winScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" drawScale=\"" + (drawScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" lostScale=\"" + (lostScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\"  win=\"" + win + "\" draw=\"" + draw + "\" lost=\"" + lost + "\" />");
                }

                sb.Append("</xml>");

                new JsFile(@ini.Read("Options", "XMLFilePath"), "sfc_" + dr["Name"].ToString() + ".xml").Write(sb.ToString());

                sb.Remove(0, sb.ToString().Length);
            }

            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                int Result = MSSQL.ExecuteNonQuery(ConnectionString, sbSQL.ToString());

                if (Result < 0)
                {
                    new Log("System").Write("胜负彩执行更新语句出现错误，语句为：" + sbSQL.ToString());
                }
            }
        }
        #endregion

        #region 任选九场
        public static void MakeRXJCFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select ID, Name, StartTime, EndTime from T_Isuses where LotteryID = 75 and IsOpened = 0 and EndTime > Getdate()");

            if (dt == null)
            {
                new Log("System").Write("获取胜负彩的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            DataSet ds = new DataSet();
            DataTable dtTeam = new DataTable();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbSQL = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<xml>");
                sb.Append("<head expect=\"" + dr["Name"].ToString() + "\" starttime=\"" + dr["StartTime"].ToString() + "\" EndTime=\"" + dr["EndTime"].ToString() + "\"/>");

                try
                {
                    ds.ReadXml("http://www.500wan.com/static/public/sfc/daigou/xml/" + dr["Name"].ToString() + ".xml");
                }
                catch
                {
                    new Log("System").Write("获取胜负彩赔率数据出现错误");

                    continue;
                }

                if (ds == null)
                {
                    return;
                }

                if (ds.Tables.Count < 2)
                {
                    return;
                }

                dtTeam = MSSQL.Select(ConnectionString, "select ID, IsuseID, No, HostTeam, QuestTeam, [DateTime],  lostScale, drawScale, winScale from T_IsuseForSFC where IsuseID=" + dr["ID"].ToString() + " order by no ");

                if (dtTeam.Rows.Count < 1)
                {
                    continue;
                }

                string plurl = "";
                string win = "";
                string draw = "";
                string lost = "";

                int winScale = 0;
                int drawScale = 0;
                int lostScale = 0;

                for (int i = 0; i < dtTeam.Rows.Count; i++)
                {
                    plurl = ds.Tables[1].Rows[i]["plurl"].ToString().Replace("&nbsp;", ",");

                    win = "";
                    draw = "";
                    lost = "";

                    if (!string.IsNullOrEmpty(plurl))
                    {
                        if (plurl.Split(',').Length == 3)
                        {
                            win = plurl.Split(',')[0];
                            draw = plurl.Split(',')[1];
                            lost = plurl.Split(',')[2];
                        }

                        sbSQL.AppendLine("update T_IsuseForSFC set win=" + win + ", draw=" + draw + ", lost=" + lost + " where id=" + dtTeam.Rows[i]["ID"].ToString() + " ;");
                    }

                    winScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0);
                    drawScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0);
                    lostScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0);

                    sb.Append("<row ordernum=\"" + dtTeam.Rows[i]["no"].ToString() + "\" hometeam=\"" + dtTeam.Rows[i]["HostTeam"].ToString() + "\" QuestTeam=\"" + dtTeam.Rows[i]["QuestTeam"].ToString() + "\" resultscore=\"" + dtTeam.Rows[i]["DateTime"].ToString() + "\" winScale=\"" + (winScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" drawScale=\"" + (drawScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" lostScale=\"" + (lostScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\"  win=\"" + win + "\" draw=\"" + draw + "\" lost=\"" + lost + "\" />");
                }

                sb.Append("</xml>");

                new JsFile(@ini.Read("Options", "XMLFilePath"), "rxj_" + dr["Name"].ToString() + ".xml").Write(sb.ToString());

                sb.Remove(0, sb.ToString().Length);
            }

            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                int Result = MSSQL.ExecuteNonQuery(ConnectionString, sbSQL.ToString());

                if (Result < 0)
                {
                    new Log("System").Write("任选九场执行更新语句出现错误，语句为：" + sbSQL.ToString());
                }
            }
        }
        #endregion

        #region 超级大乐透
        public static void MakeDLTFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=39 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

            if (dt == null)
            {
                new Log("System").Write("获取大乐透的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<isuse expect=\"" + dr["Name"].ToString() + "\" EndTime=\"" + Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\" WinLotteryNumber=\"" + dr["WinLotteryNumber"].ToString() + "\"/>");
            }

            sb.Append("</xml>");

            new JsFile(@ini.Read("Options", "XMLFilePath"), "dlt.xml").Write(sb.ToString());
        }
        #endregion

        #region 七星彩
        public static void MakeQXCFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=3 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

            if (dt == null)
            {
                new Log("System").Write("获取大乐透的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<isuse expect=\"" + dr["Name"].ToString() + "\" EndTime=\"" + Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\" WinLotteryNumber=\"" + dr["WinLotteryNumber"].ToString() + "\"/>");
            }

            sb.Append("</xml>");

            new JsFile(@ini.Read("Options", "XMLFilePath"), "qxc.xml").Write(sb.ToString());
        }
        #endregion

        #region 六场半全场
        public static void MakeLCBQCFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select ID, Name, StartTime, EndTime from T_Isuses where LotteryID = 15 and IsOpened = 0 and EndTime > Getdate()");

            if (dt == null)
            {
                new Log("System").Write("获取六场半全场的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            DataSet ds = new DataSet();
            DataTable dtTeam = new DataTable();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbSQL = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<xml>");
                sb.Append("<head expect=\"" + dr["Name"].ToString() + "\" starttime=\"" + dr["StartTime"].ToString() + "\" EndTime=\"" + dr["EndTime"].ToString() + "\"/>");

                try
                {
                    ds.ReadXml("http://www.500wan.com/static/public/zc6/daigou/xml/" + dr["Name"].ToString() + ".xml");
                }
                catch
                {
                    new Log("System").Write("获取胜负彩赔率数据出现错误");

                    continue;
                }

                if (ds == null)
                {
                    return;
                }

                if (ds.Tables.Count < 2)
                {
                    return;
                }

                dtTeam = MSSQL.Select(ConnectionString, "select ID, IsuseID, No, HostTeam, QuestTeam, [DateTime],  lostScale, drawScale, winScale from T_IsuseForLCBQC where IsuseID=" + dr["ID"].ToString() + " order by no ");

                if (dtTeam.Rows.Count < 1)
                {
                    continue;
                }

                string plurl = "";
                string win = "";
                string draw = "";
                string lost = "";

                int winScale = 0;
                int drawScale = 0;
                int lostScale = 0;

                for (int i = 0; i < dtTeam.Rows.Count; i++)
                {
                    plurl = ds.Tables[1].Rows[i]["pl"].ToString().Replace("&nbsp;", ",");

                    win = "";
                    draw = "";
                    lost = "";

                    if (!string.IsNullOrEmpty(plurl))
                    {
                        if (plurl.Split(',').Length == 3)
                        {
                            win = plurl.Split(',')[0].Replace("全:", "");
                            draw = plurl.Split(',')[1];
                            lost = plurl.Split(',')[2];
                        }

                        sbSQL.AppendLine("update T_IsuseForLCBQC set win=" + win + ", draw=" + draw + ", lost=" + lost + " where id=" + dtTeam.Rows[i]["ID"].ToString() + " ;");
                    }

                    winScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["winScale"].ToString(), 0);
                    drawScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["drawScale"].ToString(), 0);
                    lostScale = Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["lostScale"].ToString(), 0);

                    sb.Append("<row ordernum=\"" + dtTeam.Rows[i]["no"].ToString() + "\" hometeam=\"" + dtTeam.Rows[i]["HostTeam"].ToString() + "\" QuestTeam=\"" + dtTeam.Rows[i]["QuestTeam"].ToString() + "\" resultscore=\"" + dtTeam.Rows[i]["DateTime"].ToString() + "\" winScale=\"" + (winScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" drawScale=\"" + (drawScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\" lostScale=\"" + (lostScale * 100 / (winScale + drawScale + lostScale)).ToString() + "\"  win=\"" + win + "\" draw=\"" + draw + "\" lost=\"" + lost + "\" />");
                }

                sb.Append("</xml>");

                new JsFile(@ini.Read("Options", "XMLFilePath"), "lcbqc_" + dr["Name"].ToString() + ".xml").Write(sb.ToString());

                sb.Remove(0, sb.ToString().Length);
            }

            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                int Result = MSSQL.ExecuteNonQuery(ConnectionString, sbSQL.ToString());

                if (Result < 0)
                {
                    new Log("System").Write("任选九场执行更新语句出现错误，语句为：" + sbSQL.ToString());
                }
            }
        }
        #endregion

        #region 四场进球彩
        public static void MakeJQCFile(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select ID, Name, StartTime, EndTime from T_Isuses where LotteryID = 2 and IsOpened = 0 and EndTime > Getdate()");

            if (dt == null)
            {
                new Log("System").Write("获取胜负彩的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            DataSet ds = new DataSet();
            DataTable dtTeam = new DataTable();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbSQL = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<xml>");
                sb.Append("<head expect=\"" + dr["Name"].ToString() + "\" starttime=\"" + dr["StartTime"].ToString() + "\" EndTime=\"" + dr["EndTime"].ToString() + "\"/>");

                try
                {
                    ds.ReadXml("http://www.500wan.com/static/public/jq4/daigou/xml/" + dr["Name"].ToString() + ".xml");
                }
                catch
                {
                    new Log("System").Write("获取胜负彩赔率数据出现错误");

                    continue;
                }

                if (ds == null)
                {
                    return;
                }

                if (ds.Tables.Count < 2)
                {
                    return;
                }

                dtTeam = MSSQL.Select(ConnectionString, "select ID, IsuseID, No, Team, [DateTime],  First, Second, three, Fourth from T_IsuseForJQC where IsuseID=" + dr["ID"].ToString() + " order by no ");

                if (dtTeam.Rows.Count < 1)
                {
                    continue;
                }

                string plurl = "";
                string win = "";
                string draw = "";
                string lost = "";
                int First = 0;
                int Second = 0;
                int three = 0;
                int Fourth = 0;

                for (int i = 0; i < dtTeam.Rows.Count; i++)
                {
                    plurl = ds.Tables[1].Rows[i / 2]["pl"].ToString().Replace("&nbsp;", ",");

                    if (!string.IsNullOrEmpty(plurl))
                    {
                        if (plurl.Split(',').Length == 3)
                        {
                            win = plurl.Split(',')[0].Replace("全:", "");
                            draw = plurl.Split(',')[1];
                            lost = plurl.Split(',')[2];
                        }

                        sbSQL.AppendLine("update T_IsuseForJQC set win=" + win + ", draw=" + draw + ", lost=" + lost + " where id=" + dtTeam.Rows[i]["ID"].ToString() + " ;");
                    }

                    First = Shove._Convert.StrToInt(dtTeam.Rows[i]["First"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["First"].ToString(), 0);
                    Second = Shove._Convert.StrToInt(dtTeam.Rows[i]["Second"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["Second"].ToString(), 0);
                    three = Shove._Convert.StrToInt(dtTeam.Rows[i]["three"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["three"].ToString(), 0);
                    Fourth = Shove._Convert.StrToInt(dtTeam.Rows[i]["Fourth"].ToString(), 0) == 0 ? 1 : Shove._Convert.StrToInt(dtTeam.Rows[i]["Fourth"].ToString(), 0);

                    sb.Append("<row ordernum=\"" + dtTeam.Rows[i]["no"].ToString() + "\" Team=\"" + dtTeam.Rows[i]["Team"].ToString() + "\" resultscore=\"" + dtTeam.Rows[i]["DateTime"].ToString() + "\" First=\"" + (First * 100 / (First + Second + three + Fourth)).ToString("") + "\" Second=\"" + (Second * 100 / (First + Second + three + Fourth)).ToString("") + "\" three=\"" + (three * 100 / (First + Second + three + Fourth)).ToString("") + "\" Fourth=\"" + (Fourth * 100 / (First + Second + three + Fourth)).ToString("") + "\"  win=\"" + win + "\" draw=\"" + draw + "\" lost=\"" + lost + "\" />");
                }

                sb.Append("</xml>");

                new JsFile(@ini.Read("Options", "XMLFilePath"), "jqc_" + dr["Name"].ToString() + ".xml").Write(sb.ToString());

                sb.Remove(0, sb.ToString().Length);
            }

            if (!string.IsNullOrEmpty(sbSQL.ToString()))
            {
                int Result = MSSQL.ExecuteNonQuery(ConnectionString, sbSQL.ToString());

                if (Result < 0)
                {
                    new Log("System").Write("胜负彩执行更新语句出现错误，语句为：" + sbSQL.ToString());
                }
            }
        }
        #endregion

        #region 排列三
        public static void MakePL3File(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=63 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

            if (dt == null)
            {
                new Log("System").Write("获取大乐透的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<isuse expect=\"" + dr["Name"].ToString() + "\" EndTime=\"" + Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\" WinLotteryNumber=\"" + dr["WinLotteryNumber"].ToString() + "\"/>");
            }

            sb.Append("</xml>");

            new JsFile(@ini.Read("Options", "XMLFilePath"), "pl3.xml").Write(sb.ToString());
        }
        #endregion

        #region 排列五
        public static void MakePL5File(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=64 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

            if (dt == null)
            {
                new Log("System").Write("获取大乐透的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<isuse expect=\"" + dr["Name"].ToString() + "\" EndTime=\"" + Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\" WinLotteryNumber=\"" + dr["WinLotteryNumber"].ToString() + "\"/>");
            }

            sb.Append("</xml>");

            new JsFile(@ini.Read("Options", "XMLFilePath"), "pl5.xml").Write(sb.ToString());
        }
        #endregion

        #region 22选5
        public static void Make22X5File(string ConnectionString)
        {
            DataTable dt = MSSQL.Select(ConnectionString, "SELECT TOP 100 Name,WinLotteryNumber,EndTime FROM dbo.T_Isuses WHERE LotteryId=9 and GETDATE()>EndTime AND ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC");

            if (dt == null)
            {
                new Log("System").Write("获取大乐透的期号数据");
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<xml>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<isuse expect=\"" + dr["Name"].ToString() + "\" EndTime=\"" + Shove._Convert.StrToDateTime(dr["EndTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\" WinLotteryNumber=\"" + dr["WinLotteryNumber"].ToString() + "\"/>");
            }

            sb.Append("</xml>");

            new JsFile(@ini.Read("Options", "XMLFilePath"), "22x5.xml").Write(sb.ToString());
        }
        #endregion

        #region 公共函数

        /// <summary>
        /// 格式化开奖号码
        /// </summary>
        /// <param name="LotteryID"></param>
        /// <param name="winNumber"></param>
        /// <returns></returns>
        private static string FormatWinNumber(string LotteryID, string winNumber)
        {
            StringBuilder sb = new StringBuilder();

            switch (LotteryID)
            {
                case "28":

                    if (winNumber.Length > 0)
                    {
                        for (int i = 0; i < winNumber.Length; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                        }
                    }

                    break;

                case "61":

                    if (winNumber.Length > 0)
                    {
                        for (int i = 0; i < winNumber.Length; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                        }
                    }

                    break;

                case "62":
                    if (winNumber.Length > 0)
                    {
                        string[] number = winNumber.Split(' ');

                        for (int i = 0; i < number.Length && i < 5; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                    }

                    break;

                case "70":
                    if (winNumber.Length > 0)
                    {
                        string[] number = winNumber.Split(' ');

                        for (int i = 0; i < number.Length && i < 5; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                    }

                    break;
                case "68":

                    if (winNumber.Length > 0)
                    {
                        for (int i = 0; i < winNumber.Length; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                        }
                    }

                    break;

                case "77":
                    if (winNumber.Length > 0)
                    {
                        string[] number = winNumber.Split(' ');

                        for (int i = 0; i < number.Length && i < 5; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                    }
                    break;

                case "78":
                    if (winNumber.Length > 0)
                    {
                        string[] number = winNumber.Split(' ');

                        for (int i = 0; i < number.Length && i < 5; i++)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                    }
                    break;
            }

            return sb.ToString();

        }
        /// <summary>
        /// 获取已开奖期数
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="lotteryID"></param>
        /// <returns></returns>
        private static string DataBindIsuseCount(string ConnectionString, int lotteryID)
        {

            string[] temp = DAL.Functions.F_GetIsuseCount(ConnectionString, lotteryID).Split(',');

            if (temp == null || temp.Length < 2)
            {
                return "";
            }

            return "WORD_TWO&nbsp;<span class='red12'>" + temp[0] + "</span>&nbsp;WORD_THREE&nbsp;<span class='red12'>" + temp[1] + "</span>&nbsp;WORD_FOUR";
        }


        #endregion

    }
}

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using System.Text;
using System.Net;
using System.Security.Cryptography;

using Shove.Database;
using Shove.Alipay;
using System.Collections.Generic;

public class PF
{
    public const int SystemStartYear = 2008;
    public const int SchemeMaxBettingMoney = 10000000;

    public static string DesKey = Shove._Web.WebConfig.GetAppSettingsString("DesKey");
    public static string CenterMD5Key = Shove._Web.WebConfig.GetAppSettingsString("CenterMD5Key");

    public static string GetCallCert()
    {
        string Result = "";

        Result = Shove._Web.WebConfig.GetAppSettingsString("DllCallCert");

        return Result;
    }

    #region 发送通知

    public static int SendEmail(Sites Site, string MailTo, string Subject, string Body)
    {
        SendEmailTask sendemailtask = new SendEmailTask(Site, MailTo, Subject, Body);
        sendemailtask.Run();

        return 0;
    }

    public static int SendSMS(Sites Site, long UserID, string Mobile, string Content)  // UserID 付费用户， < 0 则系统付费
    {
        if ((Site == null) || (Site.ID < 0))
        {
            return -1;
        }

        DAL.Tables.T_SMS sms = new DAL.Tables.T_SMS();

        sms.SiteID.Value = Site.ID;
        sms.SMSID.Value = -1;
        sms.From.Value = "";
        sms.To.Value = Mobile;
        sms.Content.Value = Content;
        sms.IsSent.Value = false; 
        sms.Insert(); 
        return 0; 
        //SendSMSTask sendsmstask = new SendSMSTask(Site, UserID, Mobile, Content); 
        //sendsmstask.Run(); 
        
    }

    public static int SendStationSMS(Sites Site, long SourceUserID, long AimUserID, short StationSMSType, string Content)  // UserID 付费用户， < 0 则系统付费
    {
        DAL.Tables.T_StationSMS T_StationSMS = new DAL.Tables.T_StationSMS(); 
        T_StationSMS.SiteID.Value = Site.ID;
        T_StationSMS.SourceID.Value = SourceUserID;
        T_StationSMS.AimID.Value = AimUserID;
        T_StationSMS.Type.Value = StationSMSType;
        T_StationSMS.Content.Value = Content;
        T_StationSMS.isRead.Value = false;
        T_StationSMS.Insert();

        return 0;
    }

    #endregion

    #region GoError()

    //public static void GoError()
    //{
    //    GoError("~/Error.aspx", 1, "");
    //}

    //public static void GoError(int ErrorNumber)
    //{
    //    GoError("~/Error.aspx", ErrorNumber, "");
    //}

    //public static void GoError(string Tip)
    //{
    //    GoError("~/Error.aspx", 1, Tip);
    //}

    public static void GoError(int ErrorNumber, string Tip, string ClassName)
    {
        GoError("~/Error.aspx", ErrorNumber, Tip, ClassName);
    }

    public static void GoError(string ErrorPageUrl, int ErrorNumber, string Tip, string ClassName)
    {
        System.Web.HttpContext.Current.Response.Redirect(ErrorPageUrl + "?ErrorNumber=" + ErrorNumber.ToString() + "&Tip=" + System.Web.HttpUtility.UrlEncode(Tip) + "&ClassName=" + System.Web.HttpUtility.UrlEncode(Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ClassName)), true);
    }

    #endregion

    #region GoLogin()

    public static void GoLogin()
    {
        GoLogin("UserLogin.aspx", "", true);
    }

    public static void GoLogin(bool isAtFramePageLogin)
    {
        GoLogin("UserLogin.aspx", "", isAtFramePageLogin);
    }

    public static void GoLogin(string RequestLoginPage)
    {
        GoLogin("UserLogin.aspx", RequestLoginPage, true);
    }

    public static void GoLogin(string RequestLoginPage, bool isAtFramePageLogin)
    {
        GoLogin("UserLogin.aspx", RequestLoginPage, isAtFramePageLogin);
    }

    public static void GoLogin(string LoginPageFileName, string RequestLoginPage)
    {
        GoLogin(LoginPageFileName, RequestLoginPage, true);
    }

    public static void GoLogin(string LoginPageFileName, string RequestLoginPage, bool isAtFramePageLogin)
    {
        if (RequestLoginPage.Contains("/Home/Alipay/"))
        {
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Home/Alipay/Login.aspx";
        }
        else if ((RequestLoginPage.Contains("/Web/OnlinePay/")) || (RequestLoginPage.Contains("/Web/") && !RequestLoginPage.Contains("/Home/Web/")))
        {
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Web/"+LoginPageFileName;
        }
        else
        {
            //LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Home/Room/" + LoginPageFileName;
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/" + LoginPageFileName;
        }

        if (isAtFramePageLogin)
        {
            HttpContext.Current.Response.Redirect(LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage), true);
        }
        else
        {

            HttpContext.Current.Response.Write("<script language=\"javascript\">window.top.location.href=\"" + LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "\";</script>");
        }
    }

    #endregion

    #region 版本号相关

    public static string GetEdition()
    {
        return "5.0.001";
    }

    public static string GetDatabaseEdition()
    {
        return new SystemOptions()["SystemDatabaseVersion"].ToString("");
    }

    public static bool ValidEdition()
    {
        return (GetEdition() == GetDatabaseEdition());
    }

    #endregion

    // <summary>
    /// 标题加亮显示
    /// </summary>
    /// <param name="Str">要加亮的字符</param>
    /// <returns></returns>
    public static string StringAddsBrightly(string Str)
    {
        if (Str.IndexOf("[") >= 0)
        {
            Str = Str.Replace("[", "<font color='red'>[").Replace("]", "]</font>");

            return Str;
        }
        else
        {
            return Str;
        }
    }

    // 转换时间格式
    public static string ConvertDateTimeMMDDHHMM(string strDateTime)
    {
        DateTime dt;

        try
        {
            dt = DateTime.Parse(strDateTime);
        }
        catch
        {
            return "";
        }

        return dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');

    }

    // 加密密码
    public static string EncryptPassword(string input)
    {
        bool IsMD5 = Shove._Web.WebConfig.GetAppSettingsBool("IsMD5", false);

        if (IsMD5)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5"); //宁波客户彩票俱乐部 使用的是纯MD5 加密方式
        }
        else
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input + "7ien.shovesoft.shove 中国深圳 2007-10-25", "MD5");
        }
    }

    // 获取总站类实例
    public static Sites GetMasterSite()
    {
        return new Sites()[DAL.Functions.F_GetMasterSiteID()];
    }

    // 检验彩种是否有效
    public static bool ValidLotteryID(Sites Site, int LotteryID)
    {
        return (("," + Site.UseLotteryList + ",").IndexOf("," + LotteryID.ToString() + ",") >= 0);
    }

    // 校验彩票时间输入是否有效
    public static object ValidLotteryTime(string Time)
    {
        Time = Time.Trim();
        Regex regex = new Regex(@"[\d]{4}[-][\d]{1,2}[-][\d]{1,2}[\s][\d]{1,2}[:][\d]{1,2}[:][\d]{1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        if (!regex.IsMatch(Time))
        {
            return null;
        }

        System.DateTime dt;

        try
        {
            dt = System.DateTime.Parse(Time);
        }
        catch
        {
            return null;
        }

        return dt;
    }

    // 拼合期号的附加 XML 字串(1球队)
    public static string BuildIsuseAdditionasXmlFor1Team(int Count, params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < Count; i++)
        {
            Result += "<Team No=\"" + (i + 1).ToString() + "\" Team=\"" + str[i * 2] + "\" Time=\"" + str[i * 2 + 1] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(2球队)
    public static string BuildIsuseAdditionasXmlFor2Team(int Count, params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < Count; i++)
        {
            Result += "<Team No=\"" + (i + 1).ToString() + "\" HostTeam=\"" + str[i * 3] + "\" QuestTeam=\"" + str[i * 3 + 1] + "\" Time=\"" + str[i * 3 + 2] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(足彩单场)
    public static string BuildIsuseAdditionasXmlForZCDC(params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < (str.Length / 5); i++)
        {
            Result += "<Team LeagueTypeID=\"" + str[i * 5] + "\" No=\"" + (i + 1).ToString() + "\" HostTeam=\"" + str[i * 5 + 1] + "\" QuestTeam=\"" + str[i * 5 + 2] + "\" LetBall=\"" + str[i * 5 + 3] + "\" Time=\"" + str[i * 5 + 4] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(高频玩法)
    public static string BuildIsuseAdditionasXmlForBJKL8(params string[] str)
    {
        string Result = "<ChaseDetail>";

        for (int i = 0; i < str.Length / 9; i++)
        {
            Result += "<Isuse IsuseID=\"" + str[i * 9] + "\" PlayTypeID = \"" + str[i * 9 + 1] + "\" LotteryNumber = \"" + str[i * 9 + 2] + "\" Multiple = \"" + str[i * 9 + 3] + "\" Money = \"" + str[i * 9 + 4] + "\" SecrecyLevel =\"" + str[i * 9 + 5] + "\" Share=\"" + str[i * 9 + 6] + "\" BuyedShare=\"" + str[i * 9 + 7] + "\" AssureShare=\"" + str[i * 9 + 8] + "\"/>";
        }

        return Result += "</ChaseDetail>";
    }

    // 拼合期号的附加 XML 字串(追号)
    public static string BuildIsuseAdditionasXmlForChase(params string[] str)
    {
        string Result = "<ChaseDetail>";

        for (int i = 0; i < str.Length / 6; i++)
        {
            Result += "<Isuse IsuseID=\"" + str[i * 6] + "\" PlayTypeID = \"" + str[i * 6 + 1] + "\" LotteryNumber = \"" + str[i * 6 + 2] + "\" Multiple = \"" + str[i * 6 + 3] + "\" Money = \"" + str[i * 6 + 4] + "\" SecrecyLevel =\"" + str[i * 6 + 5] + "\" Share=\"1\" BuyedShare=\"1\" AssureShare=\"0\"/>";
        }

        return Result += "</ChaseDetail>";
    }

    public static string GetPassWay(string val)
    {
        string Group = "单关,2x1,3x1,3x3,3x4,4x1,4x4,4x5,4x6,4x11,5x1,5x5,5x6,5x10,5x16,5x20,5x26,6x1,6x6,6x7,6x15,6x20,6x22,6x35,6x42,6x50,6x57,7x1,7x7,7x8,7x21,7x35,7x120,8x1,8x8,8x9,8x28,8x56,8x70,8x247";
        string letter = "A0AAABACADAEAFAGAHAIAJAKALAMANAOAPAQARASATAUAVAWAXAYAZBABBBCBDBEBFBGBHBIBJBKBLBM";

        string[] Groups = Group.Split(',');

        int len = val.Split(';').Length;

        if (len < 3)
        {
            return "";
        }

        string curType = val.Split(';')[2].ToString().Replace("]", "").Replace("[", "");

        string way = "";
        string g = "";

        foreach (string ways in curType.Split(','))
        {
            g = ways;

            int i = -1;

            if (g.Substring(0, 1) == "B")
            {
                i = letter.LastIndexOf(g.Substring(0, 2));
            }
            else
            {
                i = letter.IndexOf(g.Substring(0, 2));
            }

            if (i == -1)
            {
                g = "A" + g.Substring(0, 1);
                i = letter.IndexOf(g.Substring(0, 2));
            }

            way += Groups[i / 2] + ",";
        }

        way = way.EndsWith(",") ? way.Substring(0, way.Length - 1) : way;

        return way;
    }

    public static void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
    {
        IStart = str.IndexOf(strStart);
        if (IStart != -1)
            ILen = str.LastIndexOf(strEnd) - IStart;
        else
        {
            IStart = 0;
            ILen = 0;
        }
    }

    public static string GetScriptResTable(string val)
    {
        try
        {
            string way = GetPassWay(val);               //得到过关方式

            StringBuilder sb = new StringBuilder();

            val = val.Trim();

            int Istart, Ilen;

            GetStrScope(val, "[", "]", out Istart, out Ilen);

            string matchlist = val.Substring(Istart + 1, Ilen - 1);

            string type = val.Split(';')[0];

            if (type.Substring(0, 2) != "72" && type.Substring(0, 2) != "73")
            {
                return val;
            }

            int PlayTypeID = Shove._Convert.StrToInt(val.Split(';')[0], 7201);

            if (type.Substring(0, 2) == "72")
            {
                sb.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.Append("<th scope='col' width='60'>赛事编号</th>");
                sb.Append("<th scope='col' width='100'>联赛</th><th scope='col' width='150'>主队 VS 客队</th>");
                sb.Append("<th scope='col' width='150'>预计停售时间</th><th scope='col' width='50'>设胆</th><th scope='col'>投注内容</th><th scope='col'>赛果</th>");
            }
            else
            {
                sb.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.Append("<th scope='col' width='60'>赛事编号</th>");
                sb.Append("<th scope='col' width='100'>联赛</th><th scope='col' width='150'>客队 VS 主队</th>");
                sb.Append("<th scope='col' width='150'>预计停售时间</th><th scope='col' width='50'>设胆</th><th scope='col'>投注内容</th><th scope='col'>赛果</th>");
            }

            string Matchids = "";
            string[] ArrRes = null;
            string MatchListDan = "";
            string MatchidsDan = "";

            DataTable dtMatch = new DataTable();

            dtMatch.Columns.Add("MatchID", typeof(System.String));
            dtMatch.Columns.Add("PlayType", typeof(System.String));

            if (val.Split(';').Length == 4)
            {
                MatchListDan = matchlist.Split(']')[0];
                matchlist = matchlist.Split('[')[1];

                foreach (string match in MatchListDan.Split('|'))
                {
                    DataRow dr = dtMatch.NewRow();

                    dr["MatchID"] = match.Split('(')[0];
                    dr["PlayType"] = match.Split('(')[1].Substring(0, match.Split('(')[1].LastIndexOf(')'));

                    MatchidsDan += match.Split('(')[0] + ",";
                    Matchids += match.Split('(')[0] + ",";

                    dtMatch.Rows.Add(dr);
                    dtMatch.AcceptChanges();
                }

                if (MatchidsDan.EndsWith(","))
                {
                    MatchidsDan = MatchidsDan.Substring(0, MatchidsDan.Length - 1);
                }

                if (string.IsNullOrEmpty(MatchidsDan))
                {
                    return val;
                }
            }

            foreach (string match in matchlist.Split('|'))
            {
                DataRow dr = dtMatch.NewRow();

                dr["MatchID"] = match.Split('(')[0];
                dr["PlayType"] = match.Split('(')[1].Substring(0, match.Split('(')[1].LastIndexOf(')'));

                Matchids += match.Split('(')[0] + ",";

                dtMatch.Rows.Add(dr);
                dtMatch.AcceptChanges();
            }

            if (Matchids.EndsWith(","))
            {
                Matchids = Matchids.Substring(0, Matchids.Length - 1);
            }

            if (string.IsNullOrEmpty(Matchids))
            {
                return val;
            }

            DataTable table = null;

            if (type.Substring(0, 2) == "72")
            {
                string SQL = "game,mainteam +'VS'+guestteam teamname,MatchNumber,DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellingTime) date, id";

                switch (PlayTypeID)
                {
                    case 7201:
                        {
                            SQL += ", SPFResult as Result";
                        }
                        break;
                    case 7202:
                        {
                            SQL += ", ZQBFResult  as Result";
                        }
                        break;
                    case 7203:
                        {
                            SQL += ",ZJQSResult  as Result ";
                        }
                        break;
                    case 7204:
                        {
                            SQL += ", BQCResult as Result ";
                        }
                        break;
                }

                table = new DAL.Tables.T_Match().Open(SQL, "id in (" + Matchids + ")", "");
            }
            else
            {
                string SQL = "game,guestteam +'VS'+mainteam teamname,MatchNumber,DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellingTime) date, id";

                switch (PlayTypeID)
                {
                    case 7301:
                        {
                            SQL += ", SFResult as Result";
                        }
                        break;
                    case 7302:
                        {
                            SQL += ", RFSFResult  as Result";
                        }
                        break;
                    case 7303:
                        {
                            SQL += ",SFCResult  as Result ";
                        }
                        break;
                    case 7304:
                        {
                            SQL += ", DXResult as Result ";
                        }
                        break;
                }

                table = new DAL.Tables.T_MatchBasket().Open(SQL, "id in (" + Matchids + ")", "");
            }

            if (table == null)
            {
                return val;
            }

            if (table.Rows.Count < 1)
            {
                return val;
            }

            string res = "";
            string Result = "";

            foreach (DataRow dr in table.Rows)
            {
                sb.Append("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td>" + dr["MatchNumber"].ToString() + "</td><td>" + dr["game"].ToString() + "</td><td>" + dr["teamname"].ToString() + "</td><td>" + Shove._Convert.StrToDateTime(dr["date"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>");
                sb.Append("<td>");

                if (MatchidsDan.IndexOf(dr["id"].ToString()) >= 0)
                {
                    sb.Append("是");
                }

                sb.Append("</td>");

                res = dtMatch.Select("MatchID='" + dr["id"].ToString() + "'")[0]["PlayType"].ToString();

                ArrRes = res.Split(',');
                sb.Append("<td>");

                foreach (string r in ArrRes)
                {
                    Result = Getesult(type, r);
                    if (Result.Equals(dr["Result"].ToString()))
                    {
                        Result = "<span class=\"red eng\">" + Result + "</span>";
                    }

                    sb.Append(Result + " ");
                }

                sb.Append("</td><td><span class=\"red eng\">" + dr["Result"].ToString() + "</span>");
                sb.Append("</td></tr>");
            }

            sb.Append("</table></div>");
            sb.Append("<div style=\"text-align:center;width:660px;\">过关方式：" + way + "</div>");

            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            new Log("TWZT").Write(ex.Message);

            return val;
        }
    }

    public static string Getesult(string PlayType, string num)
    {
        string res = string.Empty;

        switch (PlayType)
        {
            case "7201":
                res = Get7201(num);
                break;
            case "7202":
                res = Get7202(num);
                break;
            case "7203":
                res = Get7203(num);
                break;
            case "7204":
                res = Get7204(num);
                break;
            case "7301":
                res = Get7301(num);
                break;
            case "7302":
                res = Get7302(num);
                break;
            case "7303":
                res = Get7303(num);
                break;
            case "7304":
                res = Get7304(num);
                break;
        }

        return res;
    }

    #region 足彩

    /// <summary>
    /// 胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "1:0";
                break;
            case "2": res = "2:0";
                break;
            case "3": res = "2:1";
                break;
            case "4": res = "3:0";
                break;
            case "5": res = "3:1";
                break;
            case "6": res = "3:2";
                break;
            case "7": res = "4:0";
                break;
            case "8": res = "4:1";
                break;
            case "9": res = "4:2";
                break;
            case "10": res = "5:0";
                break;
            case "11": res = "5:1";
                break;
            case "12": res = "5:2";
                break;
            case "13": res = "胜其它";
                break;
            case "14": res = "0:0";
                break;
            case "15": res = "1:1";
                break;
            case "16": res = "2:2";
                break;
            case "17": res = "3:3";
                break;
            case "18": res = "平其它";
                break;
            case "19": res = "0:1";
                break;
            case "20": res = "0:2";
                break;
            case "21": res = "1:2";
                break;
            case "22": res = "0:3";
                break;
            case "23": res = "1:3";
                break;
            case "24": res = "2:3";
                break;
            case "25": res = "0:4";
                break;
            case "26": res = "1:4";
                break;
            case "27": res = "2:4";
                break;
            case "28": res = "0:5";
                break;
            case "29": res = "1:5";
                break;
            case "30": res = "2:5";
                break;
            case "31": res = "负其它";
                break;


            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion 足彩

    #region 篮彩

    /// <summary>
    /// 胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7301(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "主负";
                break;
            case "2": res = "主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 让分胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7302(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "让分主负";
                break;
            case "2": res = "让分主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 胜分差
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "客胜 1-5";
                break;
            case "2": res = "主胜 1-5";
                break;
            case "3": res = "客胜 6-10";
                break;
            case "4": res = "主胜 6-10";
                break;
            case "5": res = "客胜 11-15";
                break;
            case "6": res = "主胜 11-15";
                break;
            case "7": res = "客胜 16-20";
                break;
            case "8": res = "主胜 16-20";
                break;
            case "9": res = "客胜 21-25";
                break;
            case "10": res = "主胜 21-25";
                break;
            case "11": res = "客胜 26+";
                break;
            case "12": res = "主胜 26+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 大小分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private static string Get7304(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "大";
                break;
            case "2": res = "小";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion 篮彩

    // 中奖的记录，发送通知
    public static void SendWinNotification(DataSet ds)
    {
        if (ds == null)
        {
            return;
        }

        for (int i = 0; i < ds.Tables.Count; i++)
        {
            DataTable dt = ds.Tables[i];

            if (dt.Rows.Count < 1)
            {
                continue;
            }

            long SiteID = long.Parse(dt.Rows[0]["SiteID"].ToString());

            bool isWinSendEmail = (DAL.Functions.F_GetSiteSendNotificationTypes(SiteID, NotificationManners.Email).IndexOf("[" + NotificationTypes.Win + "]") >= 0);
            bool isWinSendSMS = (DAL.Functions.F_GetSiteSendNotificationTypes(SiteID, NotificationManners.SMS).IndexOf("[" + NotificationTypes.Win + "]") >= 0);
            bool isWinSendStationSMS = (DAL.Functions.F_GetSiteSendNotificationTypes(SiteID, NotificationManners.StationSMS).IndexOf("[" + NotificationTypes.Win + "]") >= 0);

            if (!isWinSendEmail && !isWinSendSMS && !isWinSendStationSMS)
            {
                continue;
            }

            Sites ts = new Sites()[SiteID];

            if (ts == null)
            {
                continue;
            }

            string Old_EmailSubject = "", Old_EmailBody = "";

            if (isWinSendEmail)
            {
                ts.SiteNotificationTemplates.SplitEmailTemplate(ts.SiteNotificationTemplates[NotificationManners.Email, NotificationTypes.Win], ref Old_EmailSubject, ref Old_EmailBody);
            }

            string Old_SMSBody = "";

            if (isWinSendSMS)
            {
                Old_SMSBody = ts.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.Win];
            }

            string Old_StationSMSBody = "";

            if (isWinSendStationSMS)
            {
                Old_StationSMSBody = ts.SiteNotificationTemplates[NotificationManners.StationSMS, NotificationTypes.Win];
            }

            if (((Old_EmailSubject == "") || (Old_EmailBody == "")) && (Old_SMSBody == "") && (Old_StationSMSBody == ""))
            {
                continue;
            }

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = dt.Rows[j];

                Users tu = new Users(ts.ID)[ts.ID, long.Parse(dr["UserID"].ToString())];

                if (tu == null)
                {
                    continue;
                }

                //Send Email
                if (isWinSendEmail && (Old_EmailSubject != "") && (Old_EmailBody != "") && (tu.Email != "") && (DAL.Functions.F_GetIsSendNotification(ts.ID, NotificationManners.Email, NotificationTypes.Win, tu.ID)))
                {
                    string EmailSubject = Old_EmailSubject.Replace("[UserName]", tu.Name);
                    string EmailBody = Old_EmailBody.Replace("[UserName]", tu.Name).
                                                    Replace("[SchemeID]", dr["SchemeID"].ToString()).
                                                    Replace("[Money]", double.Parse(dr["WinMoney"].ToString()).ToString("N"));

                    PF.SendEmail(ts, tu.Email, EmailSubject, EmailBody);
                }

                //Send SMS
                if (isWinSendSMS && (Old_StationSMSBody != "") && (tu.Mobile != "") && tu.isMobileValided && (DAL.Functions.F_GetIsSendNotification(ts.ID, NotificationManners.SMS, NotificationTypes.Win, tu.ID)))
                {
                    string Body = Old_StationSMSBody.Replace("[UserName]", tu.Name).
                                                Replace("[SchemeID]", dr["SchemeID"].ToString()).
                                                Replace("[Money]", double.Parse(dr["WinMoney"].ToString()).ToString("N"));

                    PF.SendSMS(ts, tu.ID, tu.Mobile, Body);
                }

                //Send StationSMS
                if (isWinSendStationSMS && (Old_StationSMSBody != "") && DAL.Functions.F_GetIsSendNotification(ts.ID, NotificationManners.StationSMS, NotificationTypes.Win, tu.ID))
                {
                    string Body = Old_StationSMSBody.Replace("[UserName]", tu.Name).
                        Replace("[SchemeID]", dr["SchemeID"].ToString()).
                        Replace("[Money]", double.Parse(dr["WinMoney"].ToString()).ToString("N"));

                    PF.SendStationSMS(ts, ts.AdministratorID, tu.ID, StationSMSTypes.SystemMessage, Body);
                }
            }
        }
    }

    public static void DataGridBindData(DataGrid g, DataTable dt)
    {
        g.DataSource = dt;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;

                //g.DataBind();
            }
            else
            {
                throw new Exception(e.Message);
            }
        }
    }

    public static void DataGridBindData(DataGrid g, DataTable dt, Shove.Web.UI.ShoveGridPager gPager)
    {
        g.DataSource = dt;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;
                gPager.PageIndex = 0;

                //g.DataBind();
            }
            else
            {
                throw new Exception(e.Message);
            }
        }

        gPager.Visible = (dt.Rows.Count > 0);
    }

    public static void DataGridBindData(DataGrid g, DataView dv, Shove.Web.UI.ShoveGridPager gPager)
    {
        g.DataSource = dv;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;
                gPager.PageIndex = 0;

                //g.DataBind();
            }
            else
            {
                throw new Exception(e.Message);
            }
        }

        gPager.Visible = (dv.Count > 0);
    }

    public static string FilterNoRestrictionsLottery(string LotteryListRestrictions, string LotteryList)
    {
        if ((LotteryListRestrictions == "") || (LotteryList == ""))
        {
            return "";
        }

        LotteryListRestrictions = "," + LotteryListRestrictions + ",";

        string[] strs = LotteryList.Split(',');

        if ((strs == null) || (strs.Length < 1))
        {
            return "";
        }

        string Result = "";

        foreach (string str in strs)
        {
            if (LotteryListRestrictions.IndexOf("," + str + ",") >= 0)
            {
                if (Result != "")
                {
                    Result += ",";
                }

                Result += str;
            }
        }

        return Result;
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

    //获取彩种的第二种类别ID
    public static int GetLotteryType2(int LotteryID)
    {
        int LotteryType2 = 0;

        object obj = Shove.Database.MSSQL.ExecuteScalar("Select [Type2] From T_Lotteries Where [ID] = @p1", new Shove.Database.MSSQL.Parameter("@p1", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID));

        if (obj == null)
        {
            return LotteryType2;
        }
        else
        {
            LotteryType2 = int.Parse(obj.ToString());
        }

        return LotteryType2;
    }

    #region 快乐扑克格式转换
    //系统格式转换成湖北快乐扑克期官方格式
    public static string ConvertIsuseName_HBKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-18", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002010 + Day * 49 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成山东快乐扑克期官方格式
    public static string ConvertIsuseName_SDKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成河北快乐扑克期官方格式
    public static string ConvertIsuseName_HeBKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成安徽快乐扑克期官方格式
    public static string ConvertIsuseName_AHKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002065 + Day * 48 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成黑龙江快乐扑克期官方格式
    public static string ConvertIsuseName_HLJKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成辽宁快乐扑克期官方格式
    public static string ConvertIsuseName_LLKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成陕西快乐扑克期官方格式
    public static string ConvertIsuseName_SXKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8001936 + Day * 49 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成浙江快乐扑克期官方格式
    public static string ConvertIsuseName_ZJKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002237 + Day * 52 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成四川快乐扑克期官方格式
    public static string ConvertIsuseName_SCKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002237 + Day * 52 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成山西快乐扑克期官方格式
    public static string ConvertIsuseName_ShXKLPK(string IsuseName)
    {
        return IsuseName;          //http://www.pinble.com网站暂时没有开奖数据
    }
    #endregion

    //系统格式转换成十一运夺金方格式
    public static string ConvertIsuseName_SYYDJ(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }

        return IsuseName.Substring(2);
    }

    //系统格式转换成江西时时彩期官方格式
    public static string ConvertIsuseName_JxSSC(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }

        return IsuseName.Substring(4).Insert(4, "0");
    }

    //系统格式转换成快赢481的格式
    public static string ConvertIsuseName_HNKY481(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2010-02-22", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (10003240 + Day * 72 + Time).ToString();  //09018200 是2009年12月8号的最后一期期号

        return Isuse;
    }

    #region     足彩单场显示信息转换

    public static DataTable GetZCDCBuyContent(string BuyNum, long SchemeID, ref string vote)
    {
        string lotteryNumber = Shove._Convert.ToHtmlCode(BuyNum);

        string BuyContent = "";

        bool IsGetScheme = new SLS.Lottery()[SLS.Lottery.ZCDC.sID].GetSchemeSplit(lotteryNumber, ref BuyContent, ref vote);

        string PlayType = lotteryNumber.Split(';')[0].ToString();

        string[] Team = BuyContent.Split('|');

        DataTable dtnew = new DataTable();
        DataColumn newDC;

        newDC = new DataColumn("No", System.Type.GetType("System.Int32"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("LeagueTypeName", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("HostTeam", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("QuestTeam", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("Content", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("MarkersColor", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("sp", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("LotteryResult", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("GamesResult", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        newDC = new DataColumn("HalftimeResult", System.Type.GetType("System.String"));
        dtnew.Columns.Add(newDC);

        for (int i = 0; i < Team.Length; i++)
        {
            DataTable dtp = GetTeamInfo(Team[i].Split('(')[0], SchemeID);

            if (dtp == null)
            {
                return null;
            }

            string TeamResult = Team[i].Split('(')[1].Substring(0, Team[i].Split('(')[1].Length - 1);

            string LotteryResult = "";
            string GamesResult = "";
            string HalftimeResult = "";
            string LetBall = "";
            string sp = "";
            if (PlayType == SLS.Lottery.ZCDC.PlayType_SPF.ToString())
            {
                TeamResult = TeamResult.Replace("0", "负").Replace("1", "平").Replace("3", "胜");

                if (dtp.Rows[0]["SPFResult"] != null)
                {
                    LotteryResult = dtp.Rows[0]["SPFResult"].ToString();
                    LotteryResult = LotteryResult.Replace("0", "负").Replace("1", "平").Replace("3", "胜");
                }

                if (dtp.Rows[0]["SPF_SP"] != null)
                {
                    sp = dtp.Rows[0]["SPF_SP"].ToString();
                }
            }

            if (PlayType == SLS.Lottery.ZCDC.PlayType_ZJQ.ToString())
            {
                TeamResult = TeamResult.Replace("7", "7+");

                if (dtp.Rows[0]["ZJQResult"] != null)
                {
                    LotteryResult = dtp.Rows[0]["ZJQResult"].ToString();
                    LotteryResult = LotteryResult.Replace("7", "7+");
                }

                if (dtp.Rows[0]["ZJQ_SP"] != null)
                {
                    sp = dtp.Rows[0]["ZJQ_SP"].ToString();
                }
            }

            if (PlayType == SLS.Lottery.ZCDC.PlayType_SXDS.ToString())
            {
                TeamResult = TeamResult.Replace("1", "上-单").Replace("2", "上-双").Replace("3", "下-单").Replace("4", "下-双");

                if (dtp.Rows[0]["SXDSResult"] != null)
                {
                    LotteryResult = dtp.Rows[0]["SXDSResult"].ToString();
                    LotteryResult = LotteryResult.Replace("1", "上-单").Replace("2", "上-双").Replace("3", "下-单").Replace("4", "下-双");
                }

                if (dtp.Rows[0]["SXDS_SP"] != null)
                {
                    sp = dtp.Rows[0]["SXDS_SP"].ToString();
                }
            }

            string SaleTeam = "";
            string SaleTeamResult = "";

            if (PlayType == SLS.Lottery.ZCDC.PlayType_ZQBF.ToString())
            {
                string[] Teams = TeamResult.Split(',');

                for (int j = 0; j < Teams.Length; j++)
                {
                    if (Teams[j].Length > 1)
                    {
                        SaleTeam += Teams[j].Replace("25", "负其他").Replace("24", "2:4").Replace("23", "1:4").Replace("22", "0:4").Replace("21", "2:3").Replace("20", "1:3").Replace("19", "0:3").Replace("18", "1:2").Replace("17", "0:2").Replace("16", "0:1").Replace("15", "平其他").Replace("14", "3:3").Replace("13", "2:2").Replace("12", "1:1").Replace("11", "0:0").Replace("10", "胜其他");
                    }
                    else
                    {
                        SaleTeam += Teams[j].Replace("1", "1:0").Replace("2", "2:0").Replace("3", "2:1").Replace("4", "3:0").Replace("5", "3:1").Replace("6", "3:2").Replace("7", "4:0").Replace("8", "4:1").Replace("9", "4:2");

                    }

                    SaleTeam += ",";
                }

                TeamResult = SaleTeam.Substring(0, SaleTeam.Length - 1);

                if (dtp.Rows[0]["ZQBFResult"] != null)
                {
                    string[] TeamsResult = dtp.Rows[0]["ZQBFResult"].ToString().Split(',');

                    for (int j = 0; j < TeamsResult.Length; j++)
                    {

                        if (TeamsResult[j].Length > 1)
                        {
                            SaleTeamResult += TeamsResult[j].Replace("25", "负其他").Replace("24", "2:4").Replace("23", "1:4").Replace("22", "0:4").Replace("21", "2:3").Replace("20", "1:3").Replace("19", "0:3").Replace("18", "1:2").Replace("17", "0:2").Replace("16", "0:1").Replace("15", "平其他").Replace("14", "3:3").Replace("13", "2:2").Replace("12", "1:1").Replace("11", "0:0").Replace("10", "胜其他");
                        }
                        else
                        {
                            SaleTeamResult += TeamsResult[j].Replace("1", "1:0").Replace("2", "2:0").Replace("3", "2:1").Replace("4", "3:0").Replace("5", "3:1").Replace("6", "3:2").Replace("7", "4:0").Replace("8", "4:1").Replace("9", "4:2");

                        }

                        SaleTeamResult += ",";
                    }

                    LotteryResult = SaleTeamResult.Substring(0, SaleTeamResult.Length - 1);
                }


                if (dtp.Rows[0]["ZQBF_SP"] != null)
                {
                    sp = dtp.Rows[0]["ZQBF_SP"].ToString();
                }

            }

            if (PlayType == SLS.Lottery.ZCDC.PlayType_BQCSPF.ToString())
            {
                TeamResult = TeamResult.Replace("1", "胜-胜").Replace("2", "胜-平").Replace("3", "胜-负").Replace("4", "平-胜").Replace("5", "平-平").Replace("6", "平-负").Replace("7", "负-胜").Replace("8", "负-平 ").Replace("9", "负-负");

                if (dtp.Rows[0]["BQCSPFResult"] != null)
                {
                    LotteryResult = dtp.Rows[0]["BQCSPFResult"].ToString();
                    LotteryResult = LotteryResult.Replace("1", "胜-胜").Replace("2", "胜-平").Replace("3", "胜-负").Replace("4", "平-胜").Replace("5", "平-平").Replace("6", "平-负").Replace("7", "负-胜").Replace("8", "负-平 ").Replace("9", "负-负");
                }

                if (dtp.Rows[0]["BQCSPF_SP"] != null)
                {
                    sp = dtp.Rows[0]["BQCSPF_SP"].ToString();
                }
            }

            if (LotteryResult == "*")
            {
                LotteryResult = "延时";
                sp = "1";
            }

            if (dtp.Rows[0]["Result"] != null)
            {
                GamesResult = dtp.Rows[0]["Result"].ToString();
            }

            if (dtp.Rows[0]["HalftimeResult"] != null)
            {
                HalftimeResult = dtp.Rows[0]["HalftimeResult"].ToString();
            }

            if (dtp.Rows[0]["LetBall"] != null)
            {
                LetBall = dtp.Rows[0]["LetBall"].ToString();
            }

            if (LetBall == "0")
            {
                LetBall = "";
            }
            else
            {
                LetBall = "[" + LetBall + "]";
            }

            DataRow dr1 = dtnew.NewRow();
            dr1[0] = dtp.Rows[0]["No"].ToString();
            dr1[1] = dtp.Rows[0]["LeagueTypeName"].ToString();
            dr1[2] = dtp.Rows[0]["HostTeam"].ToString() + LetBall;
            dr1[3] = dtp.Rows[0]["QuestTeam"].ToString();
            dr1[4] = TeamResult;
            dr1[5] = dtp.Rows[0]["MarkersColor"].ToString();
            dr1[6] = sp;
            dr1[7] = LotteryResult;
            dr1[8] = GamesResult;
            dr1[9] = HalftimeResult;
            dtnew.Rows.Add(dr1);
        }

        return dtnew;
    }

    public static DataTable GetTeamInfo(string No, long SchemeID)
    {
        DataTable dt = new DAL.Views.V_SchemeForZCDC().Open("*", "[SchemesID] = " + SchemeID.ToString() + " and No = " + No, "");

        return dt;
    }

    #endregion

    //格式化中奖号码
    public static void FormatNumber(int LotteryID, string LastWinNumber, out string[] RedNumber, out string[] OrangeNumber, out string[] BlueNumber)
    {
        //若有两个空格，替换成一个空格
        LastWinNumber = LastWinNumber.Replace("  ", " ");

        if (LotteryID == 5) //双色球(10 11 15 16 25 29 + 02)
        {
            try
            {
                RedNumber = LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                RedNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 6 || LotteryID == 29)//福彩3D(813) //时时乐(812)
        {
            try
            {
                string Number = LastWinNumber.Trim();
                BlueNumber = new string[Number.Length];

                for (int i = 0; i < BlueNumber.Length; i++)
                {
                    BlueNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];
            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 13) //七乐彩
        {
            try
            {
                OrangeNumber = LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                OrangeNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];

            return;
        }

        if (LotteryID == 59) //15X5
        {
            try
            {
                RedNumber = LastWinNumber.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                RedNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 58)//东方6+1(451069+鼠)
        {
            try
            {
                string Number = LastWinNumber.Split('+')[0];
                OrangeNumber = new string[Number.Length];

                for (int i = 0; i < OrangeNumber.Length; i++)
                {
                    OrangeNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                OrangeNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split('+')[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];

            return;
        }

        if (LotteryID == 60 || LotteryID == 61)//天天彩选4(3204)
        {
            try
            {
                string Number = LastWinNumber.Trim();
                RedNumber = new string[Number.Length];

                for (int i = 0; i < RedNumber.Length; i++)
                {
                    RedNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                RedNumber = new string[0];
            }

            OrangeNumber = new string[0];
            BlueNumber = new string[0];

            return;
        }

        RedNumber = new string[0];
        OrangeNumber = new string[0];
        BlueNumber = new string[0];
    }

    /// <summary>
    /// 用户名合法校验
    /// 1,只能以汉字,大小写字母,数字,下划线开头开头 
    /// 2,不能有其他特殊字符 
    /// 3,长度在2--30位
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static bool CheckUserName(string userName)
    {
        Regex regex = new Regex(@"^[\u4e00-\u9fa5a-zA-Z0-9_]{1}[\w]{1,29}$|^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(userName);

        if (m.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// UBB代码处理函数
    /// </summary>
    ///	<param name="_postpramsinfo">UBB转换参数表</param>
    /// <returns>输出字符串</returns>
    public static string UBBToHTML(string sDetail)
    {
        RegexOptions options = RegexOptions.IgnoreCase;
        StringBuilder sb = new StringBuilder();

        // [smilie]处标记
        sDetail = Regex.Replace(sDetail, @"\[smilie\](.+?)\[\/smilie\]", "<img src=\"$1\" />", options);

        sDetail = Regex.Replace(sDetail, @"\[b(?:\s*)\]", "<b>", options);
        sDetail = Regex.Replace(sDetail, @"\[i(?:\s*)\]", "<i>", options);
        sDetail = Regex.Replace(sDetail, @"\[u(?:\s*)\]", "<u>", options);
        sDetail = Regex.Replace(sDetail, @"\[/b(?:\s*)\]", "</b>", options);
        sDetail = Regex.Replace(sDetail, @"\[/i(?:\s*)\]", "</i>", options);
        sDetail = Regex.Replace(sDetail, @"\[/u(?:\s*)\]", "</u>", options);

        // Sub/Sup
        //
        sDetail = Regex.Replace(sDetail, @"\[sup(?:\s*)\]", "<sup>", options);
        sDetail = Regex.Replace(sDetail, @"\[sub(?:\s*)\]", "<sub>", options);
        sDetail = Regex.Replace(sDetail, @"\[/sup(?:\s*)\]", "</sup>", options);
        sDetail = Regex.Replace(sDetail, @"\[/sub(?:\s*)\]", "</sub>", options);

        // P
        //
        sDetail = Regex.Replace(sDetail, @"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", "<p>$3</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);




        // Anchors
        //
        sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\](www\.(.*?))\[/url(?:\s*)\]", "<a href=\"http://$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\]\s*((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent)([^\[""']+?))\s*\[\/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url=www.([^\[""']+?)(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"http://www.$1\" target=\"_blank\">$2</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url=((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent://)([^\[""']+?))(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$4</a>", options);

        // Email
        //
        sDetail = Regex.Replace(sDetail, @"\[email(?:\s*)\](.*?)\[\/email\]", "<a href=\"mailto:$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[email=(.[^\[]*)(?:\s*)\](.*?)\[\/email(?:\s*)\]", "<a href=\"mailto:$1\" target=\"_blank\">$2</a>", options);

        #region

        // Font
        //
        sDetail = Regex.Replace(sDetail, @"\[color=([^\[\<]+?)\]", "<font color=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[size=(\d+?)\]", "<font size=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[size=(\d+(\.\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\]", "<font style=\"font-size: $1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[font=([^\[\<]+?)\]", "<font face=\"$1\">", options);

        sDetail = Regex.Replace(sDetail, @"\[align=([^\[\<]+?)\]", "<p align=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[float=(left|right)\]", "<br style=\"clear: both\"><span style=\"float: $1;\">", options);


        sDetail = Regex.Replace(sDetail, @"\[/color(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/size(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/font(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/align(?:\s*)\]", "</p>", options);
        sDetail = Regex.Replace(sDetail, @"\[/float(?:\s*)\]", "</span>", options);

        // BlockQuote
        //
        sDetail = Regex.Replace(sDetail, @"\[indent(?:\s*)\]", "<blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[/indent(?:\s*)\]", "</blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[simpletag(?:\s*)\]", "<blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[/simpletag(?:\s*)\]", "</blockquote>", options);

        // List
        //
        sDetail = Regex.Replace(sDetail, @"\[list\]", "<ul>", options);
        sDetail = Regex.Replace(sDetail, @"\[list=(1|A|a|I|i| )\]", "<ul type=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[\*\]", "<li>", options);
        sDetail = Regex.Replace(sDetail, @"\[/list\]", "</ul>", options);


        #endregion

        // shadow
        //
        sDetail = Regex.Replace(sDetail, @"(\[SHADOW=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/SHADOW\])", "<table width='$2'  style='filter:SHADOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

        // glow
        //
        sDetail = Regex.Replace(sDetail, @"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/glow\])", "<table width='$2'  style='filter:GLOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

        // center
        //
        sDetail = Regex.Replace(sDetail, @"\[center\]([\s]||[\s\S]+?)\[\/center\]", "<center>$1</center>", options);


        #region 处理[quote][/quote]标记

        int intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]");
        int quotecount = 0;
        while (intQuoteIndexOf >= 0 && sDetail.ToLower().IndexOf("[/quote]") >= 0 && quotecount < 3)
        {
            quotecount++;
            sDetail = Regex.Replace(sDetail, @"\[quote\]([\s\S]+?)\[/quote\]", "<br /><br /><div class=\"msgheader\">引用:</div><div class=\"msgborder\">$1</div>", options);


            intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]", intQuoteIndexOf + 7);
        }
        //sDetail = sDetail.Replace("[quote]", string.Empty).Replace("[/quote]", string.Empty);

        #endregion


        //处理[area]标签
        sDetail = Regex.Replace(sDetail, @"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]", "<br /><br /><div class=\"msgheader\">$1</div><div class=\"msgborder\">$2</div>", options);

        sDetail = Regex.Replace(sDetail, @"\[area\]([\s\S]+?)\[/area\]", "<br /><br /><div class=\"msgheader\"></div><div class=\"msgborder\">$1</div>", options);



        #region 将网址字符串转换为链接

        sDetail = sDetail.Replace("&amp;", "&");

        // p2p link
        sDetail = Regex.Replace(sDetail, @"^((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)$", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"[^>=\]""]((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        #endregion


        #region 处理[img][/img]标记

        sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$1\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[image\](http://[\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$1\" border=\"0\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this)\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this)\"  />", options);
        sDetail = Regex.Replace(sDetail, @"\[image\](http://[\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);

        #endregion

        #region 处理空格

        sDetail = sDetail.Replace("\t", "&nbsp; &nbsp; ");
        sDetail = sDetail.Replace("  ", "&nbsp; ");

        #endregion

        // [r/]
        //
        sDetail = Regex.Replace(sDetail, @"\[r/\]", "\r", options);

        // [n/]
        //
        sDetail = Regex.Replace(sDetail, @"\[n/\]", "\n", options);
        //
        //			// 换行
        //			//
        //			sDetail = Regex.Replace(sDetail, @"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", "<br />　　$正文", options);


        #region 处理换行

        //处理换行,在每个新行的前面添加两个全角空格
        sDetail = sDetail.Replace("\r\n", "<br />");
        sDetail = sDetail.Replace("\r", "");
        sDetail = sDetail.Replace("\n\n", "<br /><br />");
        sDetail = sDetail.Replace("\n", "<br />");
        sDetail = sDetail.Replace("{rn}", "\r\n");
        sDetail = sDetail.Replace("{nn}", "\n\n");
        sDetail = sDetail.Replace("{r}", "\r");
        sDetail = sDetail.Replace("{n}", "\n");

        #endregion

        return sDetail;
    }

    /// <summary>
    /// 获取盈利指数 
    /// 盈利指数：每次盈利金额对应的分数称为盈利指数
    /// 盈利金额=中奖金额-方案金额 
    /// </summary>
    /// <param name="profitMoney">盈利金额</param>
    /// <returns>盈利指数</returns>
    public static int GetProfitPoints(double profitMoney)
    {
        int profitPoints = 0;
        if (profitMoney >= 1 && profitMoney <= 20)
        {
            profitMoney = 1;
        }
        else if (profitMoney >= 21 && profitMoney <= 50)
        {
            profitMoney = 2;
        }
        else if (profitMoney >= 51 && profitMoney <= 100)
        {
            profitMoney = 5;
        }
        else if (profitMoney >= 101 && profitMoney <= 200)
        {
            profitMoney = 10;
        }
        else if (profitMoney >= 201 && profitMoney <= 500)
        {
            profitMoney = 20;
        }
        else if (profitMoney >= 501 && profitMoney <= 1000)
        {
            profitMoney = 40;
        }
        else if (profitMoney >= 1001 && profitMoney <= 2000)
        {
            profitMoney = 80;
        }
        else if (profitMoney >= 2001 && profitMoney <= 5000)
        {
            profitMoney = 160;
        }
        else if (profitMoney >= 5001 && profitMoney <= 10000)
        {
            profitMoney = 320;
        }
        else if (profitMoney >= 10001 && profitMoney <= 20000)
        {
            profitMoney = 640;
        }
        else if (profitMoney >= 20001 && profitMoney <= 50000)
        {
            profitMoney = 1280;
        }
        else if (profitMoney >= 50001 && profitMoney <= 100000)
        {
            profitMoney = 2560;
        }
        else if (profitMoney >= 100001 && profitMoney <= 500000)
        {
            profitMoney = 5120;
        }
        else if (profitMoney >= 500001 && profitMoney <= 1000000)
        {
            profitMoney = 10240;
        }
        else if (profitMoney >= 1000001 && profitMoney <= 2000000)
        {
            profitMoney = 20480;
        }
        else if (profitMoney >= 2000001 && profitMoney <= 5000000)
        {
            profitMoney = 40960;
        }
        else if (profitMoney >= 5000001)
        {
            profitMoney = 81920;
        }
        return profitPoints;
    }

    /// <summary>
    /// 根据某列的值 查询某个表中该列的总值
    /// </summary>
    /// <param name="dt">要计算的表格</param>
    /// <param name="index">列所在的索引 从0开始</param>
    /// <param name="isPage">是否是统计本页数据</param>
    /// <param name="pageSize">每页记录数</param>
    /// <param name="pageIndex">统计的 dt 的当前页索引</param>
    /// <returns></returns>
    public static double GetSumByColumn(DataTable dt, int index, bool isPage, int pageSize, int pageIndex)
    {
        double sum = 0;

        int columnLength = dt.Columns.Count;
        if (index >= columnLength || index < 0)
        {
            return sum;
        }
        else
        {
            int rowLength = dt.Rows.Count;
            if (!isPage)    //统计总数据
            {
                for (int i = 0; i < rowLength; i++)
                {
                    sum += Shove._Convert.StrToDouble(dt.Rows[i][index].ToString(), 0);
                }
            }
            else          //统计当前页数据
            {
                rowLength = (pageIndex + 1) * pageSize;
                if (rowLength > dt.Rows.Count)
                {
                    rowLength = (pageIndex) * pageSize + (dt.Rows.Count % pageSize);
                }
                for (int i = pageIndex * pageSize; i < rowLength; i++)
                {
                    sum += Shove._Convert.StrToDouble(dt.Rows[i][index].ToString(), 0);
                }
            }
        }
        return sum;
    }

   
    /// <summary>
    /// 指定的提款方式,计算所需的提款手续费
    /// </summary>
    /// <param name="DistillType">提款方式: 1 支付宝提款, 2.银行卡提款</param>
    /// <param name="DistillMoney">提款金额</param>
    /// <param name="BankDetailedName">如果是 银行卡提款 要提供具体的银行名称,如:广东省深圳市中国工商银行宝安支行</param>
    /// <returns></returns>
    /*  工商银行  工行本地不收，异地最低1.8元，最高45元，200~5000按照0.9%收取  
        招商银行  招行本地不收，异地最低5元，最高50元，2500~25000按照0.2%收取  
        建设银行  建行本地不收，异地最低2元，最高25元，800~10000以下按照0.25%收取  
        其它银行  深圳本地2元/笔，异地最低10元/笔，最高50元/笔，1000~5000元按照1%收取  
        支付宝账户  实时到帐  5000元以内的1块钱，5000元以上每笔10元手续费。
    */  

    public static double CalculateDistillFormalitiesFees(int DistillType, double DistillMoney, string BankDetailedName)
    {
        double formalitiesFees = 0;
        if (DistillType == 1)//支付宝提款
        {
            if (DistillMoney <10000)
            {
                formalitiesFees = 1;
            }
            else
            {
                formalitiesFees = 10;
            }
        }
        else if (DistillType == 2)//银行卡提款
        {
            if (BankDetailedName.Replace("深圳发展","").IndexOf("深圳") >= 0) //本地:银行卡
            {
                if (BankDetailedName.IndexOf("工商银行") >= 0 || BankDetailedName.IndexOf("招商银行") >= 0 || BankDetailedName.IndexOf("建设银行") >= 0
                    || BankDetailedName.IndexOf("工行") >= 0 || BankDetailedName.IndexOf("招行") >= 0 || BankDetailedName.IndexOf("建行") >= 0)
                {
                    formalitiesFees = 0;
                }
                else
                {
                    formalitiesFees = 2;
                }
            }
            else //异地:银行卡
            {
                if (BankDetailedName.IndexOf("工商银行") >= 0 || BankDetailedName.IndexOf("工行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.009;
                    if (formalitiesFees < 1.8)
                    {
                        formalitiesFees = 1.8;//最低1.8元
                    }
                    else if (formalitiesFees > 45)
                    {
                        formalitiesFees = 45;//最高45元
                    }
                }
                else if (BankDetailedName.IndexOf("招商银行") >= 0 || BankDetailedName.IndexOf("招行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.002;
                    if (formalitiesFees < 5)
                    {
                        formalitiesFees = 5;//最低5元
                    }
                    else if (formalitiesFees > 50)
                    {
                        formalitiesFees = 50;//最高50元
                    }
                }
                else if (BankDetailedName.IndexOf("建设银行") >= 0 || BankDetailedName.IndexOf("建行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.0025;// 按0.25%
                    if (formalitiesFees < 2)
                    {
                        formalitiesFees = 2;//最低5元
                    }
                    else if (formalitiesFees > 25)
                    {
                        formalitiesFees = 25;//最高25元
                    }
                }
                else //其他银行异地
                {
                    formalitiesFees = DistillMoney * 0.01; // 按1%
                    if (formalitiesFees < 10)
                    {
                        formalitiesFees = 10;//最低10元
                    }
                    else if (formalitiesFees > 50)
                    {
                        formalitiesFees = 50;//最高50元
                    }
                }
            }
        }


        return formalitiesFees;
    }
}

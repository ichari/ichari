using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Text.RegularExpressions;

using System.Linq;

using Shove.Database;
using Shove.Alipay;
using System.Text;
using System.Data.SqlClient;


public partial class ElectronTicket_ZCW_Receive : System.Web.UI.Page
{
    private const int TimeoutSeconds = 100;

    private bool ElectronTicket_ZCW_Status_ON;
    private string ElectronTicket_ZCW_Getway;
    private string ElectronTicket_ZCW_UserName;
    private string ElectronTicket_ZCW_UserPassword;
    private string ElectronTicket_ZCW_IP;

    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        ElectronTicket_ZCW_Status_ON = so["ElectronTicket_ZCW_Status_ON"].ToBoolean(false);
        ElectronTicket_ZCW_Getway = so["ElectronTicket_ZCW_Getway"].ToString("");
        ElectronTicket_ZCW_UserName = so["ElectronTicket_ZCW_UserName"].ToString("");
        ElectronTicket_ZCW_UserPassword = so["ElectronTicket_ZCW_UserPassword"].ToString("");
        ElectronTicket_ZCW_IP = so["ElectronTicket_ZCW_IP"].ToString("").Trim();

        if (!ElectronTicket_ZCW_Status_ON)
        {
            this.Response.End();

            return;
        }

        if (!this.IsPostBack)
        {
            if (!String.IsNullOrEmpty(ElectronTicket_ZCW_IP))
            {
                ElectronTicket_ZCW_IP = "," + ElectronTicket_ZCW_IP + ",";

                if (ElectronTicket_ZCW_IP.IndexOf("," + GetClientIPAddress() + ",") < 0)
                {
                    new Log("ElectronTicket\\ZCW").Write("电子票异常客户端 IP 请求。" + GetClientIPAddress());

                    this.Response.End();

                    return;
                }
            }

            string request;
            using (Stream MyStream = Request.InputStream)
            {
                byte[] _tmpData = new byte[MyStream.Length];
                MyStream.Read(_tmpData, 0, _tmpData.Length);
                request = Encoding.UTF8.GetString(_tmpData);
            }

            string transactiontype = "";

            new Log("ElectronTicket\\ZCW").Write(request);

            if (!ValidMessage(request, ref transactiontype))
            {
                new Log("ElectronTicket\\ZCW").Write("校验码错误：" + request);

                Response.Write("校验码错误");
                Response.End();

                return;
            }

            Receive(transactiontype, request);
        }
    }

    private void Receive(string TransType, string TransMessage)
    {
        switch (TransType)
        {
            case "13008":
                IsuseNotice(TransMessage);              // 接收奖期通知
                break;
            case "13006":
                TicketInfo(TransMessage);              // 票投注信息返回
                break;
            default:
                break;
        }
    }

    // 接收奖期通知
    private void IsuseNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));
        }
        catch { }

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        string Body = TransMessage.Substring(TransMessage.IndexOf("<elements"), TransMessage.LastIndexOf("</elements>") - TransMessage.IndexOf("<elements")) + "</elements>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(Body));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count < 1)
        {
            return;
        }

        string MessageID = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];

            string LotteryName = dr["lotteryid"].ToString();
            int LotteryID = GetLotteryID(LotteryName);
            string IsuseName = GetIsusesNameToCaiyou(LotteryID.ToString(), dr["issue"].ToString());
            string Status = dr["status"].ToString();
            string StartTime = dr["starttime"].ToString();
            string EndTime = dr["endtime"].ToString();
            string WinNumber = "";

            if (Shove._Convert.StrToInt(Status, 0) > 1)
            {
                try
                {
                    WinNumber = GetWinNumber(LotteryID, dr["bonuscode"].ToString());
                }
                catch { }
            }

            if ((LotteryID < 0) || (String.IsNullOrEmpty(IsuseName)))
            {
                continue;
            }

            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IsuseName) + "' and year(StartTime) = YEAR(GETDATE())") < 1)
            {
                DateTime _StartTime = DateTime.Now;
                DateTime _EndTime = DateTime.Now;

                try
                {
                    _StartTime = DateTime.Parse(StartTime);
                    _EndTime = DateTime.Parse(EndTime);
                }
                catch
                {
                    continue;
                }

                long IsuseID = -1;
                string ReturnDescription = "";

                if (DAL.Procedures.P_IsuseAdd(LotteryID, IsuseName, _StartTime, _EndTime, "", ref IsuseID, ref ReturnDescription) < 0)
                {
                    continue;
                }

                if (IsuseID < 0)
                {
                    continue;
                }
            }

            DataTable dtIsuse = t_Isuses.Open("ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IsuseName) + "' and year(StartTime) = YEAR(GETDATE())", "");

            if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
            {
                continue;
            }

            if (Status == "1")
            {
                int ReturnValue = 0;
                string ReturnDescprtion = "";

                if (DAL.Procedures.P_ElectronTicketAgentSchemeQuash(Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), 0), ref ReturnValue, ref ReturnDescprtion) < 0)
                {
                    new Log("ElectronTicket\\ZCW").Write("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash");

                    continue;
                }
            }

            bool isHasUpdate = false;

            if (dtIsuse.Rows[0]["State"].ToString() != Status)
            {
                isHasUpdate = true;
            }

            if (isHasUpdate)
            {
                int ReturnValue = 0;
                string ReturnDescprtion = "";

                if (DAL.Procedures.P_IsuseUpdate(LotteryID, Shove._Web.Utility.FilteSqlInfusion(IsuseName), Shove._Convert.StrToShort(Status, 0), Shove._Convert.StrToDateTime(StartTime, DateTime.Now.ToString()), Shove._Convert.StrToDateTime(EndTime, DateTime.Now.ToString()), DateTime.Now, WinNumber, ref ReturnValue, ref ReturnDescprtion) < 0)
                {
                    new Log("ElectronTicket\\ZCW").Write("电子票撤销追号错误P_IsuseEdit。");
                }

                if (ReturnValue < 0)
                {
                    new Log("ElectronTicket\\ZCW").Write(ReturnDescprtion);
                }
            }

            if (!String.IsNullOrEmpty(WinNumber) && (dtIsuse.Rows[0]["WinLotteryNumber"].ToString() != WinNumber))
            {
                DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open("", "LotteryID =" + LotteryID.ToString(), "");

                double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];

                for (int k = 0; k < dtWinTypes.Rows.Count; k++)
                {
                    if (Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoney"].ToString(), 1) < 1)
                    {
                        MessageID = GetFromXPath(TransMessage, "message/header/messengerid");
                        ReNotice(MessageID, "13008");

                        return;
                    }

                    WinMoneyList[k * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoney"].ToString(), 1);
                    WinMoneyList[k * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoneyNoWithTax"].ToString(), 1);
                }

                #region 开奖第一步

                DataTable dtWin = null;

                dtWin = new DAL.Tables.T_Schemes().Open("LotteryNumber,PlayTypeID,Multiple,ID", "isOpened = 0 and IsuseID = " + dtIsuse.Rows[0]["ID"].ToString(), "[ID]");

                if (dtWin == null)
                {
                    return;
                }

                for (int y = 0; y < dtWin.Rows.Count; y++)
                {
                    string LotteryNumber = "";

                    try
                    {
                        LotteryNumber = dtWin.Rows[y]["LotteryNumber"].ToString();
                    }
                    catch
                    { }

                    string Description = "";
                    double WinMoneyNoWithTax = 0;

                    try
                    {

                        double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtWin.Rows[y]["PlayTypeID"].ToString()), WinMoneyList);

                        Shove.Database.MSSQL.ExecuteNonQuery("update T_Schemes set PreWinMoney = @p1, PreWinMoneyNoWithTax = @p2, EditWinMoney = @p3, EditWinMoneyNoWithTax = @p4, WinDescription = @p5 where [ID] = " + dtWin.Rows[y]["ID"].ToString(),
                            new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                            new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                            new Shove.Database.MSSQL.Parameter("p3", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                            new Shove.Database.MSSQL.Parameter("p4", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtWin.Rows[y]["Multiple"].ToString(), 1)),
                            new Shove.Database.MSSQL.Parameter("p5", SqlDbType.VarChar, 0, ParameterDirection.Input, Description));
                    }
                    catch
                    {
                        continue;
                    }
                }

                #endregion

                #region 开奖第二步

                string OpenAffiche = new OpenAfficheTemplates()[LotteryID];

                int SchemeCount, QuashCount, WinCount, WinNoBuyCount;
                bool isEndOpen = false;

                int ReturnValue = 0;
                string ReturnDescription = "";

                while (!isEndOpen)
                {
                    SchemeCount = 0;
                    QuashCount = 0;
                    WinCount = 0;
                    WinNoBuyCount = 0;
                    //  总方案数，处理时撤单数，中奖数，中奖但未成功数

                    ReturnValue = 0;
                    ReturnDescription = "";
                    DataSet dsWin = null;

                    DAL.Procedures.P_Win(ref dsWin,
                         Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), 0),
                         WinNumber,
                         OpenAffiche,
                         1,
                         true,
                         ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount,
                         ref isEndOpen,
                         ref ReturnValue, ref ReturnDescription);
                }

                #endregion
            }

        }

        MessageID = GetFromXPath(TransMessage, "message/header/messengerid");
        ReNotice(MessageID, "13008");
    }

    //票投注信息返回
    private void TicketInfo(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));
        }
        catch { }

        string Body = TransMessage.Substring(TransMessage.IndexOf("<elements"), TransMessage.LastIndexOf("</elements>") - TransMessage.IndexOf("<elements")) + "</elements>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(Body));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count < 1)
        {
            return;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];
            string TicketID = dr["id"].ToString();
            string Status = dr["errorcode"].ToString();

            int ReturnValue = 0;
            string ReturnDescription = "";

            int Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DateTime.Now, true, Status, ref ReturnValue, ref ReturnDescription);

            if ((Result < 0) || (ReturnValue < 0))
            {
                new Log("ElectronTicket\\ZCW").Write("对返回电子票信息第一次处理出错_P_SchemesSendToCenterHandle，票ID：" + TicketID + ", 错误代码:" + Result.ToString() + ", 错误原因:" + ReturnValue);

                System.Threading.Thread.Sleep(1000);

                ReturnValue = 0;
                ReturnDescription = "";

                Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DateTime.Now, true, Status, ref ReturnValue, ref ReturnDescription);

                if ((Result < 0) || (ReturnValue < 0))
                {
                    new Log("ElectronTicket\\ZCW").Write("对返回电子票信息第二次处理出错_P_SchemesSendToCenterHandle，票ID：" + TicketID + ", 错误代码:" + Result.ToString() + ", 错误原因:" + ReturnValue);

                    continue;
                }
            }
        }

        string MessageID = GetFromXPath(TransMessage, "message/header/messengerid");
        ReNotice(MessageID, "13006");
    }

    // 对中彩网返回的信息进行验证
    private bool ValidMessage(string TransMessage, ref string transactiontype)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));

            nodes = XmlDoc.GetElementsByTagName("*");
        }
        catch { }

        if (nodes == null)
        {
            return false;
        }

        string Digest = "";
        string Body = "";
        transactiontype = "";

        for (int j = 0; j < nodes.Count; j++)
        {
            if (nodes[j].Name.ToUpper() == "AGENTER")
            {
                if (ElectronTicket_ZCW_UserName != nodes[j].InnerText)
                {
                    return false;
                }
            }

            if (nodes[j].Name.ToUpper() == "DIGEST")
            {
                Digest = nodes[j].InnerText;
            }

            if (nodes[j].Name.ToUpper() == "TRANSACTIONTYPE")
            {
                transactiontype = nodes[j].InnerText;
            }
        }

        Body = GetBody(TransMessage);

        return (Digest.ToLower() == Shove._Security.Encrypt.MD5(ElectronTicket_ZCW_UserPassword + Body, "utf-8").ToLower());
    }

    // 返回成功信息
    private void ReNotice(string MessageID, string Type)
    {
        DateTime Now = DateTime.Now;

        string Body = "<body><oelement><errorcode>0</errorcode><errormsg>成功，系统处理正</errormsg></oelement></body>";

        string TimeStamp = Now.ToString("yyyyMMdd") + Now.ToString("HHmmss");

        string Message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        Message += "<message version=\"1.0\">";
        Message += "<header>";
        Message += "<messengerID>" + MessageID + "</messengerID>";
        Message += "<timestamp>" + TimeStamp + "</timestamp>";
        Message += "<transactiontype>" + Type + "</transactiontype>";
        Message += "<digest>" + Shove._Security.Encrypt.MD5(ElectronTicket_ZCW_UserPassword + Body, "utf-8") + "</digest>";
        Message += "</header>";
        Message += Body;
        Message += "</message>";

        Response.Write(Message);

        WriteElectronTicketLog(true, Type, "transType=" + Type + "&transMessage=" + Message);
        Response.End();
    }

    #region 公用方法

    private string GetFromXPath(string XML, string XPath)
    {
        if (XML.Trim() == "")
            return "";

        string Result = "";
        try
        {
            XPathDocument doc = new XPathDocument(new StringReader(XML));
            XPathNavigator nav = doc.CreateNavigator();
            XPathNodeIterator nodes = nav.Select(XPath);
            while (nodes.MoveNext())
                Result += nodes.Current.Value;
        }
        catch//(Exception ee)
        {
            return "";
            //return ee.Message;
        }

        return Result;
    }

    private string GetBody(string XML)
    {
        Regex regex = new Regex(@"(?<body><Body[\S\s]*?>[\S\s]*?</Body>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(XML);
        return m.Groups["body"].Value;
    }

    private string GetLotteryName(int LotteryID)
    {
        switch (LotteryID)
        {
            case 2:
                return "111";
            case 3:
                return "103";
            case 5:
                return "118";
            case 6:
                return "116";
            case 9:
                return "104";
            case 13:
                return "117";
            case 15:
                return "110";
            case 39:
                return "106";
            case 62:
                return "112";
            case 63:
                return "100";
            case 64:
                return "102";
            case 65:
                return "105";
            case 70:
                return "113";
            case 74:
                return "108";
            case 75:
                return "109";
            default:
                return "";
        }
    }

    private int GetLotteryID(string LotteryName)
    {
        switch (LotteryName)
        {
            case "111":
                return 2;
            case "103":
                return 3;
            case "118":
                return 5;
            case "116":
                return 6;
            case "104":
                return 9;
            case "117":
                return 13;
            case "110":
                return 15;
            case "106":
                return 39;
            case "112":
                return 62;
            case "100":
                return 63;
            case "102":
                return 64;
            case "105":
                return 65;
            case "113":
                return 70;
            case "108":
                return 74;
            case "109":
                return 75;
            default:
                return -1;
        }
    }

    private string GetIsusesNameToCaiyou(string LotteryID, string IsusesName)
    {
        switch (LotteryID)
        {
            case "74":
                return "20" + IsusesName;
            case "75":
                return "20" + IsusesName;
            case "2":
                return "20" + IsusesName;
            case "15":
                return "20" + IsusesName;
            case "39":
                return "20" + IsusesName;
            case "3":
                return "20" + IsusesName;
            case "63":
                return "20" + IsusesName;
            case "64":
                return "20" + IsusesName;
            case "9":
                return "20" + IsusesName;
            case "65":
                return "20" + IsusesName;
            default:
                return IsusesName;
        }
    }

    private string GetIsusesNameToZcw(string LotteryID, string IsusesName)
    {
        switch (LotteryID)
        {
            case "74":
                return IsusesName.Substring(2);
            case "75":
                return IsusesName.Substring(2);
            case "2":
                return IsusesName.Substring(2);
            case "15":
                return IsusesName.Substring(2);
            case "39":
                return IsusesName.Substring(2);
            case "3":
                return IsusesName.Substring(2);
            case "63":
                return IsusesName.Substring(2);
            case "64":
                return IsusesName.Substring(2);
            case "9":
                return IsusesName.Substring(2);
            case "65":
                return IsusesName.Substring(2);
            default:
                return IsusesName;
        }
    }

    private class CompareToLength : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            long _x = Shove._Convert.StrToLong(x.ToString(), -1);
            long _y = Shove._Convert.StrToLong(y.ToString(), -1);

            return _x.CompareTo(_y);
        }
    }

    private string GetWinNumber(int LotteryID, string WinNumber)
    {
        switch (LotteryID)
        {
            case 1:
                return WinNumber.Replace(",", "");
            case 2:
                return WinNumber.Replace(",", "");
            case 3:
                return WinNumber.Replace(",", "");
            case 5:
                return WinNumber.Replace(",", " ").Insert(17, "+");
            case 6:
                return WinNumber.Replace(",", "");
            case 9:
                return WinNumber.Replace(",", " ");
            case 13:
                return WinNumber.Replace(",", " ").Insert(21, "+");
            case 15:
                return WinNumber.Replace(",", "");
            case 29:
                return WinNumber.Replace(",", "");
            case 39:
                return WinNumber.Replace(",", " ").Insert(15, " + ");
            case 65:
                return WinNumber.Replace(",", " ").Insert(21, "+");
            case 62:
                return WinNumber.Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 63:
                return WinNumber.Replace(",", "");
            case 64:
                return WinNumber.Replace(",", "");
            case 70:
                return WinNumber.Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 74:
                return WinNumber.Replace(",", "");
            case 75:
                return WinNumber.Replace(",", "");
            default:
                return WinNumber;
        }
    }

    //写日志文件
    private void WriteElectronTicketLog(bool isSend, string TransType, string TransMessage)
    {
        new Log("ElectronTicket\\ZCW").Write("isSend: " + isSend.ToString() + "\tTransType: " + TransType + "\t" + TransMessage);
    }

    private string GetClientIPAddress()
    {
        if (Context.Request.ServerVariables["HTTP_VIA"] != null)
        {
            return Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else
        {
            return Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
    }

    #endregion
}

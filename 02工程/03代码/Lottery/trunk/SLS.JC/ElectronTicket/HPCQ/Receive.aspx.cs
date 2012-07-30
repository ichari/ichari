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

using Shove.Database;
using Shove.Alipay;


public partial class ElectronTicket_HPCQ_Receive : System.Web.UI.Page
{
    private const int TimeoutSeconds = 120;

    private string TransType;
    private string TransMessage;

    private bool ElectronTicket_HPCQ_Status_ON;
    private string ElectronTicket_HPCQ_Getway;
    private string ElectronTicket_HPCQ_UserName;
    private string ElectronTicket_HPCQ_UserPassword;

    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        ElectronTicket_HPCQ_Status_ON = so["ElectronTicket_HPCQ_Status_ON"].ToBoolean(false);
        ElectronTicket_HPCQ_Getway = so["ElectronTicket_HPCQ_Getway"].ToString("");
        ElectronTicket_HPCQ_UserName = so["ElectronTicket_HPCQ_UserName"].ToString("");
        ElectronTicket_HPCQ_UserPassword = so["ElectronTicket_HPCQ_UserPassword"].ToString("");

        if (!ElectronTicket_HPCQ_Status_ON)
        {
            return;
        }

        if (!this.IsPostBack)
        {
            TransType = Shove._Web.Utility.GetRequest("transType");
            TransMessage = Shove._Web.Utility.GetRequest("transMessage");

            if (TransType != "" && TransMessage != "")
            {
                if (ValidMessage(TransType, TransMessage))
                {
                    Receive(TransType, TransMessage);
                }
            }
        }
    }

    private void Receive(string TransType, string TransMessage)
    {
        switch (TransType)
        {
            case "501":
                InitiativeNotice(TransMessage);         //主动发送奖期通知
                break;
            case "502":
                IssueInquiry(TransMessage);             //奖期查询
                break;
            case "503":
                Concentration(TransMessage);            //投注响应
                break;
            case "104":
                ConcentrationNotice(TransMessage);      //投注结果响应
                break;
            case "505":
                TicketInquiry(TransMessage);            //票查询
                break;
            case "506":
                //BonusQuery(TransMessage);               //返奖查询
                break;
            case "507":
                SalesVolumeInquiry(TransMessage);       //销量查询
                break;
            case "101":
                PassiveNotice(TransMessage);            //被动接收奖期通知
                break;
            default:
                break;
        }
    }

    //对奖期通知进行处理
    private void InitiativeNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");

        //string Status = "";
        //string Number = "";
        //string LotteryName = "";
        string MessageID = "";

        if (nodes != null)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (nodes[j].Name.ToUpper() == "MESSENGERID")
                {
                    MessageID = nodes[j].Value;
                }
            }
        }
    }

    //对奖期通知进行处理
    private void PassiveNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");
        System.Xml.XmlNodeList nodesIssue = XmlDoc.GetElementsByTagName("issue");

        string Status = "";
        string Number = "";
        string LotteryName = "";
        string MessageID = "";

        if (nodes != null)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (nodes[j].Name.ToUpper() == "BODY" && nodes[j].FirstChild.Name.ToUpper() == "ISSUENOTIFY")
                {
                    for (int i = 0; i < nodesIssue.Count; i++)
                    {
                        //系统主动发送奖期通知
                        Number = nodesIssue[i].Attributes["number"].Value;
                        LotteryName = nodesIssue[i].Attributes["gameName"].Value;
                        Status = nodesIssue[i].Attributes["status"].Value;

                        switch (Status)
                        {
                            case "1":   // 开启奖期
                                if (new DAL.Tables.T_Isuses().GetCount("LotteryID = " + GetLotteryID(LotteryName).ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(Number) + "'") < 1)
                                {
                                    //调用查询奖期
                                    QueryIssue(LotteryName, Number);
                                }
                                break;
                            case "2":   // 暂停奖期
                                break;
                            case "3":   // 奖期截止，可以开始销量查询
                                break;
                            case "4":   // 完成期结，可以开始销量查询
                                break;
                            case "5":
                                QueryOpen();
                                break;
                        }
                    }
                }
            }

            MessageID = nodes[0].Attributes["id"].Value;
            ReNotice(MessageID, "501");
            return;
        }
    }

    //奖期查询构建 类型102(奖期查询)
    private void QueryIssue(string LotteryName, string Number)
    {
        DateTime Now = DateTime.Now;

        string Body = "<body><issueQuery><issue gameName=\"" + LotteryName + "\" number=\"" + Number + "\"/></issueQuery></body>";

        string MessageID = ElectronTicket_HPCQ_UserName + Now.ToString("yyyyMMdd") + Now.ToString("HHmmss") + "00";
        string TimeStamp = Now.ToString("yyyyMMdd") + Now.ToString("HHmmss");

        string Message = "<?xml version=\"1.0\" encoding=\"GBK\"?>";
        Message += "<message version=\"1.0\" id=\"" + MessageID + "\">";
        Message += "<header>";
        Message += "<messengerID>" + ElectronTicket_HPCQ_UserName + "</messengerID>";
        Message += "<timestamp>" + TimeStamp + "</timestamp>";
        Message += "<transactionType>102</transactionType>";
        Message += "<digest>" + Shove._Security.Encrypt.MD5(MessageID + TimeStamp + ElectronTicket_HPCQ_UserPassword + Body, "gb2312") + "</digest>";
        Message += "</header>";
        Message += Body;
        Message += "</message>";

        string MessageInfo = PF.Post(ElectronTicket_HPCQ_Getway, "transType=102&transMessage=" + Message, TimeoutSeconds);
        IssueInquiry(MessageInfo.Substring(27));
    }

    //对期号查询进行处理
    private void IssueInquiry(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("response");
        System.Xml.XmlNodeList nodesIssue = XmlDoc.GetElementsByTagName("issue");

        string LotteryName = "";
        string Number = "";
        DateTime StartTime;
        DateTime EndTime;

        if (nodes != null)
        {
            if (nodes[0].Attributes["code"].Value == "0000")
            {
                if (nodesIssue != null)
                {
                    for (int i = 0; i < nodesIssue.Count; i++)
                    {
                        LotteryName = nodesIssue[i].Attributes["gameName"].Value;       //彩种
                        Number = nodesIssue[i].Attributes["number"].Value;              //期号
                        StartTime = DateTime.Parse(nodesIssue[i].Attributes["startTime"].Value);        //奖期开始时间
                        EndTime = DateTime.Parse(nodesIssue[i].Attributes["stopTime"].Value);           //奖期结束时间  

                        int LotteryID = GetLotteryID(LotteryName);

                        if (LotteryName == "ssc")
                        {
                            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);
                            int Interval = int.Parse(IntervalType.Substring(1, IntervalType.IndexOf(";") - 1));
                            StartTime = EndTime.AddMinutes(-Interval);
                        }

                        if (LotteryID > 0)
                        {
                            if (new DAL.Tables.T_Isuses().GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(Number) + "'") < 1)
                            {
                                long IssueID = -1;
                                string ReturnDescription = "";

                                if (DAL.Procedures.P_IsuseAdd(LotteryID, Number, StartTime, EndTime, "", ref IssueID, ref ReturnDescription) < 0)
                                {
                                    new Log("ElectronTicket\\HPCQ").Write("写入恒朋-重庆电子票网关通知的期号错误，彩种：" + LotteryName + "，期号：" + Number);

                                    return;
                                }

                                if (IssueID < 0)
                                {
                                    new Log("ElectronTicket\\HPCQ").Write("写入恒朋-重庆电子票网关通知的期号错误：" + ReturnDescription + "，彩种：" + LotteryName + "，期号：" + Number);

                                    return;
                                }
                            }
                        }

                        System.Threading.Thread.Sleep(2000);    //进行休眠两秒
                    }
                }
            }
        }
    }

    // 返奖查询 查询类型106(查询返奖)
    private void QueryOpen()
    {
        DataTable dt = new DAL.Views.V_Isuses().Open("top 1 [ID], LotteryID, [Name]", " IsOpened = 0 and EndTime <  GetDate()", " EndTime desc");

        if (dt == null || dt.Rows.Count < 1)
        {
            return;
        }

        DataRow dr = dt.Rows[0];
        DateTime Now = DateTime.Now;

        string Body = "<body><bonusQuery><issue gameName=\"" + GetGameName(int.Parse(dr["LotteryID"].ToString())) + "\" number=\"" + dr["Name"].ToString() + "\"/></bonusQuery></body>";

        string MessageID = ElectronTicket_HPCQ_UserName + Now.ToString("yyyyMMdd") + Now.ToString("HHmmss") + "00";
        string TimeStamp = Now.ToString("yyyyMMdd") + Now.ToString("HHmmss");

        string Message = "<?xml version=\"1.0\" encoding=\"GBK\"?>";
        Message += "<message version=\"1.0\" id=\"" + MessageID + "\">";
        Message += "<header>";
        Message += "<messengerID>" + ElectronTicket_HPCQ_UserName + "</messengerID>";
        Message += "<timestamp>" + TimeStamp + "</timestamp>";
        Message += "<transactionType>106</transactionType>";
        Message += "<digest>" + Shove._Security.Encrypt.MD5(MessageID + TimeStamp + ElectronTicket_HPCQ_UserPassword + Body, "gb2312") + "</digest>";
        Message += "</header>";
        Message += Body;
        Message += "</message>";

        string MessageInfo = PF.Post(ElectronTicket_HPCQ_Getway, "transType=106&transMessage=" + Message, TimeoutSeconds);

        BonusQuery(MessageInfo.Substring(27));
    }

    //对返回的开奖号码，进行程序自动派奖
    private void BonusQuery(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");

        string WinNumber = "";
        string Issue = "";
        string LotteryName = "";

        if (nodes == null)
        {
            return;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].Name.ToUpper() == "BONUSQUERYRESULT")
            {
                WinNumber = nodes[i].Attributes["bonusNumber"].Value.Replace(",", "");
            }

            if (nodes[i].Name.ToUpper() == "ISSUE")
            {
                Issue = nodes[i].Attributes["number"].Value;
                LotteryName = nodes[i].Attributes["gameName"].Value;
            }
        }

        int LotteryID = GetLotteryID(LotteryName);

        if (LotteryID < 0)
        {
            return;
        }

        DataTable dtIssue = new DAL.Tables.T_Isuses().Open("top 1 *", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(Issue) + "' and isOpened = 0 and EndTime < GetDate()", "");

        if (dtIssue == null)
        {
            new Log("ElectronTicket\\HPCQ").Write("恒朋-重庆电子票网关自动开奖错误，彩种：" + LotteryName + "，期号：" + Issue);

            return;
        }

        if (dtIssue.Rows.Count < 1)
        {
            return;
        }

        // 准备开奖，开奖之前，对出票不完整的方案进行处理(按系统的设定：当作已出票，或者撤单处理)
        //int ReturnValue = 0;
        //string ReturnDescription = "";

        //DAL.Procedures.P_PrintOutNotFullHandle(LotteryID, "请电询", ref ReturnValue, ref ReturnDescription);


        DataTable dtWinMoneyList = new DAL.Tables.T_WinTypes().Open("DefaultMoney, DefaultMoneyNoWithTax", "LotteryID = " + LotteryID.ToString(), "[Order]");

        if (dtWinMoneyList == null)
        {
            new Log("ElectronTicket\\HPCQ").Write("恒朋-重庆电子票网关自动开奖错误，彩种：" + LotteryName + "，期号：" + Issue);

            return;
        }

        double[] WinMoneyList = new double[dtWinMoneyList.Rows.Count * 2];

        for (int i = 0; i < dtWinMoneyList.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(dtWinMoneyList.Rows[i][0].ToString(), 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(dtWinMoneyList.Rows[i][1].ToString(), 0);
        }

        DataTable dtScheme = new DAL.Tables.T_Schemes().Open("", "IsuseID = " + dtIssue.Rows[0]["ID"].ToString() + " and isOpened = 0", "");

        if (dtScheme == null)
        {
            new Log("ElectronTicket\\HPCQ").Write("恒朋-重庆电子票网关自动开奖错误，彩种：" + LotteryName + "，期号：" + Issue);

            return;
        }

        if (dtScheme.Rows.Count > 0)
        {
            for (int i = 0; i < dtScheme.Rows.Count; i++)
            {
                string LotteryNumber = dtScheme.Rows[i]["LotteryNumber"].ToString();
                string Description = "";
                double WinMoneyNoWithTax = 0;

                double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtScheme.Rows[i]["PlayTypeID"].ToString()), WinMoneyList);

                Shove.Database.MSSQL.ExecuteNonQuery("update T_Schemes set PreWinMoney = @p1, PreWinMoneyNoWithTax = @p2, EditWinMoney = @p3, EditWinMoneyNoWithTax = @p4, WinDescription = @p5 where [ID] = " + dtScheme.Rows[i]["ID"].ToString(),
                        new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtScheme.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtScheme.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p3", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtScheme.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p4", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtScheme.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p5", SqlDbType.VarChar, 0, ParameterDirection.Input, Description));

            }
        }

        //int SchemeCount = 0, QuashCount = 0, WinCount = 0, WinNoBuyCount = 0;
        //  总方案数，处理时撤单数，中奖数，中奖但未成功数

        //ReturnValue = 0;
        //ReturnDescription = "";

        DataSet ds = null;

        //DAL.Procedures.P_Win(ref ds,
        //     long.Parse(dtIssue.Rows[0]["ID"].ToString()),
        //     WinNumber,
        //     "暂无",
        //     1,
        //     true,
        //     ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount,
        //     ref ReturnValue, ref ReturnDescription);

        //if ((ds == null) || (ReturnDescription != ""))
        //{
        //    PF.GoError(ErrorNumber.DataReadWrite, ReturnDescription, this.GetType().BaseType.FullName);

        //    return;
        //}

        PF.SendWinNotification(ds);
    }

    //对投注结果进行处理(暂未用)
    private void Concentration(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");

        string Status = "";
        string Message = "";
        string messengerID = "";
        string SendID = "";

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name.ToUpper() == "MESSAGE")
                {
                    SendID = nodes[i].Attributes["id"].Value.Substring(nodes[i].Attributes["id"].Value.Length, 8);
                    messengerID = nodes[i].FirstChild.FirstChild["messengerID"].Value;
                }

                if (nodes[i].Name.ToUpper() == "RESPONSE")
                {
                    Status = nodes[i].FirstChild.Attributes["code"].Value;

                    if (Status == "0000")
                    {
                        Message = nodes[i].FirstChild.Attributes["message"].Value;
                    }
                }

            }
        }
    }

    //对系统返回的票信息进行处理
    private void ConcentrationNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodesMessengerID = XmlDoc.GetElementsByTagName("messengerID");
        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("ticket");

        string TicketID;
        string DealTime;
        string Status;
        string Message;

        if (nodes != null)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                TicketID = nodes[j].Attributes["id"].Value;
                DealTime = nodes[j].Attributes["dealTime"].Value;
                Status = nodes[j].Attributes["status"].Value;
                Message = nodes[j].Attributes["message"].Value;

                int ReturnValue = 0;
                //string ReturnDescription = "";

                int Result = 0;

                //if (Status == "0000")
                //{
                //    Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DealTime, 1, "", 101, ref ReturnValue, ref ReturnDescription);
                //}
                //else
                //{
                //    Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DealTime, -1, Message, 101, ref ReturnValue, ref ReturnDescription);
                //}

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "ElectronTicket_HPCQ_Receive");

                    return;
                }

                if (ReturnValue < 0)
                {
                    new Log("ElectronTicket\\HPCQ").Write("对恒朋-重庆电子票网所发送的电子票数据处理出错：部分票写入错误。票号：" + TicketID.ToString());

                    continue;
                }
            }

            ReNotice(GetFromXPath(TransMessage, "message/header/messengerID"), "504");
        }
    }

    //票查询
    private void TicketInquiry(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        //System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");
        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("ticket");

        string TicketID;
        string Status;
        string Message;

        if (nodes != null)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                TicketID = nodes[j].Attributes["id"].Value;
                Status = nodes[j].Attributes["status"].Value;
                Message = nodes[j].Attributes["message"].Value;

                //对查询的票信息进行处理
            }
        }
    }

    //返奖查询
    private void SalesVolumeInquiry(string Transmessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("body");

        string Status = "";
        string SalesMoney = "";
        string BonusMoney = "";
        string GameName = "";
        string Number = "";

        if (nodes != null)
        {
            if (nodes[0].Name.ToUpper() == "RESPONSE")
            {
                Status = nodes[0].FirstChild.Attributes["code"].Value;

                if (Status == "0000" && nodes[0].FirstChild.FirstChild.Name.ToUpper() == "BALANCEQUERYRESULT")
                {
                    SalesMoney = nodes[0].FirstChild.FirstChild.Attributes["salesMoney"].Value;  //销量
                    BonusMoney = nodes[0].FirstChild.FirstChild.Attributes["bonusMoney"].Value;   //返奖

                    GameName = nodes[0].FirstChild.FirstChild.FirstChild.Attributes["gameName"].Value;  //彩种
                    Number = nodes[0].FirstChild.FirstChild.FirstChild.Attributes["number"].Value;    //期号
                }
            }
        }
    }

    //对恒朋-重庆返回的信息进行验证
    private bool ValidMessage(string TransType, string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        System.Xml.XmlNodeList nodes = XmlDoc.GetElementsByTagName("*");

        string TimeStamp = "";
        string MessageId = "";
        string Digest = "";
        string Body = "";

        if (nodes != null)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (nodes[j].Name.ToUpper() == "MESSAGE")
                {
                    if (ElectronTicket_HPCQ_UserName != GetFromXPath(TransMessage, "message/header/messengerID"))
                    {
                        return false;
                    }

                    MessageId = nodes[j].Attributes["id"].Value;
                }

                if (nodes[j].Name.ToUpper() == "TIMESTAMP")
                {
                    TimeStamp = nodes[j].InnerText;
                }

                if (nodes[j].Name.ToUpper() == "DIGEST")
                {
                    Digest = nodes[j].InnerText;
                }
            }

            Body = GetBody(TransMessage);

            return (Digest.ToLower() == Shove._Security.Encrypt.MD5(MessageId + TimeStamp + ElectronTicket_HPCQ_UserPassword + Body, "gb2312").ToLower());
        }

        return false;
    }

    // 返回成功信息
    private void ReNotice(string MessageID, string Type)
    {
        DateTime Now = DateTime.Now;

        string Body = "<body><response code=\"0000\" message=\"成功，系统处理正常\"/></body>";

        string TimeStamp = Now.ToString("yyyyMMdd") + Now.ToString("HHmmss");

        string Message = "<?xml version=\"1.0\" encoding=\"GBK\"?>";
        Message += "<message version=\"1.0\" id=\"" + MessageID + "\">";
        Message += "<header>";
        Message += "<messengerID>" + ElectronTicket_HPCQ_UserName + "</messengerID>";
        Message += "<timestamp>" + TimeStamp + "</timestamp>";
        Message += "<transactionType>" + Type + "</transactionType>";
        Message += "<digest>" + Shove._Security.Encrypt.MD5(MessageID + TimeStamp + ElectronTicket_HPCQ_UserPassword + Body, "gb2312") + "</digest>";
        Message += "</header>";
        Message += Body;
        Message += "</message>";

        Response.Write("transType=" + Type + "&transMessage=" + Message);
        Response.End();

        //string aa = PF.Post(ElectronTicket_HPCQ_Getway, "transType=" + Type + "&transMessage=" + Message);
    }


    //------------------- 公用方法

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

    public static string GetBody(string XML)
    {
        Regex regex = new Regex(@"(?<body><Body[\S\s]*?>[\S\s]*?</Body>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(XML);
        return m.Groups["body"].Value;
    }

    private string GetGameName(int LotteryID)
    {
        switch (LotteryID)
        {
            case 5:
                return "ssq";
            case 28:
                return "ssc";
            default:
                return "";
        }
    }

    private int GetLotteryID(string gameName)
    {
        switch (gameName)
        {
            case "ssq":
                return 5;
            case "ssc":
                return 28;
            default:
                return -1;
        }
    }
}

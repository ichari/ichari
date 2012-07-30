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
using System.Security.Cryptography;

public partial class ElectronTicket_CTTCSD_Receive : System.Web.UI.Page
{
    private const int TimeoutSeconds = 120;

    private bool ElectronTicket_CTTCSD_Status_ON;
    private string ElectronTicket_CTTCSD_Getway;
    private string ElectronTicket_CTTCSD_DownloadGetway;
    private string ElectronTicket_CTTCSD_UserName;
    private string ElectronTicket_CTTCSD_UserPassword;

    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        ElectronTicket_CTTCSD_Status_ON = so["ElectronTicket_CTTCSD_Status_ON"].ToBoolean(false);
        ElectronTicket_CTTCSD_Getway = so["ElectronTicket_CTTCSD_Getway"].ToString("");
        ElectronTicket_CTTCSD_DownloadGetway = so["ElectronTicket_CTTCSD_DownloadGetway"].ToString("");
        ElectronTicket_CTTCSD_UserName = so["ElectronTicket_CTTCSD_UserName"].ToString("");
        ElectronTicket_CTTCSD_UserPassword = so["ElectronTicket_CTTCSD_UserPassword"].ToString("");

        //if (!ElectronTicket_CTTCSD_Status_ON)
        //{
        //    return;
        //}

        if (!this.IsPostBack)
        {
            string request;

            using (Stream MyStream = Request.InputStream)
            {
                byte[] _tmpData = new byte[MyStream.Length];
                MyStream.Read(_tmpData, 0, _tmpData.Length);
                request = Encoding.UTF8.GetString(_tmpData);
            }

            string transactiontype = "";
            string Body = "";

            new Log("ElectronTicket\\CTTCSD").Write(System.Web.HttpUtility.UrlDecode(request));

            if (!ValidMessage(request, ref transactiontype, ref Body))
            {
                new Log("ElectronTicket\\CTTCSD").Write("校验码错误：" + request);

                Response.Write("校验码错误");
                Response.End();

                return;
            }

            Receive(transactiontype, Body);
        }
    }

    private void Receive(string TransType, string TransMessage)
    {
        switch (TransType)
        {
            case "1100":
                ConcentrationNotice(TransMessage);              //对系统返回的票信息进行处理
                break;
            case "1101":
                ConcentrationNotice(TransMessage);              //对系统返回的竞彩票信息进行处理
                break;
            case "1203":
                OpenIssueNotice(TransMessage);                  //对开售通知进行处理
                break;
            case "1204":
                StopIssueNotice(TransMessage);                  //对停售通知进行处理
                break;
            case "1202":
                ReceiveWinNumberNotice(TransMessage);           //对开奖号码通知进行处理
                break;
            case "1300":
                BetResultNotice(TransMessage);                  //投注结果对账文件生成通知
                break;
            case "1301":
                AwardResultNotice(TransMessage);                //中奖明细对账文件生成通知
                break;
            default:
                OtherNotice(TransType, TransMessage);
                break;
        }
    }

    //对系统返回的票信息进行处理
    private void ConcentrationNotice(string TransMessage)
    {
        string elements = TransMessage.Substring(TransMessage.IndexOf("<records"), TransMessage.LastIndexOf("</records>") - TransMessage.IndexOf("<records")) + "</records>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dtTicket = ds.Tables[0];

        if (dtTicket.Rows.Count < 1)
        {
            return;
        }

        int ReturnValue = 0;
        string ReturnDescription = "";

        string TicketID = "";
        string Status = "";
        string ID = "";

        string Body = "<body><records>";

        for (int k = 0; k < dtTicket.Rows.Count; k++)
        {
            DataRow dr = dtTicket.Rows[k];

            TicketID = dr["ticketId"].ToString();
            Status = dr["result"].ToString();
            ID = dr["id"].ToString();

            int Result = 0;

            if (Status == "0")
            {
                Result = DAL.Procedures.P_SchemesSendToCenterHandle(ID, DateTime.Now, true, "", ref ReturnValue, ref ReturnDescription);
            }
            else
            {
                Result = DAL.Procedures.P_SchemesSendToCenterHandle(ID, DateTime.Now, false, "", ref ReturnValue, ref ReturnDescription);
            }

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "ElectronTicket_CTTCSD_Receive");

                return;
            }

            if (ReturnValue < 0)
            {
                new Log("ElectronTicket\\CTTCSD").Write("对恒朋-重庆电子票网所发送的电子票数据处理出错：部分票写入错误。票号：" + ID.ToString() + "; 错误原因：" + ReturnDescription);

                continue;
            }

            Body += "<record><id>" + ID + "</id><result>0</result></record>";
        }

        Body += "</records></body>";

        ReNotice("1100", Body);
    }

    //对开售通知进行处理
    private void OpenIssueNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        string elements = TransMessage.Substring(TransMessage.IndexOf("<body"), TransMessage.LastIndexOf("</body>") - TransMessage.IndexOf("<body")) + "</body>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dtIsuses = ds.Tables[0];

        if (dtIsuses.Rows.Count < 1)
        {
            return;
        }

        int LotteryID = 0;
        string IssueName = "";
        DateTime StartTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        foreach (DataRow dr in dtIsuses.Rows)
        {
            LotteryID = GetLotteryID(dr["lotteryId"].ToString());
            IssueName = dr["issue"].ToString();

            try
            {
                StartTime = Shove._Convert.StrToDateTime(dr["startTime"].ToString().Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-"), DateTime.Now.ToString());
                EndTime = Shove._Convert.StrToDateTime(dr["endTime"].ToString().Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-"), DateTime.Now.ToString());                
            }
            catch
            {
                continue;
            }

            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "'") < 1)
            {
                long IsuseID = -1;
                string ReturnDescription = "";

                if (DAL.Procedures.P_IsuseAdd(LotteryID, IssueName, StartTime, EndTime, "", ref IsuseID, ref ReturnDescription) < 0)
                {
                    continue;
                }

                if (IsuseID < 0)
                {
                    continue;
                }
            }

            DataTable dtIsuse = t_Isuses.Open("ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 108) and year(getdate()) = year(StartTime)", "");

            if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
            {
                continue;
            }

            int ReturnValue = 0;
            string ReturnDescprtion = "";

            if (DAL.Procedures.P_IsuseUpdate(LotteryID, IssueName, 1, StartTime, EndTime, DateTime.Now, "", ref ReturnValue, ref ReturnDescprtion) < 0)
            {
                new Log("ElectronTicket\\CTTCSD").Write("电子票期号状态更新错误_P_IsuseUpdate");

                continue;
            }
        }

        string Body = "<body><result>0</result></body>";

        ReNotice("1203", Body);
    }

    //对停售通知进行处理
    private void StopIssueNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        string elements = TransMessage.Substring(TransMessage.IndexOf("<body"), TransMessage.LastIndexOf("</body>") - TransMessage.IndexOf("<body")) + "</body>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dtIsuses = ds.Tables[0];

        if (dtIsuses.Rows.Count < 1)
        {
            return;
        }

        int LotteryID = 0;
        string IssueName = "";

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        foreach (DataRow dr in dtIsuses.Rows)
        {
            LotteryID = GetLotteryID(dr["lotteryId"].ToString());
            IssueName = dr["issue"].ToString();

            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "' and year(getdate()) = year(StartTime) and LotteryID  in (select id from T_Lotteries where PrintOutType = 108)") < 1)
            {
                continue;
            }

            DataTable dtIsuse = t_Isuses.Open("ID, StartTime, EndTime, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 108) and year(getdate()) = year(StartTime)", "");

            if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
            {
                continue;
            }

            int ReturnValue = 0;
            string ReturnDescprtion = "";

            if (DAL.Procedures.P_IsuseUpdate(LotteryID, IssueName, 5, Shove._Convert.StrToDateTime(dr["startTime"].ToString().Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-"), DateTime.Now.ToString()), Shove._Convert.StrToDateTime(dr["endTime"].ToString().Insert(12, ":").Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-"), DateTime.Now.ToString()), DateTime.Now, "", ref ReturnValue, ref ReturnDescprtion) < 0)
            {
                new Log("ElectronTicket\\CTTCSD").Write("电子票期号截止更新错误_P_IsuseUpdate");

                continue;
            }
        }

        string Body = "<body><result>0</result></body>";

        ReNotice("1204", Body);
    }

    //对开奖号码通知进行处理
    private void ReceiveWinNumberNotice(string TransMessage)
    {
        string elements = TransMessage.Substring(TransMessage.IndexOf("<body"), TransMessage.LastIndexOf("</body>") - TransMessage.IndexOf("<body")) + "</body>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        if (ds.Tables.Count < 1)
        {
            return;
        }

        DataTable dtLottery = ds.Tables[0];
        DataTable dtIssue = ds.Tables[1];

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        string number = dtLottery.Rows[0]["issue"].ToString();
        string LotteryName = dtLottery.Rows[0]["lotteryId"].ToString();

        string BonusNumber = dtIssue.Rows[0]["baseCode"].ToString() + " " + dtIssue.Rows[0]["specialCode"].ToString();

        int LotteryID = GetLotteryID(LotteryName);
        string WinNumber = GetWinNumber(LotteryID, BonusNumber);

        string Body = "";

        DataTable dtIsuse = new DAL.Tables.T_Isuses().Open("", "[Name] = '" + number + "' and LotteryID = " + LotteryID.ToString() + " and IsOpened = 0", "");

        if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
        {
            Body = "<body><result>0</result></body>";

            ReNotice("1202", Body);

            return;
        }

        long IsuseID = Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), 0);

        DAL.Tables.T_Isuses T_Isuses = new DAL.Tables.T_Isuses();

        T_Isuses.WinLotteryNumber.Value = WinNumber;
        T_Isuses.OpenOperatorID.Value = 1;
        T_Isuses.Update( "[ID] = " + IsuseID + " and [Name] = '" + number + "' and LotteryID = " + LotteryID.ToString());


        DAL.Tables.T_IsuseInfo t_IsuseInfo = new DAL.Tables.T_IsuseInfo();

        t_IsuseInfo.TotalSaleMoney.Value = Shove._Convert.StrToDouble(dtLottery.Rows[0]["totalSaleMoney"].ToString(), -1);
        t_IsuseInfo.PoolOut.Value = Shove._Convert.StrToDouble(dtLottery.Rows[0]["poolOut"].ToString(), -1);
        t_IsuseInfo.TotalAwardMoney.Value = Shove._Convert.StrToDouble(dtLottery.Rows[0]["totalAwardMoney"].ToString(), -1);
        t_IsuseInfo.TotalSaleMoneyLocal.Value = Shove._Convert.StrToDouble(dtLottery.Rows[0]["totalSaleMoneyLocal"].ToString(), -1);
        t_IsuseInfo.TotalAwardMoneyLocal.Value = Shove._Convert.StrToDouble(dtLottery.Rows[0]["totalAwardMoneyLocal"].ToString(), -1);
        t_IsuseInfo.IssueID.Value = IsuseID;

        if (new DAL.Tables.T_IsuseInfo().GetCount("IssueID=" + IsuseID.ToString()) < 1)
        {
            t_IsuseInfo.Insert();
        }
        else
        {
            t_IsuseInfo.Update("IssueID=" + IsuseID.ToString());
        }

        DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open("", " LotteryID =" + LotteryID.ToString(), "");

        if ((dtWinTypes == null) || dtWinTypes.Rows.Count < 1)
        {
            //log.Write("执行电子票--获取彩种: " + LotteryID.ToString() + "，奖金等级时出现错误!");

            Body = "<body><result>0</result></body>";

            ReNotice("1202", Body);

            return;
        }

        double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];

        double DefaultMoney = 0;
        double DefaultMoneyNoWithTax = 0;

        for (int i = 0; i < dtWinTypes.Rows.Count; i++)
        {
            DefaultMoney = Shove._Convert.StrToDouble(dtWinTypes.Rows[i]["DefaultMoney"].ToString(), 0);
            DefaultMoneyNoWithTax = Shove._Convert.StrToDouble(dtWinTypes.Rows[i]["DefaultMoneyNoWithTax"].ToString(), 0);

            WinMoneyList[i * 2] = DefaultMoney == 0 ? 1 : DefaultMoneyNoWithTax;
            WinMoneyList[i * 2 + 1] = DefaultMoneyNoWithTax == 0 ? 1 : DefaultMoneyNoWithTax;
        }

        DataTable dtSchemesWithTaskDetails = new DAL.Views.V_Schemes().Open("", " IsuseName = '" + number + "' and LotteryID = " + LotteryID.ToString() + " and WinMoney = 0  and Buyed = 0 and ID in ( select ID from V_ChaseTaskDetails where IsuseName = '" + number + "' and LotteryID = " + LotteryID.ToString() + ")", "");

        string LotteryNumber = "";
        SLS.Lottery.LotteryBase lb = new SLS.Lottery()[LotteryID];

        int ReturnValue = 0;
        string ReturnDescription = "";
        string Description = "";
        double WinMoneyNoWithTax = 0;

        for (int i = 0; i < dtSchemesWithTaskDetails.Rows.Count; i++)
        {
            LotteryNumber = dtSchemesWithTaskDetails.Rows[i]["LotteryNumber"].ToString();

            Description = "";
            WinMoneyNoWithTax = 0;

            double WinMoney = lb.ComputeWin(LotteryNumber, WinNumber.Trim(), ref Description, ref WinMoneyNoWithTax, int.Parse(dtSchemesWithTaskDetails.Rows[i]["PlayTypeID"].ToString()), WinMoneyList);

            if (WinMoney > 0)
            {
                if (DAL.Procedures.P_ChaseTaskStopWhenWin(Shove._Convert.StrToLong(dtSchemesWithTaskDetails.Rows[i]["SiteID"].ToString(), 0), Shove._Convert.StrToLong(dtSchemesWithTaskDetails.Rows[i]["ID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescription) < 0)
                {
                    //log.Write("执行电子票--判断是否停止追号的时候出现错误");

                    continue;
                }
            }
        }

        Body = "<body><result>0</result></body>";

        ReNotice("1202", Body);
    }

    //投注结果对账文件生成通知
    private void BetResultNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        string elements = TransMessage.Substring(TransMessage.IndexOf("<body"), TransMessage.LastIndexOf("</body>") - TransMessage.IndexOf("<body")) + "</body>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dtFile = ds.Tables[0];

        if (dtFile.Rows.Count < 1)
        {
            return;
        }

        int LotteryID = 0;
        string IssueName = "";
        string FileName = "";

        string Body = "";

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        foreach (DataRow dr in dtFile.Rows)
        {
            LotteryID = GetLotteryID(dr["lotteryId"].ToString());
            IssueName = dr["issue"].ToString();
            FileName = dr["fileName"].ToString();

            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "' and year(getdate()) = year(StartTime) and LotteryID  in (select id from T_Lotteries where PrintOutType = 108)") < 1)
            {
                continue;
            }

            DataTable dtIsuse = t_Isuses.Open("ID, StartTime, EndTime, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 108) and year(getdate()) = year(StartTime)", "");

            if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
            {
                continue;
            }

            string DownLoadFileNameUrl = ElectronTicket_CTTCSD_DownloadGetway + "/" + ElectronTicket_CTTCSD_UserName + "/" + dr["lotteryId"].ToString() + "/" + "Reports/" + FileName;

            string Html = PF.GetHtml(DownLoadFileNameUrl, "utf-8", 120);

            if (Html == "")
            {
                Body = "<body><result>100004</result></body>";
            }

            elements = Html.Substring(Html.IndexOf("<head>"), Html.LastIndexOf("</body>") - Html.IndexOf("<head>")) + "</body>";

            ds = new DataSet();

            ds.ReadXml(new StringReader(Html));

            if (ds == null)
            {
                Body = "<body><result>100004</result></body>";

                continue;
            }

            if (ds.Tables.Count != 4)
            {
                Body = "<body><result>100004</result></body>";

                continue;
            }

            if (Shove._Security.Encrypt.MD5(elements) != ds.Tables[0].Rows[0]["sign"].ToString())
            {
                Body = "<body><result>100004</result></body>";

                continue;
            }

            DataTable dtTicket = ds.Tables[3];

            if (dtTicket.Rows.Count < 1)
            {
                Body = "<body><result>0</result></body>";

                continue;
            }

            LotteryID = GetLotteryID(ds.Tables[1].Rows[0]["lotteryId"].ToString());
            IssueName = ds.Tables[1].Rows[0]["issue"].ToString();

            string[] StrTickets = null;
            string TicketID = "";

            foreach (DataRow drTicket in dtTicket.Rows)
            {
                StrTickets = drTicket[0].ToString().Split(',');

                if (StrTickets.Length != 8)
                {
                    continue;
                }

                TicketID = StrTickets[0].ToString();

                int ReturnValue = 0;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DateTime.Now, true, "", ref ReturnValue, ref ReturnDescription);

                if ((Result < 0) || ((ReturnValue < 0) && (ReturnValue != -2)))
                {
                    new Log("ElectronTicket\\CTTCSD").Write("对彩通天成电子票网所发送的电子票数据处理出错：部分票更新错误。票号：" + TicketID.ToString());

                    continue;
                }
            }
        }

        if (string.IsNullOrEmpty(Body))
        {
            Body = "<body><result>0</result></body>";
        }

        ReNotice("1300", Body);
    }

    //中奖明细对账文件生成通知
    private void AwardResultNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(new StringReader(TransMessage));

        string elements = TransMessage.Substring(TransMessage.IndexOf("<body"), TransMessage.LastIndexOf("</body>") - TransMessage.IndexOf("<body")) + "</body>";

        DataSet ds = new DataSet();

        ds.ReadXml(new StringReader(elements));

        if (ds == null)
        {
            return;
        }

        if (ds.Tables.Count == 0)
        {
            return;
        }

        DataTable dtFile = ds.Tables[0];

        if (dtFile.Rows.Count < 1)
        {
            return;
        }

        int LotteryID = 0;
        string IssueName = "";
        string FileName = "";

        string Body = "";

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        string BonusXML = "<Schemes>";
        string AgentBonusXML = "<Schemes>";


        DataTable dtIsuse = null;

        foreach (DataRow dr in dtFile.Rows)
        {
            LotteryID = GetLotteryID(dr["lotteryId"].ToString());
            IssueName = dr["issue"].ToString();
            FileName = dr["fileName"].ToString();

            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "' and year(getdate()) = year(StartTime) and LotteryID  in (select id from T_Lotteries where PrintOutType = 108)") < 1)
            {
                continue;
            }

            dtIsuse = t_Isuses.Open("ID, StartTime, EndTime, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 108) and year(getdate()) = year(StartTime) and IsOpened = 0", "");

            if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
            {
                continue;
            }

            string DownLoadFileNameUrl = ElectronTicket_CTTCSD_DownloadGetway + "/" + ElectronTicket_CTTCSD_UserName + "/" + dr["lotteryId"].ToString() + "/Award/" + FileName;

            string Html = PF.GetHtml(DownLoadFileNameUrl, "utf-8", 120);

            if (Html == "")
            {
                Body = "<body><result>100004</result></body>";

                new Log("ElectronTicket\\CTTCSD").Write("没有获取到中奖信息:期号为：" + IssueName);
            }

            if (Html.IndexOf("<body/>") >= 0)
            {
                continue;
            }

            elements = Html.Substring(Html.IndexOf("<head>"), Html.LastIndexOf("</body>") - Html.IndexOf("<head>")) + "</body>";

            DataSet ds1 = new DataSet();

            ds1.ReadXml(new StringReader(Html));

            if (ds1 == null)
            {
                Body = "<body><result>100004</result></body>";

                new Log("ElectronTicket\\CTTCSD").Write("中奖文件获取异常:读取内容为：" + Html);

                continue;
            }

            if (Shove._Security.Encrypt.MD5(elements).ToLower() != ds1.Tables[0].Rows[0]["sign"].ToString().ToLower())
            {
                Body = "<body><result>100004</result></body>";

                new Log("ElectronTicket\\CTTCSD").Write("校验错误：" + Shove._Security.Encrypt.MD5(elements).ToLower() + "----------" + ds1.Tables[0].Rows[0]["sign"].ToString().ToLower());

                continue;
            }

            DataTable dtTicket = ds1.Tables[ds1.Tables.Count - 1];

            if (dtTicket.Rows.Count < 1)
            {
                Body = "<body><result>0</result></body>";

                continue;
            }

            LotteryID = GetLotteryID(ds1.Tables[1].Rows[0]["lotteryId"].ToString());
            IssueName = ds1.Tables[1].Rows[0]["issue"].ToString();

            string[] StrTickets = null;

            DataTable dtTicketTemp = new DataTable();

            dtTicketTemp.Columns.Add("ID", typeof(System.String));
            dtTicketTemp.Columns.Add("AwardValue", typeof(System.String));

            DataRow drTicketTemp = null;

            foreach (DataRow drTicket in dtTicket.Rows)
            {
                StrTickets = drTicket[0].ToString().Split(',');

                if (StrTickets.Length != 7)
                {
                    continue;
                }

                drTicketTemp = dtTicketTemp.NewRow();

                drTicketTemp["ID"] = StrTickets[0].ToString();
                drTicketTemp["AwardValue"] = StrTickets[3].ToString().Insert(StrTickets[3].ToString().Length - 2, ".");

                dtTicketTemp.Rows.Add(drTicketTemp);
                dtTicketTemp.AcceptChanges();
            }

            DataTable dtSchemes = MSSQL.Select("SELECT SchemeID, SchemesMultiple as Multiple, Identifiers FROM V_SchemesSendToCenter WHERE (IsuseID = " + dtIsuse.Rows[0]["ID"].ToString() + ")");

            if (dtSchemes == null)
            {
                new Log("ElectronTicket\\CTTCSD").Write("电子票开奖，第 " + IssueName + " 期，读取本地方案错误。");

                this.Response.End();

                return;
            }

            try
            {
                var query1 = from NewDtTickets in dtTicketTemp.AsEnumerable()
                             join NewdtScheme in dtSchemes.AsEnumerable()
                             on NewDtTickets.Field<string>("ID") equals NewdtScheme.Field<string>("Identifiers")
                             select new
                             {
                                 ID = NewdtScheme.Field<long>("SchemeID"),
                                 Multiple = NewdtScheme.Field<int>("Multiple"),
                                 Bonus = Shove._Convert.StrToDouble(NewDtTickets.Field<string>("AwardValue"), 0)
                             };

                var query2 = from NewDt in query1.AsQueryable()
                             group NewDt by new { NewDt.ID, NewDt.Multiple } into gg
                             select new
                             {
                                 ID = gg.Key.ID,
                                 Multiple = gg.Key.Multiple,
                                 Bonus = gg.Sum(NewDt => NewDt.Bonus)
                             };

                var query3 = from NewDt in query2.AsQueryable()
                             group NewDt by new { NewDt.ID, NewDt.Multiple } into t_dtSchemes
                             select new
                             {
                                 SchemeID = t_dtSchemes.Key.ID,
                                 Multiple = t_dtSchemes.Key.Multiple,
                                 Bonus = t_dtSchemes.Sum(NewDt => NewDt.Bonus)
                             };

                foreach (var Scheme in query3)
                {
                    BonusXML += "<Scheme SchemeID=\"" + Scheme.SchemeID.ToString() + "\" WinMoney=\"" + Scheme.Bonus.ToString() + "\" WinDescription=\"\" />";
                }
            }
            catch (Exception e)
            {
                new Log("ElectronTicket\\CTTCSD").Write("电子票开奖，第 " + IssueName + " 期详细中奖数据解析错误：" + e.Message);

                this.Response.End();

                return;
            }
        }

        BonusXML += "</Schemes>";
        AgentBonusXML += "</Schemes>";

        if (dtIsuse.Rows.Count > 0)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            int Times = 0;
            int Result = -1;

            while ((Result < 0) && (Times < 5))
            {
                ReturnValue = 0;
                ReturnDescription = "";

                Result = DAL.Procedures.P_ElectronTicketWin(Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), 0), BonusXML, AgentBonusXML, ref ReturnValue, ref ReturnDescription);

                if (Result < 0)
                {
                    new Log("ElectronTicket\\CTTCSD").Write("电子票第 " + (Times + 1).ToString() + " 次派奖出现错误(IsuseOpenNotice) 期号为: " + IssueName + "，彩种为: " + LotteryID.ToString());
                    Times++;

                    if (Times < 5)
                    {
                        System.Threading.Thread.Sleep(10000);
                    }

                    continue;
                }
            }

            if (ReturnValue < 0)
            {
                new Log("ElectronTicket\\CTTCSD").Write("电子票派奖出现错误(IsuseOpenNotice) 期号为: " + IssueName + "，彩种为: " + LotteryID.ToString() + "，错误：" + ReturnDescription);

                this.Response.End();

                return;
            }
        }

        if (string.IsNullOrEmpty(Body))
        {
            Body = "<body><result>0</result></body>";
        }

        ReNotice("1301", Body);
    }

    private void OtherNotice(string TransType, string TransMessage)
    {
        string Body = "<body><result>0</result></body>";

        ReNotice(TransType, Body);
    }

    private bool ValidMessage(string TransMessage, ref string transactiontype, ref string Body)
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
        transactiontype = "";

        for (int j = 0; j < nodes.Count; j++)
        {
            if (nodes[j].Name.ToUpper() == "VENDERID")
            {
                if (ElectronTicket_CTTCSD_UserName != nodes[j].InnerText)
                {
                    return false;
                }
            }

            if (nodes[j].Name.ToUpper() == "MD")
            {
                Digest = nodes[j].InnerText;
            }

            if (nodes[j].Name.ToUpper() == "COMMAND")
            {
                transactiontype = nodes[j].InnerText;
            }
        }

        Body = GetBody(TransMessage);

        string ReceiveString = Body.Substring(Body.IndexOf("<body>") + 6, Body.LastIndexOf("</body>") - Body.IndexOf("<body>") - 6);

        Body = DecryptDES(ReceiveString, ElectronTicket_CTTCSD_UserPassword).Replace("\r", "").Replace("\n", "");

        new Log("ElectronTicket\\CTTCSD").Write(Body);

        return (Digest.ToLower() == Shove._Security.Encrypt.MD5(ReceiveString, "utf-8").ToLower());
    }

    // 返回成功信息
    private void ReNotice(string Type, string Body)
    {
        new Log("ElectronTicket\\CTTCSD").Write(Type + "-------" + Body);

        DateTime Now = DateTime.Now;

        string MessageID = ElectronTicket_CTTCSD_UserName + Now.ToString("yyyyMMdd") + Now.ToString("HHmmss") + "88";

        Body = EncryptDES(Body, ElectronTicket_CTTCSD_UserPassword);

        string Message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        Message += "<message>";
        Message += "<head>";
        Message += "<version>1500</version>";
        Message += "<command>" + Type + "</command>";
        Message += "<venderId>" + ElectronTicket_CTTCSD_UserName + "</venderId>";
        Message += "<messageId>" + MessageID + "</messageId>";
        Message += "<md>" + Shove._Security.Encrypt.MD5(Body, "utf-8") + "</md>";
        Message += "</head>";
        Message += "<body>";
        Message += Body;
        Message += "</body>";
        Message += "</message>";

        Response.Write(Message);
        Response.End();
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
            case 2:
                return "C4";
            case 3:
                return "D7";
            case 9:
                return "C522";
            case 15:
                return "C12";
            case 39:
                return "T001";
            case 62:
                return "C511";
            case 63:
                return "D3";
            case 64:
                return "D5";
            case 65:
                return "C731";
            case 74:
                return "D14";
            case 75:
                return "D9";
            default:
                return "";
        }
    }

    private int GetLotteryID(string gameName)
    {
        switch (gameName)
        {
            case "D14":
                return 74;
            case "D9":
                return 75;
            case "C4":
                return 2;
            case "D7":
                return 3;
            case "C522":
                return 9;
            case "C12":
                return 15;
            case "T001":
                return 39;
            case "C511":
                return 62;
            case "D3":
                return 63;
            case "D5":
                return 64;
            case "C731":
                return 65;
            default:
                return -1;
        }
    }

    ///   <summary> 
    ///   3des加密字符串 
    ///   </summary> 
    ///   <param   name= "a_strString "> 要加密的字符串 </param> 
    ///   <param   name= "a_strKey "> 密钥 </param> 
    ///   <returns> 加密后并经base64编码的字符串 </returns> 
    ///   <remarks> 静态方法，采用默认ascii编码 </remarks> 
    private string EncryptDES(string a_strString, string a_strKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        des.Mode = CipherMode.ECB;
        des.Padding = PaddingMode.PKCS7;
        des.IV = StringToKey(a_strKey);
        des.Key = StringToKey(a_strKey);
        des.BlockSize = 64;
        byte[] bytes = Encoding.UTF8.GetBytes(a_strString);
        byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);

        return Convert.ToBase64String(resultBytes, Base64FormattingOptions.None);
    }

    private byte[] StringToKey(String key_str)
    {
        byte[] k = new byte[8];
        for (int i = 0; i < key_str.Length; i += 2)
        {
            k[i / 2] = (byte)Convert.ToInt32(key_str.Substring(i, 2), 16);
        }
        return k;
    }

    private byte[] strToToHexByte(string hexString)
    {
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }

    ///   <summary> 
    ///   3des解密字符串 
    ///   </summary> 
    ///   <param   name= "a_strString "> 要解密的字符串 </param> 
    ///   <param   name= "a_strKey "> 密钥 </param> 
    ///   <returns> 解密后的字符串 </returns> 
    ///   <exception   cref= " "> 密钥错误 </exception> 
    ///   <remarks> 静态方法，采用默认ascii编码 </remarks> 
    private string DecryptDES(string a_strString, string a_strKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        des.Mode = CipherMode.ECB;
        des.Padding = PaddingMode.PKCS7;
        des.IV = StringToKey(a_strKey);
        des.Key = StringToKey(a_strKey);
        des.BlockSize = 64;

        byte[] bytes = Convert.FromBase64String(a_strString);

        byte[] resultBytes = des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);

        return System.Text.Encoding.UTF8.GetString(resultBytes);
    }

    private string GetWinNumber(int LotteryID, string WinNumber)
    {
        switch (LotteryID)
        {
            case 5:
                return WinNumber.Replace(",", " ").Insert(12, " +").Insert(10, " ").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 6:
                return WinNumber.Replace(",", "");
            case 13:
                return WinNumber.Replace(",", " ").Replace("#", " + ");
            case 29:
                return WinNumber.Replace(",", "");
            case 58:
                return WinNumber.Replace(",", "").Replace("#01", "+鼠").Replace("#02", "+牛").Replace("#03", "+虎").Replace("#04", "+兔").Replace("#05", "+龙").Replace("#06", "+蛇").Replace("#07", "+马").Replace("#08", "+羊").Replace("#09", "+猴").Replace("#10", "+鸡").Replace("#11", "+狗").Replace("#12", "+猪");
            case 59:
                return WinNumber.Replace(",", " ").Replace("#", " ");
            case 60:
                return WinNumber.Replace(",", "");
            case 3:
                return WinNumber.Replace(",", "");
            case 9:
                return WinNumber.Replace(",", " ").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 65:
                return WinNumber.Replace(",", " ").Insert(13, " ").Insert(11, "+ ").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 39:
                return WinNumber.Replace(" ", "+").Insert(13, " ").Replace("+", " + ").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 1:
                return WinNumber.Replace(",", "");
            case 2:
                return WinNumber.Replace(",", "");
            case 15:
                return WinNumber.Replace(",", "").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 62:
                return WinNumber.Replace(",", " ").Insert(8, " ").Insert(6, " ").Insert(4, " ").Insert(2, " ");
            case 63:
                return WinNumber.Replace(",", "");
            case 64:
                return WinNumber.Replace(",", "").Replace("#", " + ");
            default:
                return WinNumber;
        }
    }
}

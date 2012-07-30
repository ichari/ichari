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

public partial class ElectronTicket_HPJX_Receive : System.Web.UI.Page
{
    private const int TimeoutSeconds = 100;

    private string TransType;
    private string TransMessage;

    private bool ElectronTicket_HPJX_Status_ON;
    private string ElectronTicket_HPJX_Getway;
    private string ElectronTicket_HPJX_Pay_Getway;
    private string ElectronTicket_HPJX_UserName;
    private string ElectronTicket_HPJX_UserPassword;
    private string ElectronTicket_HPJX_IP;

    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();

        ElectronTicket_HPJX_Status_ON = so["ElectronTicket_HPJX_Status_ON"].ToBoolean(false);
        ElectronTicket_HPJX_Getway = so["ElectronTicket_HPJX_Getway"].ToString("");
        ElectronTicket_HPJX_UserName = so["ElectronTicket_HPJX_UserName"].ToString("");
        ElectronTicket_HPJX_UserPassword = so["ElectronTicket_HPJX_UserPassword"].ToString("");
        ElectronTicket_HPJX_IP = so["ElectronTicket_HPJX_IP"].ToString("").Trim();
        ElectronTicket_HPJX_Pay_Getway = so["ElectronTicket_HPJX_PayGetway"].ToString("").Trim();

        if (!ElectronTicket_HPJX_Status_ON)
        {
            this.Response.End();

            return;
        }

        if (!this.IsPostBack)
        {
            if (!String.IsNullOrEmpty(ElectronTicket_HPJX_IP))
            {
                ElectronTicket_HPJX_IP = "," + ElectronTicket_HPJX_IP + ",";

                if (ElectronTicket_HPJX_IP.IndexOf("," + GetClientIPAddress() + ",") < 0)
                {
                    new Log("ElectronTicket\\HPJX").Write("电子票异常客户端 IP 请求。" + GetClientIPAddress());

                    this.Response.End();

                    return;
                }
            }

            TransType = Shove._Web.Utility.GetRequest("transType");
            TransMessage = Shove._Web.Utility.GetRequest("transMessage");

            WriteElectronTicketLog(false, TransType, "transType=" + TransType + "&transMessage=" + TransMessage);

            if ((TransType != "") && (TransMessage != ""))
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
            case "101":
                IsuseNotice(TransMessage);              // 接收奖期通知
                break;
            case "104":
                TicketInfo(TransMessage);              // 票投注信息返回
                break;
            case "108":
                IsuseOpenNotice(TransMessage);          // 接收开奖通知
                break;
            case "109":
                PayResultNotice(TransMessage);          // 接收充值结果通知
                break;
            case "112":
                DistillResultNotice(TransMessage);      // 接收提款结果通知
                break;
            default:
                break;
        }
    }

    // 接收奖期通知
    private void IsuseNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;
        System.Xml.XmlNodeList nodesIssue = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));

            nodes = XmlDoc.GetElementsByTagName("*");
            nodesIssue = XmlDoc.GetElementsByTagName("issue");
        }
        catch { }

        if (nodes == null)
        {
            this.Response.End();

            return;
        }

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!(nodes[i].Name.ToUpper() == "BODY" && nodes[i].FirstChild.Name.ToUpper() == "ISSUENOTIFY"))
            {
                continue;
            }

            for (int j = 0; j < nodesIssue.Count; j++)
            {
                string IsuseName = nodesIssue[j].Attributes["number"].Value;
                string LotteryName = nodesIssue[j].Attributes["gameName"].Value;
                string Status = nodesIssue[j].Attributes["status"].Value;
                string StartTime = nodesIssue[j].Attributes["startTime"].Value;
                string EndTime = nodesIssue[j].Attributes["stopTime"].Value;
                int LotteryID = GetLotteryID(LotteryName);
                string WinNumber = "";

                try
                {
                    WinNumber = GetWinNumber(LotteryID, nodesIssue[j].Attributes["bonusCode"].Value);
                }
                catch { }

                if ((LotteryID < 0) || (String.IsNullOrEmpty(IsuseName)))
                {
                    continue;
                }

                if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IsuseName) + "'") < 1)
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

                DataTable dtIsuse = t_Isuses.Open("ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IsuseName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 103)", "");

                if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
                {
                    continue;
                }

                if (Status == "4")
                {
                    int ReturnValue = 0;
                    string ReturnDescprtion = "";

                    if (DAL.Procedures.P_ElectronTicketAgentSchemeQuash(Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), 0), ref ReturnValue, ref ReturnDescprtion) < 0)
                    {
                        new Log("ElectronTicket\\HPJX").Write("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash");

                        continue;
                    }
                }

                if (dtIsuse.Rows[0]["State"].ToString() != Status)
                {
                    int ReturnValue = 0;
                    string ReturnDescprtion = "";

                    if (DAL.Procedures.P_IsuseUpdate(LotteryID, IsuseName, Shove._Convert.StrToShort(Status, 0), Shove._Convert.StrToDateTime(StartTime, DateTime.Now.ToString()), Shove._Convert.StrToDateTime(EndTime, DateTime.Now.ToString()), DateTime.Now, WinNumber, ref ReturnValue, ref ReturnDescprtion) < 0)
                    {
                        new Log("ElectronTicket\\HPJX").Write("电子票期号状态更新错误_P_IsuseUpdate");

                        continue;
                    }
                }

                if (!String.IsNullOrEmpty(WinNumber) && (dtIsuse.Rows[0]["WinLotteryNumber"].ToString() != WinNumber))
                {
                    if (LotteryID == SLS.Lottery.JXSSC.ID)
                    {
                        DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open("", "LotteryID =" + LotteryID.ToString(), "");

                        double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];

                        for (int k = 0; k < dtWinTypes.Rows.Count; k++)
                        {
                            WinMoneyList[k * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoney"].ToString(), 1);
                            WinMoneyList[k * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoneyNoWithTax"].ToString(), 1);
                        }

                        DataTable dtChaseTaskDetails = new DAL.Tables.T_ChaseTaskDetails().Open("", "IsuseID=" + dtIsuse.Rows[0]["ID"].ToString() + " and SchemeID IS NOT NULL", "");

                        for (int k = 0; k < dtChaseTaskDetails.Rows.Count; k++)
                        {
                            string LotteryNumber = dtChaseTaskDetails.Rows[k]["LotteryNumber"].ToString();

                            string Description = "";
                            double WinMoneyNoWithTax = 0;

                            double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtChaseTaskDetails.Rows[k]["PlayTypeID"].ToString()), WinMoneyList);

                            if (WinMoney < 1)
                            {
                                continue;
                            }

                            int ReturnValue = 0;
                            string ReturnDescprtion = "";

                            if (DAL.Procedures.P_ChaseTaskStopWhenWin(Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SiteID"].ToString(), 1), Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SchemeID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescprtion) < 0)
                            {
                                new Log("ElectronTicket\\HPJX").Write("电子票撤销追号错误_P_ChaseTaskStopWhenWin。");
                            }
                        }
                    }
                }
            }
        }

        string MessageID = nodes[0].Attributes["id"].Value;
        ReNotice(MessageID, "501");
    }

    //票投注信息返回
    private void TicketInfo(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;
        System.Xml.XmlNodeList nodesTicket = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));

            nodes = XmlDoc.GetElementsByTagName("*");
            nodesTicket = XmlDoc.GetElementsByTagName("ticket");
        }
        catch { }

        if (nodes == null)
        {
            this.Response.End();

            return;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!(nodes[i].Name.ToUpper() == "BODY" && nodes[i].FirstChild.Name.ToUpper() == "TICKETNOTIFY"))
            {
                continue;
            }

            for (int j = 0; j < nodesTicket.Count; j++)
            {
                string TicketID = nodesTicket[j].Attributes["id"].Value;
                string Status = nodesTicket[j].Attributes["status"].Value;
                bool IsSuccessTicket = false;

                if (Status == "2048")     // 重复发送的投注票
                {
                    continue;
                }

                if (Status == "0017")     // 重复发送的MessageID
                {
                    continue;
                }

                if (Status == "0000")
                {
                    IsSuccessTicket = true;
                }

                string DealTime = nodesTicket[j].Attributes["dealTime"].Value;

                int ReturnValue = 0;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, DateTime.Now, IsSuccessTicket, Status, ref ReturnValue, ref ReturnDescription);

                if ((Result < 0) || ((ReturnValue < 0) && (ReturnValue != -2)))
                {
                    new Log("ElectronTicket\\HPJX").Write("对返回电子票信息第一次处理出错_P_SchemesSendToCenterHandle，票ID：" + TicketID);

                    System.Threading.Thread.Sleep(1000);

                    ReturnValue = 0;
                    ReturnDescription = "";

                    Result = DAL.Procedures.P_SchemesSendToCenterHandle(TicketID, Shove._Convert.StrToDateTime(DealTime, DateTime.Now.ToString()), true, "000", ref ReturnValue, ref ReturnDescription);

                    if ((Result < 0) || ((ReturnValue < 0) && (ReturnValue != -2)))
                    {
                        new Log("ElectronTicket\\HPJX").Write("对返回电子票信息第二次处理出错_P_SchemesSendToCenterHandle，票ID：" + TicketID);

                        continue;
                    }
                }
            }
        }

        string MessageID = nodes[0].Attributes["id"].Value;
        ReNotice(MessageID, "504");
    }

    // 接收开奖通知
    private void IsuseOpenNotice(string Transmessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;
        System.Xml.XmlNodeList nodesBonusItem = null;
        System.Xml.XmlNodeList nodesIssue = null;

        try
        {
            XmlDoc.Load(new StringReader(Transmessage));

            nodes = XmlDoc.GetElementsByTagName("*");
            nodesBonusItem = XmlDoc.GetElementsByTagName("bonusItem");
            nodesIssue = XmlDoc.GetElementsByTagName("issue");
        }
        catch { }

        if (nodes == null)
        {
            return;
        }

        string BonusNumber = "";

        for (int j = 0; j < nodes.Count; j++)
        {
            if (!(nodes[j].Name.ToUpper() == "BODY" && nodes[j].FirstChild.Name.ToUpper() == "BONUSNOTIFY"))
            {
                continue;
            }

            BonusNumber = nodes[j].FirstChild.Attributes["bonusNumber"].InnerText;
        }

        if (nodesIssue == null)
        {
            this.Response.End();

            return;
        }

        string MessageID = nodes[0].Attributes["id"].Value;
        string number = nodesIssue[0].Attributes["number"].Value;
        string LotteryName = nodesIssue[0].Attributes["gameName"].Value;

        int LotteryID = GetLotteryID(LotteryName);

        string WinNumber = GetWinNumber(LotteryID, BonusNumber);

        DataTable dtIsuse = new DAL.Tables.T_Isuses().Open("", " [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + " and IsOpened = 0 and LotteryID  in (select id from T_Lotteries where PrintOutType = 103)", "");

        if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
        {
            this.Response.End();

            return;
        }

        string IsuseID = dtIsuse.Rows[0]["ID"].ToString();

        DAL.Tables.T_Isuses T_Isuses = new DAL.Tables.T_Isuses();

        T_Isuses.WinLotteryNumber.Value = WinNumber;
        T_Isuses.OpenOperatorID.Value = 1;
        T_Isuses.Update(" ID = " + IsuseID);


        DataTable dtWinTypesSSL = new DAL.Tables.T_WinTypes().Open("", " LotteryID =" + LotteryID.ToString(), "");

        if ((dtWinTypesSSL != null) && dtWinTypesSSL.Rows.Count > 0)
        {
            double[] WinMoneyList = new double[dtWinTypesSSL.Rows.Count * 2];

            double DefaultMoney = 0;
            double DefaultMoneyNoWithTax = 0;

            for (int i = 0; i < dtWinTypesSSL.Rows.Count; i++)
            {
                DefaultMoney = Shove._Convert.StrToDouble(dtWinTypesSSL.Rows[i]["DefaultMoney"].ToString(), 0);
                DefaultMoneyNoWithTax = Shove._Convert.StrToDouble(dtWinTypesSSL.Rows[i]["DefaultMoneyNoWithTax"].ToString(), 0);

                WinMoneyList[i * 2] = DefaultMoney == 0 ? 1 : DefaultMoneyNoWithTax;
                WinMoneyList[i * 2 + 1] = DefaultMoneyNoWithTax == 0 ? 1 : DefaultMoneyNoWithTax;
            }

            DataTable dtSchemesWithTaskDetails = new DAL.Tables.T_Schemes().Open("", "IsuseID = " + IsuseID + " and WinMoney = 0  and Buyed = 1 and isnull(Identifiers, '') = ''", "");

            string LotteryNumber = "";
            SLS.Lottery.LotteryBase lb = new SLS.Lottery()[LotteryID];

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
                    Shove.Database.MSSQL.ExecuteNonQuery("update T_Schemes set PreWinMoney = @p1, PreWinMoneyNoWithTax = @p2, WinMoney = @p3, WinMoneyNoWithTax = @p4, WinDescription = @p5 where [ID] = " + dtSchemesWithTaskDetails.Rows[i]["ID"].ToString(),
                        new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtSchemesWithTaskDetails.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtSchemesWithTaskDetails.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p3", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney * Shove._Convert.StrToInt(dtSchemesWithTaskDetails.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p4", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax * Shove._Convert.StrToInt(dtSchemesWithTaskDetails.Rows[i]["Multiple"].ToString(), 1)),
                        new Shove.Database.MSSQL.Parameter("p5", SqlDbType.VarChar, 0, ParameterDirection.Input, Description));

                    continue;
                }
            }
        }

        string BonusXML = "<Schemes>";
        string AgentBonusXML = "<Schemes>";

        if ((nodesBonusItem != null) && (nodesBonusItem.Count > 0))
        {
            string bonusItemXML = Transmessage.Substring(Transmessage.IndexOf("<bonusNotify"), Transmessage.LastIndexOf("</body>") - Transmessage.IndexOf("<bonusNotify"));
            DataSet dsXML = new DataSet();

            try
            {
                dsXML.ReadXml(new StringReader(bonusItemXML));
            }
            catch (Exception e)
            {
                new Log("ElectronTicket\\HPJX").Write("电子票开奖，第 " + number + " 期解析开奖数据错误：" + e.Message);

                this.Response.End();

                return;
            }

            if ((dsXML == null) || (dsXML.Tables.Count < 3))
            {
                new Log("ElectronTicket\\HPJX").Write("电子票开奖，第 " + number + " 期开奖数据格式不符合要求。");

                this.Response.End();

                return;
            }

            DataTable dtTickets = dsXML.Tables[2];
            DataTable dtSchemes = MSSQL.Select("SELECT SchemeID, 0 AS AgentID, SchemesMultiple as Multiple, Identifiers FROM V_SchemesSendToCenter WHERE (IsuseID = " + IsuseID + ")");

            if (dtSchemes == null)
            {
                new Log("ElectronTicket\\HPJX").Write("电子票开奖，第 " + number + " 期，读取本地方案错误。");

                this.Response.End();

                return;
            }

            try
            {
                var query1 = from NewDtTickets in dtTickets.AsEnumerable()
                             join NewdtScheme in dtSchemes.AsEnumerable()
                             on NewDtTickets.Field<string>("ticketID") equals NewdtScheme.Field<string>("Identifiers")
                             select new
                             {
                                 ID = NewdtScheme.Field<long>("SchemeID"),
                                 AgentID = 0,//NewdtScheme.Field<long>("AgentID"),
                                 Multiple = NewdtScheme.Field<int>("Multiple"),
                                 Bonus = Shove._Convert.StrToDouble(NewDtTickets.Field<string>("money"), 0),
                                 BonusLevel = NewDtTickets.Field<string>("bonusLevel"),
                                 Size = Shove._Convert.StrToInt(NewDtTickets.Field<string>("size"), 1)
                             };

                var query2 = from NewDt in query1.AsQueryable()
                             group NewDt by new { NewDt.ID, NewDt.BonusLevel, NewDt.AgentID, NewDt.Multiple } into gg
                             select new
                             {
                                 ID = gg.Key.ID,
                                 AgentID = gg.Key.AgentID,
                                 Multiple = gg.Key.Multiple,
                                 Bonus = gg.Sum(NewDt => NewDt.Bonus),
                                 BonusLevel = GetSchemeWinDescription(gg.Key.BonusLevel, LotteryID, (gg.Sum(NewDt => NewDt.Size) / gg.Key.Multiple))
                             };

                var query3 = from NewDt in query2.AsQueryable()
                             group NewDt by new { NewDt.ID, NewDt.Multiple, NewDt.AgentID } into t_dtSchemes
                             select new
                             {
                                 SchemeID = t_dtSchemes.Key.ID,
                                 AgentID = t_dtSchemes.Key.AgentID,
                                 Multiple = t_dtSchemes.Key.Multiple,
                                 Bonus = t_dtSchemes.Sum(NewDt => NewDt.Bonus),
                                 BonusLevel = t_dtSchemes.Merge(NewDt => NewDt.BonusLevel) + ((t_dtSchemes.Key.Multiple != 1) ? "(" + t_dtSchemes.Key.Multiple.ToString() + "倍)" : "")
                             };

                foreach (var Scheme in query3)
                {
                    if (Scheme.AgentID == 0)
                    {
                        BonusXML += "<Scheme SchemeID=\"" + Scheme.SchemeID.ToString() + "\" WinMoney=\"" + Scheme.Bonus.ToString() + "\" WinDescription=\"" + Scheme.BonusLevel + "\" />";
                    }
                    else
                    {
                        AgentBonusXML += "<Scheme SchemeID=\"" + Scheme.SchemeID.ToString() + "\" WinMoney=\"" + Scheme.Bonus.ToString() + "\" WinDescription=\"" + Scheme.BonusLevel + "\" />";
                    }
                }
            }
            catch (Exception e)
            {
                new Log("ElectronTicket\\HPJX").Write("电子票开奖，第 " + number + " 期详细中奖数据解析错误：" + e.Message);

                this.Response.End();

                return;
            }
        }

        BonusXML += "</Schemes>";
        AgentBonusXML += "</Schemes>";

        dtIsuse = new DAL.Tables.T_Isuses().Open("", "[ID] = " + IsuseID + " and IsOpened = 0", "");

        if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
        {
            this.Response.End();

            return;
        }

        int ReturnValue = 0;
        string ReturnDescription = "";

        DataSet ds = null;
        int Times = 0;
        int Result = -1;

        while ((Result < 0) && (Times < 5))
        {
            ReturnValue = 0;
            ReturnDescription = "";

            Result = DAL.Procedures.P_ElectronTicketWin(ref ds, Shove._Convert.StrToLong(IsuseID, 0), BonusXML, AgentBonusXML, ref ReturnValue, ref ReturnDescription);

            if (Result < 0)
            {
                new Log("ElectronTicket\\HPJX").Write("电子票第 " + (Times + 1).ToString() + " 次派奖出现错误(IsuseOpenNotice) 期号为: " + number + "，彩种为: " + LotteryID.ToString());
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
            new Log("ElectronTicket\\HPJX").Write("电子票派奖出现错误(IsuseOpenNotice) 期号为: " + number + "，彩种为: " + LotteryID.ToString() + "，错误：" + ReturnDescription);

            this.Response.End();

            return;
        }

        PF.SendWinNotification(ds);

        DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open("", " LotteryID =" + LotteryID.ToString(), "");

        if ((dtWinTypes != null) && dtWinTypes.Rows.Count > 0)
        {

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

            DataTable dtSchemesWithTaskDetails = new DAL.Views.V_Schemes().Open("", "IsuseID = " + IsuseID + " and IsuseName = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + " and WinMoney = 0  and Buyed = 0 and ID in ( select ID from V_ChaseTaskDetails where IsuseName = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + ")", "");

            string LotteryNumber = "";
            SLS.Lottery.LotteryBase lb = new SLS.Lottery()[LotteryID];

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
                        new Log("ElectronTicket\\HPJX").Write("执行电子票--判断是否停止追号的时候出现错误");
                    }

                    continue;
                }
            }
        }

        MessageID = nodes[0].Attributes["id"].Value;
        ReNotice(MessageID, "508");
    }

    private int P_ElectronTicketWin(SqlConnection conn, ref DataSet ds, long IsuseID, string BonusXML, string AgentBonusXML, ref int ReturnValue, ref  string ReturnDescription)
    {
        MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

        int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketWin", ref ds, ref Outputs,
            new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
            new MSSQL.Parameter("BonusXML", SqlDbType.NText, 0, ParameterDirection.Input, BonusXML),
            new MSSQL.Parameter("AgentBonusXML", SqlDbType.NText, 0, ParameterDirection.Input, AgentBonusXML),
            new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
            new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
            );

        return CallResult;
    }

    // 对恒朋-上海返回的信息进行验证
    private bool ValidMessage(string TransType, string TransMessage)
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

        string TimeStamp = "";
        string MessageId = "";
        string Digest = "";
        string Body = "";

        for (int j = 0; j < nodes.Count; j++)
        {
            if (nodes[j].Name.ToUpper() == "MESSAGE")
            {
                if (ElectronTicket_HPJX_UserName != GetFromXPath(TransMessage, "message/header/messengerID"))
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

        return (Digest.ToLower() == Shove._Security.Encrypt.MD5(MessageId + TimeStamp + ElectronTicket_HPJX_UserPassword + Body, "gb2312").ToLower());
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
        Message += "<messengerID>" + ElectronTicket_HPJX_UserName + "</messengerID>";
        Message += "<timestamp>" + TimeStamp + "</timestamp>";
        Message += "<transactionType>" + Type + "</transactionType>";
        Message += "<digest>" + Shove._Security.Encrypt.MD5(MessageID + TimeStamp + ElectronTicket_HPJX_UserPassword + Body, "gb2312") + "</digest>";
        Message += "</header>";
        Message += Body;
        Message += "</message>";

        Response.Write("transType=" + Type + "&transMessage=" + Message);

        WriteElectronTicketLog(true, Type, "transType=" + Type + "&transMessage=" + Message);
        Response.End();
    }

    // 接收充值结果通知
    private void PayResultNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;
        System.Xml.XmlNodeList nodesFill = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));

            nodes = XmlDoc.GetElementsByTagName("*");
            nodesFill = XmlDoc.GetElementsByTagName("fill");
        }
        catch { }


        for (int i = 0; i < nodes.Count; i++)
        {
            if (!(nodes[i].Name.ToUpper() == "BODY" && nodes[i].FirstChild.Name.ToUpper() == "FillNOTIFY"))
            {
                continue;
            }

            //这里只会执行一次这样的循环
            //循环更新数据库
            for (int j = 0; j < nodesFill.Count; j++)
            {
                string Id = nodesFill[j].Attributes["id"].Value;
                string UserName = nodesFill[j].Attributes["userName"].Value;
                int Status = Shove._Convert.StrToInt(nodesFill[j].Attributes["status"].Value, -1);
                double Money = Shove._Convert.StrToDouble(nodesFill[j].Attributes["money"].Value, 0);

                if ((string.IsNullOrEmpty(Id) || (String.IsNullOrEmpty(UserName))))
                {
                    continue;
                }

                if (Status == -1)
                {
                    new Log("System").Write("在线支付：充值失败！" + " 支付号：" + Id);

                    continue;
                }

                if (Status == 0)
                {
                    new Log("System").Write("在线支付：充值正在处理！" + " 支付号：" + Id);

                    continue;
                }

                if (Status == 2)
                {
                    new Log("System").Write("在线支付：不存在此笔充值交易！" + " 支付号：" + Id);

                    continue;
                }

                Users user = new Users(1)[1, UserName];

                if (user == null)
                {
                    new Log("System").Write("在线支付：异常用户数据！" + " 支付号：" + Id);

                    continue;
                }

                //进行补单
                if (WriteUserAccount(user, Id, Money.ToString(), "充值流水号：" + Id))
                {
                    continue;
                }
                else
                {
                    new Log("System").Write("在线支付：写入返回数据出错！" + " 支付号：" + Id);

                    continue;
                }
            }
        }

        string MessageID = nodes[0].Attributes["id"].Value;
        ReNotice(MessageID, "509");

    }

    // 接收提款结果通知
    private void DistillResultNotice(string TransMessage)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;
        System.Xml.XmlNodeList nodesDistill = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));

            nodes = XmlDoc.GetElementsByTagName("*");
            nodesDistill = XmlDoc.GetElementsByTagName("drawing");
        }
        catch { }


        for (int i = 0; i < nodes.Count; i++)
        {
            if (!(nodes[i].Name.ToUpper() == "BODY" && nodes[i].FirstChild.Name.ToUpper() == "DRAWINGNOTIFY"))
            {
                continue;
            }

            //这里只会执行一次这样的循环
            //循环更新数据库
            for (int j = 0; j < nodesDistill.Count; j++)
            {
                string Id = nodesDistill[j].Attributes["id"].Value;
                string UserName = nodesDistill[j].Attributes["userName"].Value;
                int Status = Shove._Convert.StrToInt(nodesDistill[j].Attributes["status"].Value, -1);
                double Money = Shove._Convert.StrToDouble(nodesDistill[j].Attributes["money"].Value, 0);

                if ((string.IsNullOrEmpty(Id) || (String.IsNullOrEmpty(UserName))))
                {
                    continue;
                }

                //status=-1,-2,3 表示提款申请拒绝了，status=0, 1 表示提款申请正在处理，status=2 表示提款申请已经受理

                bool isSuccess = false;
                string memo = "";

                switch (Status)
                {
                    case -1:
                        memo = "无法转到银行卡";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        break;
                    case -2:
                        memo = "已经退单";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        break;
                    case 3:
                        memo = "提款ID不存在";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        break;
                    case 0:
                        memo = "未处理提款单";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        break;
                    case 1:
                        memo = "已处理提款单";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        break;
                    case 2:
                        memo = "已转到银行卡";
                        new Log("System").Write("在线提款：" + memo + "！" + " 提款号：" + Id);
                        isSuccess = true;
                        break;
                }

                //如果提款申请已经处理了
                if ((Status != 0) && (Status != 1))
                {
                    Users user = new Users(1)[1, UserName];

                    if (user == null)
                    {
                        new Log("System").Write("在线提款：异常用户数据！" + " 提款号：" + Id);

                        continue;
                    }

                    DataTable dt = new DAL.Tables.T_UserDistills().Open("ID,AlipayID", "DistillNumber='" + Id + "'", "");

                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        new Log("System").Write("在线提款：数据库不存在此条提款申请！" + " 提款号：" + Id);

                        continue;
                    }

                    string ReturnDescription = "";

                    //完成请求之后，处理此提款记录
                    if (isSuccess)
                    {
                        //int stateResult = -1;

                        //stateResult = user.DistillAccept(Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0), DataCache.Banks[user.BankType] + user.BankName, user.BankCardNumber, "", "", memo, 1, ref ReturnDescription);

                        //if (stateResult < 0)
                        //{
                        //    new Log("System").Write("在线提款失败：本地处理提款出错！描述：" + ReturnDescription + " 提款号：" + Id);

                        //    continue;
                        //}

                        //new Log("System").Write("在线提款成功！提款号：" + Id);
                    }
                    else
                    {
                        //拒绝提款
                        if (user.DistillNoAccept(Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0), memo, user.ID, ref ReturnDescription) < 0)
                        {
                            new Log("System").Write("在线提款失败：本地处理提款出错！描述：" + ReturnDescription + " 提款号：" + Id);

                            continue;
                        }

                        new Log("System").Write("在线提款失败：申请被拒绝！描述：" + memo + " 提款号：" + Id);

                    }
                }
            }
        }

        string MessageID = nodes[0].Attributes["id"].Value;
        ReNotice(MessageID, "512");
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
            case 5:
                return "ssq";
            case 13:
                return "307";
            case 29:
                return "ssl";
            case 58:
                return "601";
            case 59:
                return "155";
            case 60:
                return "4d";
            case 61:
                return "jxssc";
            case 67:
                return "3d";
            default:
                return "";
        }
    }

    private int GetLotteryID(string LotteryName)
    {
        switch (LotteryName)
        {
            case "ssq":
                return 5;
            case "307":
                return 13;
            case "ssl":
                return 29;
            case "601":
                return 58;
            case "155":
                return 59;
            case "4d":
                return 60;
            case "jxssc":
                return 61;
            case "3d":
                return 67;
            default:
                return -1;
        }
    }

    private string GetSchemeWinDescription(string WinList, int LotteryID, int size)
    {
        string Description = "";

        Description += (GetWinDescription(LotteryID, WinList) + size.ToString() + "注");

        return Description;
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
            case 5:
                return WinNumber.Replace(",", " ").Replace("#", " + ");
            case 67:
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
            case 61:
                if (WinNumber.Length < 14)
                {
                    return WinNumber.Replace(",", "");
                }
                else
                {
                    WinNumber = ',' + WinNumber;

                    return WinNumber.Replace(",0", "");
                }
            default:
                return WinNumber;
        }
    }

    private string GetWinDescription(int LotteryID, string Rank)
    {
        switch (LotteryID)
        {
            case 5:
                switch (Rank)
                {
                    case "1":
                        return "一等奖";
                    case "2":
                        return "二等奖";
                    case "3":
                        return "三等奖";
                    case "4":
                        return "四等奖";
                    case "5":
                        return "五等奖";
                    case "6":
                        return "六等奖";
                    case "7":
                        return "快乐星期天";
                    default:
                        return "";
                }
            case 6:
                switch (Rank)
                {
                    case "1":
                        return "直选奖";
                    case "2":
                        return "组3奖";
                    case "3":
                        return "组6奖";
                    default:
                        return "";
                }
            case 13:
                switch (Rank)
                {
                    case "1":
                        return "一等奖";
                    case "2":
                        return "二等奖";
                    case "3":
                        return "三等奖";
                    case "4":
                        return "四等奖";
                    case "5":
                        return "五等奖";
                    case "6":
                        return "六等奖";
                    case "7":
                        return "七等奖";
                    default:
                        return "";
                }
            case 29:
                switch (Rank)
                {
                    case "1":
                        return "直选奖";
                    case "2":
                        return "组三奖";
                    case "3":
                        return "组六奖";
                    case "4":
                        return "前二奖";
                    case "5":
                        return "后二奖";
                    case "6":
                        return "前一奖";
                    case "7":
                        return "后一奖";
                    default:
                        return "";
                }
            case 58:
                switch (Rank)
                {
                    case "1":
                        return "一等奖";
                    case "2":
                        return "二等奖";
                    case "3":
                        return "三等奖";
                    case "4":
                        return "四等奖";
                    case "5":
                        return "五等奖";
                    case "6":
                        return "六等奖";
                    default:
                        return "";
                }
            case 59:
                switch (Rank)
                {
                    case "0":
                        return "特等奖";
                    case "1":
                        return "一等奖";
                    case "2":
                        return "二等奖";
                    case "3":
                        return "派送奖";
                    default:
                        return "";
                }
            case 60:
                switch (Rank)
                {
                    case "1":
                        return "直选奖";
                    case "2":
                        return "组四奖";
                    case "3":
                        return "组六奖";
                    case "4":
                        return "组12奖";
                    case "5":
                        return "组24奖";
                    default:
                        return "";
                }
            case 61:
                switch (Rank)
                {
                    case "1":
                        return "五星奖";
                    case "2":
                        return "四星奖";
                    case "3":
                        return "三星奖";
                    case "4":
                        return "二星奖";
                    case "5":
                        return "一星奖";
                    default:
                        return "";
                }
            default:
                return "";
        }
    }

    //写日志文件
    private void WriteElectronTicketLog(bool isSend, string TransType, string TransMessage)
    {
        new Log("ElectronTicket\\HPJX").Write("isSend: " + isSend.ToString() + "\tTransType: " + TransType + "\t" + TransMessage);
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

    //充值写入账户
    private bool WriteUserAccount(Users _User, string Id, string amount, string Memo)
    {
        //充值金额
        double Money = Shove._Convert.StrToDouble(amount, 0);

        if (Money == 0)
        {
            return false;
        }


        string ReturnDescription = "";
        bool ok = (_User.AddUserBalance(Money, 0, Id, "", Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[PayNumber] = '" + Id + "'", "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log("System").Write("在线支付：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log("System").Write("在线支付：对应的数据未处理");

                return false;
            }
        }

        return ok;
    }


}

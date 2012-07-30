using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using Shove.Database;

public partial class ElectronTicket_SunLotto_Receive : System.Web.UI.Page
{
    private const int TimeoutSeconds = 100;
    private string TransType;
    private string TransMessage;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //TransType = Shove._Web.Utility.GetRequest("cmd");
            //TransMessage = Shove._Web.Utility.GetRequest("msg");
            //WriteElectronTicketLog(false, TransType, "transType=" + TransType + "&transMessage=" + TransMessage);
            //string SunLottoServerAddr = ConfigurationManager.AppSettings["SunServerAddr"];
            //if (!GetClientIPAddress().Contains(SunLottoServerAddr))
            //{
            //    new Log("ElectronTicket\\SunLotto").Write("电子票异常客户端 IP 请求。" + GetClientIPAddress());
            //    this.Response.End();
            //    return;
            //}

            //if ((TransType != "") && (TransMessage != ""))
            //{
            //    if (ValidateMessage(TransType, TransMessage))
            //    {
            //        Receive(TransType, TransMessage);
            //    }
            //}
            TransType = Shove._Web.Utility.GetRequest("cmd");
            var notifyWrapper = new SLS.Notify.NotifyWrapper(false)
            {
                ApiRequestWrap = new SLS.Notify.ApiRequest() { 
                    AgentId = SLS.Common.WebUtils.GetAppSettingValue("SunAgentID"),
                    AgentPwd = SLS.Common.WebUtils.GetAppSettingValue("xicaigufen"),
                    ApiUrl = SLS.Common.WebUtils.GetAppSettingValue("SunPostAddr"),
                    TransType = TransType
                }
            };
            notifyWrapper.Current.Recieve();

            ReturnResponse();
            
        }
    }
    // 对返回的信息进行验证
    private bool ValidateMessage(string TransType, string TransMessage)
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlNodeList nodes = null;
        
        try
        {
            XmlDoc.Load(new StringReader(TransMessage));
            nodes = XmlDoc.GetElementsByTagName("issue");
        }
        catch { return false; }

        if (nodes == null || nodes.Count < 1)
        {
            return false;
        }

        for (int j = 0; j < nodes.Count; j++)
        {
            if (nodes[j].Attributes.Count < 5)
            {
                return false;
            }  
        }
        return true;
    }

    private void Receive(string TransType, string TransMessage)
    {
        switch (TransType)
        {
            case "1000":
                IsuseNotice(TransMessage);              // 接收奖期通知
                break;
        }
    }

    // 接收奖期通知
    private void IsuseNotice(string TransMessage)
    {
        XmlDocument XmlDoc = new XmlDocument();
        //XmlNodeList nodes = null;
        XmlNodeList nodesIssue = null;

        try
        {
            XmlDoc.Load(new StringReader(TransMessage));
            //nodes = XmlDoc.GetElementsByTagName("");
            nodesIssue = XmlDoc.GetElementsByTagName("issue");
        }
        catch {
            this.Response.End();
            return; 
        }

        if (nodesIssue == null)
        {
            this.Response.End();
            return;
        }

        DAL.Tables.T_Isuses t_Isuses = new DAL.Tables.T_Isuses();

        for (int j = 0; j < nodesIssue.Count; j++)
        {
            string lotoid = null; 
            string issue = null; 
            string starttime = null; 
            string endtime = null;
            string bonuscode = null;
            string status = null; 
            int LotteryID = 0;
            string IssueName = null;
            string WinNumber = null;
            var sunlotto = new SLS.Common.EtSunLotto();
            try{
                lotoid = nodesIssue[j].Attributes["lotoid"].Value;
                issue = nodesIssue[j].Attributes["issue"].Value;
                starttime = nodesIssue[j].Attributes["starttime"].Value;
                endtime = nodesIssue[j].Attributes["endtime"].Value;
                status = nodesIssue[j].Attributes["status"].Value;
                LotteryID = sunlotto.GetSystemLotteryID(lotoid);
                IssueName = sunlotto.ConvertIntoSystemIssue(lotoid, issue);
            }
            catch (Exception e){
                new Log("ElectronTicket\\SunLotto").Write(nodesIssue[j].Value + " 错误 : " + e.Message);
                continue; 
            }
            if (nodesIssue[j].Attributes.Count == 6)
            {
                try{
                    bonuscode = nodesIssue[j].Attributes["bonuscode"].Value;
                    WinNumber = sunlotto.ConverToSystemLottoNum(lotoid, bonuscode);
                }
                catch{
                    new Log("ElectronTicket\\SunLotto").Write(nodesIssue[j].Value + " 错误");
                    continue;
                }
            }
            if ((LotteryID < 1) || (String.IsNullOrEmpty(issue)))
            {
                new Log("ElectronTicket\\SunLotto").Write(lotoid + " : 期号 " + issue + " 错误");
                continue;
            }
            long IssueID = 0;
            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'") < 1)
            {
                DateTime _StartTime = DateTime.Now;
                DateTime _EndTime = DateTime.Now;

                try
                {
                    _StartTime = DateTime.ParseExact(starttime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                    _EndTime = DateTime.ParseExact(endtime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
                catch{
                    new Log("ElectronTicket\\SunLotto").Write(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 时间错误");
                    continue;
                }
                string ReturnDescription = "";

                if (DAL.Procedures.P_IsuseAdd(LotteryID, IssueName, _StartTime, _EndTime, "", ref IssueID, ref ReturnDescription) < 0)
                {
                    new Log("ElectronTicket\\SunLotto").Write(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 新增错误");
                    continue;
                }

                if (IssueID < 0){
                    new Log("ElectronTicket\\SunLotto").Write(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 错误");
                    continue;
                }
            }

            DataTable dtIssue = t_Isuses.Open("ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 301)", "");

            if ((dtIssue == null) || (dtIssue.Rows.Count < 1))
            {
                continue;
            }

            if (status == "4")
            {
                int ReturnValue = -1;
                string ReturnDescprtion = "";

                int Result = DAL.Procedures.P_ElectronTicketAgentSchemeQuash(Shove._Convert.StrToLong(dtIssue.Rows[0]["ID"].ToString(), 0), ref ReturnValue, ref ReturnDescprtion);
                if (Result < 0)
                {
                    new Log("ElectronTicket\\SunLotto").Write("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash : " + IssueID.ToString());
                    continue;
                }
            }

            bool isHasUpdate = false;

            if (dtIssue.Rows[0]["State"].ToString() != status)
            {
                isHasUpdate = true;
            }
            /*
            if (!String.IsNullOrEmpty(WinNumber) && (dtIssue.Rows[0]["WinLotteryNumber"].ToString() != WinNumber))
            {
                if (LotteryID == SLS.Lottery.SHSSL.ID)
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

                        int ReturnValue = -1;
                        string ReturnDescprtion = "";

                        int Result = DAL.Procedures.P_ChaseTaskStopWhenWin(Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SiteID"].ToString(), 1), Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SchemeID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescprtion);

                        if (Result < 0)
                        {
                            new Log("ElectronTicket\\HPSH").Write("电子票撤销追号错误_P_ChaseTaskStopWhenWin。");
                        }
                    }
                }
            }
            */
            //if (isHasUpdate)
            //{
            //    int ReturnValue = -1;
            //    string ReturnDescprtion = "";

            //    int Result = DAL.Procedures.P_IsuseUpdate(LotteryID, Shove._Web.Utility.FilteSqlInfusion(IssueName), Shove._Convert.StrToShort(status, 1), , endtime, DateTime.Now, WinNumber, ref ReturnValue, ref ReturnDescprtion);

            //    if (Result < 0)
            //    {
            //        new Log("ElectronTicket\\HPSH").Write("电子票更新期号P_IsuseEdit。");
            //    }

            //    if (ReturnValue < 0)
            //    {
            //        new Log("ElectronTicket\\HPSH").Write(ReturnDescprtion);
            //    }
            //}
        }
        ReturnResponse();
    }

    // 接收开奖通知
    /*
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

        DataTable dtIsuse = new DAL.Tables.T_Isuses().Open("", " [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + " and IsOpened = 0 and LotteryID  in (select id from T_Lotteries where PrintOutType = 102)", "");

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

        string BonusXML = "<Schemes>";
        string AgentBonusXML = "<Schemes>";

        int Result = -1;

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
                new Log("ElectronTicket\\HPSH").Write("电子票开奖，第 " + number + " 期解析开奖数据错误：" + e.Message);

                this.Response.End();

                return;
            }

            if ((dsXML == null) || (dsXML.Tables.Count < 3))
            {
                new Log("ElectronTicket\\HPSH").Write("电子票开奖，第 " + number + " 期开奖数据格式不符合要求。");

                this.Response.End();

                return;
            }

            DataTable dtTickets = dsXML.Tables[2];
            DataTable dtSchemes = MSSQL.Select("SELECT SchemeID, 0 AS AgentID, SchemesMultiple as Multiple, Identifiers FROM V_SchemesSendToCenter WHERE (IsuseID = " + IsuseID + ")");

            if (dtSchemes == null)
            {
                new Log("ElectronTicket\\HPSH").Write("电子票开奖，第 " + number + " 期，读取本地方案错误。");

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
                new Log("ElectronTicket\\HPSH").Write("电子票开奖，第 " + number + " 期详细中奖数据解析错误：" + e.Message);

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

        int ReturnValue = -1;
        string ReturnDescription = "";

        DataSet ds = null;
        int Times = 0;

        while ((Result < 0) && (Times < 5))
        {
            ReturnValue = -1;
            ReturnDescription = "";

            Result = DAL.Procedures.P_ElectronTicketWin(ref ds, Shove._Convert.StrToLong(IsuseID, 0), BonusXML, AgentBonusXML, ref ReturnValue, ref ReturnDescription);

            if (Result < 0)
            {
                new Log("ElectronTicket\\HPSH").Write("电子票第 " + (Times + 1).ToString() + " 次派奖出现错误(IsuseOpenNotice) 期号为: " + number + "，彩种为: " + LotteryID.ToString());
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
            new Log("ElectronTicket\\HPSH").Write("电子票派奖出现错误(IsuseOpenNotice) 期号为: " + number + "，彩种为: " + LotteryID.ToString() + "，错误：" + ReturnDescription);

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

            DataTable dtSchemesWithTaskDetails = new DAL.Views.V_Schemes().Open("", "IsuseID = " + IsuseID + " and IsuseName = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + " and WinMoney = 0  and Buyed = 0 and ID in ( select ID from V_ChaseTaskDetails where IsuseID = " + IsuseID + " and IsuseName = '" + Shove._Web.Utility.FilteSqlInfusion(number) + "' and LotteryID = " + LotteryID.ToString() + ")", "");

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
                    Result = DAL.Procedures.P_ChaseTaskStopWhenWin(Shove._Convert.StrToLong(dtSchemesWithTaskDetails.Rows[i]["SiteID"].ToString(), 0), Shove._Convert.StrToLong(dtSchemesWithTaskDetails.Rows[i]["ID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescription);

                    if (Result < 0)
                    {
                        new Log("ElectronTicket\\HPSH").Write("执行电子票--判断是否停止追号的时候出现错误");
                    }

                    continue;
                }
            }
        }
        ReturnResponse();
    }
    */
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

    
    // 返回成功信息
    private void ReturnResponse()
    {
        string Message = "<?xml version=\"1.0\" encoding=\"utf-8\"?><body><result>0</result></body>";
        Response.Write(Message);
        Response.End();
    }

    #region 公用方法
    /*
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
    */
    private string GetSchemeWinDescription(string WinList, int LotteryID, int size)
    {
        string Description = "";

        Description += (GetWinDescription(LotteryID, WinList) + size.ToString() + "注");

        return Description;
    }
    /*
    private class CompareToLength : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            long _x = Shove._Convert.StrToLong(x.ToString(), -1);
            long _y = Shove._Convert.StrToLong(y.ToString(), -1);

            return _x.CompareTo(_y);
        }
    }
    */

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
            default:
                return "";
        }
    }

    //写日志文件
    private void WriteElectronTicketLog(bool isSend, string TransType, string TransMessage)
    {
        new Log("ElectronTicket\\SunLotto").Write("isSend: " + isSend.ToString() + "\tTransType: " + TransType + "\t" + TransMessage);
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
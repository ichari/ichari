using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Shove.Database;
using System.Web;
using System.Xml;
using System.IO;
using SLS.Common;
using System.Diagnostics;

/// <summary>
/// methods for dealing with eTickets goes here, changes to eTicket provider calls to the corrosponding method and file
/// </summary>
namespace eTicket
{
    public class eTickets
    {
        private SLS.Common.EtSunLotto eTicketProvider;
        private string ConnectionString = null;
        private System.Threading.Thread thread;
        private Log log = new Log("eTicket");
        private Log issLog = new Log("Issue");
        
        public int StateService = 0;   // 0 停止 1 运行中 2 置为停止
        private int eCount = 0;
        private int qCount = 0;
        private int dCount = 0;

        public eTickets(string connectionString)
        {
            eTicketProvider = new SLS.Common.EtSunLotto();
            ConnectionString = connectionString;
        }

        public void Run()
        {
            string AgentID = ConfigurationManager.AppSettings["SunAgentID"];
            string AgentPwd = ConfigurationManager.AppSettings["SunAgentPwd"];
            string UrlPost = ConfigurationManager.AppSettings["SunPostAddr"];

            if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(AgentPwd) || string.IsNullOrEmpty(UrlPost)){
                log.Write("参数配置不完整.");
                return;
            }

            // 已经启动
            if (StateService == 1){
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                StateService = 1;
                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;
                thread.Start();
                log.Write("Sun Lotto eTicket Start.");
            }
        }

        public void Exit()
        {
            StateService = 2;
        }
        
        public void Do()
        {
            while (true)
            {
                if (StateService == 2)
                {
                    log.Write("Sun Lotto eTicket Stopped.");
                    StateService = 0;
                    Stop();
                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位
                eCount++;
                qCount++;
                dCount++;

                #region 10 秒，发送电子票数据
                if (eCount >= 10)
                {
                    eCount = 0;
                    try {
                        WriteTickets();             // 满员方案拆分为票
                    }
                    catch (Exception e)
                    {
                        log.Write("WriteTickets failed warning: " + e.Message);
                    }

                    try {
                        QueryTickets();            // 代购票查询
                    }
                    catch (Exception e) {
                        log.Write("QueryTickets failed warning: " + e.Message);
                    }

                    try {
                        SendTickets();              // 发送代购电子票
                    }
                    catch (Exception e) {
                        log.Write("SendTickets failed warning: " + e.Message);
                    }
                }

                #endregion

                #region 2 分钟，查询奖期状态
                if (qCount >= 60 * 2)
                {
                    qCount = 0;

                    try {
                        QueryIsuseState();      // 查询奖期状态
                    }
                    catch (Exception e) {
                        issLog.Write("QueryIsuseState is Fail: " + e.Message + " Source: " + e.Source);
                    }
                }

                #endregion
            }
        }
        
        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
        
        private void WriteTickets()
        {   
            DataTable dt = new SLS.Dal.Views.V_SchemeSchedules().Open(ConnectionString, "ID, LotteryID, PlayTypeID, LotteryNumber, Multiple, Money, (case LotteryID when 29 then -29 else LotteryID end) as LotteryID_2", "Buyed = 0 and (GetDate() between StartTime and EndTime) and BuyedShare >= Share and PrintOutType = 301 and State = 1 and dateadd(mi, 1, StateUpdateTime) <= GetDate() and LotteryID <> 29", "LotteryID_2, UserType desc, [ID]"); // and isnull(Identifiers, '') = '' removed from condition
            if (dt == null)
            {
                //msg.Send("读取方案错误(WriteTickets)。");
                log.Write("读取方案错误(WriteTickets)。");
                return;
            }

            SLS.Dal.Tables.T_Schemes t_Schemes = new SLS.Dal.Tables.T_Schemes();
            foreach (DataRow dr in dt.Rows)
            {
                long SchemeID = Shove._Convert.StrToLong(dr["ID"].ToString(), -1);
                int LotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), -1);
                string LotteryNumber = dr["LotteryNumber"].ToString();
                int PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), -1);
                int Multiple = Shove._Convert.StrToInt(dr["Multiple"].ToString(), -1);

                if ((SchemeID < 0) || (LotteryID < 0) || (PlayTypeID < 0) || (Multiple < 1))
                {
                    //msg.Send("读取方案错误(WriteTickets)。方案号：" + SchemeID.ToString());
                    log.Write("读取方案错误(WriteTickets)。方案号：" + SchemeID.ToString());
                    continue;
                }

                double Money = 0;
                //etSunLotto.slTicket[] Tickets = null;
                SLS.Lottery.Ticket[] tics = null;
                try
                {
                    if (LotteryID == SLS.Lottery.CQSSC.ID)
                    {
                        if (PlayTypeID == 2803)
                            tics = eTicketProvider.ToElectronicTicket_ZH(PlayTypeID, LotteryNumber, Multiple, 50, ref Money);
                        else
                            tics = new SLS.Lottery()[LotteryID].ToElectronicTicket_HPCQ(PlayTypeID, LotteryNumber, Multiple, 50, ref Money);
                    }
                    else
                        tics = new SLS.Lottery()[LotteryID].ToElectronicTicket_HPSH(PlayTypeID, LotteryNumber, Multiple, 50, ref Money);
                }
                catch(Exception e)
                {
                    log.Write("拆票错误(WriteTickets)。方案号：" + SchemeID.ToString() + "，" + e.Message);
                    continue;
                }

                if (tics == null)
                {
                    log.Write("分解票错误(WriteTickets)。方案号：" + SchemeID.ToString());
                    continue;
                }

                if ((LotteryID != SLS.Lottery.CQSSC.ID) && (Money != Shove._Convert.StrToDouble(dr["Money"].ToString(), -1)))
                {
                    log.Write("异常警告！！！！(WriteTickets)。方案号： " + SchemeID.ToString() + " 的购买金额与实际票的金额不符合！！！！");
                    continue;
                }

                string TicketXML = "<Tickets>";
                foreach (SLS.Lottery.Ticket ticket in tics)
                {
                    TicketXML += "<Ticket LotteryNumber=\"" + ticket.Number + "\" Multiple=\"" + ticket.Multiple + "\" Money=\"" + ticket.Money + "\" />";
                }
                TicketXML += "</Tickets>";

                int ReturnValue = 0;
                string ReturnDescription = "";
                int Result = SLS.Dal.Procedures.P_SchemesSendToCenterAdd(ConnectionString, SchemeID, PlayTypeID, TicketXML, ref ReturnValue, ref ReturnDescription);
                if ((Result < 0) || (ReturnValue < 0))
                {
                    log.Write("票写入错误(WriteTickets)：方案号：" + SchemeID.ToString() + "，" + ReturnDescription);
                }
            }
        }
        
        // 查询奖期状态
        private void QueryIsuseState()
        {
            // 查询的几组条件说明：
            //  1 有效期内未开奖、未开启的
            //  2 已截止未开奖的 (2 days ago = 2280 minutes)
            DataTable dt = new SLS.Dal.Views.V_Isuses().Open(ConnectionString, "[ID], LotteryID, [Name], [EndTime]", "((isOpened = 0 and (Getdate() between StartTime and EndTime) and State = 0) or (isOpened = 0 and DATEDIFF(MINUTE, Getdate(), EndTime) > -2280 AND GETDATE() > EndTime and State < 6)) and PrintOutType = 301", "EndTime");
           
            if (dt == null)
            {
                //msg.Send("期号状态查询错误(QueryIsuseState)。");
                issLog.Write("期号状态查询错误(QueryIsuseState)。");
                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % 100 == 99)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                #region 查询奖期 一次1期
                // inquery xml here
                int lottoID = 0;
                if (!int.TryParse(dt.Rows[i]["LotteryID"].ToString(), out lottoID))
                    continue;
                string ReceiveString = null;
                string ErrorCode = null;
                try
                {   // post to provider and wait for response
                    ErrorCode = eTicketProvider.GetLotteryInfo(lottoID, dt.Rows[i]["Name"].ToString(), out ReceiveString);
                }
                catch{
                    continue;
                }
                string logString = string.Empty;
                if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAll"]) || ConfigurationManager.AppSettings["LogAll"].Equals("true"))
                {
                    XmlDocument xdLog = new XmlDocument();
                    xdLog.LoadXml(ReceiveString);
                    XmlNodeList xNode = xdLog.GetElementsByTagName("body");
                    if (xNode != null && xNode.Count > 0)
                        logString = xNode[0].OuterXml;
                    else
                        logString = ReceiveString;
                }
                issLog.Write("期号: (" + lottoID.ToString() + ") " + dt.Rows[i]["Name"].ToString() + " | " + logString);
                #endregion
                if (ErrorCode == null || ReceiveString == null)
                    continue;
                if (ErrorCode != "0")
                    continue;

                #region 处理结果

                XmlDocument XmlDoc = new XmlDocument();
                XmlNodeList nodesIssue = null;

                try
                {
                    XmlDoc.Load(new StringReader(ReceiveString));
                    nodesIssue = XmlDoc.GetElementsByTagName("issue");
                }
                catch{
                    continue;
                }

                if (nodesIssue == null || nodesIssue.Count < 1)
                    continue;

                SLS.Dal.Tables.T_Isuses t_Isuses = new SLS.Dal.Tables.T_Isuses();
                string lotoid = null;
                string issue = null;
                string Status = null;
                string lottoNum = null;
                int LotteryID = 0;
                string IssueName = null;

                if (nodesIssue[0].Attributes.Count < 1){
                    continue;
                }
                try
                {
                    lotoid = nodesIssue[0].Attributes["lotoid"].Value;
                    issue = nodesIssue[0].Attributes["issue"].Value;
                    Status = nodesIssue[0].Attributes["status"].Value;
                    if (nodesIssue[0].Attributes["bonuscode"] != null)
                        lottoNum = nodesIssue[0].Attributes["bonuscode"].Value;
                    LotteryID = eTicketProvider.GetSystemLotteryID(lotoid);
                    IssueName = eTicketProvider.ConvertIntoSystemIssue(lotoid, issue);
                }
                catch { continue; }
                if ((LotteryID == 0)) { continue; }
                
                DataTable dtIsuse = t_Isuses.Open(ConnectionString, "ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "'", "");
                if ((dtIsuse == null) || (dtIsuse.Rows.Count != 1)){ continue; }

                bool isHasUpdate = false;
                // 奖期状态：0 未开启 1 开始 2 暂停 3 截止 4 期结 5 返奖 6 返奖结束
                // Sun : 0 未开启 1 已开新期 2 暂停 3 截止投注 4 摇出奖号 5 兑奖中 6 结期兑奖
                if (dtIsuse.Rows[0]["State"].ToString() != Status)
                {
                    t_Isuses.State.Value = Status;
                    t_Isuses.StateUpdateTime.Value = DateTime.Now;
                    isHasUpdate = true;
                }
                string WinNumber = null;
                if (lottoNum != null)
                    WinNumber = eTicketProvider.ConverToSystemLottoNum(lotoid, lottoNum);

                if (!String.IsNullOrEmpty(WinNumber) && (dtIsuse.Rows[0]["WinLotteryNumber"].ToString() != WinNumber))
                {
                    t_Isuses.WinLotteryNumber.Value = WinNumber;
                    isHasUpdate = true;

                    if (LotteryID == SLS.Lottery.SHSSL.ID)
                    {
                        DataTable dtWinTypes = new SLS.Dal.Tables.T_WinTypes().Open(ConnectionString, "", "LotteryID =" + LotteryID.ToString(), "");
                        double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];
                        for (int k = 0; k < dtWinTypes.Rows.Count; k++)
                        {
                            WinMoneyList[k * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoney"].ToString(), 1);
                            WinMoneyList[k * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoneyNoWithTax"].ToString(), 1);
                        }

                        DataTable dtChaseTaskDetails = new SLS.Dal.Tables.T_ChaseTaskDetails().Open(ConnectionString, "", "IsuseID=" + dtIsuse.Rows[0]["ID"].ToString() + " and SchemeID IS NOT NULL", "");
                        for (int k = 0; k < dtChaseTaskDetails.Rows.Count; k++)
                        {
                            string LotteryNumber = dtChaseTaskDetails.Rows[k]["LotteryNumber"].ToString();
                            string Description = "";
                            double WinMoneyNoWithTax = 0;
                            double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtChaseTaskDetails.Rows[k]["PlayTypeID"].ToString()), WinMoneyList);

                            if (WinMoney < 1){
                                continue;
                            }
                            int ReturnValue = 0;
                            string ReturnDescprtion = "";
                            if (SLS.Dal.Procedures.P_ChaseTaskStopWhenWin(ConnectionString, Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SiteID"].ToString(), 1), Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SchemeID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescprtion) < 0)
                            {
                                log.Write("(QueryIsuseState)电子票撤销追号错误 P_ChaseTaskStopWhenWin。");
                            }
                        }
                    }
                }
                
                if (isHasUpdate){
                    t_Isuses.Update(ConnectionString, "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + IssueName + "'");
                }
                #endregion
            }
        }

        // 代购票查询
        private void QueryTickets()
        {
            DataTable dt = new SLS.Dal.Views.V_SchemesSendToCenter().Open(ConnectionString, "distinct SchemeID", "(((Sends > 0) AND (Sends < 100)) or (sends = 3301) or (sends = 2148)) AND (HandleResult = 0) AND (IsOpened = 0) and LotteryID <> 29  and PrintOutType = 301", "");

            if (dt == null){
                log.Write("查询代购票出错(QueryTickets)：读取未成功票错误。");
                return;
            }
            if (dt.Rows.Count < 1){
                return;
            }
            string lastOrder = null;
            string ReceiveString = null;
            string ErrorCode = null;
            DataTable dtSchemesSendToCenter = null;
            SLS.Dal.Tables.T_SchemesSendToCenter t_SchemesSendToCenter = new SLS.Dal.Tables.T_SchemesSendToCenter();
            // process all schemes that is sent to provider
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtSchemesSendToCenter = new SLS.Dal.Tables.T_SchemesSendToCenter().Open(ConnectionString, "top 1 *", "schemeid=" + dt.Rows[i]["SchemeID"].ToString() + " and (Sends > 0) AND (Sends < 100)", "");
                if (dtSchemesSendToCenter == null){
                    continue;
                }
                if (dtSchemesSendToCenter.Rows.Count < 1){
                    continue;
                }
                string orderNumber = dtSchemesSendToCenter.Rows[0]["Identifiers"].ToString();
                if (orderNumber == null || orderNumber == ""){
                    continue;
                }
                if (orderNumber != lastOrder)
                {
                    try{
                        ErrorCode = eTicketProvider.CheckOrderStatus(orderNumber, out ReceiveString);
                    }
                    catch{
                        continue;
                    }
                    lastOrder = orderNumber;
                }
                if (ErrorCode == null)
                    continue;
                if ("100 101 102 103 104 105 106 107 108 111 301 302 303 304 305 306 350 360 400 401".Split(' ').Contains(ErrorCode))
                {
                    t_SchemesSendToCenter.Sends.Value = ErrorCode + 100;
                    t_SchemesSendToCenter.Update(ConnectionString, "SchemeID = " + dt.Rows[i]["SchemeID"].ToString());
                    continue;
                }
                if (ErrorCode == "112" || ErrorCode == "402"){
                    log.Write("IP / AgentID cannot be used, Code: " + ErrorCode);
                    return;
                }
                if (ErrorCode != "0")
                    continue;
                XmlDocument XmlDoc = new XmlDocument();
                XmlNodeList orderNodes = null;
                XmlNodeList ticketNodes = null;
                string orderStatus = null;
                try
                {
                    XmlDoc.Load(new StringReader(ReceiveString));
                    orderNodes = XmlDoc.GetElementsByTagName("order");
                    orderStatus = orderNodes[0].Attributes["status"].Value;
                    ticketNodes = XmlDoc.GetElementsByTagName("ticket");
                }
                catch { continue; }

                if (ticketNodes == null || ticketNodes.Count == 0){
                    continue;
                }
                if (orderStatus == "0"){
                    continue;
                }
                // Order is successful process all tickets at once
                if (orderStatus == "1")
                {   
                    int ReturnValue = 0;
                    string ReturnDescription = "";
                    int Result = SLS.Dal.Procedures.P_SchemesSendToCenterHandleUniteAnte(ConnectionString, Shove._Convert.StrToLong(dt.Rows[i]["SchemeID"].ToString(), -1), DateTime.Now, true, ref ReturnValue, ref ReturnDescription);
                    if ((Result < 0) || (ReturnValue < 0))
                    {
                        log.Write("对所查询到的电子票数据第一次处理出错(QueryTickets)：数据读写错误。Scheme：" + dt.Rows[i]["SchemeID"].ToString() + "，" + ReturnDescription);
                        System.Threading.Thread.Sleep(1000);
                        Result = SLS.Dal.Procedures.P_SchemesSendToCenterHandleUniteAnte(ConnectionString, Shove._Convert.StrToLong(dt.Rows[i]["SchemeID"].ToString(), -1), DateTime.Now, true, ref ReturnValue, ref ReturnDescription);
                        if ((Result < 0) || (ReturnValue < 0)){
                            log.Write("对所查询到的电子票数据第二次处理出错(QueryTickets)：数据读写错误。票号：" + dt.Rows[i]["SchemeID"].ToString() + "，" + ReturnDescription);
                        }
                    }
                    continue;
                }
                // order failed
                if (orderStatus == "3")
                {
                    t_SchemesSendToCenter.Sends.Value = 999;
                    t_SchemesSendToCenter.Update(ConnectionString, "SchemeID = " + dt.Rows[i]["SchemeID"].ToString());
                    continue;
                }
                // not done
                // parts of order failed, process individual tickets returned
                if (orderStatus == "2")
                {
                    string ticketFailed = string.Empty;
                    for (int k = 0; k < ticketNodes.Count; k++)
                    {
                        try
                        {
                            string ticketStatus = ticketNodes[k].Attributes["status"].Value;
                            string ticketSeq = ticketNodes[k].Attributes["seq"].Value;
                            if (ticketStatus.ToLower() == "N")
                            {
                                ticketFailed += ticketSeq + ",";
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    ticketFailed = "(" + ticketFailed.Trim().Trim(',') + ")";
                    if (MSSQL.ExecuteNonQuery(ConnectionString, "update T_SchemesSendToCenter set Sends = 1000 where (SchemeID = " + dt.Rows[i]["SchemeID"].ToString() + ") AND ((ID % 100000) in " + ticketFailed + ") ") < 0)
                    {
                        log.Write("更新票Partially Failed(SendTickets)。方案ID：" + dt.Rows[i]["SchemeID"].ToString());
                    }
                }
            }
        }

        // 发送代购电子票
        private void SendTickets()
        {
            SLS.Dal.Views.V_SchemesSendToCenter v_SchemesSendToCenter = new SLS.Dal.Views.V_SchemesSendToCenter();
            //changed state = 1 to state = 0 for testing purposes
            // selects all scheme to be sent to provider
            //DataTable dt = v_SchemesSendToCenter.Open(ConnectionString, "distinct SchemeID, SiteID, UserType", "Buyed = 0 and (GetDate() between StartTime and EndTime) and Sends < 99 and HandleResult = 0 and State = 0 and LotteryID <> 29 and PrintOutType = 301", " UserType desc");
            // changed to select all schemes from same lottery and issue to be sent to provider
            DataTable dt = v_SchemesSendToCenter.Open(ConnectionString, "distinct LotteryID, SiteID, IsuseName", "Buyed = 0 and QuashStatus = 0 and (GetDate() between StartTime and EndTime) and Sends < 99 and HandleResult = 0 and State = 1 and LotteryID <> 29 and PrintOutType = 301", "");
            
            if (dt == null){
                log.Write("发送代购票出错(SendTickets)：读取方案错误。");
                return;
            }
            //log.Write("SendTickets Started " + dt.Rows.Count);
            //SLS.Dal.Tables.T_SchemesSendToCenter t_SchemesSendToCenter = new SLS.Dal.Tables.T_SchemesSendToCenter();
            // each ticket from a certain Issue that is waiting to be sent to provider, dt row # = # of issues waiting to be sent
            Stopwatch counter = new Stopwatch();
            counter.Start();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtSchemesSend = v_SchemesSendToCenter.Open(ConnectionString, "", "LotteryID =" + dt.Rows[i]["LotteryID"].ToString() + " and Buyed = 0 and QuashStatus = 0 and (GetDate() between StartTime and EndTime) and Sends < 99 and HandleResult = 0 and State = 1 and PrintOutType = 301", "SchemeID");
                if (dtSchemesSend == null){
                    log.Write("发送代购票出错(SendTickets)：读取错误。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString());
                    continue;
                }
                if (dtSchemesSend.Rows.Count < 1){
                    continue;
                }
                log.Write("SchemeSendToCenter LotteryID: " + dt.Rows[i]["LotteryID"].ToString() + " | Count: " + dtSchemesSend.Rows.Count.ToString());
                int lotteryID = int.Parse(dt.Rows[i]["LotteryID"].ToString());
                string issueName = dt.Rows[i]["IsuseName"].ToString();
                string ticketXML = string.Empty;

                #region Process SchemeSendToCenter retrieved
                string idString = "(";
                foreach (DataRow drScheme in dtSchemesSend.Rows)
                {
                    long schemeID = 0;
                    string lottoNumber = null;
                    string ticketSeq = null;
                    int playTypeID = 0;
                    string multiplier = null;
                    double total = 0;
                    
                    try
                    {
                        schemeID = long.Parse(drScheme["SchemeID"].ToString());
                        lottoNumber = drScheme["Ticket"].ToString();
                        ticketSeq = string.Format("{0:D5}", long.Parse(drScheme["ID"].ToString()) % 100000);
                        playTypeID = int.Parse(drScheme["PlayTypeID"].ToString());
                        multiplier = drScheme["Multiple"].ToString();
                        total = double.Parse(drScheme["Money"].ToString());
                        idString += drScheme["ID"].ToString() + ",";
                    }
                    catch(Exception e){
                        log.Write("parse data 出错(SendTickets drScheme)。方案ID：" + drScheme["SchemeID"].ToString() + " | M: " + e.Message + " >> " + e.Source);
                        continue;
                    }
                    ticketXML += eTicketProvider.ConvertToTicketXML(ticketSeq, lotteryID, playTypeID, lottoNumber, multiplier, total);
                }
                idString = idString.Trim(',') + ")";
                #endregion

                string orderNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + issueName.Replace("-", "_").PadLeft(12, '0');
                string RealityName = ConfigurationManager.AppSettings["eTicketUserName"];
                string IDCardNumber = ConfigurationManager.AppSettings["eTicketIDNumber"];
                string Mobile = ConfigurationManager.AppSettings["eTicketCellphone"];
                string bodyXML = eTicketProvider.PrepareTicketsXML(orderNumber, IDCardNumber, Mobile, RealityName, lotteryID, issueName, ticketXML);
                string orderXML = eTicketProvider.GenerateOrderXML(bodyXML, orderNumber);
                log.Write("Order XML: " + orderXML);
                string ReceiveString = null;
                string ErrorCode = null;

                while (i > 0 && counter.ElapsedMilliseconds < 10000)
                    System.Threading.Thread.Sleep(1000);
                try
                {
                    ErrorCode = eTicketProvider.OrdereTicketSend(orderXML, out ReceiveString);
                    counter.Reset();
                    counter.Start();
                }
                catch(Exception ex){
                    log.Write("电子票-301 发送失败(SendTickets)。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString() + " | " + ex.Message);
                    counter.Reset();
                    counter.Start();
                    continue;
                }
                //update sends on all Schemes from dtSchemesSend and set identifiers to order number
                if (MSSQL.ExecuteNonQuery(ConnectionString, "update v_SchemesSendToCenter set Sends = Sends + 1, Identifiers = '" + orderNumber + "' where ID in " + idString) < 0)
                {
                    log.Write("更新票发送状态时出错(SendTickets)。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString() + " tickets: " + idString);
                }
                DataTable dtmp = new DataView(dtSchemesSend).ToTable(true, "SchemeID");
                string schemeUpdate = "(";
                foreach (DataRow dr in dtmp.Rows)
                {
                    schemeUpdate += dr["SchemeID"].ToString() + ",";
                }
                schemeUpdate = schemeUpdate.Trim().Trim(',') + ")";
                if (MSSQL.ExecuteNonQuery(ConnectionString, "update T_Schemes set Identifiers = " + orderNumber + " where ID in " + schemeUpdate) < 0)
                {
                    log.Write("更新 Scheme Identifiers error (SendTickets) - order # : " + orderNumber + ", Schemes : " + schemeUpdate);
                }
                if (MSSQL.ExecuteNonQuery(ConnectionString, "update T_SchemesSendToCenter set Identifiers = " + orderNumber + " where SchemeID in " + schemeUpdate) < 0)
                {
                    log.Write("更新 T_SchemesSendToCenter Identifiers error (SendTickets) - order # : " + orderNumber + ", Schemes : " + schemeUpdate);
                }
                //log.Write("Ticket Sent");
                #region Parse and proccess results
                if (ErrorCode == null){
                    log.Write("电子票-301 发送 Error (SendTickets)。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString());
                    continue;
                }
                log.Write("Response Recieved: " + ReceiveString);
                if (ErrorCode == "402") // IP restricted by provider, exit
                {
                    log.Write("IP Restricted (SendTickets)。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString());
                    return;
                }
                // 109 - 10秒内禁止重复发单
                if (ErrorCode == "109"){
                    i--;
                    counter.Reset();
                    counter.Start();
                    System.Threading.Thread.Sleep(10000);
                    continue;
                }
                // 重复发送的投注票
                if (ErrorCode == "305"){
                    log.Write("重复发送的投注票(SendTickets)：。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString());
                    continue;
                }
                int err = 0;
                if(!int.TryParse(ErrorCode, out err))
                    continue;
                if (err >= 100 && err <= 108)
                    continue;
                // Agent ID is blocked
                if (ErrorCode == "112"){
                    log.Write("[Critical] Agent ID 被禁止(SendTickets)：。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString());
                    return;
                }
                // received string is valid process, error code = 0
                int ReturnValue = 0;
                string ReturnDescription = "";
                // selects unique Schemes ID from dtSchemesSend
                DataTable dtUniqueScheme = new DataView(dtSchemesSend).ToTable(true, "SchemeID");
                if (ErrorCode == "0")
                {   // update each scheme when ticket errorcode = 0
                    
                    foreach (DataRow drSch in dtUniqueScheme.Rows)
                    {
                        int Result = SLS.Dal.Procedures.P_SchemesSendToCenterHandleUniteAnte(ConnectionString, Shove._Convert.StrToLong(drSch["SchemeID"].ToString(), -1), DateTime.Now, true, ref ReturnValue, ref ReturnDescription);
                        if ((Result < 0) || (ReturnValue < 0))
                        {
                            log.Write("对所发送的成功落地的代购票第一次处理出错(SendTickets)：数据读写错误。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString() + "，" + ReturnDescription);
                            System.Threading.Thread.Sleep(1100);
                            ReturnValue = 0;
                            ReturnDescription = "";
                            Result = SLS.Dal.Procedures.P_SchemesSendToCenterHandleUniteAnte(ConnectionString, Shove._Convert.StrToLong(drSch["SchemeID"].ToString(), -1), DateTime.Now, true, ref ReturnValue, ref ReturnDescription);
                            if ((Result < 0) || (ReturnValue < 0))
                                log.Write("对所发送的成功落地的代购票第二次处理出错(SendTickets)：数据读写错误。Issue： (" + dt.Rows[i]["LotteryID"].ToString() + ") " + dt.Rows[i]["IsuseName"].ToString() + "，" + ReturnDescription);
                        }
                    }
                    log.Write("Response Proccessed - Ticket OK (" + orderNumber + ")");
                    continue;
                }
                int SiteID = Shove._Convert.StrToInt(dt.Rows[i]["SiteID"].ToString(), 1);
                
                if ("301 302 303 304 306 400 401".Split(' ').Contains(ErrorCode)) // need to cancel this scheme
                {
                    foreach (DataRow dr in dtUniqueScheme.Rows)
                    {
                        int Result = SLS.Dal.Procedures.P_QuashScheme(ConnectionString, SiteID, Shove._Convert.StrToLong(dr["SchemeID"].ToString(), 0), true, false, ref ReturnValue, ref ReturnDescription);
                        if ((Result < 0) || (ReturnValue < 0))
                            log.Write("对所发送落地失败的代购票【作撤单】处理出错(SendTickets)：数据读写错误。票号：" + dr["SchemeID"].ToString() + "，" + ErrorCode + "，" + ReturnDescription);
                    }
                }
                #endregion
            }
        }
    }
}
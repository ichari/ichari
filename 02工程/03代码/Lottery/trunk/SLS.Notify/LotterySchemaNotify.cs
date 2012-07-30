using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace SLS.Notify
{
    /// <summary>
    /// 奖期 查询/通知 封装类
    /// </summary>
    internal class LotterySchemaNotify : NotifyBase
    {
        public override void Recieve()
        {
            base.Recieve();
            if (TransType != CommandType.LotterySchema)
                throw new Common.ElectronicException(string.Format("接口命令错误{0}，应为{1}", TransType, CommandType.LotterySchema));
            OnSendSuccess(TransMsg);
        }

        
        // 接收奖期通知 todo : 逻辑修改
        //private void IsuseNotice(string TransMessage)
        //{
        //    XmlDocument XmlDoc = new XmlDocument();
        //    //XmlNodeList nodes = null;
        //    XmlNodeList nodesIssue = null;

        //    XmlDoc.Load(new StringReader(TransMessage));
        //    //nodes = XmlDoc.GetElementsByTagName("");
        //    nodesIssue = XmlDoc.GetElementsByTagName("issue");

        //    if (nodesIssue == null)
        //        throw new Common.ElectronicException("XML数据分析错误，未找到节点【issue】");

        //    var t_Isuses = new SLS.Dal.Tables.T_Isuses();

        //    for (int j = 0; j < nodesIssue.Count; j++)
        //    {
        //        string lotoid = null;
        //        string issue = null;
        //        string starttime = null;
        //        string endtime = null;
        //        string bonuscode = null;
        //        string status = null;
        //        int LotteryID = 0;
        //        string IssueName = null;
        //        string WinNumber = null;
        //        var sunlotto = new SLS.Common.EtSunLotto();
        //        try
        //        {
        //            lotoid = nodesIssue[j].Attributes["lotoid"].Value;
        //            issue = nodesIssue[j].Attributes["issue"].Value;
        //            starttime = nodesIssue[j].Attributes["starttime"].Value;
        //            endtime = nodesIssue[j].Attributes["endtime"].Value;
        //            status = nodesIssue[j].Attributes["status"].Value;
        //            LotteryID = sunlotto.GetSystemLotteryID(lotoid);
        //            IssueName = sunlotto.ConvertIntoSystemIssue(lotoid, issue);
        //        }
        //        catch (Exception e)
        //        {
        //            base.WriteLog(nodesIssue[j].Value + " 错误 : " + e.Message);
        //            continue;
        //        }
        //        if (nodesIssue[j].Attributes.Count == 6)
        //        {
        //            try
        //            {
        //                bonuscode = nodesIssue[j].Attributes["bonuscode"].Value;
        //                WinNumber = sunlotto.ConverToSystemLottoNum(lotoid, bonuscode);
        //            }
        //            catch
        //            {
        //                base.WriteLog(nodesIssue[j].Value + " 错误");
        //                continue;
        //            }
        //        }
        //        if ((LotteryID < 1) || (String.IsNullOrEmpty(issue)))
        //        {
        //            base.WriteLog(lotoid + " : 期号 " + issue + " 错误");
        //            continue;
        //        }
        //        long IssueID = 0;
        //        if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'") < 1)
        //        {
        //            DateTime _StartTime = DateTime.Now;
        //            DateTime _EndTime = DateTime.Now;

        //            try
        //            {
        //                _StartTime = DateTime.ParseExact(starttime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
        //                _EndTime = DateTime.ParseExact(endtime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
        //            }
        //            catch
        //            {

        //                base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 时间错误");
        //                continue;
        //            }
        //            string ReturnDescription = "";

        //            if (SLS.Dal.Procedures.P_IsuseAdd(ConnectString, LotteryID, IssueName, _StartTime, _EndTime, "", ref IssueID, ref ReturnDescription) < 0)
        //            {
        //                base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 新增错误");
        //                continue;
        //            }

        //            if (IssueID < 0)
        //            {
        //                //new Log("ElectronTicket\\SunLotto").Write(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 错误");
        //                base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 错误");
        //                continue;
        //            }
        //        }

        //        var dtIssue = t_Isuses.Open("ID, State, WinLotteryNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 301)", "");

        //        if ((dtIssue == null) || (dtIssue.Rows.Count < 1))
        //        {
        //            continue;
        //        }

        //        if (status == "4")
        //        {
        //            int ReturnValue = -1;
        //            string ReturnDescprtion = "";

        //            int Result = SLS.Dal.Procedures.P_ElectronTicketAgentSchemeQuash(ConnectString, Shove._Convert.StrToLong(dtIssue.Rows[0]["ID"].ToString(), 0), ref ReturnValue, ref ReturnDescprtion);
        //            if (Result < 0)
        //            {
        //                //new Log("ElectronTicket\\SunLotto").Write("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash : " + IssueID.ToString());
        //                base.WriteLog("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash : " + IssueID.ToString());
        //                continue;
        //            }
        //        }

        //        bool isHasUpdate = false;

        //        if (dtIssue.Rows[0]["State"].ToString() != status)
        //        {
        //            isHasUpdate = true;
        //        }
        //        /*
        //        if (!String.IsNullOrEmpty(WinNumber) && (dtIssue.Rows[0]["WinLotteryNumber"].ToString() != WinNumber))
        //        {
        //            if (LotteryID == SLS.Lottery.SHSSL.ID)
        //            {
        //                DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open("", "LotteryID =" + LotteryID.ToString(), "");

        //                double[] WinMoneyList = new double[dtWinTypes.Rows.Count * 2];

        //                for (int k = 0; k < dtWinTypes.Rows.Count; k++)
        //                {
        //                    WinMoneyList[k * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoney"].ToString(), 1);
        //                    WinMoneyList[k * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[k]["DefaultMoneyNoWithTax"].ToString(), 1);
        //                }

        //                DataTable dtChaseTaskDetails = new DAL.Tables.T_ChaseTaskDetails().Open("", "IsuseID=" + dtIsuse.Rows[0]["ID"].ToString() + " and SchemeID IS NOT NULL", "");

        //                for (int k = 0; k < dtChaseTaskDetails.Rows.Count; k++)
        //                {
        //                    string LotteryNumber = dtChaseTaskDetails.Rows[k]["LotteryNumber"].ToString();
        //                    string Description = "";
        //                    double WinMoneyNoWithTax = 0;

        //                    double WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dtChaseTaskDetails.Rows[k]["PlayTypeID"].ToString()), WinMoneyList);

        //                    if (WinMoney < 1)
        //                    {
        //                        continue;
        //                    }

        //                    int ReturnValue = -1;
        //                    string ReturnDescprtion = "";

        //                    int Result = DAL.Procedures.P_ChaseTaskStopWhenWin(Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SiteID"].ToString(), 1), Shove._Convert.StrToLong(dtChaseTaskDetails.Rows[k]["SchemeID"].ToString(), 0), WinMoney, ref ReturnValue, ref ReturnDescprtion);

        //                    if (Result < 0)
        //                    {
        //                        new Log("ElectronTicket\\HPSH").Write("电子票撤销追号错误_P_ChaseTaskStopWhenWin。");
        //                    }
        //                }
        //            }
        //        }
        //        */
        //        //if (isHasUpdate)
        //        //{
        //        //    int ReturnValue = -1;
        //        //    string ReturnDescprtion = "";

        //        //    int Result = DAL.Procedures.P_IsuseUpdate(LotteryID, Shove._Web.Utility.FilteSqlInfusion(IssueName), Shove._Convert.StrToShort(status, 1), , endtime, DateTime.Now, WinNumber, ref ReturnValue, ref ReturnDescprtion);

        //        //    if (Result < 0)
        //        //    {
        //        //        new Log("ElectronTicket\\HPSH").Write("电子票更新期号P_IsuseEdit。");
        //        //    }

        //        //    if (ReturnValue < 0)
        //        //    {
        //        //        new Log("ElectronTicket\\HPSH").Write(ReturnDescprtion);
        //        //    }
        //        //}
        //    }
        //}

        public override string Send(string lotteryTypeId, string issueNo)
        {
            var ass = Assembly.GetExecutingAssembly();
            //载入请求消息格式            
            var sm = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.Request.xml"));
            var reqFormatter = sm.ReadToEnd().Replace("\r\n", "");
            //载入XML请求数据
            var xmlReader = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.Schema.xml"));
            var xmlMsg = string.Format(xmlReader.ReadToEnd().Replace("\r\n",""), lotteryTypeId, issueNo);


            var tick = (DateTime.Now - new DateTime(1900, 1, 1)).Ticks;
            var md5Msg = SLS.Common.WebUtils.GetMd5(this.AgentId + this.AgentPwd + xmlMsg);
            var regex = new System.Text.RegularExpressions.Regex("^<body>(.*)</body>$");
            var reqMsg = regex.Match(xmlMsg).Groups[1].Value;
            var formattedReq = string.Format(reqFormatter
                , tick
                , TransType
                , DateTime.Now.ToString("yyyyMMddHHmmss")
                , md5Msg
                , reqMsg);



            var req = string.Format("cmd={0}&msg={1}", TransType, formattedReq);
            var rs = SLS.Common.WebUtils.Post(this.ApiUrl, req, 10);

            //查询所有奖期
            if (lotteryTypeId == "" && issueNo == "") {
                return "";
            }
            //查询某一彩种，后续逻辑
            OnSendSuccess(rs);
            return rs;
        }

        /// <summary>
        /// 发送奖期查询后续业务逻辑
        /// </summary>
        /// <param name="returnMsg"></param>
        private void OnSendSuccess(string returnMsg)
        {
            var xdoc = XDocument.Parse(returnMsg);
            var q = xdoc.Descendants("issue").FirstOrDefault();
            if (q == null)
                throw new Common.ElectronicException("XML数据分析错误，未找到节点【issue】");
            var attrs = q.Attributes().ToDictionary(t => t.Name.LocalName);

            #region db logic
            var t_Isuses = new SLS.Dal.Tables.T_Isuses();

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
                lotoid = attrs[SunlotXmlDefin.LotoId].Value;
                issue = attrs[SunlotXmlDefin.Issue].Value;
                starttime = attrs[SunlotXmlDefin.StartTime].Value;
                endtime = attrs[SunlotXmlDefin.EndTime].Value;
                status = attrs[SunlotXmlDefin.Status].Value;
                LotteryID = sunlotto.GetSystemLotteryID(lotoid);
                IssueName = sunlotto.ConvertIntoSystemIssue(lotoid, issue);
            }
            catch (Exception e){                    
                base.WriteLog("奖期查询/通知属性分析错误 : " + e.Message); 
            }
            

            //如果包含开奖结果
            if (attrs.ContainsKey(SunlotXmlDefin.BonusCode))
            {
                bonuscode = attrs[SunlotXmlDefin.BonusCode].Value;
                WinNumber = sunlotto.ConverToSystemLottoNum(lotoid, bonuscode);
            }
            if ((LotteryID < 1) || (String.IsNullOrEmpty(issue)))
            {
                base.WriteLog(lotoid + " : 期号 " + issue + " 错误");                      
            }

            long IssueID = 0;
            //新增奖期
            if (t_Isuses.GetCount("LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'") < 1)
            {
                DateTime _StartTime = DateTime.Now;
                DateTime _EndTime = DateTime.Now;

                try {
                    _StartTime = DateTime.ParseExact(starttime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                    _EndTime = DateTime.ParseExact(endtime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                }
                catch {
                    base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 时间错误");
                }
                string ReturnDescription = "";
                //新增奖期
                if (SLS.Dal.Procedures.P_IsuseAdd(ConnectString, LotteryID, IssueName, _StartTime, _EndTime, "", ref IssueID, ref ReturnDescription) < 0)
                    base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 新增错误");

                if (IssueID < 0)                    
                    base.WriteLog(LotteryID.ToString() + " : 期号 " + IssueID.ToString() + " 错误");
            }

            var dtIssue = t_Isuses.Open(ConnectString
                ,"ID, State, WinLotteryNumber"
                , "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "' and LotteryID  in (select id from T_Lotteries where PrintOutType = 301)"
                , "");

            if ((dtIssue == null) || (dtIssue.Rows.Count < 1)) {
                //todo 
            }
            
            if (status == IssueState.Drawing) {
                int ReturnValue = -1;
                string ReturnDescprtion = "";

                int Result = SLS.Dal.Procedures.P_ElectronTicketAgentSchemeQuash(ConnectString, Shove._Convert.StrToLong(dtIssue.Rows[0]["ID"].ToString(), 0), ref ReturnValue, ref ReturnDescprtion);
                if (Result < 0)
                {
                    //new Log("ElectronTicket\\SunLotto").Write("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash : " + IssueID.ToString());
                    base.WriteLog("电子票方案撤单错误_P_ElectronTicketAgentSchemeQuash : " + IssueID.ToString());
                }
            }
            else if (status == IssueState.Open) { 
            }

            bool isHasUpdate = false;

            if (dtIssue.Rows[0]["State"].ToString() != status)
                isHasUpdate = true;

            if (isHasUpdate) {
                int ReturnValue = -1;
                string ReturnDescprtion = "";

                int Result = SLS.Dal.Procedures.P_IsuseUpdate(ConnectString
                    , LotteryID
                    , Shove._Web.Utility.FilteSqlInfusion(IssueName)
                    , Shove._Convert.StrToShort(status, 1)
                    , Shove._Convert.StrToDateTime(starttime, DateTime.Now.ToString())
                    , Shove._Convert.StrToDateTime(endtime, DateTime.Now.ToString())
                    , DateTime.Now
                    , WinNumber
                    , ref ReturnValue
                    , ref ReturnDescprtion);

                if (Result < 0)
                    base.WriteLog("电子票更新期号P_IsuseEdit。");    

                if (ReturnValue < 0)
                    base.WriteLog(ReturnDescprtion);
            }  
            #endregion
        }

    }
}

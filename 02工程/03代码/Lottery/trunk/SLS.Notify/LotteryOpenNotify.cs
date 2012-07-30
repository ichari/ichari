using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;

using Shove.Database;

namespace SLS.Notify
{
    /// <summary>
    /// 开奖请求/兑奖通知 业务处理类
    /// </summary>
    internal class LotteryOpenNotify : NotifyBase
    {
        private IGetOpenData GetOpenDataService;

        public LotteryOpenNotify(bool isUseSniffer)
        {
            if (isUseSniffer)
                GetOpenDataService = new SnifferLotteryDataProvider();
            else
            {
                GetOpenDataService = new ApiLotteryDataProvider()
                {
                    AgentId = base.AgentId,
                    AgentPwd = base.AgentPwd,
                    ApiUrl = base.ApiUrl,
                    TransType = base.TransType
                };
            }
        }

        /// <summary>
        /// 开奖通知
        /// </summary>
        public override void Recieve()
        {
            base.Recieve();
            //other logic 
        }

        /// <summary>
        /// 开奖查询请求
        /// </summary>
        /// <param name="lotteryTypeId"></param>
        /// <param name="issueNo"></param>
        /// <returns></returns>
        public override string Send(string lotteryTypeId,string issueNo)
        {
            var rs = string.Empty;               
            
            if (!string.IsNullOrEmpty(base.SimulateRecieveData))
                rs = base.SimulateRecieveData;
            else
                rs = GetOpenDataService.GetData(int.Parse(lotteryTypeId),issueNo);

            //开奖，后续逻辑
            OnOpenIssueQuerySuccess(rs,lotteryTypeId,issueNo);
            return rs;
        }

        
        /// <summary>
        /// 接收开奖信息成功后续业务逻辑
        /// </summary>
        private void OnOpenIssueQuerySuccess(string returnXml,string reqLotoTypeId,string reqIssueNo)
        {
            var issueNo = string.Empty;
            var lotoId = string.Empty;
            var winNumber = string.Empty;
            var winName = string.Empty;
            var winMoney = string.Empty;
            //校验返回的XML数据
            ValidXml(returnXml, reqLotoTypeId, reqIssueNo);
            //解析返回的XML数据
            var xdoc = XDocument.Parse(returnXml);
            var q = xdoc.Element(SunlotXmlDefin.TagBody);

            issueNo = q.Element(SunlotXmlDefin.TagLotIssue).Value;
            lotoId = q.Element(SunlotXmlDefin.TagLotId).Value;
            winNumber = q.Element(SunlotXmlDefin.TagWinNumber).Value;
            winName = q.Element(SunlotXmlDefin.TagWinName).Value;
            winMoney = q.Element(SunlotXmlDefin.TagWinMoney).Value;

            var winMoneyList = new double[]{};
            var paramXml = GenWinXmlForOpen(winName, winMoney,out winMoneyList);

            long issueId = 0L;
            ExecForOpen_StepOne(paramXml, int.Parse(lotoId), issueNo, winNumber, winMoneyList,out issueId);
        }

        private void ValidXml(string xml,string reqLotoId,string reqIssueNo)
        { 
            var xdoc = XDocument.Parse(xml);
            var body = xdoc.Element(SunlotXmlDefin.TagBody);
            if (body == null)
                throw new SLS.Common.ElectronicException(string.Format("XML解析错误 : {0}",xml));

            var tagArrs = new string[] { 
                                SunlotXmlDefin.TagLotId
                                , SunlotXmlDefin.TagLotIssue
                                , SunlotXmlDefin.TagSt
                                , SunlotXmlDefin.TagEt
                                , SunlotXmlDefin.TagWinNumber
                                , SunlotXmlDefin.TagSaleAmount
                                , SunlotXmlDefin.TagBonusBalance
                                , SunlotXmlDefin.TagWinName
                                , SunlotXmlDefin.TagWinMoney
                                , SunlotXmlDefin.TagWinCount
                            };
            foreach (var item in tagArrs) {
                ValidXmlNode(body.Element(item), item);
            }

            var lotTypeId = body.Element(SunlotXmlDefin.TagLotId).Value;
            if (lotTypeId != reqLotoId)
                throw new SLS.Common.ElectronicException(string.Format("XML返回数据错误 : 请求彩种{0}，返回彩种{1}",reqLotoId,lotTypeId));

            var issueNo = body.Element(SunlotXmlDefin.TagLotIssue).Value;
            if (issueNo != reqIssueNo)
                throw new SLS.Common.ElectronicException(string.Format("XML返回数据错误 : 彩种：{0}，请求期次{1}，返回期次{2}",lotTypeId,reqIssueNo,issueNo));

            
        }

        private void ValidXmlNode(XElement node,string nodeName)
        {             
            if( ! (node != null && !node.IsEmpty) )
                throw new SLS.Common.ElectronicException(string.Format("XML解析错误 : {0}",nodeName));
        }

        /// <summary>
        /// 生成执行开奖逻辑的XML数据
        /// </summary>
        /// <param name="prizeItems">奖等字符串</param>
        /// <param name="moneyItems">每等奖的奖金字符串</param>
        /// <returns></returns>
        private string GenWinXmlForOpen(string prizeItems,string moneyItems,out double[] winMoneyList)
        {
            bool isCompareWinMoneyNoWithFax = false; //todo : 待确认
            var pItems = prizeItems.Split(',');
            var mItems = moneyItems.Split(',');

            var WinListXML = new StringBuilder("<WinLists>");            
            double[] WinMoneyList = new double[pItems.Length * 2];

            for (var i = 0; i < pItems.Length; i++) { 
                WinMoneyList[i * 2] = Shove._Convert.StrToDouble(mItems[i], 0);
                WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(mItems[i], 0); //todo : 税后奖金待确认            

                if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
                {
                    if (isCompareWinMoneyNoWithFax)
                    {
                        throw new SLS.Common.ElectronicException("税后奖金输入错误(不能大于税前奖金)！");
                    }
                }

                WinListXML.Append(string.Format("<WinList defaultMoney=\"{0}\" DefaultMoneyNoWithTax=\"{1}\" />"
                                    , WinMoneyList[i * 2]
                                    , WinMoneyList[i * 2 + 1]));
            }

            WinListXML.Append("</WinLists>");
            winMoneyList = WinMoneyList;
            return WinListXML.ToString();
        }

        
        private void ExecForOpen_StepOne(string winXml,int lotteryId,string issueNo,string winNumber,double[] winMoneyList,out long _issueId)
        { 
            //根据彩种ID和期号查询当期的自增ID
            var dtIssue = new SLS.Dal.Tables.T_Isuses().Open(ConnectString
                                                                , "*"
                                                                , string.Format(" LotteryID={0} AND Name='{1}' ",lotteryId,issueNo)
                                                                , "");
            if (dtIssue == null)
                throw new SLS.Common.ElectronicException("数据库读写错误");
            if (dtIssue.Rows.Count == 0)
                throw new SLS.Common.ElectronicException(string.Format("未查询到当前期次，彩种：{0}，期次号：{1}",lotteryId,issueNo));

            var issueId = dtIssue.Rows[0]["ID"].ToString();
            _issueId = long.Parse(issueId);
            var dtIsuseBonuses = new SLS.Dal.Tables.T_IsuseBonuses().Open(ConnectString
                                     , ""
                                     , "IsuseID = " + issueId
                                     , "");

            if (dtIsuseBonuses == null)
                throw new SLS.Common.ElectronicException("数据库读写错误");

            if (dtIsuseBonuses.Rows.Count < 1)
            {
                int ReturnValue = -1;
                string ReturnDescription = "";


                int Result = SLS.Dal.Procedures.P_IsuseBonusesAdd(ConnectString
                                , Shove._Convert.StrToLong(issueId, 0)
                                , 1 //todo : 是否需要修改为自动开奖特殊用户ID
                                , winXml
                                , ref ReturnValue
                                , ref ReturnDescription);

                if (Result < 0)
                    throw new SLS.Common.ElectronicException("数据库读写错误");

                if (ReturnValue < 0)
                    throw new SLS.Common.ElectronicException(ReturnDescription);
            }
            //取出需要开奖的投注记录
            var dt = new SLS.Dal.Tables.T_Schemes().Open(ConnectString
                        , "* "
                        , "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(issueId) + " and isOpened = 0 and Buyed = 1"
                        , "[ID]");

            if (dt == null)
                throw new SLS.Common.ElectronicException("数据库读写错误");

            StringBuilder sb = new StringBuilder();
            string NoWinSchemeID = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString();
                string Description = "";
                double WinMoneyNoWithTax = 0;
                double WinMoney = 0;

                try
                {
                    WinMoney = new SLS.Lottery()[lotteryId].ComputeWin(LotteryNumber
                                                                , winNumber
                                                                , ref Description
                                                                , ref WinMoneyNoWithTax
                                                                , int.Parse(dt.Rows[i]["PlayTypeID"].ToString())
                                                                , winMoneyList);
                }
                catch
                {
                    WinMoney = 0;
                    base.WriteLog("方案 ID:" + dt.Rows[i]["ID"].ToString() + " 算奖出现错误!");                    
                }

                if (WinMoney == 0)
                {
                    NoWinSchemeID += dt.Rows[i]["ID"].ToString()  + ",";

                    continue;
                }

                sb.Append("update T_Schemes set EditWinMoney = ").Append(WinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1))
                    .Append(", EditWinMoneyNoWithTax = ").Append(WinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1))
                    .Append(", WinDescription = '").Append(Description).Append("'")
                    .Append(" where [ID] = ").AppendLine(dt.Rows[i]["ID"].ToString());
            }

            if(!string.IsNullOrEmpty(sb.ToString()))
                Shove.Database.MSSQL.ExecuteNonQuery(ConnectString,sb.ToString(), new Shove.Database.MSSQL.Parameter[0]);

            if (NoWinSchemeID.EndsWith(","))
            {
                NoWinSchemeID = NoWinSchemeID.Substring(0, NoWinSchemeID.Length - 1);
            }

            if (!string.IsNullOrEmpty(NoWinSchemeID))
            {
                StringBuilder sb1 = new StringBuilder();

                sb1.Append("update T_Schemes set EditWinMoney = 0")
                    .Append(", EditWinMoneyNoWithTax = 0, isOpened = 1 , OpenOperatorID=" + 1)  //todo : 是否替换userid
                    .Append(", WinDescription = ''")
                    .Append(" where [ID] in (" + NoWinSchemeID + ")");

                Shove.Database.MSSQL.ExecuteNonQuery(ConnectString,sb1.ToString(), new Shove.Database.MSSQL.Parameter[0]);
            }

            if (dt.Rows.Count == 0)
                return;
            //执行第三步，派奖逻辑
            ExecForOpen_StepThree(issueNo, lotteryId, winNumber, winMoneyList,_issueId);
        }
        /// <summary>
        /// 开奖第二步，处理代理商电子票逻辑
        /// </summary>
        /// <param name="issueNo"></param>
        /// <param name="lotoId"></param>
        /// <param name="winNumber"></param>
        /// <param name="winMoneyList"></param>
        private void ExecForOpen_StepTwo(string issueNo,int lotoId,string winNumber,double[] winMoneyList)
        {
            var dt = new SLS.Dal.Tables.T_ElectronTicketAgentSchemes().Open(ConnectString
                        , "*"
                        , "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(issueNo) + " and WinMoney is null and state = 1"
                        , "[ID]");
            if(dt == null)
                throw new SLS.Common.ElectronicException("数据库读写错误");

            var t_ElectronTicketAgentSchemes = new SLS.Dal.Tables.T_ElectronTicketAgentSchemes();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString();
                string Description = "";
                double WinMoneyNoWithTax = 0;

                double WinMoney = new SLS.Lottery()[lotoId].ComputeWin(LotteryNumber, winNumber, ref Description, ref WinMoneyNoWithTax, int.Parse(dt.Rows[i]["PlayTypeID"].ToString()), winMoneyList);

                t_ElectronTicketAgentSchemes.WinMoney.Value = WinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1);
                t_ElectronTicketAgentSchemes.WinMoneyWithoutTax.Value = WinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1);
                t_ElectronTicketAgentSchemes.WinDescription.Value = Description;

                t_ElectronTicketAgentSchemes.Update(ConnectString,"[ID] =" + dt.Rows[i]["ID"].ToString());
            }
        }
        /// <summary>
        /// 开奖第三步，派发奖金
        /// </summary>
        /// <param name="issueNo"></param>
        /// <param name="lotoId"></param>
        /// <param name="winNumber"></param>
        /// <param name="winMoneyList"></param>
        private void ExecForOpen_StepThree(string issueNo, int lotoId, string winNumber, double[] winMoneyList,long issueId)
        { 
            int SchemeCount = 0, QuashCount = 0, WinCount = 0, WinNoBuyCount = 0;
            //  总方案数，处理时撤单数，中奖数，中奖但未成功数

            int ReturnValue = -1;
            string ReturnDescription = "";
            DataSet ds = null;
            bool isEndOpen = false;
            
            SqlConnection conn1 = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(ConnectString + ";Connect Timeout=120;");
        
            int Result = P_Win(conn1, ref ds,
                             issueId,
                             winNumber,
                             "",   //todo : 开奖公告
                             1, //todo : 是否需要替换 
                             true,
                             ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount,
                             ref isEndOpen,
                             ref ReturnValue, ref ReturnDescription);
            if ((ds == null) || (ReturnDescription != "") || (ReturnValue < 0) || (Result < 0)) { 
                throw new SLS.Common.ElectronicException("开奖失败" + ReturnDescription);
            }
            //发送中奖通知
            SLS.Common.PF.SendWinNotification(ds, ConnectString);

            //重复执行
            if (!isEndOpen) {
                ExecForOpen_StepThree(issueNo, lotoId, winNumber, winMoneyList,issueId);
            }
            
            



        }

        private int P_Win(SqlConnection conn
                        , ref DataSet ds
                        , long IsuseID
                        , string WinLotteryNumber
                        , string OpenAffiche
                        , long OpenOperatorID
                        , bool isEndTheIsuse
                        , ref int SchemeCount
                        , ref int QuashCount
                        , ref int WinCount
                        , ref int WinNoBuyCount
                        , ref bool isEndOpen
                        , ref int ReturnValue
                        , ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Win", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, WinLotteryNumber),
                new MSSQL.Parameter("OpenAffiche", SqlDbType.VarChar, 0, ParameterDirection.Input, OpenAffiche),
                new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OpenOperatorID),
                new MSSQL.Parameter("isEndTheIsuse", SqlDbType.Bit, 0, ParameterDirection.Input, isEndTheIsuse),
                new MSSQL.Parameter("SchemeCount", SqlDbType.Int, 4, ParameterDirection.Output, SchemeCount),
                new MSSQL.Parameter("QuashCount", SqlDbType.Int, 4, ParameterDirection.Output, QuashCount),
                new MSSQL.Parameter("WinCount", SqlDbType.Int, 4, ParameterDirection.Output, WinCount),
                new MSSQL.Parameter("WinNoBuyCount", SqlDbType.Int, 4, ParameterDirection.Output, WinNoBuyCount),
                new MSSQL.Parameter("isEndOpen", SqlDbType.Bit, 0, ParameterDirection.Output, isEndOpen),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SchemeCount = System.Convert.ToInt32(Outputs["SchemeCount"]);
            }
            catch { }

            try
            {
                QuashCount = System.Convert.ToInt32(Outputs["QuashCount"]);
            }
            catch { }

            try
            {
                WinCount = System.Convert.ToInt32(Outputs["WinCount"]);
            }
            catch { }

            try
            {
                WinNoBuyCount = System.Convert.ToInt32(Outputs["WinNoBuyCount"]);
            }
            catch { }

            try
            {
                isEndOpen = System.Convert.ToBoolean(Outputs["isEndOpen"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }
    
    }
}

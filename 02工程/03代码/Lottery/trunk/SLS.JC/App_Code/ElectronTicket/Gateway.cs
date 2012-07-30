using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;

using Shove.Database;

/// <summary>
///Gateway 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ElectronTicket_Agent_Gateway : System.Web.Services.WebService
{
    // 校验 Sign, 代理商合法性, 时间戳
    private int Valid(ref DataSet ReturnDS, ref string UseLotteryList, ref double Balance, ref short State, long AgentID, DateTime TimeStamp, string Sign, params object[] Params)
    {
        UseLotteryList = "";
        Balance = 0;
        State = 0;

        TimeSpan ts = DateTime.Now - TimeStamp;

        if (Math.Abs(ts.TotalSeconds) > 300)
        {
            BuildReturnDataSetForError(-20, "访问超时", ref ReturnDS);

            new Log("Agent\\ElectronTicket").Write("访问超时");

            return -20;
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgents().Open("", "[ID] = " + AgentID.ToString(), "");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ReturnDS);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return -9999;
        }

        if (dt.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-1, "代理商ID错误", ref ReturnDS);

            new Log("Agent\\ElectronTicket").Write("代理商ID错误");

            return -1;
        }

        string IPAddressLimit = dt.Rows[0]["IPAddressLimit"].ToString();

        if (IPAddressLimit != "")
        {
            IPAddressLimit = "," + IPAddressLimit + ",";

            if (IPAddressLimit.IndexOf("," + GetClientIPAddress() + ",") < 0)
            {
                BuildReturnDataSetForError(-25, "IP地址限制", ref ReturnDS);

                new Log("Agent\\ElectronTicket").Write("IP地址限制");

                return -25;
            }
        }

        string Key = dt.Rows[0]["Key"].ToString();
        string SignSource = AgentID.ToString() + ParamterToString(TimeStamp);

        foreach (object Param in Params)
        {
            SignSource += ParamterToString(Param);
        }

        SignSource += Key;

        if (Shove._Security.Encrypt.MD5(SignSource).ToLower() != Sign.ToLower())
        {
            BuildReturnDataSetForError(-2, "签名校验失败", ref ReturnDS);

            new Log("Agent\\ElectronTicket").Write("签名校验失败");

            return -2;
        }

        UseLotteryList = dt.Rows[0]["UseLotteryList"].ToString();
        Balance = Shove._Convert.StrToDouble(dt.Rows[0]["Balance"].ToString(), 0);
        State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);

        return 0;
    }

    // 校验代理商是否启用了该彩种
    private int ValidLotteryID(ref DataSet ReturnDS, string UseLotteryList, int LotteryID)
    {
        if (String.IsNullOrEmpty(UseLotteryList))
        {
            UseLotteryList = "";
        }
        else
        {
            UseLotteryList = "," + UseLotteryList + ",";
        }

        if (UseLotteryList.IndexOf("," + LotteryID.ToString() + ",") < 0)
        {
            BuildReturnDataSetForError(-3, "彩种ID不存在或暂不支持", ref ReturnDS);

            new Log("Agent\\ElectronTicket").Write("彩种ID不存在或暂不支持");

            return -3;
        }

        return 0;
    }

    private string ParamterToString(object Param)
    {
        if (Param is DateTime)
        {
            return ((DateTime)Param).ToString("yyyyMMdd HHmmss");
        }
        else if (Param is DataTable)
        {
            return Shove._Convert.DataTableToXML((DataTable)Param);
        }
        else if (Param is string)
        {
            return (string)Param;
        }

        return Param.ToString();
    }

    private void BuildReturnDataSet(long ReturnNumber, ref DataSet ds)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Result", typeof(long));
        dt.Columns.Add("Description", typeof(string));

        DataRow dr = dt.NewRow();

        dr["Result"] = ReturnNumber;
        dr["Description"] = "接口调用成功";

        dt.Rows.Add(dr);

        if (ds == null)
        {
            ds = new DataSet();
        }

        ds.Tables.Clear();
        ds.Tables.Add(dt);
    }

    private void BuildReturnDataSetForError(int ErrorNumber, string Description, ref DataSet ds)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Result", typeof(long));
        dt.Columns.Add("Description", typeof(string));

        DataRow dr = dt.NewRow();

        dr["Result"] = ErrorNumber;
        dr["Description"] = Description;

        dt.Rows.Add(dr);

        if (ds == null)
        {
            ds = new DataSet();
        }

        ds.Tables.Clear();
        ds.Tables.Add(dt);
    }

    private long GetIsuseID(int LotteryID, string IsuseName)
    {
        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "LotteryID = " + LotteryID.ToString() + "and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IsuseName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            return -1;
        }

        return Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
    }

    private string GetLotteryNumber(int LotteryID, int PlayTypeID, string BettingNumber, ref int Num)   // Num 返回注数
    {
        Num = 0;

        if (String.IsNullOrEmpty(BettingNumber))
        {
            return "";
        }

        SLS.Lottery.LotteryBase lb = new SLS.Lottery()[LotteryID];

        if (lb == null)
        {
            return "";
        }

        string t_Number = lb.AnalyseScheme(BettingNumber, PlayTypeID);

        if (String.IsNullOrEmpty(t_Number))
        {
            return "";
        }

        string[] t_Numbers = t_Number.Split('\n');

        if ((t_Numbers == null) || (t_Numbers.Length < 1))
        {
            return "";
        }

        string Result = "";

        foreach (string str in t_Numbers)
        {
            string t_str = str.Trim();

            if (String.IsNullOrEmpty(t_str))
            {
                continue;
            }

            string[] t_strs = t_str.Split('|');

            if ((t_strs == null) || (t_strs.Length != 2))
            {
                continue;
            }

            int t_Num = Shove._Convert.StrToInt(t_strs[1], -1);

            if (String.IsNullOrEmpty(t_strs[0]) || (t_Num < 1))
            {
                continue;
            }

            Result += t_strs[0] + "\n";
            Num += t_Num;
        }

        if (Result.EndsWith("\n"))
        {
            Result = Result.Substring(0, Result.Length - 1);
        }

        return Result;
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

    // 获取某时间段内的奖期列表
    [WebMethod]
    public DataSet GetIssues(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, DateTime StartTime, DateTime EndTime)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetIssues\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tStartTime={4}\tEndTime={5}", AgentID, TimeStamp, Sign, LotteryID, StartTime, EndTime));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, StartTime, EndTime) < 0)
        {
            return ds;
        }

        if (LotteryID >= 0)
        {
            if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
            {
                return ds;
            }
        }
        else
        {
            if (UseLotteryList == "")
            {
                BuildReturnDataSetForError(-3, "彩种ID不存在或暂不支持", ref ds);

                new Log("Agent\\ElectronTicket").Write("彩种ID不存在或暂不支持");

                return ds;
            }
        }

        DataTable dt = null;

        if (LotteryID >= 0)
        {
            dt = new DAL.Tables.T_Isuses().Open("LotteryID, [Name] as IssueName, StartTime, EndTime, State as Status, WinLotteryNumber as BonusNumber", "LotteryID = " + LotteryID.ToString() + " and StartTime between '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'", "StartTime");
        }
        else
        {
            dt = new DAL.Tables.T_Isuses().Open("LotteryID, [Name] as IssueName, StartTime, EndTime, State as Status, WinLotteryNumber as BonusNumber", "LotteryID in (" + UseLotteryList + ") and StartTime between '" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'", "StartTime");
        }

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(dt);

        return ds;
    }

    // 获取某奖期的详细信息
    [WebMethod]
    public DataSet GetIssueInformation(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetIssueInformation\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}", AgentID, TimeStamp, Sign, LotteryID, IssueName));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName) < 0)
        {
            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("LotteryID, [Name] as IssueName, StartTime, EndTime, State as Status, WinLotteryNumber as BonusNumber", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (dt.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(dt);

        return ds;
    }

    // 投注
    [WebMethod]
    public DataSet Betting(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName, int PlayTypeID, string SchemeNumber, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string dtJoinDetailXml)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=Betting\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}\tPlayTypeID={5}\tSchemeNumber={6}\tAmount={7}\tMultiple={8}\tShare={9}", AgentID, TimeStamp, Sign, LotteryID, IssueName, PlayTypeID, SchemeNumber, Amount, Multiple, Share));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        DataTable dtJoinDetail = Shove._Convert.XMLToDataTable(dtJoinDetailXml);

        if (String.IsNullOrEmpty(SchemeNumber) || (Amount <= 0) || (Multiple < 1) || (Share < 1) || String.IsNullOrEmpty(InitiateName) || String.IsNullOrEmpty(InitiateAlipayName) || String.IsNullOrEmpty(InitiateAlipayID) ||
            String.IsNullOrEmpty(InitiateRealityName) || String.IsNullOrEmpty(InitiateIDCard) || String.IsNullOrEmpty(InitiateMobile) || (dtJoinDetail == null) || (dtJoinDetail.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-22, "参数不符合规定或者未提供", ref ds);

            new Log("Agent\\ElectronTicket").Write("参数不符合规定或者未提供");

            return ds;
        }

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName, PlayTypeID, SchemeNumber, LotteryNumber, Amount, Multiple, Share, InitiateName, InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail, InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, dtJoinDetailXml) < 0)
        {
            return ds;
        }

        if (State != 1)
        {
            BuildReturnDataSetForError(-1, "代理商ID错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("代理商ID错误");

            return ds;
        }

        if (Balance <= 0)
        {
            BuildReturnDataSetForError(-7, "投注金额超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额超限");

            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], StartTime, EndTime, State", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        long IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        DateTime StartTime = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "1980-1-1 0:0:0");
        DateTime EndTime = Shove._Convert.StrToDateTime(dt.Rows[0]["EndTime"].ToString(), "1980-1-1 0:0:0");
        short IsuseState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);

        if (IsuseID < 0)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        DateTime Now = DateTime.Now;

        if (Now < StartTime)
        {
            BuildReturnDataSetForError(-5, "奖期未开启", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期未开启");

            return ds;
        }

        if ((Now >= EndTime) || (IsuseState > 1))
        {
            BuildReturnDataSetForError(-6, "奖期已截止投注", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期已截止投注" + "|" + Now.ToString() + "|" + IsuseState);

            return ds;
        }

        DataTable dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("Price, MaxMultiple", "LotteryID = " + LotteryID.ToString() + " and [ID] = " + PlayTypeID.ToString(), "");

        if (dtPlayTypes == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (dtPlayTypes.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-21, "玩法ID不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("玩法ID不存在");

            return ds;
        }

        double Price = Shove._Convert.StrToDouble(dtPlayTypes.Rows[0]["Price"].ToString(), 2);
        int MaxMultiple = Shove._Convert.StrToInt(dtPlayTypes.Rows[0]["MaxMultiple"].ToString(), 200);

        if ((PlayTypeID == 3903) || (PlayTypeID == 3904))
        {
            Price = 3;
        }

        if ((Multiple < 1) || (Multiple > MaxMultiple))
        {
            BuildReturnDataSetForError(-8, "倍数超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("倍数超限");

            return ds;
        }

        if (new DAL.Tables.T_ElectronTicketAgentSchemes().GetCount("AgentID = " + AgentID.ToString() + " and SchemeNumber = '" + Shove._Web.Utility.FilteSqlInfusion(SchemeNumber) + "'") > 0)
        {
            BuildReturnDataSetForError(-15, "方案号重复", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案号重复");

            return ds;
        }

        if ((InitiateBonusScale < 0) || (InitiateBonusScale > 1) || (InitiateBonusLimitLower < 0))
        {
            BuildReturnDataSetForError(-10, "佣金设置不符合要求", ref ds);

            new Log("Agent\\ElectronTicket").Write("佣金设置不符合要求");

            return ds;
        }

        int Num = 0;
        LotteryNumber = GetLotteryNumber(LotteryID, PlayTypeID, LotteryNumber, ref Num);

        if (Num == 0)
        {
            BuildReturnDataSetForError(-17, "投注号码格式错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注号码格式错误");

            return ds;
        }

        if (Num * Multiple * Price != Amount)
        {
            BuildReturnDataSetForError(-9, "投注金额与票面金额不符", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额与票面金额不符");

            return ds;
        }

        if ((Share > 1) && (dtJoinDetail.Rows.Count < 2))
        {
            BuildReturnDataSetForError(-11, "方案份数(金额)与购买明细份数(金额)不符合", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案份数(金额)与购买明细份数(金额)不符合");

            return ds;
        }

        int DetailShareCount = 0;
        double DetailAmountCount = 0;
        string DetailXML = "<Details>";
        int i = 0;

        foreach (DataRow dr in dtJoinDetail.Rows)
        {
            string DetailName = "";
            string DetailAlipayName = "";
            string DetailRealityName = "";
            string DetailIDCard = "";
            string DetailTelephone = "";
            string DetailMobile = "";
            string DetailEmail = "";
            int DetailShare = -1;
            double DetailAmount = -1;

            try
            {
                DetailName = dr["Name"].ToString();
                DetailAlipayName = dr["AlipayName"].ToString();
                DetailRealityName = dr["RealityName"].ToString();
                DetailIDCard = dr["IDCard"].ToString();
                DetailTelephone = dr["Telephone"].ToString();
                DetailMobile = dr["Mobile"].ToString();
                DetailEmail = dr["Email"].ToString();
                DetailShare = Shove._Convert.StrToInt(dr["Share"].ToString(), -1);
                DetailAmount = Shove._Convert.StrToDouble(dr["Amount"].ToString(), -1);
            }
            catch
            {
                BuildReturnDataSetForError(-23, "购买明细格式错误", ref ds);

                new Log("Agent\\ElectronTicket").Write("购买明细格式错误");

                return ds;
            }

            if (String.IsNullOrEmpty(DetailName) || String.IsNullOrEmpty(DetailAlipayName) || String.IsNullOrEmpty(DetailRealityName) || String.IsNullOrEmpty(DetailIDCard) ||
                String.IsNullOrEmpty(DetailTelephone) || (DetailShare < 1) || (DetailAmount < 0))
            {
                BuildReturnDataSetForError(-23, "购买明细格式错误", ref ds);

                new Log("Agent\\ElectronTicket").Write("购买明细格式错误");

                return ds;
            }

            DetailShareCount += DetailShare;
            DetailAmountCount += DetailAmount;

            DetailXML += String.Format("<Detail No=\"{0}\" Name=\"{1}\" AlipayName=\"{2}\" RealityName=\"{3}\" IDCard=\"{4}\" Telephone=\"{5}\" Mobile=\"{6}\" Email=\"{7}\" Share=\"{8}\" Amount=\"{9}\" />", i++, DetailName, DetailAlipayName, DetailRealityName, DetailIDCard, DetailTelephone, DetailMobile, DetailEmail, DetailShare, DetailAmount);
        }

        DetailXML += "</Details>";

        if ((DetailShareCount != Share) || (DetailAmountCount != Amount))
        {
            BuildReturnDataSetForError(-11, "方案份数(金额)与购买明细份数(金额)不符合", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案份数(金额)与购买明细份数(金额)不符合");

            return ds;
        }

        if (Balance < Amount)
        {
            BuildReturnDataSetForError(-7, "投注金额超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额超限");

            return ds;
        }

        long SchemeID = -1;
        string ReturnDescription = "";
        int Results = -1;
            Results = DAL.Procedures.P_ElectronTicketAgentSchemeAdd(AgentID, SchemeNumber, LotteryID, PlayTypeID, IsuseID, LotteryNumber, Amount, Multiple, Share, InitiateName,
            InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail,
            InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, DetailXML, ref SchemeID, ref ReturnDescription) ;


        if (Results < 0)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (SchemeID < 0)
        {
            BuildReturnDataSetForError((int)SchemeID, ReturnDescription, ref ds);

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(new DataTable());

        return ds;
    }

    //Op_Type 0001 内部参数调用
    // 投注
    [WebMethod]
    public DataSet BettingWithin(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName, int PlayTypeID, string SchemeNumber, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string dtJoinDetailXml, string Op_Type)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=BettingWithin\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}\tPlayTypeID={5}\tSchemeNumber={6}\tAmount={7}\tMultiple={8}\tShare={9}\tOp_Type={10}", AgentID, TimeStamp, Sign, LotteryID, IssueName, PlayTypeID, SchemeNumber, Amount, Multiple, Share, Op_Type));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Op_Type != "0001")
        {
            BuildReturnDataSetForError(-101, "不是有效的内部参数调用", ref ds);

            new Log("Agent\\ElectronTicket").Write("不是有效的内部参数调用");

            return ds;
        }

        DataTable dtJoinDetail = Shove._Convert.XMLToDataTable(dtJoinDetailXml);

        if (String.IsNullOrEmpty(SchemeNumber) || (Amount <= 0) || (Multiple < 1) || (Share < 1) || String.IsNullOrEmpty(InitiateName) || String.IsNullOrEmpty(InitiateAlipayName) || String.IsNullOrEmpty(InitiateAlipayID) ||
            String.IsNullOrEmpty(InitiateRealityName) || String.IsNullOrEmpty(InitiateIDCard) || String.IsNullOrEmpty(InitiateMobile) || (dtJoinDetail == null) || (dtJoinDetail.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-22, "参数不符合规定或者未提供", ref ds);

            new Log("Agent\\ElectronTicket").Write("参数不符合规定或者未提供");

            return ds;
        }

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName, PlayTypeID, SchemeNumber, LotteryNumber, Amount, Multiple, Share, InitiateName, InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail, InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, dtJoinDetailXml) < 0)
        {
            return ds;
        }

        if (State != 1)
        {
            BuildReturnDataSetForError(-1, "代理商ID错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("代理商ID错误");

            return ds;
        }

        if (Balance <= 0)
        {
            BuildReturnDataSetForError(-7, "投注金额超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额超限");

            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], StartTime, EndTime, State", "LotteryID = " + LotteryID.ToString() + " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        long IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        DateTime StartTime = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "1980-1-1 0:0:0");
        DateTime EndTime = Shove._Convert.StrToDateTime(dt.Rows[0]["EndTime"].ToString(), "1980-1-1 0:0:0");
        short IsuseState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);

        if (IsuseID < 0)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        DateTime Now = DateTime.Now;

        if (Now < StartTime)
        {
            BuildReturnDataSetForError(-5, "奖期未开启", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期未开启");

            return ds;
        }

        if ((Now >= EndTime) || (IsuseState > 1))
        {
            BuildReturnDataSetForError(-6, "奖期已截止投注", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期已截止投注" + "|" + Now.ToString() + "|" + IsuseState);

            return ds;
        }

        DataTable dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("Price, MaxMultiple", "LotteryID = " + LotteryID.ToString() + " and [ID] = " + PlayTypeID.ToString(), "");

        if (dtPlayTypes == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (dtPlayTypes.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-21, "玩法ID不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("玩法ID不存在");

            return ds;
        }

        double Price = Shove._Convert.StrToDouble(dtPlayTypes.Rows[0]["Price"].ToString(), 2);
        int MaxMultiple = Shove._Convert.StrToInt(dtPlayTypes.Rows[0]["MaxMultiple"].ToString(), 200);

        if ((PlayTypeID == 3903) || (PlayTypeID == 3904))
        {
            Price = 3;
        }

        if ((Multiple < 1) || (Multiple > MaxMultiple))
        {
            BuildReturnDataSetForError(-8, "倍数超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("倍数超限");

            return ds;
        }

        if (new DAL.Tables.T_ElectronTicketAgentSchemes().GetCount("AgentID = " + AgentID.ToString() + " and SchemeNumber = '" + Shove._Web.Utility.FilteSqlInfusion(SchemeNumber) + "'") > 0)
        {
            BuildReturnDataSetForError(-15, "方案号重复", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案号重复");

            return ds;
        }

        if ((InitiateBonusScale < 0) || (InitiateBonusScale > 1) || (InitiateBonusLimitLower < 0))
        {
            BuildReturnDataSetForError(-10, "佣金设置不符合要求", ref ds);

            new Log("Agent\\ElectronTicket").Write("佣金设置不符合要求");

            return ds;
        }

        int Num = 0;
        LotteryNumber = GetLotteryNumber(LotteryID, PlayTypeID, UnEncryptString(LotteryNumber), ref Num);

        if (Num == 0)
        {
            BuildReturnDataSetForError(-17, "投注号码格式错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注号码格式错误");

            return ds;
        }

        if (Num * Multiple * Price != Amount)
        {
            BuildReturnDataSetForError(-9, "投注金额与票面金额不符", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额与票面金额不符");

            return ds;
        }

        if ((Share > 1) && (dtJoinDetail.Rows.Count < 2))
        {
            BuildReturnDataSetForError(-11, "方案份数(金额)与购买明细份数(金额)不符合", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案份数(金额)与购买明细份数(金额)不符合");

            return ds;
        }

        int DetailShareCount = 0;
        double DetailAmountCount = 0;
        string DetailXML = "<Details>";
        int i = 0;

        foreach (DataRow dr in dtJoinDetail.Rows)
        {
            string DetailName = "";
            string DetailAlipayName = "";
            string DetailRealityName = "";
            string DetailIDCard = "";
            string DetailTelephone = "";
            string DetailMobile = "";
            string DetailEmail = "";
            int DetailShare = -1;
            double DetailAmount = -1;

            try
            {
                DetailName = dr["Name"].ToString();
                DetailAlipayName = dr["AlipayName"].ToString();
                DetailRealityName = dr["RealityName"].ToString();
                DetailIDCard = dr["IDCard"].ToString();
                DetailTelephone = dr["Telephone"].ToString();
                DetailMobile = dr["Mobile"].ToString();
                DetailEmail = dr["Email"].ToString();
                DetailShare = Shove._Convert.StrToInt(dr["Share"].ToString(), -1);
                DetailAmount = Shove._Convert.StrToDouble(dr["Amount"].ToString(), -1);
            }
            catch
            {
                BuildReturnDataSetForError(-23, "购买明细格式错误", ref ds);

                new Log("Agent\\ElectronTicket").Write("购买明细格式错误");

                return ds;
            }

            if (String.IsNullOrEmpty(DetailName) || String.IsNullOrEmpty(DetailAlipayName) || String.IsNullOrEmpty(DetailRealityName) || String.IsNullOrEmpty(DetailIDCard) ||
                String.IsNullOrEmpty(DetailTelephone) || (DetailShare < 1) || (DetailAmount < 0))
            {
                BuildReturnDataSetForError(-23, "购买明细格式错误", ref ds);

                new Log("Agent\\ElectronTicket").Write("购买明细格式错误");

                return ds;
            }

            DetailShareCount += DetailShare;
            DetailAmountCount += DetailAmount;

            DetailXML += String.Format("<Detail No=\"{0}\" Name=\"{1}\" AlipayName=\"{2}\" RealityName=\"{3}\" IDCard=\"{4}\" Telephone=\"{5}\" Mobile=\"{6}\" Email=\"{7}\" Share=\"{8}\" Amount=\"{9}\" />", i++, DetailName, DetailAlipayName, DetailRealityName, DetailIDCard, DetailTelephone, DetailMobile, DetailEmail, DetailShare, DetailAmount);
        }

        DetailXML += "</Details>";

        if ((DetailShareCount != Share) || (DetailAmountCount != Amount))
        {
            BuildReturnDataSetForError(-11, "方案份数(金额)与购买明细份数(金额)不符合", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案份数(金额)与购买明细份数(金额)不符合");

            return ds;
        }

        if (Balance < Amount)
        {
            BuildReturnDataSetForError(-7, "投注金额超限", ref ds);

            new Log("Agent\\ElectronTicket").Write("投注金额超限");

            return ds;
        }

        long SchemeID = 0;
        string ReturnDescription = "";

        if (DAL.Procedures.P_ElectronTicketAgentSchemeAdd(AgentID, SchemeNumber, LotteryID, PlayTypeID, IsuseID, LotteryNumber, Amount, Multiple, Share, InitiateName,
            InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail,
            InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, DetailXML, ref SchemeID, ref ReturnDescription) < 0)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (SchemeID < 0)
        {
            BuildReturnDataSetForError((int)SchemeID, ReturnDescription, ref ds);

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(new DataTable());

        return ds;
    }

    // 查询投注结果
    [WebMethod]
    public DataSet QueryBetting(long AgentID, DateTime TimeStamp, string Sign, string SchemeNumber)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=QueryBetting\tAgentID={0}\tTimeStamp={1}\tSign={2}\tSchemeNumber={3}", AgentID, TimeStamp, Sign, SchemeNumber));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, SchemeNumber) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("[ID], State, BettingDescription", "AgentID = " + AgentID.ToString() + " and SchemeNumber = '" + Shove._Web.Utility.FilteSqlInfusion(SchemeNumber) + "'", "");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (dt.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-16, "方案号不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案号不存在");

            return ds;
        }

        long SchemeID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        short SchemeState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);
        string BettingDescription = dt.Rows[0]["BettingDescription"].ToString();

        if ((SchemeID < 0) || (SchemeState < 0))
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        if (SchemeState == 0)
        {
            BuildReturnDataSetForError(-24, "等待出票", ref ds);

            new Log("Agent\\ElectronTicket").Write("等待出票");

            return ds;
        }

        if (SchemeState >= 2)
        {
            BuildReturnDataSetForError(-13, "出票失败(" + BettingDescription + ")", ref ds);

            new Log("Agent\\ElectronTicket").Write("出票失败(" + BettingDescription + ")");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);

        DataTable dtTickets = new DAL.Tables.T_ElectronTicketAgentSchemesSendToCenter().Open("Identifiers as TicketID", "SchemeID = " + SchemeID.ToString(), "[ID]");
        ds.Tables.Add(dtTickets);

        return ds;
    }

    //获取某奖期的返奖信息
    [WebMethod]
    public DataSet GetIssueBonus(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetIssueBonus\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}", AgentID, TimeStamp, Sign, LotteryID, IssueName));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName) < 0)
        {
            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], StartTime, EndTime, State, isOpened", "LotteryID = " + LotteryID.ToString() + "and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        long IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        DateTime StartTime = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "1980-1-1 0:0:0");
        DateTime EndTime = Shove._Convert.StrToDateTime(dt.Rows[0]["EndTime"].ToString(), "1980-1-1 0:0:0");
        short IsuseState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);
        bool isOpened = Shove._Convert.StrToBool(dt.Rows[0]["isOpened"].ToString(), false);

        if (IsuseID < 0)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        DateTime Now = DateTime.Now;

        if ((Now < StartTime) || (IsuseState < 1))
        {
            BuildReturnDataSetForError(-5, "奖期未开启", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期未开启");

            return ds;
        }

        if (!isOpened)
        {
            BuildReturnDataSetForError(-18, "奖期尚未开奖", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期尚未开奖");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);

        DataTable dtSchemes = new DAL.Views.V_ElectronTicketAgentSchemes().Open("LotteryID, IsuseName as IssueName, SchemeNumber, WinMoney as Bonus, WinMoneyWithoutTax as BonusWithoutTax, WinLotteryNumber as BonusNumber, WinDescription as BonusDescription", "AgentID = " + AgentID.ToString() + " and LotteryID = " + LotteryID.ToString() + " and IsuseID = " + IsuseID.ToString() + " and WinMoney <> 0", "[ID]");
        ds.Tables.Add(dtSchemes);

        return ds;
    }

    //获取某奖期的全部方案
    [WebMethod]
    public DataSet GetSchemes(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetSchemes\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}", AgentID, TimeStamp, Sign, LotteryID, IssueName));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName) < 0)
        {
            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], StartTime, EndTime, State, isOpened", "LotteryID = " + LotteryID.ToString() + "and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        long IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        DateTime StartTime = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "1980-1-1 0:0:0");
        short IsuseState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);

        if (IsuseID < 0)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        DateTime Now = DateTime.Now;

        if ((Now < StartTime) || (IsuseState < 1))
        {
            BuildReturnDataSetForError(-5, "奖期未开启", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期未开启");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);

        DataTable dtSchemes = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("SchemeNumber, State", "AgentID = " + AgentID.ToString() + " and IsuseID = " + IsuseID.ToString(), "[ID]");
        ds.Tables.Add(dtSchemes);

        return ds;
    }

    //获取某奖期的失败方案
    [WebMethod]
    public DataSet GetUnsuccessfulSchemes(long AgentID, DateTime TimeStamp, string Sign, int LotteryID, string IssueName)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetUnsuccessfulSchemes\tAgentID={0}\tTimeStamp={1}\tSign={2}\tLotteryID={3}\tIssueName={4}", AgentID, TimeStamp, Sign, LotteryID, IssueName));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, LotteryID, IssueName) < 0)
        {
            return ds;
        }

        if (ValidLotteryID(ref ds, UseLotteryList, LotteryID) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], StartTime, EndTime, State, isOpened", "LotteryID = " + LotteryID.ToString() + "and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(IssueName) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        long IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
        DateTime StartTime = Shove._Convert.StrToDateTime(dt.Rows[0]["StartTime"].ToString(), "1980-1-1 0:0:0");
        short IsuseState = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), -1);

        if (IsuseID < 0)
        {
            BuildReturnDataSetForError(-4, "奖期不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期不存在");

            return ds;
        }

        DateTime Now = DateTime.Now;

        if ((Now < StartTime) || (IsuseState < 1))
        {
            BuildReturnDataSetForError(-5, "奖期未开启", ref ds);

            new Log("Agent\\ElectronTicket").Write("奖期未开启");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);

        DataTable dtSchemes = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("SchemeNumber, State", "AgentID = " + AgentID.ToString() + " and IsuseID = " + IsuseID.ToString() + " and State <> 1", "[ID]");
        ds.Tables.Add(dtSchemes);

        return ds;
    }

    //获取某方案的票的细节
    [WebMethod]
    public DataSet GetSchemeTickets(long AgentID, DateTime TimeStamp, string Sign, string SchemeNumber)
    {
        new Log("Agent\\ElectronTicket").Write(String.Format("Method=GetSchemeTickets\tAgentID={0}\tTimeStamp={1}\tSign={2}\tSchemeNumber={3}", AgentID, TimeStamp, Sign, SchemeNumber));

        DataSet ds = new DataSet();
        string UseLotteryList = "";
        double Balance = 0;
        short State = 0;

        if (Valid(ref ds, ref UseLotteryList, ref Balance, ref State, AgentID, TimeStamp, Sign, SchemeNumber) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("[ID]", "AgentID = " + AgentID.ToString() + " and SchemeNumber = '" + Shove._Web.Utility.FilteSqlInfusion(SchemeNumber) + "'", "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-16, "方案号不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案号不存在");

            return ds;
        }

        long SchemeID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);

        if (SchemeID < 0)
        {
            BuildReturnDataSetForError(-16, "方案号不存在", ref ds);

            new Log("Agent\\ElectronTicket").Write("方案号不存在");

            return ds;
        }

        dt = new DAL.Tables.T_ElectronTicketAgentSchemesSendToCenter().Open("Identifiers, Multiple, Money, Ticket, Sends, HandleResult, HandleDescription, WinMoney as Bonus, BonusLevel", "SchemeID = " + SchemeID.ToString(), "[ID]");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            new Log("Agent\\ElectronTicket").Write("未知错误");

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(dt);

        return ds;
    }

    private static string UnEncryptString(string d)
    {
        if ((d == null) || (d == "") || (d.Length < 2))
            return "";

        int i;
        ReplaceKey rk = new ReplaceKey();
        for (i = 51; i >= 0; i--)
            d = d.Replace(rk.Key[1, i], rk.Key[0, i]);

        int len, Key = int.Parse(d.Substring(0, 3));
        int[] Keys = new int[3];
        Keys[0] = Key / 100;
        Keys[1] = (Key % 100) / 10;
        Keys[2] = (Key % 10);

        d = d.Substring(3, d.Length - 3);
        len = d.Length / 3;

        byte[] Byte = new byte[len];

        for (i = 0; i < len; i++)
            Byte[i] = (byte)(int.Parse(d.Substring(i * 3, 3)) - Keys[i % 3]);

        return System.Text.Encoding.UTF8.GetString(Byte);
    }

    private class ReplaceKey
    {
        public string[,] Key;

        public ReplaceKey()
        {
            Key = new string[2, 84];

            Key[0, 0] = "00"; Key[1, 0] = "A";
            Key[0, 1] = "11"; Key[1, 1] = "B";
            Key[0, 2] = "22"; Key[1, 2] = "H";
            Key[0, 3] = "33"; Key[1, 3] = "e";
            Key[0, 4] = "44"; Key[1, 4] = "F";
            Key[0, 5] = "55"; Key[1, 5] = "E";
            Key[0, 6] = "66"; Key[1, 6] = "M";
            Key[0, 7] = "77"; Key[1, 7] = "z";
            Key[0, 8] = "88"; Key[1, 8] = "I";
            Key[0, 9] = "99"; Key[1, 9] = "b";

            Key[0, 10] = "10"; Key[1, 10] = "K";
            Key[0, 11] = "20"; Key[1, 11] = "L";
            Key[0, 12] = "30"; Key[1, 12] = "C";
            Key[0, 13] = "40"; Key[1, 13] = "N";
            Key[0, 14] = "50"; Key[1, 14] = "l";
            Key[0, 15] = "60"; Key[1, 15] = "n";
            Key[0, 16] = "70"; Key[1, 16] = "m";
            Key[0, 17] = "80"; Key[1, 17] = "R";
            Key[0, 18] = "90"; Key[1, 18] = "a";

            Key[0, 19] = "01"; Key[1, 19] = "T";
            Key[0, 20] = "21"; Key[1, 20] = "U";
            Key[0, 21] = "31"; Key[1, 21] = "j";
            Key[0, 22] = "41"; Key[1, 22] = "W";
            Key[0, 23] = "51"; Key[1, 23] = "X";
            Key[0, 24] = "61"; Key[1, 24] = "V";
            Key[0, 25] = "71"; Key[1, 25] = "Z";
            Key[0, 26] = "81"; Key[1, 26] = "S";
            Key[0, 27] = "91"; Key[1, 27] = "J";

            Key[0, 28] = "02"; Key[1, 28] = "c";
            Key[0, 29] = "12"; Key[1, 29] = "d";
            Key[0, 30] = "32"; Key[1, 30] = "D";
            Key[0, 31] = "42"; Key[1, 31] = "f";
            Key[0, 32] = "52"; Key[1, 32] = "G";
            Key[0, 33] = "62"; Key[1, 33] = "h";
            Key[0, 34] = "72"; Key[1, 34] = "i";
            Key[0, 35] = "82"; Key[1, 35] = "w";
            Key[0, 36] = "92"; Key[1, 36] = "k";

            Key[0, 37] = "03"; Key[1, 37] = "O";
            Key[0, 38] = "13"; Key[1, 38] = "Q";
            Key[0, 39] = "23"; Key[1, 39] = "P";
            Key[0, 40] = "43"; Key[1, 40] = "o";
            Key[0, 41] = "53"; Key[1, 41] = "p";
            Key[0, 42] = "63"; Key[1, 42] = "x";
            Key[0, 43] = "73"; Key[1, 43] = "t";
            Key[0, 44] = "83"; Key[1, 44] = "s";
            Key[0, 45] = "93"; Key[1, 45] = "r";

            Key[0, 46] = "04"; Key[1, 46] = "u";
            Key[0, 47] = "14"; Key[1, 47] = "v";
            Key[0, 48] = "24"; Key[1, 48] = "Y";
            Key[0, 49] = "34"; Key[1, 49] = "q";
            Key[0, 50] = "54"; Key[1, 50] = "y";
            Key[0, 51] = "64"; Key[1, 51] = "g";
            Key[0, 52] = "74"; Key[1, 52] = "!";
            Key[0, 53] = "84"; Key[1, 53] = "@";
            Key[0, 54] = "94"; Key[1, 54] = "#";

            Key[0, 55] = "05"; Key[1, 55] = "$";
            Key[0, 56] = "15"; Key[1, 56] = "%";
            Key[0, 57] = "25"; Key[1, 57] = "^";
            Key[0, 58] = "35"; Key[1, 58] = "&";
            Key[0, 59] = "45"; Key[1, 59] = "*";
            Key[0, 60] = "65"; Key[1, 60] = "(";
            Key[0, 61] = "75"; Key[1, 61] = ")";
            Key[0, 62] = "85"; Key[1, 62] = "_";
            Key[0, 63] = "95"; Key[1, 63] = "-";

            Key[0, 64] = "06"; Key[1, 64] = "+";
            Key[0, 65] = "16"; Key[1, 65] = "=";
            Key[0, 66] = "26"; Key[1, 66] = "|";
            Key[0, 67] = "36"; Key[1, 67] = "\\";
            Key[0, 68] = "46"; Key[1, 68] = "<";
            Key[0, 69] = "56"; Key[1, 69] = ">";
            Key[0, 70] = "76"; Key[1, 70] = ",";
            Key[0, 71] = "86"; Key[1, 71] = ".";
            Key[0, 72] = "96"; Key[1, 72] = "?";

            Key[0, 73] = "07"; Key[1, 73] = "/";
            Key[0, 74] = "17"; Key[1, 74] = "[";
            Key[0, 75] = "27"; Key[1, 75] = "]";
            Key[0, 76] = "37"; Key[1, 76] = "{";
            Key[0, 77] = "47"; Key[1, 77] = "}";
            Key[0, 78] = "57"; Key[1, 78] = ":";
            Key[0, 79] = "67"; Key[1, 79] = ";";
            Key[0, 80] = "87"; Key[1, 80] = "\"";
            Key[0, 81] = "97"; Key[1, 81] = "\'";

            Key[0, 82] = "08"; Key[1, 82] = "`";
            Key[0, 83] = "18"; Key[1, 83] = "~";
        }
    }
}


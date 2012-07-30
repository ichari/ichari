using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

/// <summary>
///Gateway 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CardPassword_Gateway : System.Web.Services.WebService
{
    // 错误码：
    //  -1 代理商ID错误
    //  -2 访问超时
    //  -3 IP地址限制
    //  -4 签名失败
    //  -5 卡号不存在
    //  -6 卡号已经被使用
    //  -7 失效时间不能小于当前时间
    //  -8 参数不符合规定或者未提供
    //  -9 代理商被停用
    //  -10 卡号不存在、已失效、已被更换、已被使用
    //  -11 时间戳格式错误

    //  -9999 未知错误

    // 校验 Sign, 代理商合法性, 时间戳
    private int Valid(ref DataSet ReturnDS, ref short State, int AgentID, string TimeStamp, string Sign, params object[] Params)
    {
        DateTime t_DateTime;

        try
        {
            t_DateTime = DateTime.Parse(TimeStamp);
        }
        catch
        {
            BuildReturnDataSetForError(-11, "时间戳格式错误", ref ReturnDS);

            return -4;
        }

        State = 0;

        TimeSpan ts = DateTime.Now - t_DateTime;

        if (Math.Abs(ts.TotalSeconds) > 300)
        {
            BuildReturnDataSetForError(-2, "访问超时", ref ReturnDS);

            return -2;
        }

        System.Threading.Thread.Sleep(200);             //休眠 200 毫秒

        DataTable dt = new DAL.Tables.T_CardPasswordAgents().Open("", "[ID] = " + AgentID.ToString(), "");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ReturnDS);

            return -9999;
        }

        if (dt.Rows.Count < 1)
        {
            BuildReturnDataSetForError(-1, "代理商ID错误", ref ReturnDS);

            return -1;
        }

        string IPAddressLimit = dt.Rows[0]["IPAddressLimit"].ToString();

        if (IPAddressLimit != "")
        {
            IPAddressLimit = "," + IPAddressLimit + ",";

            if (IPAddressLimit.IndexOf("," + GetClientIPAddress() + ",") < 0)
            {
                BuildReturnDataSetForError(-3, "IP地址限制", ref ReturnDS);

                return -3;
            }
        }

        string Key = dt.Rows[0]["Key"].ToString();
        string SignSource = AgentID.ToString() + TimeStamp;

        foreach (object Param in Params)
        {
            SignSource += ParamterToString(Param);
        }

        SignSource += Key;

        if (Shove._Security.Encrypt.MD5(SignSource).ToLower() != Sign.ToLower())
        {
            BuildReturnDataSetForError(-4, "签名校验失败", ref ReturnDS);

            return -4;
        }

        State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);

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

    // 获取某时间段内的创建的卡号列表
    [WebMethod]
    public DataSet GetNumbers(int AgentID, string TimeStamp, string Sign, string StartTime, string EndTime)
    {
        StartTime = Shove._Web.Utility.FilteSqlInfusion(StartTime);
        EndTime = Shove._Web.Utility.FilteSqlInfusion(EndTime);

        new Log("Agent\\CardPassword").Write(String.Format("Method=GetNumbers\tAgentID={0}\tTimeStamp={1}\tSign={2}\tStartTime={3}\tEndTime={4}", AgentID, TimeStamp, Sign, StartTime, EndTime));

        DataSet ds = new DataSet();
        short State = 0;

        if (Valid(ref ds, ref State, AgentID, TimeStamp, Sign, StartTime, EndTime) < 0)
        {
            return ds;
        }

        DataTable dt = new DAL.Tables.T_CardPasswords().Open("[ID], [DateTime], [Money], State, Period", "AgentID = " + AgentID.ToString() + " and (DateTime between '" + StartTime + "' and '" + EndTime + "' )", "[ID]");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            return ds;
        }

        dt.Columns.Add("Number", typeof(System.String));

        CardPassword cp = new CardPassword();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Number"] = cp.GenNumber(PF.GetCallCert(), AgentID, Shove._Convert.StrToLong(dt.Rows[i]["ID"].ToString(), -1));

            dt.AcceptChanges();
        }

        dt.Columns.Remove(dt.Columns[0]);

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(dt);

        return ds;
    }

    // 获取某卡号的有效期、是否使用状态
    [WebMethod]
    public DataSet GetNumberInformation(int AgentID, string TimeStamp, string Sign, string Number)
    {
        new Log("Agent\\CardPassword").Write(String.Format("Method=GetNumberInformation\tAgentID={0}\tTimeStamp={1}\tSign={2}\tNumber={3}", AgentID, TimeStamp, Sign, Number));

        DataSet ds = new DataSet();
        short State = 0;

        if (Valid(ref ds, ref State, AgentID, TimeStamp, Sign, Number) < 0)
        {
            return ds;
        }

        int _AgentID = -1;
        long CardPasswordID = new CardPassword().GetCardPasswordID(PF.GetCallCert(), Number, ref _AgentID);

        if ((CardPasswordID < 0) || (_AgentID != AgentID))
        {
            BuildReturnDataSetForError(-5, "卡号不存在", ref ds);

            return ds;
        }

        DataTable dt = new DAL.Tables.T_CardPasswords().Open("[DateTime], [Money], Period, State", "AgentID = " + AgentID + " and [ID] = " + CardPasswordID.ToString(), "");

        if (dt == null)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(dt);

        return ds;
    }

    // 设置卡号的有效期
    [WebMethod]
    public DataSet SetPeriod(int AgentID, string TimeStamp, string Sign, string Number, string Period)
    {
        new Log("Agent\\CardPassword").Write(String.Format("Method=SetPeriod\tAgentID={0}\tTimeStamp={1}\tSign={2}\tNumber={3}\tPeriod={4}", AgentID, TimeStamp, Sign, Number, Period));

        DataSet ds = new DataSet();
        short State = 0;

        if (Valid(ref ds, ref State, AgentID, TimeStamp, Sign, Number, Period) < 0)
        {
            return ds;
        }

        //if (Period < DateTime.Now)
        //{
        //    BuildReturnDataSetForError(-7, "失效时间不能小于当前时间", ref ds);

        //    return ds;
        //}

        DateTime t_Period;

        try
        {
            t_Period = DateTime.Parse(Period);
        }
        catch
        {
            BuildReturnDataSetForError(-8, "参数不符合规定或者未提供", ref ds);

            return ds;
        }

        long CardPasswordID = 0;
        int _AgentID = -1;

        CardPasswordID = new CardPassword().GetCardPasswordID(PF.GetCallCert(), Number, ref _AgentID);

        if ((CardPasswordID < 0) || (_AgentID != AgentID))
        {
            BuildReturnDataSetForError(-5, "卡号不存在", ref ds);

            return ds;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";

        int Results = -1;
        Results = DAL.Procedures.P_CardPasswordSetPeriod(AgentID, CardPasswordID, t_Period, ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            return ds;
        }

        if (ReturnValue < 0)
        {
            BuildReturnDataSetForError(ReturnValue, ReturnDescription, ref ds);

            return ds;
        }

        BuildReturnDataSet(0, ref ds);
        ds.Tables.Add(new DataTable());

        return ds;
    }

    // 对换新卡
    [WebMethod]
    public DataSet Exchange(int AgentID, string TimeStamp, string Sign, string CardsXml)
    {
        new Log("Agent\\CardPassword").Write(String.Format("Method=Exchange\tAgentID={0}\tTimeStamp={1}\tSign={2}\tCardsXml={3}", AgentID, TimeStamp, Sign, CardsXml));

        DataSet ds = new DataSet();
        short State = 0;

        DataTable dtCardsXml = Shove._Convert.XMLToDataTable(CardsXml);

        if ((dtCardsXml == null) || (dtCardsXml.Rows.Count < 1))
        {
            BuildReturnDataSetForError(-8, "参数不符合规定或者未提供", ref ds);

            return ds;
        }

        if (Valid(ref ds, ref State, AgentID, TimeStamp, Sign, CardsXml) < 0)
        {
            return ds;
        }

        Regex regex = new Regex(@"^[\d]{20}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        CardsXml = "<Cards>";

        CardPassword cp = new CardPassword();
        int _AgentID = -1;

        foreach (DataRow dr in dtCardsXml.Rows)
        {
            string Number = dr["Number"].ToString();

            if (!regex.IsMatch(Number))
            {
                BuildReturnDataSetForError(-8, "参数不符合规定或者未提供", ref ds);

                return ds;
            }

            long CardPasswordID = cp.GetCardPasswordID(PF.GetCallCert(), Number, ref _AgentID);

            if ((CardPasswordID < 0) || (_AgentID != AgentID))
            {
                BuildReturnDataSetForError(-5, "卡号不存在", ref ds);

                return ds;
            }

            CardsXml += "<Card ID=\"" + CardPasswordID.ToString() + "\" />";
        }

        CardsXml += "</Cards>";

        int ReturnValue = -1;
        string ReturnDescription = "";

        BuildReturnDataSet(0, ref ds);


        int Results = -1;
        Results = DAL.Procedures.P_CardPasswordExchange(ref ds, AgentID, CardsXml, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            BuildReturnDataSetForError(-9999, "未知错误", ref ds);

            return ds;
        }

        if (ReturnValue < 0)
        {
            BuildReturnDataSetForError(ReturnValue, ReturnDescription, ref ds);

            return ds;
        }

        // 置换ID为卡号
        DataTable dt = ds.Tables[1];
        dt.Columns.Add("Number", typeof(string));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Number"] = cp.GenNumber(PF.GetCallCert(), AgentID, Shove._Convert.StrToLong(dt.Rows[i]["ID"].ToString(), -1));

            dt.AcceptChanges();
        }

        dt.Columns.Remove(dt.Columns[0]);

        return ds;
    }
}


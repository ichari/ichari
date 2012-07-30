using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
///CardPasswordAddMoney 的摘要说明
/// </summary>
public class CardPassword
{
    /// <summary>
    /// 生成多张卡密号码
    /// </summary>
    /// <param name="AgentID">代理商ID</param>
    /// <param name="PeriodMonths">有效期(几个月)</param>
    /// <param name="Money">面值</param>
    /// <param name="Count">张数</param>
    /// <returns></returns>
    public int Add(int AgentID, int PeriodMonths, double Money, int Count, ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";

        if (PeriodMonths < 1)
        {
            ReturnDescription = "有效月份数超出范围";

            return -3;
        }

        if (Money < 1)
        {
            ReturnDescription = "面值超出范围";

            return -4;
        }

        if (Count < 1)
        {
            ReturnDescription = "生成卡密总数超出范围";

            return -5;
        }

        int Results = -1;
        Results = DAL.Procedures.P_CardPasswordAdd(AgentID, PeriodMonths, Money, Count, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            ReturnDescription = "数据库读取错误";

            return -6;
        }

        return ReturnValue;
    }

    /// <summary>
    /// 校验卡密
    /// </summary>
    /// <param name="AgentID"></param>
    /// <param name="CardPasswordID"></param>
    /// <param name="Money"></param>
    /// <param name="CreateDateTime"></param>
    /// <param name="Period"></param>
    /// <param name="State"></param>
    /// <param name="UserID"></param>
    /// <param name="UseDateTime"></param>
    /// <param name="ReturnDescription"></param>
    /// <returns></returns>
    public long Valid(int AgentID, long CardPasswordID, ref double Money, ref DateTime CreateDateTime, ref DateTime Period, ref short State, ref long UserID, ref DateTime UseDateTime, ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";
        int Results = -1;
        Results = DAL.Procedures.P_CardPasswordGet(AgentID, CardPasswordID, ref CreateDateTime, ref Period, ref Money, ref State, ref UserID, ref UseDateTime, ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            ReturnDescription = "数据库读取错误";

            return -2;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        return CardPasswordID;
        // -1 卡号非法, >=0 卡号ID
    }

    /// <summary>
    /// 卡密使用
    /// </summary>
    /// <param name="Number"></param>
    /// <param name="SiteID"></param>
    /// <param name="UserID"></param>
    /// <param name="ReturnDescription"></param>
    /// <returns></returns>
    public int Use(string Number, long SiteID, long UserID, ref string ReturnDescription)
    {
        int ReturnValue = -1;

        int AgentID = -1;
        long CardPasswordID = GetCardPasswordID(PF.GetCallCert(), Number, ref AgentID);

        if ((CardPasswordID < 0) || (AgentID < 0))
        {
            ReturnValue = 0;
            ReturnDescription = "";
            DAL.Procedures.P_CardPasswordTryErrorAdd(UserID, Number, ref ReturnValue, ref ReturnDescription);

            ReturnDescription = "卡号非法";

            return -1;
        }

        double t_Money = 0;
        short t_State = 0;
        long t_UserID = 0;
        DateTime t_CreateDateTime = DateTime.Now;
        DateTime t_Period = DateTime.Now;
        DateTime t_UseDateTime = DateTime.Now;

        ReturnValue = 0;
        ReturnDescription = "";

        CardPasswordID = Valid(AgentID, CardPasswordID, ref t_Money, ref t_CreateDateTime, ref t_Period, ref t_State, ref t_UserID, ref t_UseDateTime, ref ReturnDescription);

        if (CardPasswordID < 0)
        {
            ReturnValue = 0;
            ReturnDescription = "";
            DAL.Procedures.P_CardPasswordTryErrorAdd(UserID, Number, ref ReturnValue, ref ReturnDescription);

            ReturnDescription = "卡号非法";

            return -1;
        }

        if (t_State == -2)
        {
            ReturnDescription = "此卡号已经停用";

            return -2;
        }

        if ((t_State == -1) || (t_Period < DateTime.Now))
        {
            ReturnDescription = "此卡号已经过期";

            return -3;
        }

        if (t_State == 1)
        {
            ReturnDescription = "此卡号已经使用";

            return -4;
        }

        if (t_State != 0)
        {
            ReturnDescription = "此卡号不能使用";

            return -5;
        }

        ReturnDescription = "";

        int Results = -1;
        Results = DAL.Procedures.P_CardPasswordUse(AgentID, CardPasswordID, Number, SiteID, UserID, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            ReturnDescription = "数据库读取错误";

            return -6;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        return ReturnValue;
    }

    /// <summary>
    /// 生成卡号
    /// </summary>
    /// <param name="CallCert"></param>
    /// <param name="AgentID"></param>
    /// <param name="CardPasswordID"></param>
    /// <returns></returns>
    public string GenNumber(string CallCert, int AgentID, long CardPasswordID)
    {
        if (CallCert != PF.GetCallCert())
        {
            throw new Exception("Call the CardPassword.GenNumber is request a CallCert.");
        }

        return SLS.Security.CardPassword.GenNumber(CallCert, AgentID, CardPasswordID);
    }

    /// <summary>
    /// 提取卡真实ID
    /// </summary>
    /// <param name="CallCert"></param>
    /// <param name="Number"></param>
    /// <param name="AgentID"></param>
    /// <returns></returns>
    public long GetCardPasswordID(string CallCert, string Number, ref int AgentID)
    {
        if (CallCert != PF.GetCallCert())
        {
            throw new Exception("Call the CardPassword.GenNumber is request a CallCert.");
        }

        AgentID = -1;

        return SLS.Security.CardPassword.GetCardPasswordID(CallCert, Number, ref AgentID);
    }
}

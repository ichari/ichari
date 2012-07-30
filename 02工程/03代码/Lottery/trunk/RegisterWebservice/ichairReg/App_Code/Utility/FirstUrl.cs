using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///FirstUrl 的摘要说明
/// </summary>
public class FirstUrl
{
    private string DefaultUrl = Shove._Web.Utility.GetUrlWithoutHttp();

    private string Key = "FirstUrl";
    private string CpsIDKey = "SLS.TWZT.CpsID";
    private string UserIDKey = "SLS.TWZT.UnionUserID";
    private string PidKey = "SLS.TWZT.UnionPid";

    public void Save()
    {
        object FirstUrl = HttpContext.Current.Session[Key];
        if (FirstUrl == null) //只存在一次
        {
            long cpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("cpsid"), -1);
            long userID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);
            string Pid = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("PID"));

            if (!string.IsNullOrEmpty(Pid))
                HttpContext.Current.Session[PidKey] = Pid;

            //站长非域名形式的推广地址的CPS ID参数
            HttpContext.Current.Session[CpsIDKey] = cpsID;
            HttpContext.Current.Session[UserIDKey] = userID;

            HttpContext.Current.Session[Key] = DefaultUrl;

        }
    }

    public string Get()
    {
        object FirstUrl = HttpContext.Current.Session[Key];

        if ((FirstUrl == null) || (String.IsNullOrEmpty(FirstUrl.ToString())))
        {
            return DefaultUrl;
        }

        return FirstUrl.ToString();
    }

    public string PID
    {
        get
        {
            return HttpContext.Current.Session[PidKey] == null ? string.Empty : HttpContext.Current.Session[PidKey].ToString();
        }
    }
    public string CpsID
    {
        get
        {
            return HttpContext.Current.Session[CpsIDKey] == null ? string.Empty : HttpContext.Current.Session[CpsIDKey].ToString();
        }
    }
    public string USERID
    {
        get
        {
            return HttpContext.Current.Session[UserIDKey] == null ? string.Empty : HttpContext.Current.Session[UserIDKey].ToString();
        }
    }

}

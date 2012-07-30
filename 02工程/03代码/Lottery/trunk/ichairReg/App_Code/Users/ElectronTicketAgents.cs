using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Shove.Database;

/// <summary>
///ElectronTicketAgents 的摘要说明
/// </summary>
public class ElectronTicketAgents
{
     #region 成员变量

    public int ID;
    public string Name;
    public string Key;
    public string Password;
    public string Company;
    public double Balance;
    public string Url;
    public short State;
    public string UseLotteryList;
    public string IPAddressLimit;

    #endregion

    public ElectronTicketAgents()
    {
        ID = -1;
        Name = "";
        Key = "";
        Password = "";
        Company = "";
        Url = "";
        Balance = 0;
        State = 0;
        UseLotteryList = "";
        IPAddressLimit = "";
    }

    // 正常用户登录
    public int Login(ref string ReturnDescription)
    {
        DataTable dt = new DAL.Tables.T_ElectronTicketAgents().Open("", "ID=" + ID, "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            ReturnDescription = "用户不存在";

            return -1;
        }

        if (dt.Rows[0]["Password"].ToString() != PF.EncryptPassword(Password))
        {
            ReturnDescription = "密码错误";

            return -2;
        }

        if(dt.Rows[0]["State"].ToString() != "1")
        {
            ReturnDescription = "代理商帐号已经过期";

            return -2;
        }

        Name = dt.Rows[0]["Name"].ToString();
        Password = dt.Rows[0]["Password"].ToString();
        Company = dt.Rows[0]["Company"].ToString();
        Url = dt.Rows[0]["Url"].ToString();
        Balance = Convert.ToDouble(dt.Rows[0]["Balance"].ToString()); 
        State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);
        UseLotteryList = dt.Rows[0]["UseLotteryList"].ToString();
        IPAddressLimit = dt.Rows[0]["IPAddressLimit"].ToString();

        // 校验成功
        SaveUserIDToCookie();

        return 0;
    }

    // 注销
    public int Logout(ref string ReturnDescription)
    {
        ReturnDescription = "";

        RemoveUserIDFromCookie();

        return 0;
    }

    public int GetInformationByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgents().Open("", "[ID] = " + ID, "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        Name = dt.Rows[0]["Name"].ToString();
        Password = dt.Rows[0]["Password"].ToString();
        Company = dt.Rows[0]["Company"].ToString();
        Url = dt.Rows[0]["Url"].ToString();
        Balance = Convert.ToDouble(dt.Rows[0]["Balance"].ToString()); 
        State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);
        UseLotteryList = dt.Rows[0]["UseLotteryList"].ToString();
        IPAddressLimit = dt.Rows[0]["IPAddressLimit"].ToString();

        return 0;
    }

    public int GetInformationByName(ref string ReturnDescription)
    {
        if (Name == "")
        {
            throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgents().Open("", "[Name] = " + Shove._Web.Utility.FilteSqlInfusion(Name), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        ID = Shove._Convert.StrToInt(dt.Rows[0]["ID"].ToString(), 0);
        Password = dt.Rows[0]["Password"].ToString();
        Company = dt.Rows[0]["Company"].ToString();
        Url = dt.Rows[0]["Url"].ToString();
        Balance = Convert.ToDouble(dt.Rows[0]["Balance"].ToString()); 
        State = Shove._Convert.StrToShort(dt.Rows[0]["State"].ToString(), 0);
        UseLotteryList = dt.Rows[0]["UseLotteryList"].ToString();
        IPAddressLimit = dt.Rows[0]["IPAddressLimit"].ToString();

        return 0;
    }

    public int EditByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("CardPassword 尚未初始化到具体的数据实例上，请先使用 GetInformation 等获取数据信息");
        }

        DataTable dt = new DAL.Tables.T_ElectronTicketAgents().Open("", "[ID] = " + ID, "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        DAL.Tables.T_ElectronTicketAgents t_ElectronTicketAgents = new DAL.Tables.T_ElectronTicketAgents();

        t_ElectronTicketAgents.Balance.Value = Balance;
        t_ElectronTicketAgents.Company.Value = Company;
        t_ElectronTicketAgents.Password.Value = Password;
        t_ElectronTicketAgents.Name.Value = Name;
        t_ElectronTicketAgents.State.Value = State;
        t_ElectronTicketAgents.Url.Value = Url;
        t_ElectronTicketAgents.UseLotteryList.Value = UseLotteryList;
        t_ElectronTicketAgents.IPAddressLimit.Value = IPAddressLimit;

        t_ElectronTicketAgents.Update("[ID] = " + ID.ToString());

        return 0;
    }

    private void SaveUserIDToCookie()
    {
        string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_ElectronTicketAgents";

        // 写入 Cookie
        HttpCookie cookieUser = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ID.ToString()), PF.DesKey));
        
        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieUser);
        }
        catch { }
    }

    private void RemoveUserIDFromCookie()
    {
        string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_ElectronTicketAgents";

        HttpCookie cookieUser = new HttpCookie(Key);
        cookieUser.Value = "";
        cookieUser.Expires = DateTime.Now.AddYears(-20);

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieUser);
        }
        catch { }
    }

    public static ElectronTicketAgents GetCurrentUser()
    {
        string Key = (System.Web.HttpContext.Current.Session.SessionID + "").ToLower() + "_ElectronTicketAgents";

        // 从 Cookie 中取出 UserID
        HttpCookie cookieUser = HttpContext.Current.Request.Cookies[Key];

        if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Value)))
        {
            return null;
        }

        string CookieUserID = cookieUser.Value;

        try
        {
            CookieUserID = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), CookieUserID, PF.DesKey));
        }
        catch
        {
            CookieUserID = "";
        }

        if (String.IsNullOrEmpty(CookieUserID))
        {
            return null;
        }

        int UserID = Shove._Convert.StrToInt(CookieUserID, -1);

        if (UserID < 1)
        {
            return null;
        }

        ElectronTicketAgents electronTicketAgents = new ElectronTicketAgents();

        electronTicketAgents.ID = UserID;

        string ReturnDescription = "";
        int Result = electronTicketAgents.GetInformationByID(ref ReturnDescription);

        if (Result < 0)
        {
            return null;
        }

        return electronTicketAgents;
    }

    public void Clone(ElectronTicketAgents electronTicketAgents)
    {
        electronTicketAgents.ID = ID;
        electronTicketAgents.Name = Name;
        electronTicketAgents.Key = Key;
        electronTicketAgents.Password = Password;
        electronTicketAgents.Company = Company;
        electronTicketAgents.Balance = Balance;
        electronTicketAgents.Url = Url;
        electronTicketAgents.State = State;
        electronTicketAgents.UseLotteryList = UseLotteryList;
        electronTicketAgents.IPAddressLimit = IPAddressLimit;
    }
}

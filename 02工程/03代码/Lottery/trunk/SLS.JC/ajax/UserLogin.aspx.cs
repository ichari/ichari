using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ajax_UserLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string UserName = "";

        if (!string.IsNullOrEmpty(Request["UserName"]))
        {
            UserName = Shove._Web.Utility.GetRequest("UserName");
        }

        string Password = "";

        if (!string.IsNullOrEmpty(Request["Password"]))
        {
            Password = Shove._Web.Utility.GetRequest("Password");
        }

        string RegCode = "";

        if (!string.IsNullOrEmpty(Request["RegCode"]))
        {
            RegCode = Shove._Web.Utility.GetRequest("RegCode");
        }

        if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(RegCode))
        {
            try
            {
                if (Shove._Web.Cache.GetCacheAsString("CheckCode_" + Request.Cookies["ASP.NET_SessionId"].Value, "") != Shove._Security.Encrypt.MD5(PF.GetCallCert() + RegCode))
                {
                    Response.Write("{\"message\": \"验证码输入错误，请重新输入。\"}");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
            }

            string ReturnDescription = "";

            Users user = new Users(1);
            user.Name = UserName;
            user.Password = Password;

            int Result = 0;

            Result = user.Login(ref ReturnDescription);

            if (Result < 0)
            {
                Response.Write("{\"message\": \"" + ReturnDescription + "\"}");
                Response.End();
            }
        }

        Users _User = Users.GetCurrentUser(1);
        
        string action = "";

        if (!string.IsNullOrEmpty(Request["action"]))
        {
            action = Shove._Web.Utility.GetRequest("action");
        }

        if (action.Equals("loginout"))
        {

            string ReturnDescption = "";
            int Result = 0;

            if (_User != null)
            {
                Result = _User.Logout(ref ReturnDescption);
            }

            if (_User == null)
            {
                Response.Write("{\"message\": \"-1\"}");
                Response.End();
            }

            if (Result < 0 || ReturnDescption != "")
            {
                Response.Write("{\"message\": \"退出失败，请重新退出。\"}");
                Response.End();
            }

            Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\",\"ismanager\": \"False\" }");
            Response.End();
        }

        if (_User == null)
        {
            Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\" ,\"ismanager\": \"False\" }");
            Response.End();
        }
        string ut = "";
        switch (_User.UserType)
        {
            case 1: ut = "普通会员";
                break;
            case 2: ut = "高级会员";
                break;
            case 3: ut = "VIP会员";
                break;
        }
        
        Response.Write("{\"message\": \"" + _User.ID.ToString() + "\", \"Date\": \"" + _User.LastLoginTime.ToShortDateString() + "\", \"name\": \"" + _User.Name + "\", \"Balance\": \"" + _User.Balance.ToString() + "\", \"Time\" : \"" + _User.LastLoginTime.ToShortTimeString() + "\", \"ismanager\": \"" + (_User.Competences.CompetencesList != "").ToString() + "\" }");
        Response.End();
    }
}

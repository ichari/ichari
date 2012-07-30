<%@ WebHandler Language="C#" Class="UserLogin" %>

using System;
using System.Web;
using System.Data;
using System.Text;

using Shove.Database;

public class UserLogin : IHttpHandler
{
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.AddHeader("Cache-Control", "no-cache");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        string UserName = "";
        if (!string.IsNullOrEmpty(context.Request["UserName"]))
        {
            UserName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "UserName"));
        }

        string Password = "";
        if (!string.IsNullOrEmpty(context.Request["Password"]))
        {
            Password = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "Password"));
        }

        string RegCode = "";
        if (!string.IsNullOrEmpty(context.Request["RegCode"]))
        {
            RegCode = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "RegCode"));
        }

        if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
        {
            // 增加 无验证码登陆
            if (!string.IsNullOrEmpty(RegCode))
            {
                try
                {
                    if (Shove._Web.Cache.GetCacheAsString("CheckCode_" + context.Request.Cookies["ASP.NET_SessionId"].Value, "") != Shove._Security.Encrypt.MD5(PF.GetCallCert() + RegCode))
                    {
                        context.Response.Write("{\"message\": \"验证码输入错误，请重新输入。\"}");

                        return;
                    }
                }
                catch (Exception e)
                {
                }
            }

            string ReturnDescription = "";

            Users user = new Users(1);
            user.Name = UserName;
            user.Password = Password;

            int Result = 0;
            
            Result = user.Login(ref ReturnDescription);

            if (Result < 0)
            { 
               context.Response.Write("{\"message\": \"" + ReturnDescription + "\"}");

                return;
            }
        }
        
        Users _User = Users.GetCurrentUser(1);

        string action = "";

        if (!string.IsNullOrEmpty(context.Request["action"]))
        {
            action = Shove._Web.Utility.GetRequest(context, "action");
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
                context.Response.Write("{\"message\": \"-1\"}");
                context.Response.End();
            }

            if (Result < 0 || ReturnDescption != "")
            {
                context.Response.Write("{\"message\": \"退出失败，请重新退出。\"}");
                context.Response.End();
            }

            context.Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\",\"ismanager\": \"False\" }");

            return;
        }

        if (_User == null)
        {
            context.Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\" ,\"ismanager\": \"False\" }");

            return;
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
        context.Response.Write("{\"message\": \"" + _User.ID.ToString() + "\", \"name\": \"" + _User.Name + "\", \"Date\": \"" + _User.LastLoginTime.ToShortDateString() + "\", \"Time\": \"" + _User.LastLoginTime.ToShortTimeString() + "\", \"Balance\": \"" + _User.Balance.ToString() + "\", \"Type\": \"" + ut + "\", \"ismanager\": \"" + (_User.Competences.CompetencesList != "").ToString() + "\" }");
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
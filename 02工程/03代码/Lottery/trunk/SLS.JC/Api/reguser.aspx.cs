using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Api_reguser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Log("Users").Write(string.Format("接收同步注册请求，来源地址{0}",Request.UserHostAddress));
        if (Request.UserHostAddress != System.Configuration.ConfigurationManager.AppSettings["SyncRequestIp"])
            return;
        Response.Write(RegUser());
    }

    private string RegUser()
    {
        string userName = Request.Form["userName"];
        string pwd = Request.Form["pwd"];
        string email = Request.Form["email"];
        string trueName = Request.Form["trueName"];

        var u = new Users(1);
        u.Name = userName;
        u.Password = pwd;
        u.Email = email;
        u.RealityName = trueName;
        u.UserType = 1;

        string returnMsg = string.Empty;
        int r = u.Add(ref returnMsg);
        if (r < 0) {
            var msg = string.Format("注册失败！{0}", returnMsg);
            new Log("Users").Write(msg);
            return msg;
        }
        return r.ToString();
    }
}
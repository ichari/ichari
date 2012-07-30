using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Api_UpdatePwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UserHostAddress != System.Configuration.ConfigurationManager.AppSettings["SyncRequestIp"])
            return;
        var errMsg = string.Empty;
        UpdatePwd(ref errMsg);
        if (string.IsNullOrEmpty(errMsg))
            Response.Write("1");
        else
            Response.Write(errMsg);
    }

    private void UpdatePwd(ref string errMsg)
    {
        string lotteryUserId = Request.Form["lotUserId"];
        string pwd = Request.Form["pwd"];
        
        var userId = 0L;
        long.TryParse(lotteryUserId,out userId);

        if (userId <= 0)
        {
            errMsg = string.Format("请求参数不正确lotUserId={0}",userId);
            return ;
        }

        var _User = new Users(1);
        _User.ID = userId;
        var rs = string.Empty;
        _User.GetUserInformationByID(ref rs);

        
        _User.Password = pwd;

        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            new Log("Users").Write("同步修改密码失败：" + ReturnDescription);
            errMsg = "同步修改密码失败：" + ReturnDescription;

        }

    }
}
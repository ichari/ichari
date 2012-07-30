using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_ChaseLogin : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
            CheckCode.Visible = isUseCheckCode;

            new Login().SetCheckCode(_Site, ShoveCheckCode1);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void btnLogin_Click(object sender, System.EventArgs e)
    {
        string ReturnDescription = "";

        int Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, tbFormCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Response.Write("<script>window.top.location.href='../../LotteryPackage.aspx'</script>");
    }
}

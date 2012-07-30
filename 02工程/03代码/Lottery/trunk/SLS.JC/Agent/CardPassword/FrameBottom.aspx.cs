using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Agent_CardPassword_FrameBottom : CardPasswordPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        if (_CardPasswordAgentUser != null)
        {
            string ReturnDescription = "";

            _CardPasswordAgentUser.Logout(ref ReturnDescription);
        }

        Response.Write("<script language=\"javascript\">window.top.location.href=\"../Default.aspx\"</script>");
    }
}

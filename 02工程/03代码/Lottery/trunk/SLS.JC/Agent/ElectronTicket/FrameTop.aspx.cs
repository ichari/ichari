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

public partial class Agent_ElectronTicket_FrameTop : ElectronTicketAgentsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        labUserName.Text = _ElectronTicketAgents.Name;
    }

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        if (_ElectronTicketAgents != null)
        {
            string ReturnDescription = "";

            _ElectronTicketAgents.Logout(ref ReturnDescription);
        }

        Response.Write("<script language=\"javascript\">window.top.location.href=\"Login.aspx\"</script>");
    }
}

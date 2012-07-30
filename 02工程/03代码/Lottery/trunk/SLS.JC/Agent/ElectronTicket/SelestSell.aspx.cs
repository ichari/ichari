using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Agent_ElectronTicket_SelestSell : ElectronTicketAgentsPageBase
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

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        lbElectronTicketID.Text = _ElectronTicketAgents.ID.ToString();
        lbName.Text = _ElectronTicketAgents.Name;

        lbState.Text = "暂未开通";

        if (_ElectronTicketAgents.State == 1)
        {
            lbState.Text = "开通";
        }

        lbBalance.Text = _ElectronTicketAgents.Balance.ToString("N");
    }
}

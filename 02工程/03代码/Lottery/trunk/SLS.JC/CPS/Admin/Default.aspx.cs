using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_Admin_Default : CpsPageBase
{
    public string SubPage = "BaseInfo.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SubPage = Shove._Web.Utility.GetRequest("SubPage");

            if (SubPage == "")
            {
                SubPage = "BaseInfo.aspx";
            }

            lbUserName.Text = _User.Name;
            if (_User.cps.Type == 2)
            {
                lbUserType.Text = "代理商";
                trPromoter.Visible = true;
            }
        }

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx";

        base.OnInit(e);
    }

    #endregion

    protected void lbExit_Click(object sender, EventArgs e)
    {
        if (_User != null)
        {
            string ReturnDescription = "";
            _User.Logout(ref ReturnDescription);
        }

        this.Response.Redirect("../Default.aspx");
    }
}

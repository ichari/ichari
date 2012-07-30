using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_Administrator_Admin_Default : CpsAdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Administrator/Admin/Default.aspx";
        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    protected void lbExit_Click(object sender, EventArgs e)
    {
        Users users = Users.GetCurrentUser(1);

        if (users != null)
        {
            string ReturnDescription = "";
            users.Logout(ref ReturnDescription);
        }

        if (_User != null && Session["CpsAdminPageBase_Users"] != null)
        {
            Session.Remove("CpsAdminPageBase_Users");
        }

        this.Response.Redirect("../Default.aspx");
    }
}

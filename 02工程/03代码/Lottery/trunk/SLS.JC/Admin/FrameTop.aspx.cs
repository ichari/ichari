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

public partial class Admin_FrameTop : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (_User.Competences.CompetencesList == "")
            {
                PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", "Admin_FrameTop");

                return;
            }

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
        labUserName.Text = _User.Name;
    }

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        if (_User != null)
        {
            string ReturnDescription = "";

            _User.Logout(ref ReturnDescription);
        }

        Response.Write("<script language=\"javascript\">top.location.href=\"Default.aspx\"</script>");
    }
}

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

public partial class Admin_OptionEmail : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }

        tb4.Attributes.Add("value", tb4.Text);
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        tb1.Text = _Site.SiteOptions["Opt_EmailServer_From"].ToString("");
        tb2.Text = _Site.SiteOptions["Opt_EmailServer_EmailServer"].ToString("");
        tb3.Text = _Site.SiteOptions["Opt_EmailServer_UserName"].ToString("");
        tb4.Text = _Site.SiteOptions["Opt_EmailServer_Password"].ToString("");
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            _Site.SiteOptions["Opt_EmailServer_From"] = new OptionValue(tb1.Text);
            _Site.SiteOptions["Opt_EmailServer_EmailServer"] = new OptionValue(tb2.Text);
            _Site.SiteOptions["Opt_EmailServer_UserName"] = new OptionValue(tb3.Text);
            _Site.SiteOptions["Opt_EmailServer_Password"] = new OptionValue(tb4.Text);
        }
        catch (Exception exception)
        {
            Shove._Web.JavaScript.Alert(this.Page, exception.Message);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "设置成功。");
    }
}

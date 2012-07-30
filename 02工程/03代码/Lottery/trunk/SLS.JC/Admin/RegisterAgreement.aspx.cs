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

public partial class Admin_RegisterAgreement : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        tbContent.Value = _Site.SiteOptions["Opt_UserRegisterAgreement"].ToString("");
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            _Site.SiteOptions["Opt_UserRegisterAgreement"] = new OptionValue(tbContent.Value);
        }
        catch (Exception exception)
        {
            PF.GoError(ErrorNumber.Unknow, exception.Message, "Admin_Questions");

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "保存成功。");
    }
}

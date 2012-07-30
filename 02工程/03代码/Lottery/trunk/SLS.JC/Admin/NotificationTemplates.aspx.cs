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

public partial class Admin_NotificationTemplates : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForNotificationType();

            ddlTemplateType_SelectedIndexChanged(ddlTemplateType, e);

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForNotificationType()
    {
        DataTable dt = new DAL.Tables.T_NotificationTypes().Open("[Name], [Code]", "", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_NotificationTemplates");

            return;
        }

        listTemplateFile.DataSource = dt;
        listTemplateFile.DataTextField = "Name";
        listTemplateFile.DataValueField = "Code";
        listTemplateFile.DataBind();

        listTemplateFile.SelectedIndex = 0;
    }

    protected void ddlTemplateType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        listTemplateFile_SelectedIndexChanged(listTemplateFile, new EventArgs());
    }

    protected void listTemplateFile_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        tbContent.Text = _Site.SiteNotificationTemplates[short.Parse(ddlTemplateType.SelectedValue), listTemplateFile.SelectedValue];
    }

    protected void btnOK_Click(object sender, System.EventArgs e)
    {
        try
        {
            _Site.SiteNotificationTemplates[short.Parse(ddlTemplateType.SelectedValue), listTemplateFile.SelectedValue] = tbContent.Text;
        }
        catch(Exception exception)
        {
            Shove._Web.JavaScript.Alert(this.Page, exception.Message);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "保存成功。");
    }
}

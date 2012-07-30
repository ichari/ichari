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

using Shove.Database;

public partial class Admin_SendSMSList : AdminPageBase
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

        RequestCompetences = Competences.BuildCompetencesList(Competences.SendMessage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        int days = int.Parse(ddlDateTime.SelectedValue);
        string CmdStr;

        if (days == -1)
        {
            CmdStr = "select * from T_SMS where SiteID = " + _Site.ID.ToString() + " and SMSID = -1 order by [DateTime] desc";
        }
        else
        {
            CmdStr = "select * from T_SMS where SiteID = " + _Site.ID.ToString() + " and Datediff(day, [DateTime], GetDate()) <= " + days.ToString() + " and SMSID = -1 order by [DateTime] desc";
        }

        DataTable dt = MSSQL.Select(CmdStr);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SendSMSList");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void ddlDateTime_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }
}

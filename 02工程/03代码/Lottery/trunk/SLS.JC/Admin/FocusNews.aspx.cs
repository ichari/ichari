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
using System.Text.RegularExpressions;

public partial class Admin_FocusNews : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
            g.Columns[3].Visible = btnAdd.Visible;
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
        DataTable dt = new DAL.Tables.T_FocusNews().Open("", "", "DateTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FocusNews");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("FocusNewsAdd.aspx?id=" + e.Item.Cells[4].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            new DAL.Tables.T_FocusNews().Delete("ID="+e.Item.Cells[4].Text.Trim());

            Shove._Web.Cache.ClearCache("Default_GetFocusNews");

            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("FocusNewsAdd.aspx", true);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.FindControl("cbIsMaster");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[5].Text, true);
        }
    }
}

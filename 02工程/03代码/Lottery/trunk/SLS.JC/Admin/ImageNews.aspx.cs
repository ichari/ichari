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

public partial class Admin_ImageNews : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.EditNews));
            g.Columns[2].Visible = btnAdd.Visible;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.EditNews,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion
   
    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_FocusImageNews().Open("", "", "ID desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("ImageNewsEdit.aspx?ID=" + e.Item.Cells[3].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            new DAL.Tables.T_FocusImageNews().Delete("ID="+e.Item.Cells[3].Text.Trim());

            Shove._Web.Cache.ClearCache("Home_Room_UserControls_Index_banner_ImagePlay");

            Shove._Web.Cache.ClearCache("Default_BindFocusImage");

            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("ImageNewsAdd.aspx", true);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void tv_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindData();
    }
}

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

public partial class Admin_SiteAffiches : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
            g.Columns[4].Visible = btnAdd.Visible;
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
        DataTable dt = new DAL.Tables.T_SiteAffiches().Open("", "SiteID = " + _Site.ID.ToString(), "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffiches");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[2].FindControl("cbisShow");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[6].Text, true);

            cb = (CheckBox)e.Item.Cells[3].FindControl("cbisCommend");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[7].Text, true);
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("SiteAffichesEdit.aspx?id=" + e.Item.Cells[5].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";
            int Results = -1;
            Results = DAL.Procedures.P_SiteAfficheDelete(_Site.ID, long.Parse(e.Item.Cells[5].Text), ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffiches");

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_SiteAffiches");

                return;
            }
            Shove._Web.Cache.ClearCache(CacheKey.SiteAffiches);
            Shove._Web.Cache.ClearCache("Default_GetSiteAffiches");

            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("SiteAffichesAdd.aspx", true);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
